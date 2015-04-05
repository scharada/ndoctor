[Parent](AdministrationTestSuite.md)
# Authorisation management #
## Requirements ##
[See here](RightManager.md)
## Steps ##
  1. Create an Administrator and connect into nDoctor
  1. Create a Doctor and connect into nDoctor
  1. Create a Secretary and connect into nDoctor

## Expected results ##
  1. All the feature are available
  1. Only the administrative features aren't available. That's:
    * Import data
    * Authorisation
    * Settings
    * Add a new user
  1. The result should be:
    * All the "Save" features are deactivated but the patient data
    * All the "Add ..." features are deactivated
    * User can add/remove appointments
    * User can't import data or manage authorisations
    * User can modify his/her password and personal data

## Issues ##
### version 3.0.2: ###
  * [issue 46](https://code.google.com/p/ndoctor/issues/detail?id=46)
  * [issue 47](https://code.google.com/p/ndoctor/issues/detail?id=47)

### version 3.0.3: ###
  * [issue 106](https://code.google.com/p/ndoctor/issues/detail?id=106)
  * [issue 107](https://code.google.com/p/ndoctor/issues/detail?id=107)
  * [issue 108](https://code.google.com/p/ndoctor/issues/detail?id=108)
  * [issue 109](https://code.google.com/p/ndoctor/issues/detail?id=109)
  * [issue 110](https://code.google.com/p/ndoctor/issues/detail?id=110)
  * [issue 123](https://code.google.com/p/ndoctor/issues/detail?id=123)
  * [issue 135](https://code.google.com/p/ndoctor/issues/detail?id=135)
  * [issue 136](https://code.google.com/p/ndoctor/issues/detail?id=136)