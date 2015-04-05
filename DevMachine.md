# Configure the development machine #
Before starting development, you have to configure your machine as follow:

## Allow `PowerShell` to execute local scripts ##

  1. Open a `PowerShell` session as administrator
  1. Type this comment `Set-ExecutionPolicy RemoteSigned`
  1. Follow the instructions

## Configure Visual Studio to be able to execute the script that copies the plugin files ##

To add this tool in Visual Studio follow these steps:
  1. In Visual Studion go to Tools\External Tools
  1. Fill the window as follow:
    1. Title            : cp plugins (or whatever name you want)
    1. Command          : `powershell.exe ` (if you haven't configured the environment variables, type the full path)
    1. Arguments        : `-ExecutionPolicy RemoteSigned -File "[working copy]\tools\scripts\CopyPluginsFiles.ps1" debug` (replace _debug_ with _release_ if you compile in release mode)

## Checkout the code: ##

  1. Ask the `write` privilege to the project owner sending him a mail to **`ndoctor(dot)software(at)gmail(dot)com`** explaining why you'd like to be part of the team.
  1. Check out the code at this address `https://ndoctor.googlecode.com/svn/trunk/`

## Add code snippets ##
Copy the `*.snippet` files
  * from: `[workingcopy]`\tools\Snippets
  * to  : `%userprofile%\My Documents\Visual Studio 2010\Code Snippets\Visual C#\My Code Snippets`

## Import the plugin project template: ##
Drop the file
  * `[working copy]\tools\Probel.NDoctor.Plugins.Plugin.zip`
  * into :`%userprofile%\My Documents\Visual Studio 2010\Templates\ProjectTemplates\Visual C#\`

## Configure TortoiseSVN: ##

  1. Configure SubWCRev for automatic versionning based on SVN revision number:
    1. Install TortoiseSVN
    1. Set SubWCRev path in the environment variable
      1. `Windows+Break`
      1. Click on "Change Settings"
      1. Click on "Environment Variables"
      1. Edit PATH (On my machine I add "`C:\Program Files\TortoiseSVN\bin`"

## Configure nArrange: ##

nArrange is a little tool to beautify the code. I plan to trigger it every time nAnt build file is executed.

To add this tool in Visual Studio follow these steps:
  1. In Visual Studion go to Tools\External Tools
  1. Fill the windows as follow:
    1. Title            : nArrange (or whatever name you want)
    1. Command          : `[workingcopy]\tools\NArrange\narrange-console.exe`
    1. Arguments        : `$(SolutionFileName) /b`
    1. Initial directory: `$(SolutionDir)`

## Configure nAnt: ##
  1. Download [nAnt](http://nant.sourceforge.net/) and [nAnt contrib](http://nantcontrib.sourceforge.net/).
  1. Copy the downoaded files into `%programfiles%\nAnt` (or whatever directory) and copy the nAnt Contrib files from `bin` directory into the **`bin`** nAnt directory you've created.
  1. Update the Environment Variables to allow nAnt.exe to be accessible from everywhere
  1. Configure Visual Studio to work with build files.
    * [FAQ](https://github.com/nant/nant/wiki/Frequently-Asked-Questions)
    * [How to use \*.xsd for IntelliSense](http://stackoverflow.com/questions/5157041/how-to-make-intellisense-work-with-nant-files-in-visual-studio-2008)

To add this tool in Visual Studio follow these steps:
  1. In Visual Studion go to Tools\External Tools
  1. Fill the window as follow:
    1. Title            : nAnt(or whatever name you want)
    1. Command          : `nAnt.exe` (if you haven't configured the environment variables, type the full path)
    1. Arguments        : `-buildfile:nDoctor.build.xml -D:build-mode=debug`
    1. Initial directory: `$(SolutionDir)\..\tools`

## Configure WiX to work with the nAnt script ##
  1. Create an environment variable
    1. Win+Break
    1. Go to "Advanced System Settings
    1. Click on Environment Variable
    1. In "System Variables", click on "New..."
    1. In "Variable name", put NEST
    1. In "Variable value", put the path of the nest ("%RELEASE\_DIRECTORY%\Nest\nDoctor")