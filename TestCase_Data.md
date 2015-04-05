[Parent](AdministrationTestSuite.md)
# Requirements: #
[See here](Administration.md)

# Manage data: #
## Steps ##
  1. Create a new item for each type
  1. Update an item of each type
  1. Delete an item of each type
  1. Try to remove an item of each type when it is linked to another entity
  1. Update all entity with a freshly created item (of each type)
## Expected results ##
  1. The items are created
  1. The items are updated
  1. The items are deleted
  1. You see a warning message about referential integrity
  1. The freshly added item is linked to the entity and all its value aren't changed
## Issues ##
### version 3.0.2: ###
  * n.a.
### version 3.0.3: ###
  * [issue 116](https://code.google.com/p/ndoctor/issues/detail?id=116).
  * [issue 134](https://code.google.com/p/ndoctor/issues/detail?id=134).
  * [issue 151](https://code.google.com/p/ndoctor/issues/detail?id=151).
  * [issue 152](https://code.google.com/p/ndoctor/issues/detail?id=152).
  * [issue 157](https://code.google.com/p/ndoctor/issues/detail?id=157).
### version 3.0.4: ###
  * n.a.