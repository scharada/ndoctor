# Introduction #

This feature provides some tools for debugging purpose. A doctor doesn't need these tools to work efficiently with nDoctor. This is why this functionality is hidden by default.

# How to activate the features? #

You can start nDoctor with command line argument to help production debugging.
  * **-hookconsole**: Hook a console to the application to display the logging
  * -**debugtools**: Add statistics and log menus

If you're not a `Command line lad`, just edit the shortcut. To do so, right click on the shortcut and click on _Properties_

![https://lh6.googleusercontent.com/-7cuCoNu6DYk/US-93_hH0ZI/AAAAAAAAAWc/vyUbf5Q94So/s533/3.jpg](https://lh6.googleusercontent.com/-7cuCoNu6DYk/US-93_hH0ZI/AAAAAAAAAWc/vyUbf5Q94So/s533/3.jpg)

## The list of tools ##

Most of you don't really care about this information. But, I'm sure, most of you are curious and this is why I've documented these features ;)
### Session logs ###

Logging is very important for an application. You can find and fix bugs faster and understand what is happening under the hood.

nDoctor logs into files stored in `%APPDATA%\Probel\nDcotor\Logs` but sometimes, I just need the logs of the current session, this is why this tool exists. It just shows the log that was written since you've started the application.

![https://lh4.googleusercontent.com/-54YvmWBEtG4/US-9xOqAc3I/AAAAAAAAAWQ/aN8DYuwEFgo/s1024/2.jpg](https://lh4.googleusercontent.com/-54YvmWBEtG4/US-9xOqAc3I/AAAAAAAAAWQ/aN8DYuwEFgo/s1024/2.jpg)

### Statistics ###

This data is used to highlight which components are critical (that's used very often) and where are the slowness of nDoctor.

This provides:
  * Chart _Average usage by components_: this shows how many times each component has been used.
  * Chart _Average execution time (msec)_: this is the average time (in milliseconds) each feature took to be executed
  * The list of _Bottlenecks_: this is the list of features that needed too much time to be executed



![https://lh3.googleusercontent.com/-UQ6SqtaChCI/US-9xIRz5aI/AAAAAAAAAWM/VF7Qd9MLStg/s1024/1.jpg](https://lh3.googleusercontent.com/-UQ6SqtaChCI/US-9xIRz5aI/AAAAAAAAAWM/VF7Qd9MLStg/s1024/1.jpg)