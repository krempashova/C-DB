CREATE DATABASE[SoftUni]

--•	Towns (Id, Name)
CREATE TABLE[Towns](
[Id] INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(200) NOT NULL,
)
--•	Addresses (Id, AddressText, TownId)
CREATE TABLE[Addresses](
[Id] INT PRIMARY KEY IDENTITY,
[AddressText] NVARCHAR(MAX) NOT NULL,
[TownId] INT FOREIGN KEY REFERENCES [Towns]([Id])
)
--•	Departments (Id, Name)
CREATE TABLE[Departments](
[Id] INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(200) NOT NULL,
)
--•	Employees (Id, FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary, AddressId)
CREATE TABLE [Employees](
[Id] INT PRIMARY KEY IDENTITY,
[FirstName] NVARCHAR(50) NOT NULL,
[MiddleName] NVARCHAR(50),
[LastName] NVARCHAR(50) NOT NULL,
[JobTitle] NVARCHAR(200) NOT NULL,
[DepartmentId] INT FOREIGN KEY REFERENCES [Departments]([Id]),
[HireDate] DATE ,
[Salary] DECIMAL(10,2) NOT NULL,
[AddressId]INT FOREIGN KEY REFERENCES [Addresses]([Id])
)
INSERT INTO[Towns]([Name])
VALUES
('Sofia'), 
('Plovdiv'), 
('Varna'), 
('Burgas')

INSERT INTO[Departments]([Name])
VALUES
('Engineering'),
('Sales'),
('Marketing'),
('Software Development'),
('Quality Assurance')

INSERT INTO [Employees]([FirstName],[MiddleName],[LastName],[JobTitle],[DepartmentId],[HireDate],[Salary])
VALUES
('Ivan','Ivanov','Ivanov','.NET Developer',4,'2013-02-01',3500.00),
('Petar ','Petrov','Petrov','Senior Engineer',1,'2004-03-02',4000.00),
('Maria  ','Petrova','Ivanova','Intern',5,'2016-08-28',525.25),
('Georgi  ','Teziev ','Ivanov','CEO',2,'2007-12-09',3000.00),
('Petar ','Pan','Pan','Intern',3,'2016-08-28',599.88)
 SELECT* FROM[Towns]
 SELECT*FROM[Departments]
 SELECT*FROM[Employees]


 --BACKUP BUT NOT WORK.....

BACKUP DATABASE [SoftUni] TO DISK = 'C:\softuni-backup.bak';

USE CarRental;

DROP DATABASE SoftUni;

RESTORE DATABASE SoftUni FROM DISK = 'C:\softuni-backup.bak';


SELECT *FROM [Towns]
ORDER BY [Name] ASC;

SELECT *
FROM [Departments]
ORDER BY Name ASC;

SELECT *
FROM [Employees]
ORDER BY Salary DESC;




SELECT[Name] FROM [Towns]
ORDER BY [Name] ASC;

SELECT[Name] FROM [Departments]
ORDER BY Name ASC;
SELECT [FirstName], [LastName], [JobTitle], [Salary] FROM [Employees]ORDER BY Salary DESC;


UPDATE [Employees]
  SET
      Salary *= 1.10;

SELECT [Salary]
FROM Employees;


