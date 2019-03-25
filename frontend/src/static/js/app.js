/* global $, $$, Modernizr, Document, Element */
/* eslint-env browser */

// jQuery-esque shorthand for common element selection operations
window.$ = function $ (selector, el) {
  return (el || document).$(selector)
}

window.$$ = function $$ (selector, el) {
  return (el || document).$$(selector)
}

Document.prototype.$ = Element.prototype.$ = function (selector) {
  return this.querySelector(selector)
}

Document.prototype.$$ = Element.prototype.$$ = function (selector) {
  return Array.from(this.querySelectorAll(selector))
}

function restoreSavedFormInputs () {
  // we only support restoration on modern browsers
  if (!window.URLSearchParams) return

  // if there is no "restore" key, there's nothing to do
  const url = new URL(window.location)
  const qs = new URLSearchParams(decodeURI(url.search))
  if (!qs.has('restore')) return

  // iterate over all the form elements that can be found, check if
  // a restoration key exists and if so restore that value
  $$('#content [name]:not([type=hidden])').forEach(function (elInput) {
    if (qs.has(elInput.name)) {
      elInput.value = qs.get(elInput.name)
      elInput.dispatchEvent(new InputEvent('input'))
    }
  })

  // finally remove the entire query string to avoid it being reused
  window.history.replaceState(null, '', window.location.origin + window.location.pathname + window.location.hash)
}

// Compensate for fixed page header height causing anchored links to appear
// off-screen on page load. Note that this can't be DOMContentLoaded as the
// initial scroll position isn't set at that point. Nor is it set during the
// load event on IE and Edge. So, the actual compensatory scroll is deferred
// for a brief period to allow all browsers to settle on the required state.
window.onload = window.onhashchange = function () {
  const adjustmentElement = $('body > header')
  const adjustment = adjustmentElement.offsetHeight
  const scrollingElement = document.scrollingElement || document.documentElement

  setTimeout(function () {
    scrollingElement.scrollTop = Math.max(0, scrollingElement.scrollTop - adjustment)

    restoreSavedFormInputs()
  }, 25)
}

// Simulate support for the form attribute on inputs for IE11
if (!Modernizr.formattribute) {
  document.addEventListener('DOMContentLoaded', function () {
    $$('body > header, #unsaved-changes').forEach(function (elContainer) {
      elContainer.addEventListener('click', function (ev) {
        if (ev.target.tagName === 'INPUT' && ev.target.hasAttribute('form')) {
          const form = document.getElementById(ev.target.getAttribute('form'))
          if (form) {
            ev.preventDefault()

            // append a hidden input with the name and value of the clicked button to
            // the form, then ask it to submit
            const input = document.createElement('input')
            input.type = 'hidden'
            input.name = ev.target.name
            input.value = ev.target.value

            form.appendChild(input)
            form.submit()
          }
        }
      })
    })
  })
}

document.addEventListener('DOMContentLoaded', function () {

  /**
   * Global Dirty Flag
   * Used to ensure that unsaved changes are communicated.
   *
   * If there are active save buttons on the page, and there is an error on the page
   * then the user has unsaved changes and should be warned.
   */
  var DIRTY_PAGE = ($('#errors') && $('input[type="submit"]')) || false

  // collapse all but the first collapsible fieldsets by default, except ones that contain
  // invalid fields
  function collapseAllFieldsetsExcept (elCurrent) {
    $$('fieldset.collapsible').forEach(function (el) {
      if (el !== elCurrent && !el.classList.contains('invalid')) {
        el.classList.add('collapsed')
      }
    })
  }

  // HACK: improperly moved code broke the script at the original position, expose the
  //       missing function
  window.collapseAllFieldsetsExcept = collapseAllFieldsetsExcept

  collapseAllFieldsetsExcept($('fieldset.collapsible:first-of-type'))

  function handleCollapsibleFieldset (ev) {
    if (ev.target.closest('legend')) {
      const elFieldset = ev.target.closest('fieldset')
      if (elFieldset.classList.contains('collapsible')) {
        ev.preventDefault()

        elFieldset.classList.toggle('collapsed')
        if (!elFieldset.classList.contains('collapsed')) {
          collapseAllFieldsetsExcept(elFieldset)
          ev.target.scrollIntoView({ behavior: 'smooth' })
        }

        return true
      }
    }
  }

  // support both mouse and keyboard access
  $('#content').addEventListener('click', handleCollapsibleFieldset)
  $('#content').addEventListener('keypress', function (ev) {
    if (ev.key === ' ' || ev.key === 'Enter') {
      handleCollapsibleFieldset(ev)
    }
  })

  // character counts on inputs with a maxlength and associated count display element
  $$('.control input[maxlength], .control textarea[maxlength]').forEach(function (elInput) {
    const elCount = elInput.parentElement.$('.character-count')
    if (!elCount) return

    const maxLength = +elInput.maxLength
    if (!maxLength || elInput.readOnly) {
      elCount.parentNode.removeChild(elCount)
      return
    }

    function refresh () {
      const remaining = maxLength - elInput.value.length
      const isInvalid = remaining < 0

      elCount.classList.toggle('invalid', isInvalid)

      const characters = Math.abs(remaining) === 1 ? ' character' : ' characters'
      if (isInvalid) {
        elCount.textContent = 'You have ' + -remaining + characters + ' too many (out of ' + maxLength + ').'
      } else {
        elCount.textContent = 'You have ' + remaining + characters + ' remaining (out of ' + maxLength + ').'
      }
    }

    // remove the physical maxLength restriction
    elInput.removeAttribute('maxlength')
    refresh()
    elInput.addEventListener('input', refresh)
  })

  window.addEventListener('beforeunload', function (event) {
    if (DIRTY_PAGE) {
      event.preventDefault()
      event.returnValue = ''
    } else {
      setTimeout(function () {
        $('body > .loading-spinner').classList.add('enabled')
      }, 2000)
    }
  })

  /**
   * Flag that there are unsaved changes
   */
  $$('form').forEach(function (form) {
    form.addEventListener('change', function () {
      DIRTY_PAGE = true
    })
  })

  const dirtyPageSubmitHandler = function (input) {
    input.addEventListener('click', function () {
      DIRTY_PAGE = false
      $('#unsaved-changes').classList.remove('enabled')
    })
  }

  /**
   * Submitting a Form flags that there are no unsaved changes
   */
  $$('input[type="submit"]').forEach(dirtyPageSubmitHandler)
  $$('button[type="submit"]').forEach(dirtyPageSubmitHandler)

  // global click listener for handling navigation away from a dirty page.
  window.addEventListener('click', function (event) {
    // Is whatever was clicked an anchor or is it wrapped in one?
    const clickedAnchor = event.target.tagName === 'A' ? event.target : event.target.closest('a')
    // bail if the click isn't targeting an anchor else or the page isn't Dirty.
    if (
      !clickedAnchor ||
      !DIRTY_PAGE ||
      clickedAnchor.getAttribute('href').startsWith('#')
    ) return

    const continueButton = $('#unsaved-changes a.button')

    // Allow 'Continue without Saving' button to leave.
    if (event.target === continueButton) {
      DIRTY_PAGE = false
      return
    }

    // Set continue without saving button href and display the notice.
    $('#unsaved-changes a.button').setAttribute('href', clickedAnchor.href)
    $('#unsaved-changes').classList.add('enabled')
    document.body.scrollTop = document.documentElement.scrollTop = 0
    event.preventDefault()
  })
})
