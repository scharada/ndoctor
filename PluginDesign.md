# Plugins #

The plugin module has these roles
  * It loads the plugins from the repository.
  * It checks whether the module has the correct version to be loaded.
  * It offers a system where the user can disable some plugins he/she doesn't need


The plugin repository is a directory named _Plugins_ in the application root. At the root of this directory there's a XML file which has the information about which plugin is activated and the name of the plugins. Each plugin has its own directory that contains the plugin dll and all the needed files.

`To be completed and analysed:` Maybe install the plugins into _%programfiles%_ needs administrator privileges and could be a security issue.

## Remark ##

These explanations are only about the plugin loading and validation. The interface `IPluginHost` exposes an API the plugins can use. This API is explained [here](PluginFeatures.md).

## Class diagrams ##
![https://lh3.googleusercontent.com/-C9NappHLIHs/UQZztFYwPVI/AAAAAAAAAUM/XgePfhKDF68/s661/Plugins%2520design.jpg](https://lh3.googleusercontent.com/-C9NappHLIHs/UQZztFYwPVI/AAAAAAAAAUM/XgePfhKDF68/s661/Plugins%2520design.jpg)
## Sequence diagrams ##
![https://lh5.googleusercontent.com/-zBpc_6zrQNQ/UQZ1FfK9DzI/AAAAAAAAAUs/wx-XDdHlaxQ/s576/Load%2520plugins.jpg](https://lh5.googleusercontent.com/-zBpc_6zrQNQ/UQZ1FfK9DzI/AAAAAAAAAUs/wx-XDdHlaxQ/s576/Load%2520plugins.jpg)
## Visual studio diagram ##
https://lh6.googleusercontent.com/-f2_BWu1d294/UQZxKzWU0kI/AAAAAAAAATs/4aNuukWcJT4/s720/VSPlugin.JPG