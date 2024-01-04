CREATE DATABASE [TableRelation]

--ONE TO ONE 
CREATE TABLE [Passports](
[PassportID] INT PRIMARY KEY  IDENTITY(101,1),
[PassportNumber]NVARCHAR(8) NOT NULL
)

INSERT INTO [Passports](PassportNumber)
VALUES 
('N34FG21B'),
('K65LO4R7'),
('ZE657QP2')
 --SELECT *FROM [Passports]

  CREATE TABLE [Persons]
  (
  [PersonID] INT PRIMARY KEY IDENTITY,
  [FirstName]NVARCHAR(50) NOT NULL,
  [Salary] DECIMAL(10,2) NOT NULL,
  [PassportID]INT FOREIGN KEY REFERENCES[Passports]([PassportID]) UNIQUE NOT NULL 
  )
  --SELECT *FROM [Persons]
  INSERT INTO[Persons]([FirstName],[Salary],[PassportID])
  VALUES
('Roberto',43300.00,102),
('Tom',56100.00,103),
('Yana',60200.00,101)


SELECT *FROM [Passports]
SELECT *FROM [Persons]

--02. One-To-Many Relationship
GO

CREATE TABLE[Manufacturers]
(
[ManufacturerID] INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(50) NOT NULL,
[EstablishedOn]DATE NOT NULL
)
CREATE TABLE[Models]
(
[ModelID] INT PRIMARY KEY IDENTITY(101,1),
[Name] NVARCHAR(50) NOT NULL,
[ManufacturerID] INT FOREIGN KEY REFERENCES [Manufacturers]([ManufacturerID]) NOT NULL
)

INSERT INTO [Manufacturers]([Name],[EstablishedOn])
VALUES 
('BMW','07/03/1916'),
('Tesla','01/01/2003'),
('Lada','01/05/1966')
 
 INSERT INTO [Models]([Name], [ManufacturerID])
 VALUES
 ('X1',1),
 ('i6',1),
 ('Model S',2),
 ('Model X',2),
 ('Model 3',2),
 ('Nova',3)

 GO


 --Many to many relatuioins
 CREATE TABLE[Students]
 (
 [StudentID] INT PRIMARY KEY IDENTITY,
 [Name] NVARCHAR(50) NOT NULL
 )
  CREATE TABLE [Exams]
  (
  [ExamID]INT PRIMARY KEY IDENTITY(101,1),
  [Name] NVARCHAR(50) NOT NULL
  
  )
  CREATE TABLE [StudentsExams]
  (
  [StudentID] INT FOREIGN KEY REFERENCES [Students]([StudentID]) NOT NULL,
  [ExamID] INT FOREIGN KEY REFERENCES [Exams]([ExamID]) NOT NULL,
  PRIMARY KEY([StudentID],[ExamID])

  )
  INSERT INTO [Students]([Name])
  VALUES
  ('Mila'),
  ('Toni'),
  ('Ron')

   INSERT INTO [Exams]([Name])
   VALUES 
   ('SpringMVC'),
   ('Neo4j'),
   ('Oracle 11g')


   --4.	Self-Referencing 
   CREATE TABLE [Teachers]
   (
   [TeacherID] INT PRIMARY KEY IDENTITY(101,1),
   [Name] NVARCHAR(50) NOT NULL,
   [ManagerID] INT FOREIGN KEY REFERENCES[Teachers]([TeacherID])
   )
   GO
   --05 Online Store Database
   CREATE DATABASE[ONLINESTOREDATABASE]
  
    
   CREATE TABLE[ItemTypes]
   (
   [ItemTypeID]  INT PRIMARY KEY IDENTITY,
   [Name] NVARCHAR(50) NOT NULL
   
   )

   CREATE TABLE[Cities]
   (
   [CityID] INT PRIMARY KEY IDENTITY,
   [Name] NVARCHAR(50) NOT NULL
   )

    CREATE TABLE[Items]
	(
	[ItemID] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	[ItemTypeID] INT FOREIGN KEY REFERENCES [ItemTypes]([ItemTypeID])  NOT NULL
	)

	CREATE TABLE [Customers]
	(
	[CustomerID]INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	[Birthday] DATE NOT NULL,
	[CityID] INT FOREIGN KEY REFERENCES [Cities]([CityID])NOT NULL
	)

	 CREATE TABLE [Orders]
	 (
	 [OrderID] INT PRIMARY KEY IDENTITY,
	 [CustomerID]INT FOREIGN KEY REFERENCES [Customers]([CustomerID])
	 )
	  CREATE TABLE [OrderItems]
	  (
	  [OrderID] INT FOREIGN KEY REFERENCES [Orders]([OrderID]),
	  [ItemID] INT FOREIGN KEY REFERENCES [Items]([ItemID]),
	  PRIMARY KEY([OrderID],[ItemID])
	  
	  )
	  GO
	  --06.	University Database
	  CREATE DATABASE[Universaty]
	  USE [Universaty]
	  
	  CREATE TABLE [Majors]
	  (
	  [MajorID] INT PRIMARY KEY IDENTITY,
	  [Name] NVARCHAR(50) NOT NULL
	  )
	   CREATE TABLE[Subjects]
	   (
	   [SubjectID] INT PRIMARY KEY IDENTITY,
	   [SubjectName] NVARCHAR(50) NOT NULL
	   
	   )

	   CREATE TABLE [Students]
	   (
	   [StudentID] INT PRIMARY KEY IDENTITY,
	   [StudentNumber] NVARCHAR(100) NOT NULL,
	   [StudentName] NVARCHAR(50) NOT NULL,
	   [MajorID] INT FOREIGN KEY REFERENCES [Majors]([MajorID])
	   )
	   CREATE TABLE [Agenda]
	   (
	   [StudentID] INT FOREIGN KEY REFERENCES[Students]([StudentID]),

	   [SubjectID] INT FOREIGN KEY REFERENCES [Subjects]([SubjectID]),
	   PRIMARY KEY([StudentID],[SubjectID])
	   )
	   CREATE TABLE[Payments]
	   (
	   [PaymentID] INT PRIMARY KEY IDENTITY,
	   [PaymentDate]DATE NOT NULL,
	   [PaymentAmount] DECIMAL(8,2) NOT NULL,
	   [StudentID] INT FOREIGN KEY REFERENCES[Students]([StudentID])
	   )

	   USE [Geography]
	   SELECT 
	   m.[MountainRange],
	   p.[PeakName],
	   p.[Elevation]
	  FROM [Peaks]
	  AS p
	  LEFT JOIN [Mountains]
	  AS m
	  ON p.[MountainId]=m.[Id]
	  WHERE m.[MountainRange]='Rila'
	  ORDER BY p.[Elevation] DESC