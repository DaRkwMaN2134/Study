namespace CompanyLibrary
{
    public class EmployeeData
    {
        public string Name;
        public double Salary;
        public EmployeeData(string name, double salary)
        {
            Name = name;
            Salary = salary;
        }
    }

    public class Employee
    {
        public IBonusCalculator _bonusCalculator;
        public EmployeeData _employeedata;
        public Employee(EmployeeData data, IBonusCalculator bonusCalculator)
        {
            _bonusCalculator = bonusCalculator;
            _employeedata = data;
        }
        public double GetTotalSalary()
        {
            return _employeedata.Salary + _bonusCalculator.CalculateBonus(_employeedata);
        }
        public void PrintInfo()
        {
            Console.WriteLine($"Имя: {_employeedata.Name}, Зарплата: {GetTotalSalary()}");
        }
    }
    public class Manager: IWorkable, IPrintable
    {
        private Employee _employee;
        
        public Manager(string name, double salary, int teamcount)
        {
            _employee = new Employee(new EmployeeData(name, salary), new ManagerBonusCalculator());
        }
        public void PrintInfo()
        {
            _employee.PrintInfo();
        }

        public void work()
        {
            Console.WriteLine($"Manager is managing");
        }


    }
    public class Developer : IWorkable, IPrintable
    {
        private Employee _employee;

        public Developer(string name, double salary, int projectcount)
        {
            _employee = new Employee(new EmployeeData(name, salary), new DeveloperBonusCalculator());
        }

        public void PrintInfo()
        {
            _employee.PrintInfo();
        }

        public void work()
        {
            Console.WriteLine($"Developer is coding");
        }
    }



    public interface IBonusCalculator
    {
        public double CalculateBonus(EmployeeData data);
    }

    public interface IWorkable
    {
        public void work();
    }

    public interface IPrintable
    {
        public void PrintInfo();
    }



    public class ManagerBonusCalculator : IBonusCalculator
    {
        public double CalculateBonus(EmployeeData data)
        {
            return data.Salary * 0.20;
        }
    }

    public class DeveloperBonusCalculator : IBonusCalculator
    {
        public double CalculateBonus(EmployeeData data)
        {
            return data.Salary * 0.10;
        }
    }
}

