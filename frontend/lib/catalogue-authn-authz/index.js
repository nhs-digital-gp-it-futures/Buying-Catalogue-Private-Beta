// set up OpenID Connect authentication
const passport = require('passport')
const { Issuer, Strategy } = require('openid-client')
const { dataProvider } = require('catalogue-data')

const OIDC_AUTHENTICATE_PATH = '/oidc/authenticate'
const OIDC_CALLBACK_PATH = '/oidc/callback'

function authentication (app) {
  app.use(passport.initialize())
  app.use(passport.session())

  app.get(OIDC_AUTHENTICATE_PATH, passport.authenticate('oidc'))
  app.get(OIDC_CALLBACK_PATH, authenticationHandler)

  Issuer.defaultHttpOptions = { timeout: 10000, retries: 3 }

  const makeIssuer = process.env.OIDC_ISSUER_URL
    ? Issuer.discover(process.env.OIDC_ISSUER_URL)
    : Promise.resolve(
      new Issuer({
        issuer: 'http://oidc-provider:9000',
        authorization_endpoint: 'http://localhost:9000/auth',
        token_endpoint: 'http://oidc-provider:9000/token',
        userinfo_endpoint: 'http://oidc-provider:9000/me',
        jwks_uri: 'http://oidc-provider:9000/certs',
        check_session_iframe: 'http://localhost:9000/session/check',
        end_session_endpoint: 'http://localhost:9000/session/end'
      })
    )

  return makeIssuer
    .then(issuer => {
      const client = new issuer.Client({
        client_id: process.env.OIDC_CLIENT_ID,
        client_secret: process.env.OIDC_CLIENT_SECRET
      })

      client.CLOCK_TOLERANCE = 60


      // So we're not setting a session key here, so the Strategy is defaulting to using
      // information found with the Issuer. Is it possible that when horizontally scaling
      // this application that the strategy _key ends up different for each instance of
      // the front-end container?
      passport.use('oidc', new Strategy({
        client,
        params: {
          scope: 'openid email',
          redirect_uri: `${process.env.BASE_URL}${OIDC_CALLBACK_PATH}/`
        }
        // Should we retrieve store and retrieve a session key from redis???
        // ,
        // false
        // getKeyFromRedisIfItExistsAndIsStillValid
      }, authCallback))

      passport.serializeUser((user, done) => {
        done(null, user)
      })

      passport.deserializeUser((user, done) => {
        if (user.auth_header) {
          dataProvider.setAuthenticationToken(user.auth_header)
        }
        done(null, user)
      })

      app.get('/logout', (req, res) => {
        req.logout()

        if (!process.env.OIDC_ISSUER_URL) {
          const endSessionUrl = client.endSessionUrl()
          res.redirect(endSessionUrl)
        } else {
          res.redirect('/')
        }
      })
    })
    .catch(err => {
      console.error('OIDC initialisation failed:', err)
      process.exit(1)
    })
}

async function authCallback (tokenset, userinfo, done) {
  const authHeader = tokenset.access_token
  dataProvider.setAuthenticationToken(authHeader)

  if (!userinfo) {
    done(null, false)
    return
  }

  // load the contact corresponding with the email address
  // if there is no such contact, the user is still authenticated but the
  // lack of contact or organisation will prevent authorisation for
  // supplier-specific routes
  try {
    const { contact, org } = await dataProvider.contactByEmail(userinfo.email)
    const user = {
      ...userinfo,
      org,
      contact,
      first_name: contact.firstName,
      is_authenticated: true,
      auth_header: authHeader
    }
    done(null, user)
  } catch (err) {
    console.log('API Error', err)
    return done(null, {
      ...userinfo,
      first_name: userinfo.email,
      is_authenticated: true,
      auth_header: authHeader
    })
  }
}

function authenticationHandler (req, res, next) {
  // Preserve the originally requested page so that it can be returned to
  // once authentication is successful. Blacklist the OIDC callback page
  // as returning to that would cause an endless loop.
  if (!req.session.redirectTo && !req.originalUrl.startsWith(OIDC_CALLBACK_PATH)) {
    req.session.redirectTo = req.originalUrl
  }

  passport.authenticate('oidc', function (err, user, info) {
    // FIXME: using "private" members, not a sustainable long-term strategy
    const passportOIDCSessionKey = passport._strategies.oidc._key

    // This is where the "did not find expected authorization request details
    // in session, req.session["oidc:oidc-provider"] is undefined" happens.
    // The error object itself is not differentiated enough to use as a signal
    // so check the actual session entry and, if it doesn't exist, force a login.
    if (err) {
      if (!(passportOIDCSessionKey in req.session)) {
        req.session.bodyOnRedirect = {
          ...req.body
        }
        delete req.session.bodyOnRedirect._csrf
        return res.redirect(OIDC_AUTHENTICATE_PATH)
      }

      return next(err)
    }

    // If no user resulted from authentication, send the user back through the process.
    if (!user) { return res.redirect(OIDC_AUTHENTICATE_PATH) }

    // Log the user in and send them to their originally requested page.
    req.logIn(user, function (err) {
      if (err) { return next(err) }

      let redirectTo = req.session.redirectTo || '/'
      if (req.session.redirectTo && req.session.bodyOnRedirect) {
        const redirectUrl = new URL(redirectTo, process.env.BASE_URL)
        redirectUrl.search = require('qs').stringify({
          restore: '',
          ...req.session.bodyOnRedirect
        })
        redirectTo = redirectUrl.pathname + redirectUrl.search + redirectUrl.hash
      }

      delete req.session.redirectTo
      delete req.session.bodyOnRedirect
      return res.redirect(redirectTo)
    })
  })(req, res, next)
}

function authenticatedOnly (req, res, next) {
  if (!req.user || !req.user.is_authenticated) {
    authenticationHandler(req, res, next)
  } else next()
}

/**
 * A generic error handler for dealing with API failures.
 *
 * Call this in the catch handler for a failed API call to automatically
 * deal with errors caused by authentication failure. If this function
 * returns true, exit the request handler as quickly as possible e.g. return.
 */
function catchHandler (err, res, context) {
  // if the API call returns unauthorised, redirect the user to login again
  // otherwise add the error to the page
  if (err.status === 401) {
    res.redirect(OIDC_AUTHENTICATE_PATH)
    return true
  } else if (context && context.errors) {
    if (!context.errors.items) {
      context.errors.items = []
    }

    context.errors.items.push({ msg: err.toString() })
  }
}

module.exports = {
  authentication,
  authorisation: {
    authenticatedOnly,
    suppliersOnly: [
      authenticatedOnly,
      (req, res, next) => {
        if (req.user && req.user.org && req.user.org.isSupplier) next()
        else res.redirect('/')
      }
    ],
    assessmentTeamOnly: [
      authenticatedOnly,
      (req, res, next) => {
        if (req.user && req.user.org && req.user.org.isNHSDigital) next()
        else res.redirect('/')
      }
    ]
  },
  catchHandler
}
