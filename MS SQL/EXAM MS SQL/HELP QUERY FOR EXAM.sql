HELP FOR EXAM:

--vzemane na sledwash red
   LEAD()OVER(ORDER BY[])AS[]-êîãàòî èñêàìå äà âçåìåì ñëåäâàù ðåä îò ñúùàòà òàáëèöà . áåç äà ïîëçâàìå selfJoin
   âúòðå ñ lead() ïèøåì ðåäà , êîéòî èñêàìå äà âçåìåì, â order by êàê äà ñå ïîäðåäè .

     -- vzemane na predishen red

	 LAG() OVER()


  -- ÊÎÏÈÐÀÍÅ ÍÀ ÒÀÁËÈÖÀ --
  SELECT* INTO[ÈÌÅÒÎ ÍÀ NOVATA] FROM[ÒÀÁËÈÖÀÒÀ ÎÒ ÊÎßÒÎ ÊÎÏÈÐÀØ]
  ÒÓÊ ÌÎÆÅØ È ÄÀ ÑËÎÆÈØ ÓÑËÎÂÈÅ!


    ---òðèåíå îò òàáëèöà
	 DELETE FROM [NewTable]
    WHERE  ìîæåø äà ñëîæèø óñëîâèå!


	--update na ñòîéíîñòè â òàáëèöà!

	UPDATE NewTable
          SET
          Salary += 5000
      WHERE DepartmentID = 1;

	   --- AKO ISKAME DA NE E W GRANICI OT 
	   NOT BETWEEN 30 AND 70- ÒÎÂÀ ÎÇÍÀ×ÀÂÀ , ×Å ÍÅ Å ÃÐÀÍÈÖÈÒÅ 30-70


	   --AKO IMAME POWTARQSHTI SE REDOWE, MOVEM DA IZPOLZWAME DISTINCT
	    SELECT
	    DISTINCT
	   [DepartmentID],
	   [Salary]
	   AS[ThirdHighestSalary]
	    
----------******************************************************************
		------FUNCTIONS AND PROCEDURES--------
		-- ******************************************

		--*Creating procedures SYNTAXIS BEZ PARAMETRI

		 CREATE PROCEDURE [usp_GetEmployeesSalaryAbove35000]
          AS
	         BEGIN
			 --code
			       SELECT
				    [FirstName],
					[LastName]
				   FROM[Employees]
				   WHERE [Salary]>35000

	          END
			 
			 EXEC [dbo].[usp_GetEmployeesSalaryAbove35000]

			 ---creating procedure with parametar
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

				 --- ako  imame imeto na apochva sus:
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

------FUNCTIONS-------------
         CREATE  FUNCTION  imeto (@ podavame parametyr , tupe )
                        RETURNS type of result ( varchar, int )
					    AS 
                        BEGIN
						    DECLARE imeto na resultata s @ i tip 
							-- body:
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
                           !!!!  RETURN @salaryLevel
                        END
						 izvikva se s select:
						  SELECT 
	                              [Salary],
	                               dbo.ufn_GetSalaryLevel([Salary])
	                                AS [Salary Level]
	                                FROM [Employees]

									-- DEFINE A FUNCTION
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
						  -------- deleted some details from DB WHEN WE HAVE RELATIONS:
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

	-----*******************************