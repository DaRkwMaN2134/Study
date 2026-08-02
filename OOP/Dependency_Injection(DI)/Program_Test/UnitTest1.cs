using Employees;

namespace Program_Test
{
    public class Program_Test
    {
        [Theory]
        [InlineData(1000.0, 1200.0)]
        public void GetTotalSalaryTest(int salary, double totalsalary)
        {
            ManagerBonusCalculator _bonusCalculator = new ManagerBonusCalculator();            
            var output = new ConsoleOutput();
            var email = new EmailNotifier();
            var sms = new SmsNotifier();
            Employee _employee = new Employee(new EmployeeData("Илья", salary), _bonusCalculator, output, email, sms);
            var res = _employee.GetTotalSalary();
            Assert.Equal(totalsalary, res);
        }
    }
}
