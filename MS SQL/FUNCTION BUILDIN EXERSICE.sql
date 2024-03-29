USE[SoftUni]

--01
SELECT [FirstName],
[LastName]
FROM [Employees]
WHERE LEFT([FirstName],2)='Sa'

--02
SELECT [FirstName],
[LastName]
FROM [Employees]
WHERE [LastName] LIKE '%ei%'

--03
SELECT [FirstName]
FROM [Employees]
WHERE [DepartmentID] IN(3,10) AND YEAR([HireDate]) BETWEEN 1995 AND 2005

--04
SELECT 
[FirstName],
[LastName]
FROM [Employees]
WHERE CHARINDEX('engineer',[JobTitle])=0

--05
SELECT 
[Name]
FROM [Towns]
WHERE LEN([Name]) IN (5,6)
ORDER BY[Name] 

--06
SELECT *

FROM [Towns]
WHERE LEFT ([NAME],1) IN ('M','K', 'B','E')
ORDER BY [Name]

--07
SELECT *

FROM [Towns]
WHERE LEFT([Name],1) NOT LIKE'[RBD%]'
ORDER BY [Name]
 
--08
CREATE VIEW V_EmployeesHiredAfter2000 
 AS
SELECT [FirstName],[LastName]
FROM [Employees]
WHERE YEAR([HireDate])>2000

SELECT * 
FROM V_EmployeesHiredAfter2000

--09
SELECT 
[FirstName],
[LastName]
FROM [Employees]
WHERE LEN([LastName])=5

---RANKING
--10.
SELECT 
   [EmployeeID],
  [FirstName],
  [LastName],
  [Salary],
   DENSE_RANK() OVER (PARTITION BY[Salary] ORDER BY[EmployeeID])
   AS[Rank]
FROM [Employees]
WHERE [Salary] BETWEEN 10000 AND 50000
ORDER BY[Salary] DESC

--11
SELECT *
FROM
(
   SELECT
     [EmployeeID],
     [FirstName],
      [LastName],
        [Salary],
       DENSE_RANK() OVER (PARTITION BY[Salary] ORDER BY[EmployeeID])
	   AS [Rank]
     FROM [Employees]
      WHERE [Salary] BETWEEN 10000 AND 50000
   )
     AS [SubquerybYRanking]
    WHERE [Rank]=2
	ORDER BY [Salary] DESC


	 USE[Geography]

	 --12.
	 SELECT 
	 [CountryName] AS [Country Name],
	 [IsoCode] AS [ISO Code]
	 FROM [Countries]
	 WHERE LOWER([CountryName]) LIKE '%a%a%a%'
	 	 ORDER BY [IsoCode]


		 --13
		 SELECT 
		[p].[PeakName],
		[r].[RiverName],
		LOWER(CONCAT(SUBSTRING([p].[PeakName],1,LEN([p].[PeakName])-1),[r].[RiverName]))
		AS [Mix]
		 FROM 
		 [Peaks] 
		 AS [p],
		 [Rivers]
		 AS [r]
		 WHERE RIGHT(lOWER([p].[PeakName]),1)=LEFT(LOWER([r].[RiverName]),1)
		 ORDER BY [Mix]


		 USE [Diablo]
		 --14
		 SELECT TOP(50)
		 [Name],
		    FORMAT(CAST(Start AS DATE), 'yyyy-MM-dd') AS [Start]
		 FROM [Games]
		 WHERE YEAR([Start])BETWEEN 2011 AND 2012
		 ORDER BY [Start],[Name]

		 

		 --15
		 SELECT [Username],
           RIGHT(Email, LEN([Email])-CHARINDEX('@', [Email])) AS [Email Provider]
            FROM [Users]
              ORDER BY [Email Provider],
                [Username]
	
	--16
	SELECT
	[Username],
	[IpAddress]  AS [IP Address]
	FROM [Users]
	WHERE 	[IpAddress] LIKE'___.1%.%.___'
	ORDER BY [Username]
		
	
	--17
	SELECT 
	[Name] AS [Game],
	CASE
	WHEN DATEPART(HOUR, [Start])>=0 AND DATEPART(HOUR,[Start])<12  THEN 'Morning'
	WHEN DATEPART(HOUR,[Start])>=12 AND DATEPART(HOUR,[Start])<18  THEN 'Afternoon'
	WHEN DATEPART(HOUR,[Start])>=18 AND DATEPART(HOUR,[Start])<24  THEN  'Evening'
	END
	AS [Part of the Day],
	CASE 
	WHEN [Duration]<=3 THEN 'Extra Short'
	WHEN [Duration] BETWEEN 4 AND 6 THEN 'Short'
	WHEN [Duration]>6 THEN 'Long'
	ELSE 'Extra Long'
	END
	AS [Duration]
	FROM[Games]
	ORDER BY [Game],
	[Duration],
	[Part of the Day]


	--18

	SELECT
	[ProductName],
	[OrderDate],
	DATEADD(day,3,[OrderDate]) AS [Pay due],
	DATEADD(month,1,[OrderDate]) AS [Deliver Due]
	FROM[Orders]



