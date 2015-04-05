[Parent](PatientSessionTestSuite.md)
# Requirements: #
[See here](MedicalRecord.md)

# Manage patient's medical records: #
## Steps ##
  1. Connect into nDoctor with user that has write privileges
  1. Load a patient
  1. Go to the medical record manager and create a new directory.
  1. Go to the medical record manager and create a new record for the new directory
  1. Edit the text and load a new medical record
  1. Create a new macro and use it
  1. Reload a previous revision
  1. Change the default font and size in the settings and create a new record
## Expected results ##
  1. ...
  1. ...
  1. Nothing is updated, you'll see the new repository in the record creation box.
  1. The modifications are saved.
  1. When you reload the first patient, you see the modified text was saved as expected.
  1. When you reload the first patient, you see the modified text was saved as expected.
  1. The macro is created and an be inserted into the medial record
  1. The new default (font and size) are used as default.

# Create new medical records: #
## Steps ##
  1. Create a new record and put some text
  1. Don't save and create a new record (with as few clicks as possible)
  1. Edit the text and go directly to the search box to load a new patient
## Expected results ##
  1. ...
  1. The unsaved text is saved
  1. The unsaved text is saved
# Use and manage macros #
## Steps ##
  1. Open the macro editor
  1. Create a macro with an unknown markup (For instance write **$unknown$**)
  1. Click on close
## Expected results ##
  1. ...
  1. ...
  1. A warning message indicates the macro is not created because it is invalid
# Issues #
## version 3.0.2: ##
  * n.a.
## version 3.0.3: ##
  * [issue 111](https://code.google.com/p/ndoctor/issues/detail?id=111).
  * [issue 123](https://code.google.com/p/ndoctor/issues/detail?id=123).
  * [issue 125](https://code.google.com/p/ndoctor/issues/detail?id=125).
  * [issue 126](https://code.google.com/p/ndoctor/issues/detail?id=126).
  * [issue 127](https://code.google.com/p/ndoctor/issues/detail?id=127).
  * [issue 130](https://code.google.com/p/ndoctor/issues/detail?id=130).
  * [issue 148](https://code.google.com/p/ndoctor/issues/detail?id=148).
## Version 3.0.4: ##
  * [issue 162](https://code.google.com/p/ndoctor/issues/detail?id=162).
  * [issue 200](https://code.google.com/p/ndoctor/issues/detail?id=200).