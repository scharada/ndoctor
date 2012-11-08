--begin transaction;
-- Tags
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(2 , 'MedicalRecord', 'Medical record Type A', 0);
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(3 , 'MedicalRecord', 'Medical record Type B', 0);
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(4 , 'MedicalRecord', 'Medical record Type C', 0);

INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(5 , 'Doctor', 'Generic Doctor', 0);
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(6 , 'Doctor', 'Dentist', 0);

INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(7 , 'Patient', 'Patient category 1', 0);
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(8 , 'Patient', 'Patient category 2', 0);
								  
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(9 , 'Picture', 'Picture category 1', 0);
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(10 , 'Picture', 'Picture category 2', 0);
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(11, 'Picture', 'Picture category 3', 0);
								  
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(12, 'Pathology', 'Summer season', 0);
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(13, 'Pathology', 'Winter season', 0);
								  
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(14, 'Drug', 'Painkiller', 0);
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(15, 'Drug', 'Vitamins', 0);
								  
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(16, 'Prescription', 'Prescription type 1', 0);
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(17, 'Prescription', 'Prescription type 2', 0);
								  
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(18, 'PrescriptionDocument', 'Default', 0);
								  
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(19, 'Appointment', 'Privé', 0);
INSERT INTO Tag(Id, Category, Name, IsImported) VALUES(20, 'Appointment', 'Professionnel', 0);

INSERT INTO Pathology (Id, Name, Notes, Tag_Id, IsImported) VALUES( 1, 'Grippe'   , 'Some notes', 12, 0);
INSERT INTO Pathology (Id, Name, Notes, Tag_Id, IsImported) VALUES( 2, 'Rhume'    , 'Some notes', 12, 0);
INSERT INTO Pathology (Id, Name, Notes, Tag_Id, IsImported) VALUES( 3, 'Pneumonie', 'Some notes', 13, 0);
INSERT INTO Pathology (Id, Name, Notes, Tag_Id, IsImported) VALUES( 4, 'Angine'   , 'Some notes', 13, 0);

INSERT INTO Practice(Id, Name, Notes, Phone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsImported) VALUES (1, 'Cabinet de Liege', 'Qques notes', '04/222.13.89','111','Li�ge', '4000', 'rue Darchis','56', 0);
INSERT INTO Practice(Id, Name, Notes, Phone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsImported) VALUES (2, 'Cabinet de Huy', 'Qques notes', '085/25.58.75','111','Huy', '4500', 'rue du Long Thier','26', 0);

INSERT INTO Insurance(Id, Name, Notes, Phone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsImported) VALUES (1, 'Insurance 1', 'Notes', '04 222 13 89', 'box', 'Liege', '4000', 'rue Darchis', '56', 0);
INSERT INTO Insurance(Id, Name, Notes, Phone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsImported) VALUES (2, 'Insurance 2', 'Notes', '04 222 13 89','box', 'Liege', '4000', 'rue Darchis', '56', 0);
INSERT INTO Insurance(Id, Name, Notes, Phone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsImported) VALUES (3, 'Insurance 3', 'Notes', '04 222 13 89','box', 'Liege', '4000', 'rue Darchis', '56', 0);

INSERT INTO Profession(id, name, notes, IsImported) VALUES (1, 'some profession', 'some notes', 0);
INSERT INTO Profession(id, name, notes, IsImported) VALUES (2, 'some other profession', 'some other notes', 0);
									  
INSERT INTO Reputation(id, name, notes, IsImported) VALUES (1, 'some reputation', 'some notes', 0);
INSERT INTO Reputation(id, name, notes, IsImported) VALUES (2, 'some other reputation', 'some other notes', 0);

-- Users
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, IsImported) VALUES ( 1,'2007-01-01', 'Male'  , 'Docteur Robert' , 'Dupont'   , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, IsImported) VALUES ( 2,'2007-01-01', 'Female', 'Docteur Albert' , 'Vroumiz'  , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, IsImported) VALUES (16,'2007-01-01', 'Male'  , 'Docteur No'     , 'Pazwordz' , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, IsImported) VALUES (17,'2007-01-01', 'Male'  , 'Docteur Wiz'     , 'NullPazwordz' , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8);
-- Patients
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id, IsImported) VALUES ( 3,'2007-01-01', 'Male'   , 'Robert'         , 'Dupont'    , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 7, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id, IsImported) VALUES ( 4,'2007-01-01', 'Female' , 'Caroline'       , 'Vroumiz'   , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id, IsImported) VALUES ( 7,'2007-01-01', 'Male'   , 'Jean-Baptiste'  , 'Wautier'   , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id, IsImported) VALUES ( 8,'2007-01-01', 'Female' , 'Martine'        , 'Vanden'    , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id, IsImported) VALUES ( 9,'2007-01-01', 'Female' , 'Stephanie'      , 'Lurken'    , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id, IsImported) VALUES (10,'2007-01-01', 'Female' , 'Caroline'       , 'Bostri'    , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id, IsImported) VALUES (11,'2007-01-01', 'Female' , 'Justine'        , 'Sansnom'   , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id, IsImported) VALUES (12,'2007-01-01', 'Female' , 'Elise'          , 'Sansidee'  , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id, IsImported) VALUES (13,'2007-01-01', 'Female' , 'Celine'         , 'Duchlouque', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id, IsImported) VALUES (14,'2007-01-01', 'Male'   , 'Vincent'        , 'Proulouxe' , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, Tag_id, IsImported) VALUES (15,'2007-01-01', 'Male'   , 'Docteur Vincent', 'Scalplou'  , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 8, 0);
-- Doctors
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, IsImported) VALUES (5,'2007-01-01', 'Male'  , 'Doctor Robert', 'Dupont'  , 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 0);
INSERT INTO Person(Id, LastUpdate, Gender, FirstName, LastName, ProMail, ProMobile, ProPhone, AddressBoxNumber, AddressCity, AddressPostalCode, AddressStreet, AddressStreetNumber, IsComplete, IsImported) VALUES (6,'2007-01-01', 'Female', 'Doctor Lucie ', 'Jravniou', 'some@mail.com', '0476/79.98.97','085/25.58.75', 'box', 'Liege', '4000', 'rue Darchis', '56', 1, 0);

INSERT INTO User(person_id, password, Header, Practice_id, AssignedRole_id, IsDefault, IsSuperAdmin) VALUES (1, 'aze', 'Some header', 1, 1, 1, 1);
INSERT INTO User(person_id, password, Header, Practice_id, AssignedRole_id, IsDefault) VALUES ( 2 , 'aze', 'Some header', 2, 1, 0);
INSERT INTO User(person_id, password, Header, Practice_id, AssignedRole_id, IsDefault) VALUES (16 , '', 'Some header', 2, 1, 0);
INSERT INTO User(person_id, password, Header, Practice_id, AssignedRole_id, IsDefault) VALUES (17 , null, 'Some header', 2, 1, 0);


INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id)                       VALUES (3 , '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 1, 1, 1, 1);   	   -- Grand father
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id)                       VALUES (4 , '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2);        -- Grand mother
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (7 , '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 3, 4);  -- Father
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id)                       VALUES (8 , '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2);  	   -- Mother
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (9 , '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 8, 7);  -- Children
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (10, '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 8, 7);  -- Children
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (11, '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 8, 7);  -- Children
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (12, '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 8, 7);  -- Children
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id, Father_Id, Mother_Id) VALUES (13, '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2, 8, 7);  -- Children
INSERT INTO Patient(Person_id, Birthdate, Fee, Height, InscriptionDate, PlaceOfBirth, PrivateMail, PrivateMobile, PrivatePhone, Reason, Insurance_Id, Practice_Id, Profession_Id, Reputation_Id)                       VALUES (14, '2007-01-01', 170, 180, '2007-01-01 10:00:00', 'Place of birth', 'private@mail.com', '088/55.33.66', '04/226.14.15', 'Some reasons', 2, 2, 2, 2);        -- Without family

INSERT INTO Doctor(Person_Id, Specialisation_Id) VALUES(5 , 5);
INSERT INTO Doctor(Person_Id, Specialisation_Id) VALUES(6 , 5);
INSERT INTO Doctor(Person_Id, Specialisation_Id) VALUES(15, 5);

INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(3, 6);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(3, 5);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(4, 6);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(4, 5);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(7, 6);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(7, 5);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(8, 6);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(8, 5);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(9, 6);
INSERT INTO DoctorsToPatients(Patient_Id, Doctor_Id) VALUES(9, 5);

INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-01', 180, 80, 7, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-02', 180, 81, 7, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-03', 180, 82, 7, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-04', 180, 83, 7, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-05', 180, 84, 7, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-06', 180, 85, 7, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-07', 180, 86, 7, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-08', 180, 87, 7, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-09', 180, 88, 7, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-10', 180, 89, 7, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-11', 180, 90, 7, 0);
                                                                               
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-01', 180, 80, 4, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-02', 180, 81, 4, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-03', 180, 82, 4, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-04', 180, 83, 4, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-05', 180, 84, 4, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-06', 180, 85, 4, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-07', 180, 86, 4, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-08', 180, 87, 4, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-09', 180, 88, 4, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-10', 180, 89, 4, 0);
INSERT INTO Bmi(Date, Height, Weight, Patient_id, IsImported) VALUES ('2007-01-11', 180, 90, 4, 0);

INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES( 1, '2010-01-01', 'Title 01', 'Demonstration Title" Hello world as a medical record 01 text', 2,  3, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES( 2, '2010-01-01', 'Title 02', 'Demonstration Title" Hello world as a medical record 02 text', 2,  4, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES( 3, '2010-01-01', 'Title 03', 'Demonstration Title" Hello world as a medical record 03 text', 3,  7, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES( 4, '2010-01-01', 'Title 04', 'Demonstration Title" Hello world as a medical record 04 text', 4,  8, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES( 5, '2010-01-01', 'Title 05', 'Demonstration Title" Hello world as a medical record 05 text', 2,  9, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES( 6, '2010-01-01', 'Title 06', 'Demonstration Title" Hello world as a medical record 06 text', 3, 10, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES( 7, '2010-01-01', 'Title 07', 'Demonstration Title" Hello world as a medical record 07 text', 4, 11, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES( 8, '2010-01-01', 'Title 08', 'Demonstration Title" Hello world as a medical record 08 text', 4, 12, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES( 9, '2010-01-01', 'Title 09', 'Demonstration Title" Hello world as a medical record 09 text', 4, 13, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES(10, '2010-01-01', 'Title 10', 'Demonstration Title" Hello world as a medical record 10 text', 4, 14, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES(11, '2010-01-01', 'Title 11', 'Demonstration Title" Hello world as a medical record 11 text', 4,  3, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES(12, '2010-01-01', 'Title 12', 'Demonstration Title" Hello world as a medical record 12 text', 4,  4, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES(13, '2010-01-01', 'Title 13', 'Demonstration Title" Hello world as a medical record 13 text', 4,  7, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES(14, '2010-01-01', 'Title 14', 'Demonstration Title" Hello world as a medical record 14 text', 4,  8, 0);
INSERT INTO MedicalRecord(Id, CreationDate, Name, rtf, Tag_Id, Patient_Id, IsImported) VALUES(15, '2010-01-01', 'Title 15', 'Demonstration Title" Hello world as a medical record 15 text', 4,  9, 0);

INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id, IsImported) VALUES (1, '2010-01-01', '2010-01-05', 1, 'Some notes about the period 1', 7, 0);
INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id, IsImported) VALUES (2, '2010-01-02', '2010-01-07', 2, 'Some notes about the period 2', 7, 0);
INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id, IsImported) VALUES (3, '2010-01-03', '2010-01-09', 3, 'Some notes about the period 3', 7, 0);
INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id, IsImported) VALUES (4, '2010-01-04', '2010-01-15', 4, 'Some notes about the period 4', 7, 0);
INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id, IsImported) VALUES (5, '2010-01-05', '2010-01-22', 1, 'Some notes about the period 5', 7, 0);
INSERT INTO IllnessPeriod (Id, Start, End, Pathology_Id, Notes, Patient_Id, IsImported) VALUES (6, '2010-01-06', '2010-01-10', 3, 'Some notes about the period 6', 7, 0);

INSERT INTO Drug(Id, Name, Notes, Tag_Id, IsImported) VALUES (1, 'Aspirine' , 'For headaches', 14, 0);
INSERT INTO Drug(Id, Name, Notes, Tag_Id, IsImported) VALUES (2, 'Vitamin C', 'For energy'   , 15, 0);
INSERT INTO Drug(Id, Name, Notes, Tag_Id, IsImported) VALUES (3, 'Vitamin D', 'For energy'   , 15, 0);
INSERT INTO Drug(Id, Name, Notes, Tag_Id, IsImported) VALUES (4, 'Vitamin E', 'For energy'   , 15, 0);
INSERT INTO Drug(Id, Name, Notes, Tag_Id, IsImported) VALUES (5, 'Dispril'  , 'For headaches', 14, 0);

INSERT INTO PrescriptionDocument(Id, CreationDate, Title, Tag_Id, Patient_Id, IsImported) VALUES(1, '2012-11-07', 'Default title', 16, 7, 0);
INSERT INTO PrescriptionDocument(Id, CreationDate, Title, Tag_Id, Patient_Id, IsImported) VALUES(2, '2012-11-07', 'Default title', 16, 7, 0);
INSERT INTO PrescriptionDocument(Id, CreationDate, Title, Tag_Id, Patient_Id, IsImported) VALUES(3, '2012-11-07', 'Default title', 16, 7, 0);

INSERT INTO Prescription(Id, Notes, Tag_Id, PrescriptionDocument_Id, Drug_Id, IsImported) VALUES (1, 'Twice a day', 15, 1, 1, 0);
INSERT INTO Prescription(Id, Notes, Tag_Id, PrescriptionDocument_Id, Drug_Id, IsImported) VALUES (2, 'Twice a day', 15, 1, 2, 0);
INSERT INTO Prescription(Id, Notes, Tag_Id, PrescriptionDocument_Id, Drug_Id, IsImported) VALUES (3, 'Twice a day', 16, 1, 3, 0);
INSERT INTO Prescription(Id, Notes, Tag_Id, PrescriptionDocument_Id, Drug_Id, IsImported) VALUES (4, 'Twice a day', 16, 2, 4, 0);

INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES ( 1, '2012-11-10 08:00', '2011-10-16 08:30', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES ( 2, '2012-11-10 08:30', '2011-10-16 09:00', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES ( 3, '2012-11-10 09:00', '2011-10-16 09:30', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES ( 4, '2012-11-10 09:30', '2011-10-16 10:00', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES ( 5, '2012-11-10 10:00', '2011-10-16 10:30', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES ( 6, '2012-11-10 10:30', '2011-10-16 11:00', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES (11, '2012-11-10 13:00', '2011-10-16 13:30', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES (12, '2012-11-10 13:30', '2011-10-16 14:00', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES (13, '2012-11-10 14:00', '2011-10-16 14:30', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES (14, '2012-11-10 14:30', '2011-10-16 15:00', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES (15, '2012-11-10 15:00', '2011-10-16 15:30', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES (16, '2012-11-10 15:30', '2011-10-16 16:00', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES (17, '2012-11-10 16:00', '2011-10-16 16:30', 'Meeting', 19, 1, 7, 0);
INSERT INTO Appointment(Id, StartTime, EndTime, Subject, Tag_Id, User_Id, Patient_Id, IsImported) VALUES (18, '2012-11-10 16:30', '2011-10-16 17:00', 'Meeting', 19, 1, 7, 0);

INSERT INTO Macro(Id, Expression, Notes, Title, IsImported) VALUES (1, 'Hello $firstname$ $lastname$.', 'Says hello to connected patient', 'Hello', 0);
INSERT INTO Macro(Id, Expression, Notes, Title, IsImported) VALUES (2, '$firstname$ $lastname$ is $age$ old.', 'Says the age of the connected patient', 'Age', 0);

--commit;