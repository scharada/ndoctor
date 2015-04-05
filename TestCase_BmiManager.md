[Parent](PatientSessionTestSuite.md)
# Requirements: #
[See here](BMI.md)

# Manage Bmi history: #
## Steps ##
  1. Connect with a user that is granted to write
  1. Create a new Bmi entry (add height and weight for the patient)
  1. Remove the created Bmi entry
  1. Create a new Bmi
  1. Create a new Bmi with a date in the future
  1. Create a new Bmi with today as the date
  1. Create a new Bmi with a date that already exist in the database
## Expected results ##
  1. ...
  1. The entry is inserted and the graphs are updated
  1. The entry is removed and the graphs are updated
  1. The entry is inserted and the graphs are updated. And the previously height is already set in the box
  1. If the date is in the future, the "Add" button is greyed out
  1. The entry is inserted and the graphs are updated. And the previously
  1. The entry is inserted and the graphs are updated.
## Issues ##
### version 3.0.2: ###
  * n.a.
### version 3.0.3: ###
  * n.a.
### version 3.0.4: ###
  * n.a.