--begin transaction;
--begin transaction;
-- Tags
INSERT INTO Tag(Id, Category, Name) VALUES(1 , 'MedicalRecord', 'Medical record Type A');
INSERT INTO Tag(Id, Category, Name) VALUES(2 , 'MedicalRecord', 'Medical record Type B');
INSERT INTO Tag(Id, Category, Name) VALUES(3 , 'MedicalRecord', 'Medical record Type C');

INSERT INTO Tag(Id, Category, Name) VALUES(4 , 'Doctor', 'Generic Doctor');
INSERT INTO Tag(Id, Category, Name) VALUES(5 , 'Doctor', 'Dentist');

INSERT INTO Tag(Id, Category, Name) VALUES(6 , 'Patient', 'Patient category 1');
INSERT INTO Tag(Id, Category, Name) VALUES(7 , 'Patient', 'Patient category 2');

INSERT INTO Tag(Id, Category, Name) VALUES(8 , 'Picture', 'Picture category 1');
INSERT INTO Tag(Id, Category, Name) VALUES(9 , 'Picture', 'Picture category 2');
INSERT INTO Tag(Id, Category, Name) VALUES(10, 'Picture', 'Picture category 2');

INSERT INTO Tag(Id, Category, Name) VALUES(11, 'Pathology', 'Summer season');
INSERT INTO Tag(Id, Category, Name) VALUES(12, 'Pathology', 'Winter season');

INSERT INTO Tag(Id, Category, Name) VALUES(13, 'Drug', 'Painkiller');
INSERT INTO Tag(Id, Category, Name) VALUES(14, 'Drug', 'Vitamins');

INSERT INTO Tag(Id, Category, Name) VALUES(15, 'Prescription', 'Pathology');
INSERT INTO Tag(Id, Category, Name) VALUES(16, 'Prescription', 'Vitamins');

INSERT INTO Tag(Id, Category, Name) VALUES(17, 'PrescriptionDocument', 'Default');

INSERT INTO Tag(Id, Category, Name) VALUES(18, 'Appointment', 'Privé');
INSERT INTO Tag(Id, Category, Name) VALUES(19, 'Appointment', 'Professionnel');

INSERT INTO Pathology (Id, Name, Notes, Tag_Id) VALUES( 1, 'Grippe', 'Some notes', 11);
INSERT INTO Pathology (Id, Name, Notes, Tag_Id) VALUES( 2, 'Rhume', 'Some notes', 11);
INSERT INTO Pathology (Id, Name, Notes, Tag_Id) VALUES( 3, 'Pneumonie', 'Some notes', 12);
INSERT INTO Pathology (Id, Name, Notes, Tag_Id) VALUES( 4, 'Angine', 'Some notes', 12);

INSERT INTO Role(Id, Name) VALUES(1, 'Role 1');
INSERT INTO Role(Id, Name) VALUES(2, 'Role 2');

INSERT INTO Task(Name, role_id) VALUES('Task1', 1);
INSERT INTO Task(Name, role_id) VALUES('Task2', 1);
INSERT INTO Task(Name, role_id) VALUES('Task3', 2);
INSERT INTO Task(Name, role_id) VALUES('Task4', 2);

INSERT INTO Practice(Id, Name, Notes, Phone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber) VALUES (1, 'Cabinet de Liege', 'Qques notes', '04/222.13.89','111','Li�ge', '4000', 'rue Darchis','56');
INSERT INTO Practice(Id, Name, Notes, Phone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber) VALUES (2, 'Cabinet de Huy', 'Qques notes', '085/25.58.75','111','Huy', '4500', 'rue du Long Thier','26');

INSERT INTO Insurance(Id, Name, Notes, Phone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber) VALUES (1, 'Insurance 1', 'Notes', '04 222 13 89', 'box', 'Liege', '4000', 'rue Darchis', '56');
INSERT INTO Insurance(Id, Name, Notes, Phone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber) VALUES (2, 'Insurance 2', 'Notes', '04 222 13 89','box', 'Liege', '4000', 'rue Darchis', '56');
INSERT INTO Insurance(Id, Name, Notes, Phone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber) VALUES (3, 'Insurance 3', 'Notes', '04 222 13 89','box', 'Liege', '4000', 'rue Darchis', '56');

INSERT INTO Profession(id, name, notes) VALUES (1, 'some profession', 'some notes');
INSERT INTO Profession(id, name, notes) VALUES (2, 'some other profession', 'some other notes');

INSERT INTO Reputation(id, name, notes) VALUES (1, 'some reputation', 'some notes');
INSERT INTO Reputation(id, name, notes) VALUES (2, 'some other reputation', 'some other notes');

-- Users
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete) VALUES (1,'2007-01-01', 'Male', 'Docteur Robert', 'Dupont', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete) VALUES (2,'2007-01-01', 'Female', 'Docteur Albert', 'Vroumiz', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1);
-- Patients
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id) VALUES (3,'2007-01-01',  'Male'  , 'Robert', 'Dupont', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 6);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id) VALUES (4,'2007-01-01',  'Female', 'Caroline', 'Vroumiz', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 7);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id) VALUES (7,'2007-01-01',  'Female', 'Julie', 'Robris', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 7);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id) VALUES (8,'2007-01-01',  'Female', 'Martine', 'Vanden', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 7);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id) VALUES (9,'2007-01-01',  'Female', 'Stephanie', 'Lurken', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 7);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id) VALUES (10,'2007-01-01', 'Female', 'Caroline', 'Bostri', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 7);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id) VALUES (11,'2007-01-01', 'Female', 'Justine', 'Sansnom', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 7);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id) VALUES (12,'2007-01-01', 'Female', 'Elise', 'Sansidee', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 7);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id) VALUES (13,'2007-01-01', 'Female', 'Celine', 'Duchlouque', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 7);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id) VALUES (14,'2007-01-01', 'Male'  , 'Vincent', 'Proulouxe', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 7);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id) VALUES (15,'2007-01-01', 'Male'  , 'Docteur Vincent', 'Scalplou', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 7);
-- Doctors
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete) VALUES (5,'2007-01-01', 'Male', 'Docteur Robert', 'Dupont', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile,ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete) VALUES (6,'2007-01-01', 'Female', 'Docteur Lucie', 'Jravniou', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1);

INSERT INTO User(person_id, password, Header, Practice_id, AssignedRole_id, IsDefault) VALUES (1, 'aze', 'Some header', 1, 1, 1);
INSERT INTO User(person_id, password, Header, Practice_id, AssignedRole_id, IsDefault) VALUES (2, 'aze', 'Some header', 2, 1, 0);

INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id)                       VALUES (3 , '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 1, 1, 1, 1);   	   -- Grand father
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id)                       VALUES (4 , '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2);       -- Grand mother
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (7 , '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 3, 4); -- Father
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id)                       VALUES (8 , '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2);	   -- Mother
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (9 , '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 8, 7); -- Children
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (10, '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 8, 7); -- Children
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (11, '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 8, 7); -- Children
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (12, '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 8, 7); -- Children
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (13, '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 8, 7); -- Children
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id)                       VALUES (14, '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2);       -- Without family

INSERT INTO Doctor(Person_Id, Specialisation_Id) VALUES(5, 5);
INSERT INTO Doctor(Person_Id, Specialisation_Id) VALUES(6, 5);
INSERT INTO Doctor(Person_Id, Specialisation_Id) VALUES(15, 5);

INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(3,6);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(3,5);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(4,6);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(4,5);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(7,6);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(7,5);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(8,6);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(8,5);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(9,6);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(9,5);

INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-01', 180, 80, 7);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-02', 180, 81, 7);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-03', 180, 82, 7);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-04', 180, 83, 7);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-05', 180, 84, 7);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-06', 180, 85, 7);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-07', 180, 86, 7);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-08', 180, 87, 7);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-09', 180, 88, 7);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-10', 180, 89, 7);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-11', 180, 90, 7);
                                                                               
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-01', 180, 80, 4);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-02', 180, 81, 4);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-03', 180, 82, 4);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-04', 180, 83, 4);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-05', 180, 84, 4);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-06', 180, 85, 4);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-07', 180, 86, 4);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-08', 180, 87, 4);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-09', 180, 88, 4);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-10', 180, 89, 4);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-11', 180, 90, 4);

INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-01', 180, 80, 3);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-02', 180, 81, 3);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-03', 180, 82, 3);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-04', 180, 83, 3);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-05', 180, 84, 3);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-06', 180, 85, 3);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-07', 180, 86, 3);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-08', 180, 87, 3);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-09', 180, 88, 3);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-10', 180, 89, 3);
INSERT INTO Bmi(Date, Height, Weight, Patient_id) VALUES ('2011-11-11', 180, 90, 3);

INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(1, '2010-01-01', 'Title 1', '<h1>Demonstration Title</h1>" Hello world as a medical record 1 <h1>text</h1>', 1, 3);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(2, '2010-01-01', 'Title 1 bis', '<h1>Demonstration Title</h1>" Hello world as a medical record 1 bis <h1>text</h1>', 1, 3);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(3, '2010-01-01', 'Title 2', '<h1>Demonstration Title</h1>" Hello world as a medical record 2<h1>text</h1>', 2, 3);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(4, '2010-01-01', 'Title 3', '<h1>Demonstration Title</h1>" Hello world as a medical record 3<h1>text</h1>', 3, 3);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(5, '2010-01-01', 'Title 4', '<h1>Demonstration Title</h1>" Hello world as a medical record 4<h1>text</h1>', 1, 4);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(6, '2010-01-01', 'Title 5', '<h1>Demonstration Title</h1>" Hello world as a medical record 5<h1>text</h1>', 2, 4);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(7, '2010-01-01', 'Title 6', '<h1>Demonstration Title</h1>" Hello world as a medical record 6<h1>text</h1>', 3, 4);

INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(8, '2010-01-01', 'Title 6', '<h1>Demonstration Title</h1>" Hello world as a medical record 6<h1>text</h1>', 3, 4);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(9, '2010-01-01', 'Title 6', '<h1>Demonstration Title</h1>" Hello world as a medical record 6<h1>text</h1>', 3, 7);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(10, '2010-01-01', 'Title 6', '<h1>Demonstration Title</h1>" Hello world as a medical record 6<h1>text</h1>', 3, 8);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(11, '2010-01-01', 'Title 6', '<h1>Demonstration Title</h1>" Hello world as a medical record 6<h1>text</h1>', 3, 9);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(12, '2010-01-01', 'Title 6', '<h1>Demonstration Title</h1>" Hello world as a medical record 6<h1>text</h1>', 3, 10);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(13, '2010-01-01', 'Title 6', '<h1>Demonstration Title</h1>" Hello world as a medical record 6<h1>text</h1>', 3, 11);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(14, '2010-01-01', 'Title 6', '<h1>Demonstration Title</h1>" Hello world as a medical record 6<h1>text</h1>', 3, 12);
INSERT INTO MedicalRecord(Id, CreationDate, Name, Html, Tag_Id, Patient_Id) VALUES(15, '2010-01-01', 'Title 6', '<h1>Demonstration Title</h1>" Hello world as a medical record 6<h1>text</h1>', 3, 13);

INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id) VALUES (1, '2010-01-01', '2010-01-05', 1, 'Some notes about the period', 7);
INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id) VALUES (2, '2010-01-02', '2010-01-07', 2, 'Some notes about the period', 7);
INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id) VALUES (3, '2010-01-03', '2010-01-09', 3, 'Some notes about the period', 7);
INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id) VALUES (4, '2010-01-04', '2010-01-15', 4, 'Some notes about the period', 7);
INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id) VALUES (5, '2010-01-05', '2010-01-22', 1, 'Some notes about the period', 7);
INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id) VALUES (6, '2010-01-06', '2010-01-10', 3, 'Some notes about the period', 7);

INSERT INTO Drug(Id, Name, Notes, Tag_Id) VALUES (1, 'Aspirine' , 'For headaches', 13);
INSERT INTO Drug(Id, Name, Notes, Tag_Id) VALUES (2, 'Vitamin C', 'For energy'   , 14);
INSERT INTO Drug(Id, Name, Notes, Tag_Id) VALUES (3, 'Vitamin D', 'For energy'   , 14);
INSERT INTO Drug(Id, Name, Notes, Tag_Id) VALUES (4, 'Vitamin E', 'For energy'   , 14);
INSERT INTO Drug(Id, Name, Notes, Tag_Id) VALUES (5, 'Dispril'  , 'For headaches', 13); 

INSERT INTO PrescriptionDocument(Id, CreationDate, Title, Tag_Id, Patient_Id) VALUES(1, '2011-10-09', 'Default title', 15, 7);
INSERT INTO PrescriptionDocument(Id, CreationDate, Title, Tag_Id, Patient_Id) VALUES(2, '2011-10-09', 'Default title', 15, 7);
INSERT INTO PrescriptionDocument(Id, CreationDate, Title, Tag_Id, Patient_Id) VALUES(3, '2011-10-09', 'Default title', 15, 7);

INSERT INTO Prescription(Id, Notes, Tag_Id, PrescriptionDocument_Id, Drug_Id) VALUES (1, 'Twice a day', 15, 1, 1);
INSERT INTO Prescription(Id, Notes, Tag_Id, PrescriptionDocument_Id, Drug_Id) VALUES (2, 'Twice a day', 15, 1, 2);
INSERT INTO Prescription(Id, Notes, Tag_Id, PrescriptionDocument_Id, Drug_Id) VALUES (3, 'Twice a day', 16, 1, 3);
INSERT INTO Prescription(Id, Notes, Tag_Id, PrescriptionDocument_Id, Drug_Id) VALUES (4, 'Twice a day', 16, 2, 4);

INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES ( 1, '2011-10-16 08:00', '2011-10-16 08:30', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES ( 2, '2011-10-16 08:30', '2011-10-16 09:00', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES ( 3, '2011-10-16 09:00', '2011-10-16 09:30', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES ( 4, '2011-10-16 09:30', '2011-10-16 10:00', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES ( 5, '2011-10-16 10:00', '2011-10-16 10:30', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES ( 6, '2011-10-16 10:30', '2011-10-16 11:00', 'Meeting', 18, 1, 7); 
-- INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES ( 7, '2011-10-16 11:00', '2011-10-16 11:30', 'Meeting', 18, 1, 7); 
-- INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES ( 8, '2011-10-16 11:30', '2011-10-16 12:00', 'Meeting', 18, 1, 7); 
-- INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES ( 9, '2011-10-16 12:00', '2011-10-16 12:30', 'Meeting', 18, 1, 7); 
-- INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES (10, '2011-10-16 12:30', '2011-10-16 13:00', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES (11, '2011-10-16 13:00', '2011-10-16 13:30', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES (12, '2011-10-16 13:30', '2011-10-16 14:00', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES (13, '2011-10-16 14:00', '2011-10-16 14:30', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES (14, '2011-10-16 14:30', '2011-10-16 15:00', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES (15, '2011-10-16 15:00', '2011-10-16 15:30', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES (16, '2011-10-16 15:30', '2011-10-16 16:00', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES (17, '2011-10-16 16:00', '2011-10-16 16:30', 'Meeting', 18, 1, 7); 
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id) VALUES (18, '2011-10-16 16:30', '2011-10-16 17:00', 'Meeting', 18, 1, 7); 

--commit;