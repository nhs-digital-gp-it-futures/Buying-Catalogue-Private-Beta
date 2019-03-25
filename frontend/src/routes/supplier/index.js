const _ = require('lodash')
const router = require('express').Router({ strict: true, mergeParams: true })
const { dataProvider } = require('catalogue-data')
const { catchHandler } = require('catalogue-authn-authz')

console.info('ENV VARIABLES:', _.omitBy(process.env, (val, key) => _.startsWith(key, 'npm')))

// work-around for Express bug 2281, open since 2014
// https://github.com/expressjs/express/issues/2281
function strictRouting (req, res, next) {
  if (req.originalUrl.slice(-1) === '/') return next()
  next('route')
}

router.get('/', strictRouting, async (req, res) => {
  const context = {
    breadcrumbs: [
      { label: 'HomePage.Breadcrumb', url: '/' },
      { label: 'MySolutions.Title' }
    ],
    errors: { items: [] },
    solutions: {
      onboarding: [],
      live: []
    },
    addUrl: 'solutions/new/#content'
  }

  try {
    context.solutions = await dataProvider.solutionsForSupplierDashboard(req.user.org.id, soln => ({
      ...soln,
      url: `solutions/${soln.id}/#content`
    }))

    context.solutions.onboarding = _.orderBy(context.solutions.onboarding, 'displayName')
    context.solutions.live = _.orderBy(context.solutions.live, 'displayName')
  } catch (err) {
    if (catchHandler(err, res, context)) return
  }

  res.render('supplier/index', context)
})

router.use('/solutions/', require('./registration'))
router.use('/compliance/', require('./compliance'))
router.use('/capabilities/', require('./capabilities'))

module.exports = router
