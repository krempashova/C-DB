using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Linq;
using System.Runtime.Loader;
using System.Text;

namespace SoftUni
{
    public class StartUp

    {

        static void Main(string[] args)
        {
            SoftUniContext dbcontext = new SoftUniContext();
            var result = RemoveTown(dbcontext);
            Console.WriteLine(result);

        }
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var orderedEmployess = context.Employees.
               OrderBy(e => e.EmployeeId)
               .Select(e => new
               {
                   e.FirstName,
                   e.LastName,
                   e.MiddleName,
                   e.JobTitle,
                   e.Salary
               }).ToArray();

            foreach (var e in orderedEmployess)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }



            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {


            StringBuilder sb = new StringBuilder();
            var employees = context.Employees
                .OrderBy(e => e.FirstName)
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary

                })
                .ToArray();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesRnD = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();

            foreach (var e in employeesRnD)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }



        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            Employee? employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");
            employee.Address = newAddress;

            context.SaveChanges();

            string[] employeeAddresses = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address!.AddressText)
                .ToArray();
            return String.Join(Environment.NewLine, employeeAddresses);
        }


        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employeesWithProjects = context.Employees
                // .Where(e => e.EmployeesProjects.Any(ep =>
                   // ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))

                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager!.FirstName,
                    ManagerLastName = e.Manager!.LastName,
                    Projects = e.EmployeesProjects
                        .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.CurrentCulture),
                            EndDate = ep.Project.EndDate.HasValue
                                ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                : "not finished"
                        })
                        .ToArray()
                })
                .ToArray();

            foreach (var e in employeesWithProjects)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    sb
                        .AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            
            var findedALLAdreses = context.Addresses
                 .OrderByDescending(a => a.Employees.Count)
                 .ThenBy(a => a.Town!.Name)
                 .ThenBy(a => a.AddressText)
                 .Take(10)
                 .Select(a => new 
                       {
                     a.AddressText,
                     TownName =a.Town!.Name,
                     EmployeesCount=a.Employees.Count,
                        }).ToArray();

            foreach (var a in findedALLAdreses)
            {
                sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeesCount} employees");
            }


            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var empoluyyes147 = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Project = e.EmployeesProjects
                     .Select(p => new
                     { p.Project.Name })
                     .OrderBy(p => p.Name)
                     .ToArray()
                })
                .FirstOrDefault();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{empoluyyes147!.FirstName} {empoluyyes147!.LastName} - {empoluyyes147.JobTitle}");
            sb.AppendLine(String.Join(Environment.NewLine, empoluyyes147.Project.Select(p => p.Name)));




            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                  .Select(d => new
                  {
                      d.Name,
                      ManagerFirstName = d.Manager.FirstName,
                      ManagerLastname = d.Manager.LastName,
                      Employees = d.Employees
                     .OrderBy(e => e.FirstName)
                     .ThenBy(e => e.LastName)
                     .Select(e => new
                     {
                         EmployeesData = ($"{e.FirstName} {e.LastName} - {e.JobTitle}")
                     })
                     .ToArray()

                  }) ; 

            foreach (var dep in departments)
            {
                sb.AppendLine($"{dep.Name} - {dep.ManagerFirstName}  {dep.ManagerLastname}");
                sb.AppendLine(String.Join(Environment.NewLine,dep.Employees.Select(e=>e.EmployeesData)));
            }

            return sb.ToString().TrimEnd();
        }
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            string[] departmentName = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };
            var employees = context.Employees
                .Where(e => departmentName.Contains(e.Department.Name))
                .ToArray();
            foreach (var e in employees)
                e.Salary *= 1.12m;
              context.SaveChanges();
            var employeeInfo = context.Employees
                .Where(e => departmentName.Contains(e.Department.Name))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                 .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.Salary
                    })
                    .ToArray();
            foreach (var empl in employeeInfo)
            {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} (${empl.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employes = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();
            foreach (var employee in employes)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectroremove = context.Projects
                .Where(p => p.ProjectId == 2);
            context.Projects.RemoveRange(projectroremove);
            var removeEmpProject = context.EmployeesProjects.Where(e => e.ProjectId == 2);
            context.EmployeesProjects.RemoveRange(removeEmpProject);
            context.SaveChanges();
            var projects = context.Projects
               .Take(10)
               .Select(p => p.Name)
               .ToArray();

            return string.Join(Environment.NewLine, projects);
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var towntodelete = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            var adressestodelete = context.Addresses
                .Where(a => a.TownId == towntodelete!.TownId)
                .ToArray();
            var employees = context.Employees
                .Where(e => adressestodelete.Contains(e.Address))
                .ToArray();

            foreach (var e in employees)
            {
                e.AddressId = null;

            }
            context.Addresses.RemoveRange(adressestodelete);
            context.Towns.Remove(towntodelete!);
            context.SaveChanges();

       

            return  $"{adressestodelete.Count()} addresses in Seattle were deleted";
        }
    }
}
