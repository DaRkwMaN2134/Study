using System.Linq;
class Programm
{
    static void Main()
    {
        List<Project> _projects = new List<Project>()
        {
            new Project("Проект 1", new List<string> { "Задача 1", "Задача 2" }),
            new Project("Проект 2", new List<string> { "Задача 1", "Задача 2" }),
            new Project("Проект 3", new List<string> { "Задача 1", "Задача 2" })
        };
        var res = Project.returnProjectList(_projects);
        Console.WriteLine(string.Join(",", res));
        Console.WriteLine($"Общее кол-во задач: {res.Count()}");

        employeeInfo();

        Aggregate.rndNumber();
        Aggregate.aggregate();
    }

    static public void employeeInfo()
    {
        List<Department> _departments = new List<Department>()
        {
            new Department("Отдел 1", "01"),
            new Department("Отдел 2", "02"),
            new Department("Отдел 3", "03")
        };
        List<Employee> _employees = new List<Employee>()
        {
            new Employee("Николай", "01", "01"),
            new Employee("Владимир", "02", "01"),
            new Employee("Антон", "03", "01")
        };

        var res = _departments.GroupJoin(
            _employees,

            dept => dept.departmentID,
            emp => emp.employeeDepartmentID,
            (dept, emp) => new
            {
                Department = dept.departmentName,
                Employees = emp.Select(x => x.employeeName).ToList()
            }
        );

        foreach (var item in res)
        {
            string employeesList = string.Join(", ", item.Employees);

            if(item.Employees.Any() == false)
            {
                Console.WriteLine($"Департамент: {item.Department} | Сотрудники: Сотрудников нет");
            }
            else
            {
                Console.WriteLine($"Департамент: {item.Department} | Сотрудники: {employeesList}");
            }
        }

    }
}

class Project
{
    string projectName;
    List<string> projectTasks = new List<string>();


    public Project(string projectname, List<string> projecttasks)
    {
        projectName = projectname;
        projectTasks = projecttasks; 
    }
    static public List<string> returnProjectList(List<Project> projects)
    {
        var allProject = projects.SelectMany(x => x.projectTasks);
        return allProject.ToList();
    }
}

public class Department
{
    public string departmentName;
    public string departmentID;
    public Department(string departmentname, string departmentid)
    {
        departmentName = departmentname;
        departmentID = departmentid;
    }
}

public class Employee
{
    public string employeeName;
    public string employeeID;
    public string employeeDepartmentID;
    public Employee(string employeename, string employeeid, string employeeDepartmentid)
    {
        employeeName = employeename;
        employeeID = employeeid;
        employeeDepartmentID = employeeDepartmentid;
    }
}


public class Aggregate
{
    static Random rnd = new Random();
    static List<int> random = new List<int>();

    static public void rndNumber()
    {
        for(int i = 0; i<10; i ++)
        {
            random.Add(rnd.Next(1, 101));
        }
    }

    static public void aggregate()
    {
        var Sum = random.Aggregate((acc, num) => acc + num);
        var Product = random.Aggregate((acc, num) => acc * num);
        var Max = random.Max();
        var Min = random.Min();
        Console.WriteLine($"Сумма {Sum}");
        Console.WriteLine($"Произведение {Product}");
        Console.WriteLine($"Максимальное {Max}");
        Console.WriteLine($"Минимальное {Min}");
    }
    


}