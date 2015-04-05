# Troubleshooting: #
Here's the list of some problem you could encounter when installing a development machine.

## I've made my first checkout and I have compilation errors: `"The name "RibbonWindow" does not exist in the namespace "clr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary""` ##


Just [download](http://www.microsoft.com/download/en/details.aspx?id=11877) and install the Ribbon Library

## nAnt is crashing displaying a security error message: ##
You have to create yourself the `nAnt` directory into `%programfiles%` and copy/paste the nAnt files into the freshly created directory. In this way, Windows 7 doesn't believe these files are untrusted internet files.

## Castle Dynamic Proxy: ##
For an unknown reason, this librarie wasn't recognised anymore when I made a fresh checkout on a new machine.
To fix it I:
  1. Edit the file `packages.config` of the project `Probel.NDoctor.Domain.Components`
  1. Removed the entry `<package id="Castle.Core" version="3.0.0.4001" />`
  1. Right clicked on `Reference` directory on this project
  1. Reinstalled the `Castle Dynamic Proxy` with NuGet

## AnkhSVN is not working with Visual Studio: ##
You just need to enable SVN as source control as explained [here](http://stackoverflow.com/questions/3869404/ankhsvn-not-integrated-to-visual-studio-2010)

## Windows 7 UAC always ask me to grant execution of Pencil or Everything ##
[Here yo can find how to solve that problem](http://www.microsofttranslator.com/translate_url?doit=done&tt=url&intl=1&fr=bf-home&trurl=http://www.helmrohr.de/Guides/Vista/TaskCreate.htm&lp=de_en&btnTrUrl=%C3%9Cbersetzen)

### Additional settings for Everything ###

  1. In the "Edit action" box, add _-startup_ as argument
  1. Create a trigger _At startup_