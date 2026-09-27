namespace Employees
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


    public static class EmployeeFactory
    {
        public static IWorkable Create(string job, string name, double salary, int extra, IOutput output, IEmailNotifier email, ISmsNotifier sms)
        {
            switch (job)
            {
                case "manager":
                    return new Manager(name, salary, extra, output, email, sms);

                case "developer":
                    return new Developer(name, salary, extra, output, email, sms);

                case "intern":
                    return new Intern(name, salary, output, email, sms);

                case "freelancer":
                    return new Freelancer(name, salary, extra, output, email, sms);
                default:
                    throw new ArgumentException($"Такой профессии не существует!!");

            }
        }
    }



    public static class Factory
    {
        public static Employee CreateEmployee(EmployeeData data, IBonusCalculator bonusCalculator, IOutput consoleoutput, IEmailNotifier emailnotifer, ISmsNotifier smsnotifer)
        {
            return new Employee(data, bonusCalculator, consoleoutput, emailnotifer, smsnotifer);
        }
    }


    public class Employee
    {
        public IBonusCalculator _bonusCalculator;
        private IOutput _output;
        private IEmailNotifier _emailnotifier;
        private ISmsNotifier _smsnotifier;
        private EmployeeData _employeedata;
        public Employee(EmployeeData data, IBonusCalculator bonusCalculator, IOutput output, IEmailNotifier emailnotifier, ISmsNotifier smsnotifier)
        {
            _bonusCalculator = bonusCalculator;
            _employeedata = data;
            _output = output;
            _emailnotifier = emailnotifier;
            _smsnotifier = smsnotifier;
        }
        public double GetTotalSalary()
        {
            return _employeedata.Salary + _bonusCalculator.CalculateBonus(_employeedata);
        }
        public void PrintInfo()
        {
            _emailnotifier.SendEmail($"admin@company.com", $"Info about {_employeedata.Name}");
            _output.Write($"Имя: {_employeedata.Name}, Зарплата: {GetTotalSalary()}");
        }
    }
    public class Manager : IWorkable, IPrintable
    {
        private Employee _employee;
        public Manager(string name, double salary, int teamcount, IOutput output, IEmailNotifier email, ISmsNotifier sms)
        {
            _employee = Factory.CreateEmployee(new EmployeeData(name, salary), new ManagerBonusCalculator(), output, email, sms);
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

        public Developer(string name, double salary, int projectcount, IOutput output, IEmailNotifier email, ISmsNotifier sms)
        {
            _employee = Factory.CreateEmployee(new EmployeeData(name, salary), new DeveloperBonusCalculator(), output, email, sms);
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


    public class Intern : IWorkable, IPrintable
    {
        private Employee _employee;

        public Intern(string name, double salary, IOutput output, IEmailNotifier email, ISmsNotifier sms)
        {
            _employee = Factory.CreateEmployee(new EmployeeData(name, salary), new InternBonusCalculator(), output, email, sms);
        }

        public void PrintInfo()
        {
            _employee.PrintInfo();
        }

        public void work()
        {
            Console.WriteLine($"Intern is Interning");
        }

    }


    public class Freelancer : IWorkable, IPrintable
    {
        private Employee _employee;
        private int _projects;

        public Freelancer(string name, double salary, int projects, IOutput output, IEmailNotifier email, ISmsNotifier sms)
        {
            _projects = projects;
            _employee = Factory.CreateEmployee(new EmployeeData(name, salary), new FreelancerBonusCalculator(), output, email, sms);
        }

        public void PrintInfo()
        {
            _employee.PrintInfo();
            Console.WriteLine($"Количетсво проектов: {_projects}");
        }

        public void work()
        {
            Console.WriteLine($"Freelancer is working on projects");
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

    public interface IOutput
    {
        public void Write(string message);
    }

    public interface IEmailNotifier
    {
        public void SendEmail(string address, string message);
    }


    public interface ISmsNotifier
    {
        public void SendSms(string phone, string message);
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

    public class InternBonusCalculator : IBonusCalculator
    {
        public double CalculateBonus(EmployeeData data)
        {
            return data.Salary * 0.05;
        }
    }

    public class FreelancerBonusCalculator : IBonusCalculator
    {
        public double CalculateBonus(EmployeeData data)
        {
            return data.Salary * 0.08;
        }
    }

    public class ConsoleOutput : IOutput
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class EmailNotifier : IEmailNotifier
    {
        public void SendEmail(string address, string message)
        {
            Console.WriteLine(address);
            Console.WriteLine(message);
        }
    }

    public class SmsNotifier : ISmsNotifier
    {
        public void SendSms(string phone, string message)
        {
            Console.WriteLine($"{phone} - {message}");
        }
    }
}

