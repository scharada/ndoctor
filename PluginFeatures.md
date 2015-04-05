# Introduction #

Here you'll find all the explanations about how to use the Plugin API


# Features #
## Logging ##

The plugin host DLL offers a base class that allows logging. If you need a class with logging enabled, derives your class from `LogObject`

## Error handling ##

The plugin host DLL offers a error handler to manage exception. Its role is pretty simple it:
  * displays an error message in a message box
  * logs the exception in a file or in whatever way you've configured log4net

## Plugin configuration ##

There's a simple mechanism to add the configuration of a plugin into a general GUI. To add configuration, follow these steps
  1. Create a `Page` that contains the configuration
  1. Create a `ViewModel` and bind it to the `Page`
  1. Bind the configuration to the application with the `SettingsConfigurator`:
```
new SettingsConfigurator().Add("Title", () => new SettingsView());
```
    1. The first argument is the title of the `Page` as it will be displayed in the GUI
    1. The second argument is a lambda that will return an instance of the setting GUI item

## Plugin context ##
This plugin context provides an small API to execute some generic tasks:
  * Logging into the GUI
  * Create left pane dynamically
  * Checking authorisation
  * Build plugin's ribbon menu using `DataBinding`

### View Service ###

#### Window manager ####

This service offers a simple mechanism to manage windows into the application.

The first thing to to is to let the manager knows about the link between the VIew and the ViewModel and configure (if necessary) the action to execute before showing and/or when closing the window

```
ViewService.Configure( e =>
{
   e.Bind<SomeView, SomeViewModel>()
      .OnShow(vm => vm.ActionFromViewModel())     //Action to execute before showing the window
      .OnClosing(vm => vm.ActionFromViewModel()); //Action to execute before closing the window
});
```

The next step is to show the Window:
```
//Show a modal window
ViewService.Manager.ShowDialog<SomeViewModel>()
ViewService.Manager.ShowDialog<SomeViewModel>(vm => vm.ActionFromViewModel());

//Show a non modal window
ViewService.Manager.Show<SomeViewModel>()
ViewService.Manager.Show<SomeViewModel>(vm => vm.ActionFromViewModel());
```

You can close the opened window from the `ViewModel` if it implements the interface `IRequestCloseViewModel`. The framework provides a basic implementation of this interface with the class `RequestCloseViewModel`

#### Message boxes ####

The `ViewService` has a property `MessageBox` that provides a easy way to use predefined message boxes.

Here's the different message boxes:
```
void Asterisk(string message);
bool? CancelableQuestion(string message);
void Error(string message);
void Exclamation(string message);
void Hand(string message);
void Information(string message);
void None(string message, string title);
bool Question(string message);
void Stop(string message);
void Warning(string message);
```

### Door Keeper ###
The door keeper is a module that checks the authorisation on demand. It keep tracks on who's connected and says whether this connected user can execute or not the specified task. For instance this code checks whether the connected user can execute a _write_ action:

```
bool isGranted = PluginContext.Host.DoorKeeper.IsGranted("write")
```

### Avalon Dock ###
nDoctor is build with the ability to show a hideable left panel. To add a new left panel use this code:

```
PluginContext.Host.AddDockablePane("Title of the pane", new CustomUserControl())
```

### Logging in status bar ###
You can display non interactive logging into the status bar of nDoctor. A log can be an error, a warning or information. Depending of the Status type, a different icon is displayed just before the message in the status bar. This code is used to log:

```
PluginContext.Host.WriteStatus(statusType.Warning, "Mind the step!");
```

`StatusType` can be:
  * Error
  * Warning
  * Info

### Build ribbon menu with data binding ###
[`Check the interface IPluginContext, the method's names are explicit`]