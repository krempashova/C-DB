CREATE DATABASE[TouristAgency]
USE TouristAgency

CREATE TABLE Countries
(
Id INT  PRIMARY KEY IDENTITY,
[Name] NVARCHAR(50) NOT NULL

)
 CREATE TABLE Destinations
 (
 Id INT  PRIMARY KEY IDENTITY,
 [Name] VARCHAR(50) NOT NULL,
 CountryId INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL
 
 )
  CREATE TABLE Rooms
  (
  Id INT  PRIMARY KEY IDENTITY,
  [Type] VARCHAR(40) NOT NULL,
  Price DECIMAL(18,2) NOT NULL,
  BedCount INT CHECK (BedCount BETWEEN 1 AND 10) NOT NULL,
  
  )
  CREATE TABLE Hotels
  (
  Id INT  PRIMARY KEY IDENTITY,
  [Name] VARCHAR(50) NOT NULL,
  DestinationId INT FOREIGN KEY REFERENCES Destinations(Id) NOT NULL
  
  )
   CREATE TABLE Tourists
   (
   Id INT  PRIMARY KEY IDENTITY,
   [Name] NVARCHAR(80) NOT NULL,
   PhoneNumber VARCHAR(20) NOT NULL,
   Email VARCHAR(80),
   CountryId INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL
   
   )
    CREATE TABLE Bookings
	(
	Id INT  PRIMARY KEY IDENTITY,
	ArrivalDate DATETIME2 NOT NULL,
	DepartureDate DATETIME2 NOT NULL,
	AdultsCount INT CHECK(AdultsCount BETWEEN 1 AND 10) NOT NULL,
	ChildrenCount INT CHECK(ChildrenCount BETWEEN 0 AND 9) NOT NULL,
	TouristId INT FOREIGN KEY REFERENCES Tourists(Id) NOT NULL,
	HotelId INT FOREIGN KEY REFERENCES Hotels(Id) NOT NULL,
	RoomId INT FOREIGN KEY REFERENCES Rooms(Id) NOT NULL
	
	)
	CREATE TABLE HotelsRooms
	(
	HotelId INT FOREIGN KEY REFERENCES Hotels(Id) NOT NULL,
	RoomId INT FOREIGN KEY REFERENCES Rooms(Id) NOT NULL,
	PRIMARY KEY(HotelId,RoomId)
	)


	--02. Insert
	
	INSERT INTO Tourists([Name],PhoneNumber,Email,CountryId)
	 VALUES 
	 ('John Rivers','653-551-1555','john.rivers@example.com',6),
	  ('Adeline Aglaé','122-654-8726','adeline.aglae@example.com',2),
	   ('Sergio Ramirez','233-465-2876','s.ramirez@example.com',3),
	    ('Johan Müller','322-876-9826','j.muller@example.com',7),
		 ('Eden Smith','551-874-2234','eden.smith@example.com',6)

	  INSERT INTO Bookings(ArrivalDate,DepartureDate,AdultsCount,ChildrenCount,TouristId,HotelId,RoomId)
	   VALUES
	   ('2024-03-01','2024-03-11',1,0,21,3,5),
	    ('2023-12-28','2024-01-06',2,1,22,13,3),
		 ('2023-11-15','2023-11-20',1,2,23,19,7),
		  ('2023-12-05','2023-12-09',4,0,24,6,4),
		   ('2024-05-01','2024-05-07',6,0,25,14,6)
		    
			--03. Update
			 UPDATE Bookings
			 SET DepartureDate=DATEADD(DAY,1,DepartureDate)
			 WHERE MONTH(ArrivalDate)=12 AND YEAR(ArrivalDate)=2023
			
			 UPDATE Tourists
			 SET Email=NULL
			 WHERE [Name] LIKE '%MA%'
			  
			  --04. Delete
			    SELECT* FROM Tourists
				WHERE [Name] LIKE '%Smith%'
			
		DELETE FROM Bookings
		WHERE TouristId IN (6,16,25)

		 DELETE FROM Tourists
		 WHERE [Name] LIKE '%Smith%'


		 --05. Bookings by Price of Room and Arrival Date

		 --ArrivalDate	AdultsCount	ChildrenCount

		  SELECT
		   FORMAT(CAST(b.ArrivalDate AS DATE), 'yyyy-MM-dd') 
		   AS ArrivalDate,
		   b.AdultsCount,
		   b.ChildrenCount
		  FROM Bookings AS b
		  JOIN Rooms AS r
		  ON b.RoomId=r.Id
		  ORDER BY r.Price DESC,b.ArrivalDate 

		  --06. Hotels by Count of Bookings
		  --Id	Name
		  SELECT 
		   h.Id,
		    h.[Name] 
		    
		  FROM 
		  Hotels AS h
		  LEFT JOIN 
		 HotelsRooms AS hr
		  ON h.Id=hr.HotelId
		 LEFT JOIN Rooms AS r
		  ON r.Id=hr.RoomId
		  LEFT JOIN Bookings AS b
		  ON b.HotelId=h.Id
		  WHERE r.[Type]='VIP Apartment'
		  GROUP BY h.[Name],h.Id
		  ORDER BY(
		        SELECT COUNT(b.Id)
				FROM Bookings AS b
				WHERE b.HotelId=h.Id
		
		)DESC
		  
		 
		  --07. Tourists without Bookings
		--  Id	Name	PhoneNumber
		  SELECT 
		    t.Id,
			t.[Name],
			t.PhoneNumber
		  FROM Tourists AS t
		  LEFT JOIN Bookings AS b
		  ON t.Id=b.TouristId
		  WHERE b.TouristId IS NULL
		  ORDER BY t.[Name]
		  
		  --8.	First 10 Bookings
		  --HotelName	DestinationName	CountryName

		  
		     SELECT TOP(10)
			   h.[Name] As HotelName,
			   d.[Name] AS DestinationName,
			   c.[Name] AS CountryName
			 FROM Bookings AS b
			  JOIN Hotels AS h
			 ON b.HotelId=h.Id
			 JOIN Destinations AS d
			 ON h.DestinationId=d.Id
			 JOIN Countries AS c
			 ON d.CountryId=c.Id
			 WHERE b.ArrivalDate<'2023-12-31' AND b.HotelId IN (
			                                                       SELECT  Id AS HotelId
			                                                                    FROM Hotels
			                                                                    WHERE Id %2 <>0
			                                                   )
		  ORDER BY c.[Name], b.ArrivalDate

      --9.	Tourists booked in Hotels
	       --HotelName	RoomPrice
		     SELECT
			   h.[Name]AS HotelName,
			   r.Price AS RoomPrice
			 FROM Tourists AS t
			  JOIN Bookings AS b
			 ON t.Id=b.TouristId
			 Join Hotels AS h
			 ON b.HotelId=h.Id
			 JOIN Rooms AS r
			 ON b.RoomId=r.Id
			 WHERE t.[Name] NOT LIKE '%EZ'
			 ORDER BY R.Price DESC

			 --10. Hotels Revenue
			 --HotelName	HotelRevenue
			  SELECT 
			  h.[Name] AS HotelName,
			     (
			       
		          r.Price*( DATEDIFF(DAY,b.ArrivalDate,b.DepartureDate))
			  
			  
			      ) AS HotelRevenue
			   
			  FROM Bookings AS b
			  JOIN Hotels AS h
			  ON b.HotelId= h.Id
			  JOIN Rooms AS r
			  ON b.RoomId=r.Id
			  GROUP BY h.[Name], r.Price,b.ArrivalDate,b.DepartureDate
			  ORDER BY  ( r.Price*( DATEDIFF(DAY,b.ArrivalDate,b.DepartureDate))) DESC
			       

					     
				 
			                   --11. Rooms with Tourists
							   GO

							  CREATE FUNCTION udf_RoomsWithTourists(@name VARCHAR(40)) 
							    RETURNS INT
								  AS
								    BEGIN
									  DECLARE @countOftourist INT=
									     
										( 
										
										
										    SELECT
									              COUNT( (t.Id) * (b.AdultsCount+b.ChildrenCount))
										 
										                         
										
										FROM
									   Bookings AS b
									   JOIN Tourists AS t
									   ON b.TouristId=T.Id
									   JOIN Rooms AS r
									   ON r.Id=b.RoomId
									   WHERE r.[Type]='Double Room'
									 
										 )
										 RETURN @countOftourist
									END
--12

GO

   CREATE PROCEDURE usp_SearchByCountry  @country VARCHAR(50)
      AS 
       BEGIN 

	       SELECT
		   t.[Name],
		   t.PhoneNumber,
		   t.Email,
		   COUNT(

		         t.CountryId


		       ) as CountOfBookings
		   FROM Bookings
		   as b
		   JOIN Tourists as t
		   ON b.TouristId=t.Id
		   JOIN Countries as C 
		   ON C.Id=t.CountryId
		   WHERE C.[Name]= @country
		   GROUP BY t.[Name],T.PhoneNumber,t.Email
       END
