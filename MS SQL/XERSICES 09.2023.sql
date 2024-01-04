

CREATE TABLE [Towns](
[Id] INT PRIMARY KEY,
[Name] NVARCHAR(50) NOT NULL


)
ALTER TABLE[Minions]
ADD [TownId] INT FOREIGN KEY REFERENCES[Towns]([Id]) NOT NULL


ALTER TABLE[Minions]
ALTER COLUMN[Age]INT

INSERT INTO[Towns] ([Id] ,[Name])
VALUES
(1,'Sofia'),
(2,'Plovdiv'),
(3,'Varna')




TRUNCATE TABLE [Minions]



CREATE TABLE[People](
[Id]INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(200) NOT NULL,
[Picture]VARBINARY(max) 
CHECK(DATALENGTH([Picture])<=2000000),
[Height] DECIMAL(3,2),
[Weight] DECIMAL(5,2),
[Gender] CHAR(1) NOT NULL
CHECK([Gender]='m' OR [Gender]='f'),
[Birthdate] DATE NOT NULL,
[Biography] NVARCHAR(MAX) 




)
INSERT INTO[People]([Name],[Height],[Weight],[Gender],[Birthdate],[Biography])
VALUES
('Krum',1.78,120,'m','1983-05-25','blalalalalalalalalalalaalal'),
('Goshko',2.00,150,'m','1979-10-18','darabaraarararararra'),
('Ornela',1.65,58.5,'f','1985-06-02','luda rabota'),
('Shalapatamana',1.28,35,'f','2005-08-25','dararaararararara'),
('Pesho',1.89,135.5,'m','1975-05-18','gaggagagggagga')

-- 08.USERS PROBLEMS--
CREATE TABLE [Users](
[Id] BIGINT PRIMARY KEY IDENTITY,
[Username] VARCHAR(30) NOT NULL UNIQUE,
[Password] VARCHAR(26) NOT NULL,
[ProfilePicture] VARBINARY(MAX)
CHECK(DATALENGTH([ProfilePicture])<=900000),
[LastLoginTime] TIME NOT NULL,
[IsDeleted] BIT NOT NULL

)

INSERT INTO[Users]([Username],[Password],[LastLoginTime],[IsDeleted])
VALUES
('kremena','kjhhggdgddd','22:55:00','true'),
('spas','dfdfdf','14:00:15','false'),
('lainoto','125478','15:15:00','true'),
('fuvkoff','458745lk','12:15:12','false'),
('danomine','djudgeisgood','14:10:58','true')
--09.Change pRIMARY KEY--

ALTER TABLE [Users]

DROP CONSTRAINT PK__Users__3214EC079EF57B97
ALTER TABLE [Users]
ADD CONSTRAINT PK_Users_Compilation
PRIMARY KEY([Id],[Username])


 --10.	Add Check Constraint--
 ALTER TABLE[Users]
 ADD CONSTRAINT CHK_Password
 CHECK(LEN(Password)>5)



--13.mOVUIES DATABASE
CREATE DATABASE[Movies]

CREATE TABLE [Directors](
[Id] INT PRIMARY KEY IDENTITY,
[DirectorName] NVARCHAR(50)NOT NULL,
[Notes] NVARCHAR(MAX)

)
ALTER TABLE [Directors]
ALTER COLUMN [Notes] NVARCHAR(MAX)


CREATE TABLE [Genres](
[Id] INT PRIMARY KEY IDENTITY,
[GenreName] NVARCHAR(30)NOT NULL,
 [Notes] NVARCHAR(MAX) 




) 

CREATE TABLE[Categoriey](
[Id] INT PRIMARY KEY IDENTITY,
[CategoryName] NVARCHAR(50) NOT NULL,
[Notes] NVARCHAR(MAX) 




)

CREATE TABLE [Movies](
[Id] INT PRIMARY KEY IDENTITY,
[Title] NVARCHAR(100) NOT NULL,
[DirectorId] INT FOREIGN KEY REFERENCES [Directors]([Id]),
[CopyrightYear] SMALLINT NOT NULL, 
[Length] FLOAT NOT NULL,
[GenreId] INT FOREIGN KEY REFERENCES[Genres]([Id]),
[CategoryId] INT FOREIGN KEY REFERENCES [Categoriey]([Id]),
[Rating] DECIMAL(3,1) NOT NULL,
[Notes] NVARCHAR(MAX)



)
INSERT INTO[Categoriey]([CategoryName],[Notes])
VALUES
('kutec','darabararararararara'),
('boi',null),
('smotanakategoriq','huhuhuhuhuhu'),
('typnq','kakaakakaakakaakak'),
('biva',null)

INSERT INTO[Directors]([DirectorName],[Notes])
VALUES
('ezekel',null),
('typak','pylen smotanqk e toq директор'),
('Rysel Nqkaakuv смотаняк','психо'),
('БЕН АФЛЕК','toq e rejisior ama nishto'),
('Rasel',null)

INSERT INTO[Genres]([GenreName],[Notes])
VALUES
('psiho',null),
('luboven bulsheet','fafafaffafaffaf'),
('zombies bulsheet',null),
('krimi','maitejahhsdhad'),
('love story',null)
 INSERT INTO[Movies]([Title],[DirectorId],[Length],[CategoryId],[CopyrightYear],[GenreId],[Rating])
 VALUES
 ('Vikings',2,2.1,1,2020,2,10),
 ('Gladiator',1,2.5,3,2023,3,10),
 ('Twiilight',5,3,1,2018,4,9),
 ('jackass',2,4,2,1985,3,8),
 ('madness',3,1.5,5,2002,4,6)

 --14.CarRental --
 CREATE DATABASE[CarRental]

 CREATE TABLE[Categories](
[Id] INT PRIMARY KEY IDENTITY,
[CategoryName] NVARCHAR(50) NOT NULL,
[DailyRate] DECIMAL(4,2) NOT NULL,
[WeeklyRate] DECIMAL(4,2) NOT NULL,
[MonthlyRate] DECIMAL(4,2) NOT NULL,
[WeekendRate] DECIMAL(4,2) NOT NULL
 )
 CREATE TABLE[Cars](
 [Id] INT PRIMARY KEY IDENTITY,
 [PlateNumber] NCHAR(8) NOT NULL,
 [Manufacturer] NVARCHAR(50) NOT NULL,
 [Model] NVARCHAR(50) NOT NULL,
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
 ('SPORT',10.5,25,58,80),
 ('LIMUZINA',58,60,25,15),
 ('NEZNAM',60,80,50,20)

 INSERT INTO[Cars]([PlateNumber],[Manufacturer],[Model],[CarYear],[Doors],[Picture],[Condition],[Available],[CategoryId])
 VALUES
 ('CA2584BG','FERARI','neznam',2000,3,null,'nova','true',5),
 ('CB4879CA','BMW','X5',2006,4,NULL,'BRAKMA','false',7),
 ('CB2233CD','MERCEDES','S400',2016,5,NULL,'JAJAAJAJA','true',6)

 
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
 (1,2,3,50,100,300,200,'2023-10-15','2022-10-15',7,17,50,'TRUE'),
 (2,3,3,20,150,170,20,'2023-10-16','2022-11-15',8,80,100,'FALSE'),
  (1,4,3,30,250000,280000,30000,'2023-06-15','2022-12-15',8,28,50,'TRUE')