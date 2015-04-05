# Introduction #

All the coding conventions are listed here. Most of these rules are automatically managed with `nArrange`. This is here fyi.

## Guidelines: ##

Follow the [Microsoft guidelines](http://msdn.microsoft.com/en-us/library/czefa0ke(v=vs.71))

## Header on each file: ##
Each `*.cs` file should have this header:
```
/*
    This file is part of NDoctor.

    NDoctor is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    NDoctor is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with NDoctor.  If not, see <http://www.gnu.org/licenses/>.
*/
```

Use the plugin [License Header Manager](http://visualstudiogallery.msdn.microsoft.com/5647a099-77c9-4a49-91c3-94001828e99e?SRC=VSIDE) for that purpose.

## `using` directive: ##
The `namespaces` should be ordered alphabetically. Each group should be separated by a line.

Here's an example:
```
using System;
using System.Windows.Input;

using Probel.Helpers.WPF;
using Probel.Mvvm.DataBinding;
using Probel.NDoctor.Domain.DTO;
using Probel.NDoctor.Domain.DTO.Collections;
using Probel.NDoctor.Domain.DTO.Helpers;
using Probel.NDoctor.Plugins.Administration.Helpers;
```

## Class: ##
You should wrap each member group into a `#region`. The `#region` should appear in this order:
  1. Fields
  1. Constructors
  1. Enumeration
  1. Events
  1. Properties
  1. Methods
  1. Nested Types

Every member should be ordered:
  * by visibility (public, internal, protected, private)
  * alphabetically,

## Member: ##
### General ###
  * Never prefix '`_`' (underscore) class wide variable or suffix '`_`' (underscore) method wide variables. Instead use the keyword `this`. If a variable is prefixed with `this`, it is a class variable otherwise a method variable.
  * Use as much as possible the keyword **`var`**. It'll force you to find better variable name and it eases refactoring.
```
    public class DummyCalculator
    {
        private int offset = 15;

        public DummyCalculator(int offset)
        {
            this.offset = offset;
        }

        public int OffsetAdd(int right, int left)
        {
            var result = right + left;
            return result + this.offset;
        }
    }
```
### Pascal case and camel case: ###
  * Properties, const, readonly members are in pascal case.
```
public int PascalCaseMethod() { return 1 + 1; }
```
  * Class wide and method wide variable are in camel case.
```
private int camelCaseVariable;
```

### Spacing conventions: ###
Bear in mind Visual Studio has automatic reordering feature: **`Ctrl+E, D`**. Keep default configuration.

  * Put a space before and after any operator
  * Put a space after the coma not before
  * Put a space after and before the curly braces if they are on the same line
```
i = i + 5;

if (number == 4) { number += 150; }

public void AcmeMethod(int arg, string text) { text += arg.ToString(); }
```

### Curly brace conventions: ###
Bear in mind Visual Studio has automatic reordering feature: **`Ctrl+E, D`**. Keep default configuration.

  * Always put the curly braces.
  * If the if statement is really short write it as follow:
```
if( this.IsTrue) { this.DoSomeWork(); }
else { this.DoOtherWork();  }
```

# Components: #

While writing a component, developer have to follow these rules:

  * All the components are suffixed with "**`Component`**". For instance, the medical record's component will be called `MedicalRecordComponent`.
  * A methods to retrieve all the entities of a type will be prefixed with **`GetAll`**. A methods to get all the patients will be called `GetAllPatients()`.
  * A method to find entities based on criteria should be prefixed with **`Get`** and suffixed with "**`By...`**" which translates on what the search will be done.
  * A DTO object is suffixed with `Dto`. For example a patient dto is called `PatientDto`

# Gui- Xaml, Mvvm: #
  * When defining a ICommand:
    * the name of the command is suffixed with **Command**
    * prefix the check method with a **Can**
    * the action performed by the command is the name of the command without the suffix **Command**
```
this.DrinkABeerCommand = new RelayCommand(() => this.DrinkABeer(), () => this.CanDrinkABeer());
```
  * If you need static events in your plugin, define them into a static class called `Notifyer`
  * The translation system uses static class. This class should have the name of the `ViewModel` class. Replace the suffix `ViewModel` with `Text`. If the ViewModel is `AddPatientViewModel` then the translation static class is `AddPatientText`
  * A plugin solution should written on that pattern: `Probel.NDoctor.Plugins.NameOfThePlugin`
  * A `ViewModel` class derives from `BaseViewModel`. This base class is in charge of error handling and logging.
  * Every DTO derives from `Probel.Mvvm.BaseDto<T>`.
  * If you need to implement [INotifyPropertyChanged](http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged.aspx) and `BaseDto<T>` doesn't fit your need, use `ValidatableObject` but **don't** reimplement INPC yourself.

# Authorisation management: #
The authorisation is build on the pattern [Convention over configuration](http://en.wikipedia.org/wiki/Convention_over_configuration).

Without the attribute `GrantedAttribute` the authorisation is set as follow:
  * Every method prefixed with **`Get`** or **`GetAll`** behaves as if they were decorated with `[Granted("Read")]`
  * Every method prefixed with **`Create`**, **`Remove`** or **`Update`** behaves as if they were decorated with `[Granted("Write")]`
  * Not decorated method are considered as granted to everyone
  * Methods that are decorated with `InspectionIgnoredAttribute` are considered as granted to everyone