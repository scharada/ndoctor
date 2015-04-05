[Parent](UserTestSuite.md)
# First user creation #
## Steps ##
  1. Delete the file %appdata%\Probel\nDoctor\Database.db
  1. Create the administrator but don't select it as default user
  1. Select the user you've created, enter the password and click on **GO**
  1. Go to the "Main menu\Authorisation" and click on "Manage users"
  1. Go to the "Main menu\Authorisation" and right-click on the only user and delete it
  1. Go to the "Main menu\Authorisation" and set the super admin as a "Secretary" and disconnect/reconnect this user
## Expected results ##
  1. ...
  1. After creation, you're in the connection view
  1. You enter in the main window, the session group is greyed out.
  1. There's only one user and the "Super admin" check box is checked for this user
  1. A warning message appear saying you can't delete this user
  1. Because this user is super admin, he/she has all the authorisation. No matter the specified role
## Issues ##
### version 3.0.2: ###
  * [issue 41](https://code.google.com/p/ndoctor/issues/detail?id=41)
  * [issue 42](https://code.google.com/p/ndoctor/issues/detail?id=42)

### version 3.0.3: ###
  * [issue 104](https://code.google.com/p/ndoctor/issues/detail?id=104)

### version 3.0.4: ###