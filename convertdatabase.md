# Introduction #
The importer reads the data from a nDoctor 2 database and import the data into nDoctor 3. The importer records when the user made an import of a old nDoctor 2 database and warn the user if he/she makes an attempts to reimport the same data more than once.

# Data import #

  * Patient
    * Direct data
      * First name, last name, address,...
      * Reputation
      * Profession (Needs to be checked as nDoctor 1.0.9 doesn't record profession)
      * Insurance
    * Indirect data
      * Doctors
        * Specialisations
      * Medical records
        * type of records
      * Illness periods
        * Pathology
          * Pathology category
      * Prescription
        * Prescription's category
        * Drugs
          * Drug's type
      * BMI
      * Pictures
        * Picture's type
      * Meetings
        * Meeting's type