const i18next = require('i18next')
const registerI18nHelper = require('handlebars-i18next').default
const path = require('path')

module.exports = function (Handlebars, localesPath) {
  i18next
    .use(require('i18next-node-fs-backend'))
    .init({
      backend: {
        loadPath: path.join(localesPath, '{{lng}}/{{ns}}.json')
      },
      lng: 'en',
      fallbackLng: false,
      debug: 'DEBUG_I18N' in process.env,
      ns: ['common', 'pages', 'validation'],
      defaultNS: 'pages',
      fallbackNS: ['common', 'validation']
    }, function (err, t) {
      if (err) throw new Error(err)
      module.exports.t = t
      registerI18nHelper(Handlebars, i18next, 't')
      registerI18nBlockHelper(Handlebars, i18next)
    })
}

/*
  A version of the translation helper that returns objects, specifically
  intended for use with {{#if}} and {{#each}} to iterate over translations
  with multiple values i.e. multiple paragraphs of text.

  Copied from the original registerI18nHelper but with returnObjects:true

  {{#if (tt 'Page.Test.Items')}}
  <ul>
    {{#each (tt 'Page.Test.Items')}}
    <li>{{.}}</li>
    {{/each}}
  </ul>
  {{/if}}
*/

function registerI18nBlockHelper (Handlebars, i18next, name) {
  Handlebars.registerHelper('tt', function (key, _ref) {
    const hash = _ref.hash
    const data = _ref.data
    const fn = _ref.fn

    const parsed = {}
    ;['lngs', 'fallbackLng', 'ns', 'postProcess', 'interpolation'].forEach(function (key) {
      if (hash[key]) {
        parsed[key] = JSON.parse(hash[key])
        delete hash[key]
      }
    })
    const options = Object.assign({}, data.root.i18next, hash, parsed, { returnObjects: true })
    const replace = options.replace = Object.assign({}, this, options.replace, hash)
    delete replace.i18next // may creep in if this === data.root
    if (fn) options.defaultValue = fn(replace)
    return i18next.t(key, options)
  })
}
