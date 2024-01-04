USE [SoftUni]
 --01.PROBLEM
  GO
    CREATE PROCEDURE [usp_GetEmployeesSalaryAbove35000]
          AS
	         BEGIN
			       SELECT
				    [FirstName],
					[LastName]
				   FROM[Employees]
				   WHERE [Salary]>35000

	          END
			 
			 EXEC [dbo].[usp_GetEmployeesSalaryAbove35000]


			 --02.pRIBLEM
			 GO
			CREATE PROCEDURE [usp_GetEmployeesSalaryAboveNumber] @minSalary DECIMAL(18,4)
			  AS
			    BEGIN 

				   SELECT 
				   [FirstName],
				   [LastName]
				   FROM[Employees]
				   WHERE [Salary]>=@minSalary
				 END
				 EXEC [dbo].[usp_GetEmployeesSalaryAboveNumber] 48100

				 --03. Town Names Starting With
				 GO
				 CREATE PROCEDURE [usp_GetTownsStartingWith] @startLetter NVARCHAR(MAX)
				   AS 
				 BEGIN

				       SELECT
					   [Name]
					   AS [Town]
					   FROM[Towns]
					   WHERE [Name] LIKE (@startLetter+'%')
				 END
				  EXEC [dbo].[usp_GetTownsStartingWith] 'b'

				  --04. Employees from Town
				  GO
				  CREATE PROCEDURE [usp_GetEmployeesFromTown] @searchingTown NVARCHAR(50)
				  AS
				     BEGIN

				   SELECT 
				   [e].[FirstName],
				   [e].[LastName]
				   FROM[Employees]
				   AS[e]
				   JOIN[Addresses]
				   AS [a]
				   ON [e].AddressID=[a].[AddressID]
				   JOIN[Towns]
				   AS [t]
				   ON [a].[TownID]=[t].[TownID]
				   WHERE [t].[Name]= @searchingTown

				   END
				   EXEC [dbo].[usp_GetEmployeesFromTown] 'Sofia'

				   --05. Salary Level Function
				    GO
				     CREATE  FUNCTION  ufn_GetSalaryLevel (@salary DECIMAL(18,4))
                        RETURNS VARCHAR(8)
					    AS 
                        BEGIN
						    DECLARE @SalaryLevel VARCHAR(8)

						IF @Salary<30000
							  BEGIN
							       SET @SalaryLevel='Low'
							  END
						ELSE IF @Salary BETWEEN 30000 AND 50000
						      BEGIN
							    SET @SalaryLevel='Average'
							  END
						ELSE IF @Salary >50000
						      BEGIN
							    SET @SalaryLevel='High'
							  END
                             RETURN @salaryLevel
                        END
						
						--06.	Employees by Salary Level
						 GO
						CREATE PROCEDURE [usp_EmployeesBySalaryLevel] @salaryLevel VARCHAR(8)
						AS 
						BEGIN 
						     SELECT 
							 [FirstName],
							 [LastName]
							 FROM[Employees]
							 WHERE [dbo].[ufn_GetSalaryLevel]([Salary])=@salaryLevel

						END

						EXEC [dbo].[usp_EmployeesBySalaryLevel]'Low'

						--07. Define Function
						  GO

						CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(50), @word VARCHAR(50)) 
						  RETURNS BIT
						  AS 
						  BEGIN 

						     DECLARE @wordIndex INT =1;

							 WHILE(@wordIndex <= LEN(@word))
							   BEGIN 

							     DECLARE @currentChar CHAR=Substring(@word,@wordIndex,1)

								   IF CHARINDEX(@currentChar,@setOfLetters)=0
								     BEGIN
									    RETURN 0;
										END
								SET @wordIndex+=1;

							   END 

							   RETURN 1;
						  END


						  GO
						  SELECT [dbo].[ufn_IsWordComprised] ('oistmiahf','Sofia')

						  --8.	Delete Employees and Departments
						   GO
						  CREATE PROCEDURE usp_DeleteEmployeesFromDepartment (@departmentId INT)
						    AS
							 BEGIN 

							 --******* WE need to store all the id that going to be deleted
							 DECLARE @employeeTodelete TABLE([Id] INT);
							   INSERT INTO @employeeTodelete
							     SELECT[EmployeeID]
								 FROM[Employees]
								 WHERE [DepartmentID]=@departmentId;

								 --+**** this employesss can be owrking for a project so we delete the first relation
								 DELETE 
								 FROM[EmployeesProjects]
								WHERE [EmployeeID] IN
								                             (
															 SELECT*
															 FROM @employeeTodelete
															
								                             )
            ---****** THIS employees can be manager to someone delete second relation
			--** first we need to make Mangerid NULLABLE:
			ALTER TABLE[Departments]
			ALTER COLUMN[ManagerID] INT

			UPDATE [Departments]
			SET [ManagerID]=NULL
			WHERE      [ManagerID] IN ( SELECT * FROM @employeeTodelete)
															
          --***** LAST THE EMPLOYEES WHIS WE GONNA REMOVED , CAN BE MANAGERS TO SOMEBODY, SO WE NEED TO DELETE THE LAST RELATION:
		UPDATE [Employees]
		SET [ManagerID]=NULL
		WHERE [ManagerID] IN( SELECT * FROM @employeeTodelete)


		--*** SINNCE WE DONT HAVE ANY RELATIONS WE CAN REMOVE THE EMPLOYESS

		 DELETE 
		 FROM[Employees]
		 WHERE [DepartmentID]=@departmentId

		  DELETE 
		  FROM[Departments]
		  WHERE [DepartmentID]=@departmentId


		   SELECT COUNT(*)
		   FROM[Employees]
		   WHERE [DepartmentID]=@departmentId




	END
	 EXEC dbo.usp_DeleteEmployeesFromDepartment 7
  GO

	 USE[Bank]
	 ----------9.	Find Full Name

	 GO
	  CREATE PROCEDURE [usp_GetHoldersFullName]
	    AS
		 BEGIN
		      SELECT
			  CONCAT([FirstName], ' ',[LastName])
			  AS [Full Name]
			  FROM[AccountHolders]
		  END

		  EXEC [dbo].usp_GetHoldersFullName

		  --10. People with Balance Higher Than
		  GO
		  CREATE PROCEDURE [usp_GetHoldersWithBalanceHigherThan] @balance DECIMAL(10,2)
		    AS 
			  BEGIN 

			      SELECT 
				  [FirstName]
				  AS[First Name],
				  [LastName]
				  AS[Last Name]

				  FROM[AccountHolders]
				  AS [ah]
				  JOIN[Accounts] 
				  AS [a] 
				ON [ah].Id=[a].AccountHolderId
				GROUP BY [ah].[FirstName], [ah].[LastName]
				HAVING SUM([a].[Balance])>@balance
				ORDER BY [ah].[FirstName],[ah].[LastName]

			  END
			  EXEC [dbo].[usp_GetHoldersWithBalanceHigherThan]25000.56

			  ------************11.	Future Value Function
			    GO
			  CREATE FUNCTION [ufn_CalculateFutureValue](@sum DECIMAL(8,2), @yearlyInterestRate FLOAT, @numberOfYears INT)
			         RETURNS DECIMAL(18,4)
					   AS 
					   BEGIN 



					    RETURN @sum*(POWER(1+@yearlyInterestRate,@numberOfYears))
					   END


					   GO
					   SELECT [dbo].[ufn_CalculateFutureValue] (1000,0.1,5)


					   --*********12. Calculating Interest

					   GO
					   CREATE PROCEDURE [usp_CalculateFutureValueForAccount]@AccountId INT , @interestRate FLOAT
					   AS 
					   BEGIN 

					       SELECT
						       [a].[Id]
							   AS[Account Id],
						        [ah].[FirstName] 
						         AS[First Name],
						         [ah].[LastName] 
							      AS[Last Name],
							   [a].[Balance]
							AS[Current Balance],
							 [dbo].[ufn_CalculateFutureValue] ([a].[Balance], @interestRate,5)
							 AS[Balance in 5 years] 
						         FROM [AccountHolders] AS [ah]
							     JOIN[Accounts] AS[a]
							      ON [ah].[Id]=[a].[AccountHolderId]
								  WHERE [a].[Id]=@AccountId
					   END


					  EXEC [usp_CalculateFutureValueForAccount]1,0.1