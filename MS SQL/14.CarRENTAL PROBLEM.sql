
CREATE DATABASE[CarRental]

CREATE TABLE[Categories](
[Id] INT PRIMARY KEY IDENTITY,
[CategoryName] NVARCHAR(50) NOT NULL,
[DailyRate] DECIMAL(5,2) NOT NULL,
[WeeklyRate] DECIMAL(5,2) NOT NULL,
[MonthlyRate] DECIMAL(5,2) NOT NULL,
[WeekendRate] DECIMAL(5,2) NOT NULL
 )
 CREATE TABLE[Cars](
 [Id] INT PRIMARY KEY IDENTITY,
 [PlateNumber] NCHAR(15) NOT NULL,
 [Manufacturer] NVARCHAR(100) NOT NULL,
 [Model] NVARCHAR(100) NOT NULL,
 [CarYear]SMALLINT NOT NULL,
 [CategoryId] INT FOREIGN KEY REFERENCES [Categories]([Id]),
 [Doors] SMALLINT NOT NULL,
 [Picture]VARBINARY,
 [Condition] NVARCHAR(25) NOT NULL,
 [Available]BIT NOT NULL
 
 
 
 ) 
 CREATE TABLE [Employees](
 [id] INT PRIMARY KEY IDENTITY,
 [FirstName] NVARCHAR(50) NOT NULL,
 [LastName] NVARCHAR(50) NOT NULL,
 [Title]NVARCHAR(100),
 [Notes] NVARCHAR(MAX)
 
 
 )

 CREATE TABLE [Customers](
 [Id] INT PRIMARY KEY IDENTITY,
 [DriverLicenceNumber] NVARCHAR(50) NOT NULL,
 [FullName] NVARCHAR(255) NOT NULL,
 [Address] NVARCHAR(500) NOT NULL,
 [City]NVARCHAR(100) NOT NULL,
 [ZIPCode] SMALLINT NOT NULL,
 [Notes] NVARCHAR(MAX)
 
 
 ) 

 CREATE TABLE [RentalOrders](
 [Id] INT PRIMARY KEY IDENTITY,
 [EmployeeId] INT FOREIGN KEY REFERENCES [Employees]([Id]),
 [CustomerId] INT FOREIGN KEY REFERENCES [Customers]([Id]),
 [CarId] INT FOREIGN KEY REFERENCES [Cars]([Id]),
 [TankLevel] DECIMAL(5,2),
 [KilometrageStart] INT NOT NULL,
 [KilometrageEnd] INT NOT NULL,
 [TotalKilometrage] INT NOT NULL,
 [StartDate]DATE NOT NULL,
 [EndDate] DATE NOT NULL,
 [TotalDays] SMALLINT NOT NULL,
 [RateApplied] INT,
 [TaxRate] INT,
 [OrderStatus] BIT NOT NULL,
 [Notes]NVARCHAR(MAX)
 )

 INSERT INTO [Categories]([CategoryName],[DailyRate],[WeeklyRate],[MonthlyRate],[WeekendRate])
 VALUES 
 ('SPORT',50,25,58,80),
 ('LIMUZINA',58,60,25,15),
 ('NEZNAM',60,80,50,20)

 INSERT INTO[Cars]([PlateNumber],[Manufacturer],[Model],[CarYear],[Doors],[Picture],[Condition],[Available],[CategoryId])
 VALUES
 ('CA2584BG','FERARI','neznam',2000,3,null,'nova','true',1),
 ('CB4879CA','BMW','X5',2006,4,NULL,'BRAKMA','false',3),
 ('CB2233CD','MERCEDES','S400',2016,5,NULL,'JAJAAJAJA','true',2)

 
 INSERT INTO [Employees]([FirstName],[LastName],[Title])
 VALUES 
 ('KREMENA','PASHOVA','NEZNAM'),
 ('ANETA','PASHOVA','TITLE'),
 ('MALOUMNIK','PASHOV','TITLE')

 INSERT INTO [Customers]([DriverLicenceNumber],[FullName],[Address],[City],[ZIPCode])
 VALUES
 ('HDJDKDK124488','MALOUMNIK MALOUMNIKOV SMOTANQRKOV','GR.HSJKDHJKSDHJKDHSJKDSJK,NBDHDHSHDSK','SOFIA',1000),
 ('1254796635254','SMOTANQK SMOTAN PROST','SELO SMOTANQRSKO','PLOVDIV',1300),
 ('HHDKD2148885555','NEZNAM NEZNAM NANA','DRAGALEVCI NESHTO SI TAM ','VARNA',1005)

 INSERT INTO[RentalOrders]([EmployeeId],[CustomerId],[CarId],[TankLevel],[KilometrageStart],[KilometrageEnd],[TotalKilometrage],[StartDate],[EndDate],[TotalDays],[RateApplied],[TaxRate],[OrderStatus])
 VALUES
 (1,2,3,50,100,300,200,'2023-10-15','2022-10-15',7,17,50,'true'),
 (2,3,1,20,150,170,20,'2023-10-16','2022-11-15',8,80,100,'false'),
  (1,3,2,30,250000,280000,30000,'2023-06-15','2022-12-15',8,28,50,'true')