using Employees;

var output = new ConsoleOutput();
var email = new EmailNotifier();
var sms = new SmsNotifier();
List<IWorkable> list = new List<IWorkable>()
{


    EmployeeFactory.Create("manager", "Илья", 20.5, 3, output, email, sms),
    EmployeeFactory.Create("manager","Валентин", 20.5, 3, output, email, sms),
    EmployeeFactory.Create("manager", "Владимир", 20.5, 3, output, email, sms),
    EmployeeFactory.Create("developer", "Александр", 21.0, 2, output, email, sms),
    EmployeeFactory.Create("developer","Владислав", 21.0, 3, output, email, sms),
    EmployeeFactory.Create("developer","Николай", 21.0, 2,output ,email, sms),
    EmployeeFactory.Create("intern","Алексей", 15.0, 0, output, email, sms),
    EmployeeFactory.Create("intern", "Дмитрий", 15.0, 0, output, email, sms),
    EmployeeFactory.Create("freelancer","Артем", 20.0, 3, output, email, sms),
    EmployeeFactory.Create("freelancer","Владислав", 20.0, 2, output, email, sms)
};

foreach (var worker in list)
{
    worker.work();
    if (worker is IPrintable PrintInfo)
    {
        PrintInfo.PrintInfo();
    }
}