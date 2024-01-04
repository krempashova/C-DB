	--Problem 1.	Records’ Count
*********************************************/

SELECT COUNT(Id) AS [Count]
FROM WizzardDeposits; 

/* ******************************************
	Problem 2.	Longest Magic Wand
*********************************************/

SELECT MAX(MagicWandSize) AS [LongestMagicWand]
FROM WizzardDeposits; 

/* ******************************************
	Problem 3.	Longest Magic Wand per Deposit Groups
*********************************************/

SELECT DepositGroup,
       MAX(MagicWandSize) AS [LongestMagicWand]
FROM WizzardDeposits
GROUP BY DepositGroup; 
 
/* ******************************************
	Problem 4.	* Smallest Deposit Group per Magic Wand Size
*********************************************/

SELECT DepositGroup
FROM WizzardDeposits
GROUP BY DepositGroup
HAVING AVG(MagicWandSize) =
(
    SELECT MIN(WizAverageWandSize.averageWandSize)
    FROM
    (
        SELECT AVG(MagicWandSize) AS averageWandSize
        FROM WizzardDeposits
        GROUP BY DepositGroup
    ) AS WizAverageWandSize
); 
							
-- Another solutiion

SELECT TOP 1 WITH TIES DepositGroup
FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY AVG(MagicWandSize);  
	   
/* ******************************************
	Problem 5.	Deposits Sum
*********************************************/

SELECT DepositGroup,
       SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
GROUP BY DepositGroup; 

/* ******************************************
	Problem 6.	Deposits Sum for Ollivander Family
*********************************************/

SELECT DepositGroup,
       SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup; 

/* ******************************************
	Problem 7.	Deposits Filter
*********************************************/

SELECT DepositGroup,
       SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
ORDER BY TotalSum DESC; 

/* ******************************************
	Problem 8.	 Deposit Charge
*********************************************/

SELECT DepositGroup,
       MagicWandCreator,
       MIN(DepositCharge) AS MinDepositCharge
FROM WizzardDeposits
GROUP BY DepositGroup,
         MagicWandCreator
ORDER BY MagicWandCreator,
         DepositGroup; 
 
/* Understood from the exercise - but judge expects the above query
SELECT wMain.DepositGroup,
(
    SELECT TOP 1 wForCreator.MagicWandCreator
    FROM WizzardDeposits AS wForCreator
    WHERE wForCreator.DepositGroup = wMain.DepositGroup
) AS MagicWandCreator,
(
    SELECT TOP 1 MIN(wForCharge.DepositCharge) AS GroupCharge
    FROM WizzardDeposits AS wForCharge
    WHERE wForCharge.DepositGroup = wMain.DepositGroup
    ORDER BY GroupCharge
) AS MinDepositCharge
FROM WizzardDeposits AS wMain
GROUP BY wMain.DepositGroup
ORDER BY MagicWandCreator,
         DepositGroup;
 */

/* ******************************************
	Problem 9.	Age Groups
*********************************************/

SELECT CASE
           WHEN w.Age BETWEEN 0 AND 10
           THEN '[0-10]'
           WHEN w.Age BETWEEN 11 AND 20
           THEN '[11-20]'
           WHEN w.Age BETWEEN 21 AND 30
           THEN '[21-30]'
           WHEN w.Age BETWEEN 31 AND 40
           THEN '[31-40]'
           WHEN w.Age BETWEEN 41 AND 50
           THEN '[41-50]'
           WHEN w.Age BETWEEN 51 AND 60
           THEN '[51-60]'
           WHEN w.Age >= 61
           THEN '[61+]'
           ELSE 'N\A'
       END AS AgeGroup,
       COUNT(*) AS WizzardsCount
FROM WizzardDeposits AS w
GROUP BY CASE
             WHEN w.Age BETWEEN 0 AND 10
             THEN '[0-10]'
             WHEN w.Age BETWEEN 11 AND 20
             THEN '[11-20]'
             WHEN w.Age BETWEEN 21 AND 30
             THEN '[21-30]'
             WHEN w.Age BETWEEN 31 AND 40
             THEN '[31-40]'
             WHEN w.Age BETWEEN 41 AND 50
             THEN '[41-50]'
             WHEN w.Age BETWEEN 51 AND 60
             THEN '[51-60]'
             WHEN w.Age >= 61
             THEN '[61+]'
             ELSE 'N\A'
         END; 
		
-- Shorter solution

SELECT grouped.AgeGroups,
       COUNT(*) AS WizzardsCount
FROM
(
    SELECT CASE
               WHEN Age BETWEEN 0 AND 10
               THEN '[0-10]'
               WHEN Age BETWEEN 11 AND 20
               THEN '[11-20]'
               WHEN Age BETWEEN 21 AND 30
               THEN '[21-30]'
               WHEN Age BETWEEN 31 AND 40
               THEN '[31-40]'
               WHEN Age BETWEEN 41 AND 50
               THEN '[41-50]'
               WHEN Age BETWEEN 51 AND 60
               THEN '[51-60]'
               WHEN Age >= 61
               THEN '[61+]'
               ELSE 'N\A'
           END AS AgeGroups
    FROM WizzardDeposits
) AS grouped
GROUP BY grouped.AgeGroups; 
		
/* ******************************************
	Problem 10.	First Letter
*********************************************/

SELECT LEFT(FirstName, 1) AS FirstLetter
FROM WizzardDeposits
WHERE DepositGroup = 'Troll Chest'
GROUP BY LEFT(FirstName, 1)
ORDER BY FirstLetter; 

/* ******************************************
	Problem 11.	Average Interest 
*********************************************/

SELECT DepositGroup,
       IsDepositExpired,
       AVG(1.0 * DepositInterest)
FROM WizzardDeposits
WHERE DepositStartDate > '01/01/1985'
GROUP BY DepositGroup,
         IsDepositExpired
ORDER BY DepositGroup DESC,
         IsDepositExpired; 


--12.

SELECT
SUM([Host Wizard Deposit]-[Guest Wizard Deposit])
AS [SumDifference]
FROM
(
 SELECT[FirstName]
        AS [Host Wizard],
           [DepositAmount]
		AS [Host Wizard Deposit],
		   LEAD([FirstName]) OVER(ORDER BY [Id])
		AS[Guest Wizard],
		LEAD([DepositAmount]) OVER (ORDER BY[Id])
		AS [Guest Wizard Deposit]
      FROM [WizzardDeposits]

)AS [SUBQUERY]

--13. Departments Total Salaries

USE[SoftUni]
GO
       SELECT
	          [DepartmentID],
			  
			  SUM([Salary])
			  AS[TotalSalary]

          FROM[Employees]
         GROUP BY[DepartmentID]

		 --1414. Employees Minimum Salaries

		   SELECT
		        [DepartmentID],
		         MIN([Salary])
		      AS[MinimumSalary]
		         FROM[Employees]
		         WHERE [HireDate]>'01/01/2000'
		         GROUP BY[DepartmentID]
		        HAVING([DepartmentID] IN(2,5,7))

				--15. Employees Average Salaries

				SELECT *INTO[Newtable] FROM[Employees]
				WHERE [Salary]>30000
			     DELETE FROM[Newtable]
			      WHERE [ManagerID]=42

			       UPDATE [Newtable]
				      SET 
					  [Salary]+=5000
					  WHERE [DepartmentID]=1
					    SELECT
						[DepartmentID],
						 AVG([Salary])
						 AS[AverageSalary]
					          FROM[Newtable]
					         GROUP BY[DepartmentID]

  --16. Employees Maximum Salaries--
  SELECT [DepartmentID],
         MAX([Salary])
         AS[MaxSalary]
         FROM[Employees]
         GROUP BY[DepartmentID]
		 HAVING MAX([Salary]) NOT BETWEEN 30000 AND 70000

		 --17. Employees Count Salaries

           SELECT 
		      COUNT([Salary])
			  AS[Count]
               FROM [Employees]
			   WHERE [ManagerID]IS NULL


			   --1818. *3rd Highest Salary


       SELECT
	    DISTINCT
	   [DepartmentID],
	   [Salary]
	   AS[ThirdHighestSalary]
	   FROM
	   (
	   
			    SELECT
			          [DepartmentID],
					  [Salary],
					  DENSE_RANK() OVER(PARTITION BY([DepartmentID]) ORDER BY[Salary] DESC)
				       AS [SalaryRANK]
			         FROM[Employees]
	   )AS[SalaryRankingSUBqUERY]
	    WHERE [SalaryRANK]=3
		--19. **Salary Challenge


		
                 
				 SELECT TOP(10)
				 [FirstName],
				 [LastName],
				 [DepartmentID]
				 FROM[Employees]
				 AS[e]
				 WHERE [e].[Salary]>(
				 
				                               SELECT
		                                       
				                                   AVG([Salary])
												   AS[AverageSalaryforDepartment]
		                                           FROM[Employees] AS[equery]
												   WHERE [e].[DepartmentID]=[equery].[DepartmentID]
		                                          GROUP BY[DepartmentID]
				 
				                   )


		    
