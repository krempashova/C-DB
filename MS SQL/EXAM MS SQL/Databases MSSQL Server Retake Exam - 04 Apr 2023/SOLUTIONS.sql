--04.delete   

      SELECT
	  Id,
	  [Name]
	  FROM Clients
	  WHERE LEFT(NumberVAT,2)='IT' ID=11 OFFICINA TURBOCAR SNC
	   
	   SELECT*
	   FROM Clients


    

	DELETE 
	FROM Invoices WHERE ClientId=11
	DELETE 
	FROM ProductsClients WHERE ClientId=11
	 DELETE FROM Clients WHERE LEFT(NumberVAT,2)='IT'

	 --05. Invoices by Amount and Date

	 SELECT
	 Number,
	 Currency
	 FROM Invoices
	 ORDER BY Amount DESC, DueDate


	 --06. Products by Category
	 

	   SELECT
	   P.Id,
	   P.[Name],
	   p.Price,
	   c.[Name]AS [CategoryName]

	   FROM Categories AS c
	   JOIN Products AS p
	   ON c.Id= p.CategoryId
	   WHERE c.[Name] IN ('ADR','Others')
	   ORDER BY p.Price DESC


	   --07. Clients without Products
	   --	•	Id Client  Address

	   SELECT
	   c.Id,
	   c.[Name] AS [Client],
	   CONCAT(a.StreetName,' ',a.StreetNumber,', ',a.City,', ',a.PostCode,', ',cnt.[Name])
	   AS [Address]
	   FROM Clients AS c
	   LEFT JOIN ProductsClients AS pc
	   ON c.Id= pc.ClientId
	   LEFT JOIN Addresses AS a
	   ON c.AddressId=a.Id
	   LEFT JOIN Countries AS cnt
	   ON  a.CountryId=cnt.Id
	   WHERE pc.ProductId IS NULL
	   ORDER BY c.[Name]
	    

		--8.	First 7 Invoices


		  --•	Number •	Amount •	Client

		  SELECT TOP(7)
		  i.Number,
		  i.Amount,
		  c.[Name] AS [Client]
		  FROM Invoices AS i
		  JOIN Clients AS c
		  ON i.ClientId=c.Id
		  WHERE i.IssueDate<'2023-01-01' AND i.Currency='EUR'
		  OR  i.Amount>500.00 AND LEFT(c.NumberVAT,2)='DE'
		  ORDER BY i.Number, i.Amount DESC

		  --09. Clients with VAT

	  SELECT
	    c.[Name] AS Client,
	     MAX(p.Price) AS Price,
		 c.NumberVAT
	   FROM ProductsClients AS pc
	 JOIN Clients AS c
	ON pc.ClientId=c.Id
    JOIN Products AS p
	ON pc.ProductId=p.Id
	WHERE c.[Name] NOT LIKE '%KG%'
	GROUP BY c.[Name], C.NumberVAT
	ORDER BY MAX(p.Price) DESC


	--10. Clients by Price

	SELECT
	  c.[Name] AS Client,
	  FLOOR(AVG(p.Price)) AS [Average Price]
	FROM Clients AS c
	JOIN ProductsClients AS pc
	ON c.Id=pc.ClientId
	JOIN Products AS p
	ON pc.ProductId=p.Id
	JOIN Vendors AS v
	ON  p.VendorId=v.Id
	WHERE LEFT(v.NumberVAT,2) ='FR'
	GROUP BY c.[Name]
	ORDER BY AVG(p.Price), c.[Name] DESC


	--11. Product with Clients

	GO
	CREATE  FUNCTION udf_ProductWithClients(@name NVARCHAR(35))
	  RETURNS INT
	    AS 
		  BEGIN 


		        DECLARE @soldproduct INT=(
				
				                           SELECT
										   COUNT(pc.ClientId)
										   FROM
										   Products AS p
										   JOIN ProductsClients AS pc
										   ON p.Id=pc.ProductId
										   WHERE P.[Name]=@name
										   GROUP BY P.[Name]
			 )

			 RETURN @soldproduct
		  END

	
	  GO
	  SELECT dbo.udf_ProductWithClients('DAF FILTER HU12103X')


	  --12. Search for Vendors from a Specific Country
	          GO
			CREATE PROCEDURE usp_SearchByCountry @country NVARCHAR(10)
			  AS 
			    BEGIN
				              SELECT
						     v.[Name] AS Vendor,
						     v.NumberVAT AS VAT,
						  CONCAT(a.StreetName,' ',a.StreetNumber) AS [Street Info],
						  CONCAT(a.City,' ',a.PostCode) AS [City Info]
						  FROM  Vendors AS v
						  JOIN Addresses AS a
						  ON v.AddressId= a.Id
						  JOIN Countries  AS c
						  ON c.Id=a.CountryId
						  WHERE c.[Name]=@country
						  ORDER BY v.[Name], a.City
						     


				  END

				  EXEC usp_SearchByCountry 'France'

	

	