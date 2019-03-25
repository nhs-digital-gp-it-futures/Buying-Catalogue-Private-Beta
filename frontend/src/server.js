require('dotenv').config()

const path = require('path')

const { PORT = 8000 } = process.env

const express = require('express')
const expresshbs = require('express-handlebars')

const app = express()

app.enable('strict routing')

const session = require('express-session')
const sessionConfig = {
  secret: process.env.SESSION_SECRET ||
            Math.floor(Math.random() * Number.MAX_SAFE_INTEGER).toString(36),
  resave: false,
  saveUninitialized: false,
  store: require('catalogue-data').sessionStore
}

app.use(session(sessionConfig))

app.use(require('body-parser').urlencoded({ extended: true }))

if (process.env.NODE_ENV === 'development') {
  app.get('/styles.css', require('node-sass-middleware')({
    src: path.join(__dirname, 'styles'),
    response: true
  }))
}

app.use(require('serve-static')(path.join(__dirname, 'static'), {
  index: false
}))

const viewPath = path.join(__dirname, 'views')
const hbs = expresshbs.create({
  layoutsDir: path.join(viewPath, 'layouts'),
  defaultLayout: 'layout.html',
  extname: '.html',
  partialsDir: path.join(viewPath, 'partials'),
  compilerOptions: {
    preventIndent: true
  }
})
app.engine('html', hbs.engine)
app.set('view engine', 'html')
app.set('views', viewPath)

require('catalogue-i18n')(hbs.handlebars, path.join(__dirname, 'locales'))

const { authentication, authorisation } = require('catalogue-authn-authz')

authentication(app).then(() => {
  // middleware to set global template context variables
  app.use('*', (req, res, next) => {
    res.locals.user = req.user
    res.locals.meta = {
      deployedEnvironment: process.env.DEPLOYED_ENV_LABEL
    }
    next()
  })

  app.get('/', (req, res) => {
    const redirectTo = req.session.redirectTo
    req.session.redirectTo = null

    if (redirectTo) {
      res.redirect(redirectTo)
    } else if (req.user && req.user.org && req.user.org.isSupplier) {
      res.redirect('/suppliers')
    } else if (req.user && req.user.org && req.user.org.isNHSDigital) {
      res.redirect('/assessment')
    } else {
      const context = { onHomePage: true }
      res.render('index', context)
    }
  })

  app.get('/about', (req, res) => {
    const context = {
      breadcrumbs: [
        { label: 'HomePage.Breadcrumb', url: '/' },
        { label: 'Support.Title' }
      ]
    }
    res.locals.meta.title = 'About'
    res.render('about', context)
  })

  app.use('/suppliers/', authorisation.suppliersOnly, require('./routes/supplier'))
  //  app.use('/assessment', authorisation.assessmentTeamOnly, require('./routes/assessment'))

  // automatically redirect to canonical path (with or without trailing slash)
  // routes mostly use trailing slashes to enable path-relative URLs
  app.use(require('express-slash')())

  // generic error handler
  app.use((err, req, res, next) => {
    const errMessage = {
      stack: err.stack,
      originalUrl: req.originalUrl,
      datetime: (new Date(Date.now())).toISOString()
    }

    console.error(errMessage)

    // don't allow the stack through to the template in production
    if (process.env.NODE_ENV === 'production') {
      // delete err.stack
    }

    res.status(500)
    res.render('error', { error: err })
  })

  // Default route, 404: Page Not Found.
  app.use('*', (req, res) => {
    const context = {
      error: {
        message: 'Error 404: Page Not Found',
        stack: `A request was made for a resource that could not be found:${req.originalUrl}\n\nPlease check you entered the correct URL.`
      }
    }
    res.render('error', context)
  })

  console.info('NODE_ENV:', process.env.NODE_ENV)
  console.info('Listening on port', PORT)

  app.listen(PORT)
})
