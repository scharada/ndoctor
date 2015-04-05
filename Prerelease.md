# Introduction #

Here, you can download and test the alpha versions of nDoctor.

Bear in mind that these versions are not tested yet!

These versions are under heavy development. I'm adding new features and changing the database structure and it possible that the new security mechanisms to check that no data is lost are not implemented at this level.

If you are a daredevil and you have made a backup of your production database, go ahead ;) Otherwise, wait for the official release!

**I advise you to make a backup of your database before doing anything!**

# How to make a backup of your database? #

The database is in `%APPDATA%\probel\ndoctor\Database.db`

You can make a backup using:
  * the file explorer:
    * Open the file explorer (WIN+E)
    * In the address bar type `%APPDATA%\probel\ndoctor\`
    * Copy the file `Database.db` in a safe place.
  * the command line:
    * Open the Run window (WIN+R)
    * Type "`powershell`"  in the text box then press `ENTER`
    * At the prompt type: `cp "$ENV:APPDATA\probel\ndoctor\Database.db" "$ENV:HOMEPATH\Desktop\Database.db - backup"`
      * The quotes are important!
      * This command will copy the database in your desktop.


# Where to download the files? #

The repository is [HERE](http://download.tuxfamily.org/phoenixsuite/nDoctorTestRelease/)