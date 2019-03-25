const { dataProvider } = require('catalogue-data')

module.exports = {
  'registration': {
    'solution.version': {
      'trim': {},
      'in': 'body',
      'isLength': {
        'errorMessage': 'Validation.Solution.Version.TooLong',
        'options': {
          'max': 10
        }
      }
    },
    'solution.description': {
      'isEmpty': {
        'negated': true,
        'errorMessage': 'Validation.Solution.Description.Missing'
      },
      'trim': {},
      'blacklist': {
        options: '\\r'
      },
      'isLength': {
        'errorMessage': 'Validation.Solution.Description.TooLong',
        'options': {
          'max': 300
        }
      },
      'in': 'body'
    },
    'solution.name': {
      'isLength': {
        'options': {
          'max': 60
        },
        'errorMessage': 'Validation.Solution.Name.TooLong'
      },
      'in': 'body',
      'isEmpty': {
        'errorMessage': 'Validation.Solution.Name.Missing',
        'negated': true
      },
      'custom': {
        'options': (value, { req }) => {
          return dataProvider.validateSolutionUniqueness({
            id: req.params.solution_id === 'new' ? undefined : req.params.solution_id,
            name: value,
            version: req.body.solution.version,
            organisationId: req.user.org.id
          }).then(valid => {
            if (!valid) return Promise.reject('Validation.Solution.Name.Duplicate')
          })
        }
      },
      'trim': {}
    },
    'solution.contacts.*.id': {
      'in': 'body'
    },
    'solution.contacts.*.contactType': {
      'in': 'body',
      'trim': {},
      'isEmpty': {
        'errorMessage': 'Validation.Solution.Contacts.ContactType.Missing',
        'negated': true
      },
      'isLength': {
        'options': {
          'max': 100
        },
        'errorMessage': 'Validation.Solution.Contacts.ContactType.TooLong'
      }
    },
    'solution.contacts.*.firstName': {
      'in': 'body',
      'trim': {},
      'isEmpty': {
        'errorMessage': 'Validation.Solution.Contacts.FirstName.Missing',
        'negated': true
      },
      'isLength': {
        'options': {
          'max': 80
        },
        'errorMessage': 'Validation.Solution.Contacts.FirstName.TooLong'
      }
    },
    'solution.contacts.*.lastName': {
      'in': 'body',
      'trim': {},
      'isEmpty': {
        'errorMessage': 'Validation.Solution.Contacts.LastName.Missing',
        'negated': true
      },
      'isLength': {
        'options': {
          'max': 80
        },
        'errorMessage': 'Validation.Solution.Contacts.LastName.TooLong'
      }
    },
    'solution.contacts.*.emailAddress': {
      'in': 'body',
      'trim': {},
      'isEmpty': {
        'errorMessage': 'Validation.Solution.Contacts.Email.Missing',
        'negated': true
      },
      'isLength': {
        'options': {
          'max': 100
        },
        'errorMessage': 'Validation.Solution.Contacts.Email.TooLong'
      },
      'isEmail': {
        'errorMessage': 'Validation.Solution.Contacts.Email.Format'
      }
    },
    'solution.contacts.*.phoneNumber': {
      'in': 'body',
      'trim': {},
      'isEmpty': {
        'errorMessage': 'Validation.Solution.Contacts.Phone.Missing',
        'negated': true
      },
      'isLength': {
        'options': {
          'max': 88
        },
        'errorMessage': 'Validation.Solution.Contacts.Phone.TooLong'
      }
    }
  },
  'capabilities': {
    'capabilities': {
      'in': 'body',
      'isEmpty': {
        'negated': true,
        'errorMessage': 'Select at least one capability to continue'
      }
    }
  }
}
