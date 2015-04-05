# Introduction #

The most important part of nDoctor is its database. It contains everything about your medical practice. It is therefore VERY important to make backups on a regular basis.

You can manually save the database into a secured place but it is a lot more interesting to let Windows do it for you. Isn't it?


# Where is the database? #

The database is stored into `%APPDATA%\probel\ndoctor\Database.db`. If your user account is "johndoe", then the file is stored in  `C:\Users\johndoe\AppData\Roaming\Probel\nDoctor\Database.db`

To open the repository, the simplest is to open the "Run Window" (The shortcut is `WIN+R` and type `%APPDATA%\probel\ndoctor`

# What kind of database is it? #

The database is [SQLite](http://www.sqlite.org/) a small local database. It is well known and used in software such as Android or Firefox.

# How do I make a oneshot backup? #

You can choose between two easy ways to do it.

  * Using the file explorer:
    * Open the _file explorer_ (WIN+E) or simply click on the shortcut
    * On the address bar, type "`%APPDATA%\probel\ndoctor`"
    * Find the file "`Database.db`" and copy it in a safe place.

![https://lh5.googleusercontent.com/-6TWfwrhT5Ww/UTTwYtjCHeI/AAAAAAAAAao/l5Bvjn_64dU/s800/12.jpg](https://lh5.googleusercontent.com/-6TWfwrhT5Ww/UTTwYtjCHeI/AAAAAAAAAao/l5Bvjn_64dU/s800/12.jpg)

  * Using the command line
    * Open the "_Run Window_" (WIN+R)
    * Type "`powershell`"

![https://lh5.googleusercontent.com/-l8hSuiTNT3A/UTIvlOf-gyI/AAAAAAAAAYg/CtKPYWUHyfY/s413/1.jpg](https://lh5.googleusercontent.com/-l8hSuiTNT3A/UTIvlOf-gyI/AAAAAAAAAYg/CtKPYWUHyfY/s413/1.jpg)

  * At the command prompt, type this command: "`cp $env:APPDATA\probel\ndoctor\Database.db $env:USERPROFILE\Desktop\Database.db`" (It will copy the file into the Desktop)

![https://lh6.googleusercontent.com/-vFrbUdTGsCc/UTIvl_JwtbI/AAAAAAAAAYs/Kysagteltlo/s885/2.jpg](https://lh6.googleusercontent.com/-vFrbUdTGsCc/UTIvl_JwtbI/AAAAAAAAAYs/Kysagteltlo/s885/2.jpg)


# How to automatise the backup? #
## Write the script to make the backup. ##
  * Create a text file with _notepad_ and write these lines of code (You can change the destination, by default it copies the backup in the desktop):
```
$date = [DateTime]::Now.ToString("yyyy-MM-dd hhmm")
$source = "$env:APPDATA\probel\ndoctor\Database.db"
$destination = "$env:USERPROFILE\Desktop\Database.db"

cp $source "$destination-$date"
```
  * Save the file with this name "`ndoctorbackup.ps1`" into the place you want
## Configure `PowerShell` to allow scripts ##
  * Click on the _Start Menu_ (aka the Orb)
  * In the search box type: "_powershell_"
  * In the result, right-click on the `PowerShell` shortcut and click on "_Run as administrator_"

![https://lh6.googleusercontent.com/--WrTD7L9XMA/UTIvlf3euWI/AAAAAAAAAYI/c0prHuzysQw/s520/10.jpg](https://lh6.googleusercontent.com/--WrTD7L9XMA/UTIvlf3euWI/AAAAAAAAAYI/c0prHuzysQw/s520/10.jpg)

  * At the command prompt type: `Set-ExecutionPolicy RemoteSigned`
  * When asked, type "Y" to confirm scripts can be executed

![https://lh6.googleusercontent.com/-rViPo79sQ-Y/UTIvlcK2WcI/AAAAAAAAAYY/L20fB_mqiO8/s997/11.jpg](https://lh6.googleusercontent.com/-rViPo79sQ-Y/UTIvlcK2WcI/AAAAAAAAAYY/L20fB_mqiO8/s997/11.jpg)

  * Close the prompt

## Configure a scheduled task ##
  * Open the "_Start Menu_", right-click on "_Computer_" and select "_Manage_"
  * Select "_Create Basic Task_"

![https://lh4.googleusercontent.com/-MYStJbN9Em8/UTIvm1XWTRI/AAAAAAAAAZY/hyjskwVL2nQ/s1000/3.jpg](https://lh4.googleusercontent.com/-MYStJbN9Em8/UTIvm1XWTRI/AAAAAAAAAZY/hyjskwVL2nQ/s1000/3.jpg)

  * Give a name to the task ("_nDcotor backup_" is a good choice) and click on "_Next_". You can leave the explanation empty or specify a description

![https://lh6.googleusercontent.com/-t67zJcyNsj4/UTIvmV9iMZI/AAAAAAAAAYk/T1EJf_hyyzU/s710/4.jpg](https://lh6.googleusercontent.com/-t67zJcyNsj4/UTIvmV9iMZI/AAAAAAAAAYk/T1EJf_hyyzU/s710/4.jpg)

  * Select the regular basis for the backup and click on "_Next_"

![https://lh3.googleusercontent.com/-tyoM-VK-ZA0/UTIvmsHuqyI/AAAAAAAAAY8/mDNy-1Uf0zg/s710/5.jpg](https://lh3.googleusercontent.com/-tyoM-VK-ZA0/UTIvmsHuqyI/AAAAAAAAAY8/mDNy-1Uf0zg/s710/5.jpg)

  * Specify the date of the first occurence (Today is a good choice) and click on "_Next_"

![https://lh6.googleusercontent.com/-OkyH4_sQ1wo/UTIvnZw-2DI/AAAAAAAAAZI/CI5Af4paQJ8/s710/6.jpg](https://lh6.googleusercontent.com/-OkyH4_sQ1wo/UTIvnZw-2DI/AAAAAAAAAZI/CI5Af4paQJ8/s710/6.jpg)

  * Select "_Start a progam_" and click on "_Next_"

![https://lh6.googleusercontent.com/-FvTc5SDwXOc/UTIvnWsCzII/AAAAAAAAAZM/Rv96h7vceSw/s710/7.jpg](https://lh6.googleusercontent.com/-FvTc5SDwXOc/UTIvnWsCzII/AAAAAAAAAZM/Rv96h7vceSw/s710/7.jpg)

  * On the text box "_Program/script_" write: "_powershell.exe_" (Point 1)
  * On the text box "_Add arguments (optional)_" specify the path of the created script (For example: "C:\Path\_To\_The\_File\ndoctorbackup.ps1". (Point 2)
  * Click on "_Next_"

![https://lh5.googleusercontent.com/-41PSpMTitbI/UTIvnzkjZ_I/AAAAAAAAAZU/uZiqU1ZUiIE/s710/8.jpg](https://lh5.googleusercontent.com/-41PSpMTitbI/UTIvnzkjZ_I/AAAAAAAAAZU/uZiqU1ZUiIE/s710/8.jpg)

  * Click on "_Finish_"

![https://lh6.googleusercontent.com/-B15vIW7oYw0/UTIvoamKqHI/AAAAAAAAAZQ/CesU47cnJpw/s710/9.jpg](https://lh6.googleusercontent.com/-B15vIW7oYw0/UTIvoamKqHI/AAAAAAAAAZQ/CesU47cnJpw/s710/9.jpg)