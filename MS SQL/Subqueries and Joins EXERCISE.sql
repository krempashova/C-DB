--01.Employee Address

   SELECT  TOP(5)
   [e].[EmployeeID],
   [e].[JobTitle],
   [e].[AddressID],
   [a].[AddressText]
    FROM [Employees] AS [e]
	LEFT JOIN [Addresses] AS [a]
	ON [e].[AddressID]=[a].[AddressID]
	ORDER BY [e].[AddressID]

	--2.	Addresses with Towns
	--•	FirstName LastName Town AddressText
	SELECT  TOP(50)
	[e].[FirstName],
	[e].[LastName],
	[t].[Name] AS [Town],
	[a].[AddressText]

	FROM[Employees] AS [e]
	LEFT JOIN [Addresses] AS [a]
	ON [e].[AddressID]=[a].[AddressID]
	LEFT JOIN[Towns] AS [t]
	ON [a].[TownID]=[t].[TownID]
	ORDER BY [e].[FirstName],[e].[LastName]

    --3.	Sales Employee

	--•	EmployeeID •	FirstName •	LastName •	DepartmentName
	SELECT 
	[e].[EmployeeID],
	[e].[FirstName],
	[e].[LastName],
	[d].[Name] AS [DepartmentName]
	FROM [Employees] AS [e]
	JOIN [Departments] AS [d]
	ON [e].[DepartmentID]=[d].[DepartmentID] 
	AND [d].[Name]='Sales'
	ORDER BY [e].[EmployeeID]
	

	--.4 Employee Departments
	--•	EmployeeID •	FirstName  •	Salary •	DepartmentName

	 SELECT  TOP(5)
	 [e].[EmployeeID],
	[e].[FirstName],
	[e].[Salary],
	[d].[Name] AS [DepartmentName]
		 FROM [Employees] AS [e]
	      JOIN [Departments] AS [d]
	        ON [e].[DepartmentID]=[d].[DepartmentID] 
	        AND [e].[Salary]>15000
	          ORDER BY [e].[DepartmentID]


--05. Employees Without Projects
 SELECT  TOP(3)
 [e].[EmployeeID],
 [e].[FirstName]
 FROM [Employees] AS [e]
 LEFT JOIN[EmployeesProjects] AS [ep]
 ON [e].[EmployeeID]=[ep].[EmployeeID]
 WHERE [ep].[ProjectID] IS NULL
 ORDER BY [e].[EmployeeID]

 --06. Employees Hired After
 --•	FirstName •	LastName •	HireDate •	DeptName

 SELECT 
 [e].[FirstName],
 [e].[LastName],
 [e].[HireDate],
 [d].[Name] AS [DeptName]
 FROM[Employees] AS[e]
 JOIN [Departments] AS[d]
 ON [e].DepartmentID=[d].[DepartmentID]
 AND [e].[HireDate]>'1999-01-01' AND [d].[Name] IN('Sales','Finance')
 ORDER BY [e].[HireDate]


 --07. Employees With Project
      SELECT  TOP(5)
	  [e].[EmployeeID],
	  [e].[FirstName],
	  [p].[Name] AS [ProjectName]
       FROM [EmployeesProjects] 
    AS[ep]
	  JOIN[Employees]
	  AS [e]
	  ON[ep].[EmployeeID]= [e].[EmployeeID]
	  JOIN[Projects] 
	  AS [p]
	  ON [ep].[ProjectID]=[p].[ProjectID]
	  WHERE [p].[StartDate]>'2002-08-13' AND [p].[EndDate] IS NULL
	  ORDER BY [e].[EmployeeID]

	  --08. Employee 24
	  --•	EmployeeID •	FirstName •	ProjectName
	  SELECT 
	  [e].[EmployeeID],
	  [e].[FirstName],
	  CASE 
	  WHEN [p].[StartDate]>='2005' 
	  THEN NULL
	  ELSE [p].[Name]
	  END AS[ProjectName]
	 
	   FROM [EmployeesProjects] 
    AS[ep]
	  JOIN[Employees]
	  AS [e]
	  ON[ep].[EmployeeID]= [e].[EmployeeID]
	  JOIN[Projects] 
	  AS [p]
	  ON [ep].[ProjectID]=[p].[ProjectID]
	  WHERE [e].[EmployeeID]=24 
	 

	 --09. Employee Manager
	  --•	EmployeeID •	FirstName •	ManagerID •	ManagerName
	  SELECT 
	      [e].[EmployeeID],
	      [e].[FirstName],
	      [e].[ManagerID],
	      [m].[FirstName] 
   AS [ManagerName]
	      FROM [Employees] 
  AS [e]
	  JOIN [Employees] 
	 AS [m]
	    ON [e].[ManagerID]=[m].[EmployeeID]
	  WHERE [e].[ManagerID] IN (3,7)
	  ORDER BY	[e].[EmployeeID]

	  --10.Employees Summary
	--•	EmployeeID •	EmployeeName •	ManagerName •	DepartmentName

	      SELECT TOP(50)
		     [e].[EmployeeID],
			 CONCAT([e].[FirstName],' ',[e].[LastName]) AS [EmployeeName],
			 CONCAT([m].[FirstName],' ', [m].[LastName]) AS [ManagerName],
			 [d].[Name] AS [DepartmentName]
	       FROM [Employees] 
             AS [e]
	        JOIN [Employees] 
	          AS [m]
	        ON [e].[ManagerID]=[m].[EmployeeID]
			  JOIN [Departments] AS[d]
			  ON [e].[DepartmentID]=[d].[DepartmentID]
		        ORDER BY	[e].[EmployeeID]

				--11. Min Average Salary
			
			SELECT MIN([Average].[AverageSalary])
			AS [MinAverageSalary]
          FROM
			(
			SELECT AVG(Salary) AS [AverageSalary]
			FROM [Employees] 
			GROUP BY [DepartmentID]
			)
			AS [Average]

			--12. Highest Peaks in Bulgaria
			USE[Geography]
			--•	CountryCode •	MountainRange •	PeakName •	Elevation
			SELECT
			[mc].[CountryCode],
			[m].[MountainRange],
			[p].[PeakName],
			[p].[Elevation]
			FROM[MountainsCountries]
			AS [mc]
			JOIN [Mountains]
			AS[m]
			ON [mc].[MountainId]=[m].[Id]
			JOIN[Peaks] 
			AS[p]
			ON [m].[Id]=[p].[MountainId]
			WHERE [mc].[CountryCode]='BG' AND [p].[Elevation]>2835
			ORDER BY [p].[Elevation] DESC

			--13.	Count Mountain Ranges
			--•	CountryCode •	MountainRanges

       SELECT 
	   [CountryCode],
	   COUNT([MountainId])
	   AS [MountainRanges]
   FROM [MountainsCountries]
   WHERE [CountryCode] IN('BG','US','RU')
   GROUP BY[CountryCode]
  
  --14. Countries With or Without Rivers
    SELECT TOP (5) 
	          [c].[CountryName],
               [r].[RiverName]
           FROM [Countries] 
	AS [c]
          LEFT JOIN [CountriesRivers] 
	 AS [cr] 
	     ON [c].[CountryCode] = [cr].[CountryCode]
         LEFT JOIN [Rivers] 
		 AS [r] 
		    ON [cr].[RiverId] = [r].[Id]
          JOIN [Continents] 
		  AS [cnt] 
		  ON [c].[ContinentCode] = [cnt].[ContinentCode]
         WHERE [cnt].[ContinentName] = 'Africa'
       ORDER BY [c].[CountryName]


	   --1616.	Countries Without Any Mountains
	   --Create SELECT COUNT(c.CountryCode) AS CountryCode
      SELECT
	  COUNT([cnt].[CountryCode])
	  AS [Count]
	  FROM[Countries]
	  AS [cnt]
	  LEFT JOIN [MountainsCountries]
	  AS[mcnt]
	  ON [cnt].[CountryCode]=[mcnt].[CountryCode]
	  WHERE [mcnt].[CountryCode] IS NULL

	  --17. Highest Peak and Longest River by Country
	  SELECT TOP(5)
	  [c].[CountryName]
	   AS [countryName],
	  MAX([p].[Elevation])
	  AS[HighestPeakElevation],
	  MAX([r].[Length])
	  AS[LongestRiverLength]
	 
	  FROM[Countries]
	  AS [c]
	  LEFT JOIN[MountainsCountries]
	  AS [mc]
	  ON [c].[CountryCode]=[mc].[CountryCode]
	  LEFT JOIN[Mountains]
	  AS[m]
	  ON [mc].[MountainId]=[m].[Id]
	  LEFT JOIN[Peaks]
	 AS [p]
	 ON [m].[Id]=[p].[MountainId]
	 LEFT JOIN[CountriesRivers]
	 AS[cr]
	 ON [c].[CountryCode]=[cr].[CountryCode]
	 LEFT JOIN[Rivers]
	 AS[r]
	 ON [cr].[RiverId]=[r].[Id]
	 GROUP BY [c].[CountryName]
	 ORDER BY [HighestPeakElevation] DESC,
	 [LongestRiverLength] DESC,
	[c].[CountryName]

	--18
	SELECT
	[c].[CountryName]
	AS[Country],


	FROM[Countries]
	AS[c]
	LEFT JOIN[MountainsCountries]
	AS[mc]
	ON [c].[CountryCode]=[mc].[CountryCode]
	LEFT JOIN[Mountains]
	AS [m]
	ON [mc].[MountainId]=[m].[Id]
	LEFT JOIN[Peaks]
	AS [p]
	ON [m].[Id]=[p].[MountainId]