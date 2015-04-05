[Parent](PatientSessionTestSuite.md)
# Requirements: #
[See here](Prescriptions.md)

# Manage patient's prescriptions: #

## Create prescriptions ##
### Steps ###
  1. Connect into nDoctor with user that has write privilege
  1. Open a session for a user
  1. Create a prescription.
    1. Search a drug and select it for the prescription and cancel this drug
    1. Search another drug, select it for the prescription add a comment and save.
### Expected results ###
  1. ...
  1. ...
  1. Here are the expected results:
    1. You can remove the drug from the prescription
    1. The prescription is saved.

## Create a prescription with (at least) twice the same drug ##
### Steps ###
  1. Connect into nDoctor with user that has write privilege
  1. Create a prescription with twice the same drug
  1. Click on "save"
### Expected results ###
  1. ...
  1. You can add the drugs into the GUI
  1. The two drugs are added into the prescription
## Remove a drug when creating a prescription ##
### Steps ###
  1. Create a new prescription
  1. Add a drug
  1. Click on "remove"
  1. Do it but with several added drugs
### Results ###
  1. ...
  1. ...
  1. The drug is removed from the unsaved prescription
  1. The drug is removed from the unsaved prescription
## Save prescription with different drugs of the same type ##
### Steps ###
  1. Create a prescription with different drugs of the same type
  1. Save this prescription
### Results ###
  1. ...
  1. The prescription is saved without error messages
## Issues ##
### version 3.0.2: ###
  * n.a.
### version 3.0.3: ###
  * [issue 112](https://code.google.com/p/ndoctor/issues/detail?id=112)
  * [issue 129](https://code.google.com/p/ndoctor/issues/detail?id=129)
  * [issue 155](https://code.google.com/p/ndoctor/issues/detail?id=155)
### version 3.0.4: ###
  * n.a.