# Introduction #

The basic GUI of nDoctor should resemble to this:

![https://docs.google.com/drawings/pub?id=16q9kqDSYWS153EoiLeCXgO8G4xI8b0xxJWUj9t4S2y4&w=640&h=480&nonsense=something_that_ends_with.png](https://docs.google.com/drawings/pub?id=16q9kqDSYWS153EoiLeCXgO8G4xI8b0xxJWUj9t4S2y4&w=640&h=480&nonsense=something_that_ends_with.png)

# Concepts #

The GUI is divided into different parts
  1. The main menu: It contains all the application wide menu such as:
    1. Home: Contains a menu group to manage a patient session and another menu group to manage the patient's managers
    1. [`Deprecated: All the items that should be in this menu are moved in the global menu.`] Tools: Contains tools to improve the user experience of nDoctor.
    1. Help: Contains help information about nDoctor.
  1. The contextual menu: this menu changes depending on the selected workbench.
  1. The status: Provide non interactive logs. That's, information about the state of the application.
  1. Workbench: it is the workbench of the selected item of the selected module. Here you manage the data of nDoctor.
  1. The application has only one **Message Box** at a time. This is for two reason.
    1. Usability: the user don't have to juggle between too many windows.
    1. Simpleness: the development stays easy.