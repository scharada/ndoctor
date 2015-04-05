[Parent](CalendarTestSuite.md)
# Requirements: #
[See here](Administration.md)
# Google Calendar misconfiguration: #
## Steps ##
  1. Misconfigure Google Calendar in nDoctor (For instance, set a wrong password)
  1. Insert an appointment
## Expected results ##
  1. ...
  1. A clear error message about misconfiguration should appear and the appointment should be inserted into nDoctor's database
# Manage data: #
## Steps ##
  1. Connect with a user that has write privilege
  1. Add a new appointment (without link with Google Calendar)
  1. Remove an existing appointment (without link with Google Calendar)
  1. Configure the link with Google Calendar and setup some overlapping appointments in the default Google calendar (use ndoctor.development)
  1. Add a new appointment from nDoctor (with Google Calendar link)
  1. Removed an existing appointment linked to Google Calendar (with Google Calendar link)
## Expected results ##
  1. ...
  1. The appointment is added
  1. The appointment is removed
  1. ...
  1. The appointment is added in nDoctor AND Google Calendar
  1. The appointment is removed in nDoctor AND Google Calendar
## Issues ##
### version 3.0.2: ###
  * n.a.
### version 3.0.3: ###
  * [issue 105](https://code.google.com/p/ndoctor/issues/detail?id=105)
  * [issue 133](https://code.google.com/p/ndoctor/issues/detail?id=133)
  * [issue 150](https://code.google.com/p/ndoctor/issues/detail?id=150)
  * [issue 156](https://code.google.com/p/ndoctor/issues/detail?id=156)
### version 3.0.4: ###
  * n.a.