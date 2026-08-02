using Employees;

namespace Program_Test
{
    public class Salary_Test
    {
        ManagerBonusCalculator _managerCalculator = new ManagerBonusCalculator();
        DeveloperBonusCalculator _developerCalculator = new DeveloperBonusCalculator();
        InternBonusCalculator _internCalculator = new InternBonusCalculator();
        FreelancerBonusCalculator _freelancerCalculator = new FreelancerBonusCalculator();

        [Theory]
        [InlineData(1000.0, 1200.0)]
        public void ManagerBonusTest(int salary, double totalsalary)
        {


            var output = new ConsoleOutput();
            var email = new EmailNotifier();
            var sms = new SmsNotifier();
            Employee _employee = new Employee(new EmployeeData("Илья", salary), _managerCalculator, output, email, sms);
            var res = _employee.GetTotalSalary();
            Assert.Equal(totalsalary, res);
        }


        [Theory]
        [InlineData(1000.0, 1100.0)]
        public void DeveloperBonusTest(int salary, double totalsalary)
        {


            var output = new ConsoleOutput();
            var email = new EmailNotifier();
            var sms = new SmsNotifier();
            Employee _employee = new Employee(new EmployeeData("Илья", salary), _developerCalculator, output, email, sms);
            var res = _employee.GetTotalSalary();
            Assert.Equal(totalsalary, res);
        }


        [Theory]
        [InlineData(1000.0, 1050.0)]
        public void InternBonusTest(int salary, double totalsalary)
        {


            var output = new ConsoleOutput();
            var email = new EmailNotifier();
            var sms = new SmsNotifier();
            Employee _employee = new Employee(new EmployeeData("Илья", salary), _internCalculator, output, email, sms);
            var res = _employee.GetTotalSalary();
            Assert.Equal(totalsalary, res);
        }


        [Theory]
        [InlineData(1000.0, 1080.0)]
        public void FreelancerBonusTest(int salary, double totalsalary)
        {


            var output = new ConsoleOutput();
            var email = new EmailNotifier();
            var sms = new SmsNotifier();
            Employee _employee = new Employee(new EmployeeData("Илья", salary), _freelancerCalculator, output, email, sms);
            var res = _employee.GetTotalSalary();
            Assert.Equal(totalsalary, res);
        }
    }
}
