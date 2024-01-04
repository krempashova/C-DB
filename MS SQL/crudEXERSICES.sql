--02.Problem

SELECT *
FROM[Departments]
--3.Problem

SELECT [Name]
FROM[Departments]
--4.Problem
SELECT [FirstName],
       [LastName],
	   [Salary]
FROM[Employees]
--5.Problem
SELECT [FirstName],
       [MiddleName],
        [LastName]
FROM [Employees]
--06.Problem
SELECT CONCAT([FirstName],'.',[LastName],'@','softuni.bg')
AS[Full Email Address]

FROM [Employees]
--07.problem

SELECT DISTINCT [Salary]
FROM[Employees]
--08.PROBLEM
SELECT*
FROM[Employees]
WHERE [JobTitle] IN('Sales Representative')
--09.Problem
SELECT [FirstName],
[LastName],
[JobTitle]
FROM[Employees]
WHERE [Salary] BETWEEN 20000 AND 30000
--10.Problem
SELECT CONCAT([FirstName],' ',[MiddleName],' ',[LastName])
AS[Full Name]
FROM [Employees]
WHERE [Salary] IN(25000, 14000, 12500 , 23600)
--11.pROBLEM
SELECT[FirstName],
      [LastName]
FROM[Employees]
WHERE [ManagerID] IS NULL

--12.Problem

SELECT  
   [FirstName],
    [LastName],
	[Salary]
FROM [Employees]
WHERE[Salary]>50000
ORDER BY [Salary]DESC

  --13.Problem
  SELECT  TOP(5)
  [FirstName],
  [LastName]

  FROM [Employees]
  ORDER BY [Salary]DESC
  

  --14.Problem
  SELECT 
  [FirstName],
  [LastName]
  FROM [Employees]
  WHERE NOT  [DepartmentID] =4

  --15.Problem
  SELECT *
  FROM [Employees]
  ORDER BY
         [Salary]DESC,
         [FirstName],
		 [LastName] DESC,
		 [MiddleName]

		 --.16.pROBLEM
		 GO
		  CREATE VIEW [V_EmployeesSalaries]
		     AS(
		     SELECT [FirstName],
			        [LastName],
					[Salary]
					FROM[Employees]
		       )
			   GO
		 SELECT * FROM

		 V_EmployeesSalaries

		 --17.Problem
		 GO
		 USE[SoftUni]
		 GO
	    CREATE VIEW [V_EmployeeNameJobTitle]
         AS
            (
                SELECT [FirstName] + ' ' + ISNULL([MiddleName], 'Replacement') + ' ' + [LastName]
                    AS [Full Name],
                       [JobTitle]
                  FROM [Employees] 
            )
 
GO
 
SELECT * FROM [V_EmployeeNameJobTitle]

--18.Problem
SELECT  DISTINCT [JobTitle]
FROM [Employees] 
--19.Problem

SELECT TOP(10)*

FROM [Projects]
ORDER BY[StartDate],  
         [Name]


		 --20.Problem
		 SELECT TOP(7)
		 [FirstName],
		 [LastName],
		 [HireDate]
		 FROM[Employees]
		 ORDER BY [HireDate] DESC

		 --21.Problem

		SELECT *
  FROM [Employees]
 
SELECT [DepartmentID]
  FROM [Departments]
 WHERE [Name] IN ('Engineering', 'Tool Design', 'Marketing', 'Information Services')

 SELECT [Salary]
 FROM[Employees]
 WHERE[DepartmentID] IN(1,2,4,11)


 UPDATE [Employees]
 SET [Salary]+=[Salary]*0.12
 WHERE[DepartmentID] IN (1,2,4,11)
  SELECT [Salary]
  FROM [Employees]

  --22.problem
  USE[Geography]
  GO


  SELECT [PeakName]
  FROM [Peaks]
  ORDER BY[PeakName]

  GO
  --23.Problem
  SELECT TOP(30) [CountryName],
         [Population]
          
  FROM [Countries]
  WHERE [ContinentCode] IN('EU')
  ORDER BY [Population] DESC

  --24.Problem

  SELECT [CountryName],
         [CountryCode],
         CASE [CurrencyCode]
              WHEN 'EUR' THEN 'Euro'
              ELSE 'Not Euro'
         END
      AS [Currency]
    FROM [Countries]
ORDER BY [CountryName]


USE[Diablo]

--24.problem
SELECT[Name]
FROM[Characters]
ORDER BY[Name]
