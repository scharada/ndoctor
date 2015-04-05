# User requirements #

When the application user has logged into the application, it opens a _User session_ that contains the modules the logged user can use.

A user is defined as a person that can log into nDoctor and manipulate its data refering the privileges he/she has been granted to.

# Super admin #

The first created user:
  * is a super administrator and can execute everything in nDoctor.
  * is undeletable
  * can't recover a lost password without a separate tweaking tool.

# Authorisation management #

For further information about authorisation management, go on [this page](RightManager.md) to see requirements.

For further information, go on [this page](ComponentAop#Authorisation.md) to see the technical documentation.

# Details #

It should contain:

  * a menu to change the user's information. This information is:
    * User name
    * User surname
    * The header that will be used in the printable documents **(Not printable)**
    * The default fee the patient should pay for a meeting **(Not printable)**
    * The address of the user's practice
    * The phone number
    * The mobile phone number
    * The email