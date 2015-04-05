# Test machine #

Test team should install [Virtual Box](https://www.virtualbox.org/) in their machine and configure a virtual Windows 7

The virtual OS should be a default installation without any software or special configuration. That's, an out of the box installation

# Test execution #

Each test case is a wiki page under the QA page.

Follow this pattern to create _test suite_ and _test cases_
  * QA
    * Test Suite 1
      * Test Case 1
        * Result version x.x.0
        * Result version x.x.1
      * Test Case 2
        * Result version x.x.0
        * Result version x.x.1
    * Test Suite 2
      * Test Case 1
        * Result version x.x.0
        * Result version x.x.1
      * Test Case 2
        * Result version x.x.0
        * Result version x.x.1

## A test suite is written on this pattern: ##
  * **Description**: contains a link to the tested requirement and/or a short description of what is tested.

## A test case is written on this pattern: ##
  * **Requirement**: if needed, give a link to the requirements
  * **Steps**: contains point by point the steps to execute the test.
  * **Expected results**: contains point by point the expected results of the executed step.
  * **Version**: contains the tested version of nDoctor.
  * **Issues**: contains a link to the issues this test highlighted.
## Tips: ##
  * Create a script to delete the database when you need to start from scratch:
```
rm -Force %appdata%\Probel\nDoctor\Database.db
```