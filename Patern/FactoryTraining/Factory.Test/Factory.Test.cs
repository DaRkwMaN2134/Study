namespace Factory.Test
{
    public class EmployeeTest
    {
        private IOutput _output;
        private IEmailNotifier _emailnotifier;
        private ISmsNotifier _smsnotifier;
        private EmployeeData _employeedata;

        [Theory]
        [InlineData(50000.0, 1.20, 60000.0)]
        [InlineData(52500.0, 1.20, 63000.0)]
        [InlineData(55000.0, 1.20, 66000.0)]
        [InlineData(57500.0, 1.20, 69000.0)]
        [InlineData(60000.0, 1.20, 72000.0)]
        public void ManagerSalary(double salary, double bonus, double excepted)
        {
            _employeedata = new EmployeeData("Гуля", salary);
            Employee _employee = new Employee(_employeedata, new ManagerBonusCalculator(), _output, _emailnotifier, _smsnotifier);
            var total = _employee.GetTotalSalary();
            Assert.Equal(excepted, total);
        }

        [Theory]
        [InlineData(60000.0, 1.10, 66000)]
        [InlineData(62500.0, 1.10, 68750.0)]
        [InlineData(65000.0, 1.10, 71500.0)]
        [InlineData(67500.0, 1.10, 74250.0)]
        [InlineData(70000.0, 1.10, 77000.0)]
        public void DeveloperrSalary(double salary, double bonus, double excepted)
        {
            _employeedata = new EmployeeData("Гуля", salary);
            Employee _employee = new Employee(_employeedata, new DeveloperBonusCalculator(), _output, _emailnotifier, _smsnotifier);
            var total = _employee.GetTotalSalary();
            Assert.Equal(excepted, total);
        }


        [Theory]
        [InlineData(30000.0, 1.05, 31500.0)]
        [InlineData(32500.0, 1.05, 34125.0)]
        [InlineData(35000.0, 1.05, 36750.0)]
        [InlineData(37500.0, 1.05, 39375.0)]
        [InlineData(40000.0, 1.05, 42000.0)]
        public void InternSalary(double salary, double bonus, double excepted)
        {
            _employeedata = new EmployeeData("Гуля", salary);
            Employee _employee = new Employee(_employeedata, new InternBonusCalculator(), _output, _emailnotifier, _smsnotifier);
            var total = _employee.GetTotalSalary();
            Assert.Equal(excepted, total);
        }

        [Theory]
        [InlineData(50000.0, 1.08, 54000.0)]
        [InlineData(52500.0, 1.08, 56700.0)]
        [InlineData(55000.0, 1.08, 59400.0)]
        [InlineData(57500.0, 1.08, 62100.0)]
        [InlineData(60000.0, 1.08, 64800.0)]

        public void FreelancerSalary(double salary, double bonus, double excepted)
        {
            _employeedata = new EmployeeData("Гуля", salary);
            Employee _employee = new Employee(_employeedata, new FreelancerBonusCalculator(), _output, _emailnotifier, _smsnotifier);
            var total = _employee.GetTotalSalary();
            Assert.Equal(excepted, total);
        }
    }
}