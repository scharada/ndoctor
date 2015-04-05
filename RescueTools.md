# Activate the **Rescue Tools** plugin #
  * Go to the **Application menu** => **Settings**
![https://lh6.googleusercontent.com/-RPtadVpLcbc/US-1LoRk9TI/AAAAAAAAAVQ/LcLsm8y9HBg/s520/1.jpg](https://lh6.googleusercontent.com/-RPtadVpLcbc/US-1LoRk9TI/AAAAAAAAAVQ/LcLsm8y9HBg/s520/1.jpg)

  * Activate the plugin
![https://lh6.googleusercontent.com/-X4pCYpgVb3c/US-1Li7Qm4I/AAAAAAAAAVI/ZEOXz4TQZcM/s600/2.jpg](https://lh6.googleusercontent.com/-X4pCYpgVb3c/US-1Li7Qm4I/AAAAAAAAAVI/ZEOXz4TQZcM/s600/2.jpg)

  * Restart nDoctor

  * Open the **Rescue tools**: **Application menu** => **Database management**
![https://lh5.googleusercontent.com/-deeVKg9TO-w/US-4P4Wm5pI/AAAAAAAAAVo/rqrFWylfuVg/s512/3.jpg](https://lh5.googleusercontent.com/-deeVKg9TO-w/US-4P4Wm5pI/AAAAAAAAAVo/rqrFWylfuVg/s512/3.jpg)
## Patient doubloons ##
This is a doctor that is inserted multiple times in the database. For nDoctor such doctors are different event if, from the business point of view they are the same.

This tool will try to spot doubloons flowing this rule: two doctors are suspected to be the same when their name, surname and specialisation have the same spelling (no matter the case).

## How does it work? ##
Here is the user interface of the plugin:

![https://lh3.googleusercontent.com/-XQ6IlA-ulwg/US-4PmsxVeI/AAAAAAAAAVs/y881mZJJEW4/s919/4.jpg](https://lh3.googleusercontent.com/-XQ6IlA-ulwg/US-4PmsxVeI/AAAAAAAAAVs/y881mZJJEW4/s919/4.jpg)

To find and fix the doctor doubloons:
  1. Select the doubloons finder tab (**Point 1**)
  1. Click on _Find doubloons_ (**Point 4**)
  1. You'll see the list of doctors with doubloons in the list view (**Point 2**)
  1. In the _Kept doctor_ list, select the doctor you want to keep and click on _Replace_ **OR** click on _Replace with first doubloon_ (That's the first doctor in the _list_ will be kept. (**Point 3**)

## When I click on _Replace_ or _Replace with first doubloon_ what does happen? ##
All the doubloons will be deleted but the selected one and all the links to the deleted doubloon doctors will be replaced by the selected one.

**Don't worry, no data is lost!** Thanks to this tools you'll recover business loss. That's nDoctor now understand that all these doctors with the same name, surname and specialisation are the same one.