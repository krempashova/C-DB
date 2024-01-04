CREATE DATABASE [NationalTouristSitesOfBulgaria]

USE [NationalTouristSitesOfBulgaria]

CREATE TABLE Categories
(
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL

)
 CREATE TABLE Locations
 (
 Id INT PRIMARY KEY IDENTITY,
 [Name] VARCHAR(50) NOT NULL,
 Municipality VARCHAR(50),
 Province VARCHAR(50)
  )

   CREATE TABLE Sites
   (
   Id INT PRIMARY KEY IDENTITY,
   [Name] VARCHAR(100) NOT NULL,
   LocationId INT FOREIGN KEY REFERENCES Locations(Id)  NOT NULL,
   CategoryId INT FOREIGN KEY REFERENCES Categories(Id)  NOT NULL,
   Establishment VARCHAR(15)
   )
    CREATE TABLE Tourists
	(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	Age INT NOT NULL CHECK (Age BETWEEN 0 AND 120),
	PhoneNumber VARCHAR(20) NOT NULL,
	Nationality VARCHAR(30) NOT NULL,
	Reward VARCHAR(20)
	)

	CREATE TABLE SitesTourists
	(
	TouristId INT  FOREIGN KEY REFERENCES Tourists(Id) NOT NULL,
	SiteId INT FOREIGN KEY REFERENCES Sites(Id) NOT NULL,
	PRIMARY KEY (TouristId,SiteId)
	)

	CREATE TABLE BonusPrizes
	(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL
	)

	CREATE TABLE TouristsBonusPrizes
	(
	TouristId INT  FOREIGN KEY REFERENCES Tourists(Id) NOT NULL,
	BonusPrizeId INT  FOREIGN KEY REFERENCES BonusPrizes(Id) NOT NULL,
	PRIMARY KEY (TouristId,BonusPrizeId)
	
	)
	  --INSERT
	  INSERT INTO Tourists([Name],Age,PhoneNumber,Nationality,Reward)
	  VALUES
	  ('Borislava Kazakova',52,'+359896354244','Bulgaria', NULL),
	   ('Peter Bosh',48,'+447911844141','UK', NULL),
	    ('Martin Smith',29,'+353863818592','Ireland', 'Bronze badge'),
		 ('Svilen Dobrev',49,'+359986584786','Bulgaria','Silver badge'),
		  ('Kremena Popova',38,'+359893298604','Bulgaria', NULL)

		  INSERT INTO Sites([Name],LocationId,CategoryId,Establishment)
		    VALUES
			('Ustra fortress',90,7,'X'),
			('Karlanovo Pyramids',65,7,NULL),
			('The Tomb of Tsar Sevt',63,8,'V BC'),
			('Sinite Kamani Natural Park',17,1,NULL),
			('St. Petka of Bulgaria – Rupite',92,6,'1994')
			--03. Update

			UPDATE Sites
			 SET Establishment='not defined'
			 WHERE Establishment IS NULL
			

			--04. Delete

			SELECT*FROM TouristsBonusPrizes
		
			

			 DELETE FROM TouristsBonusPrizes
			 WHERE TouristId IN (18,20)

			 DELETE FROM BonusPrizes
			 WHERE [Name]='Sleeping bag '

			 --05. Tourists

			 SELECT
			  [Name],
			   Age,
			   PhoneNumber,
			   Nationality

			   FROM Tourists
			   ORDER BY Nationality,Age DESC,[Name]

			   --06. Sites with Their Location and Category
			   --Site	Location	Establishment	Category

			     SELECT
				  s.[Name] AS [Site],
				  l.[Name] AS [Location],
				  s.Establishment,
				  c.[Name] AS Category
				   FROM Sites AS s
				   JOIN Locations AS l
				   ON s.LocationId=l.Id
				  JOIN Categories AS c
				  ON c.Id=s.CategoryId
				  ORDER BY c.[Name] DESC,l.[Name],s.[Name]

				  --07. Count of Sites in Sofia Province
				  --Province	Municipality	Location	CountOfSites
				 
				   SELECT
				    l.Province,
					l.Municipality,
					l.[Name] AS [Location],
					COUNT(s.Id) AS CountOfSites
				  FROM Locations AS l
				  JOIN Sites AS s
				  ON l.Id=s.LocationId
				 WHERE Province='Sofia'
				 GROUP BY l.[Name],l.Municipality,l.Province
				 ORDER BY COUNT(S.Id) DESC,l.[Name]

				 --08. Tourist Sites established BC
				 --Site	Location	Municipality	Province	Establishment

				  SELECT
				   s.[Name] AS [Site],
				   l.[Name] AS [Location],
				   l.Municipality,
				   l.Province,
				   s.Establishment

				  FROM Sites AS s
				  JOIN Locations AS l
				  ON s.LocationId=l.Id
				  WHERE SUBSTRING(l.[Name],1,1) NOT IN('M', 'B','D')
				 AND s.Establishment LIKE '_%BC'
				 ORDER BY s.[Name]

				 --09. Tourists with their Bonus Prizes

				 --Name	Age	PhoneNumber	Nationality	Reward

				  SELECT
				   t.[Name],
				   t.Age,
				   t.PhoneNumber,
				   t.Nationality,
				    CASE 
					WHEN bp.[Name] IS NULL THEN '(no bonus prize)'
					ELSE bp.[Name]
					END
					AS Reward
				  FROM Tourists AS t
				  LEFT JOIN TouristsBonusPrizes AS tbp
				  ON t.Id=tbp.TouristId
				  LEFT JOIN BonusPrizes AS bp
				  ON tbp.BonusPrizeId=bp.Id
				  ORDER BY t.[Name]

				  --10. Tourists visiting History & Archaeology sites
				  --LastName	Nationality	Age	PhoneNumber
				  SELECT
				   CASE
                      WHEN CHARINDEX(' ', t.[Name]) > 0
                      THEN REVERSE(SUBSTRING(REVERSE(t.[Name]),1,CHARINDEX(' ', REVERSE(t.[Name])) - 1))
					  END 
					  AS LastName,
				  t.Nationality,
				  t.Age,
				  t.PhoneNumber
				  FROM Tourists AS t
				  JOIN SitesTourists AS st
				  ON t.Id=st.TouristId
				  JOIN Sites AS s
				  ON st.SiteId=s.Id
				  JOIN Categories AS c
				  ON s.CategoryId=c.Id
				  WHERE c.[Name] ='History and archaeology'
				  GROUP BY t.[Name],t.Nationality,t.Age,t.PhoneNumber
				  ORDER BY(
				  
				   SELECT
				   CASE
                      WHEN CHARINDEX(' ', t.[Name]) > 0
                      THEN REVERSE(SUBSTRING(REVERSE(t.[Name]),1,CHARINDEX(' ', REVERSE(t.[Name])) - 1))
					  END 
				  
				          )
						  --11. Tourists Count on a Tourist Site
						  GO
						  CREATE FUNCTION udf_GetTouristsCountOnATouristSite (@Site VARCHAR(100))
						     RETURNS INT
							   AS
							     BEGIN
								    DECLARE @countOfvisitors INT =(
									
									  SELECT
									   COUNT(st.TouristId)
									  FROM SitesTourists AS st
									  JOIN Sites AS s
									  ON st.SiteId=s.Id
									  WHERE s.[Name]=@Site
									  GROUP BY s.[Name]

									
									
									
									                             )


                                     RETURN @countOfvisitors
								 END
								 GO
								 SELECT dbo.udf_GetTouristsCountOnATouristSite ('Regional History Museum – Vratsa')


								 --12. Annual Reward Lottery

								 GO

								 CREATE PROCEDURE usp_AnnualRewardLottery @TouristName VARCHAR(50)
								   AS 
								      BEGIN 

									     SELECT
										   t.[Name],
										   CASE 
										   WHEN COUNT(st.SiteId)>=100 THEN 'Gold badge'
										   WHEN COUNT(st.SiteId)>=50 THEN 'Silver badge'
										   WHEN COUNT(st.SiteId)>=25 THEN 'Bronze badge'
										   ELSE NULL
										   END
										   AS Reward


										 FROM Tourists AS t
										 JOIN SitesTourists AS st
										 ON t.Id=st.TouristId
										 WHERE t.[Name]=@TouristName
										 GROUP BY t.[Name]


									  END

									  EXEC usp_AnnualRewardLottery 'Gerhild Lutgard'