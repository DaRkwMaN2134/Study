using Factory;

List<IWorkable> list = new List<IWorkable>()
{
    EmployeeFactory.Create("manager", "Илья", 20.5, 3),
    EmployeeFactory.Create("manager","Валентин", 20.5, 3),
    EmployeeFactory.Create("manager", "Владимир", 20.5, 3),
    EmployeeFactory.Create("developer", "Александр", 21.0, 2),
    EmployeeFactory.Create("developer","Владислав", 21.0, 3),
    EmployeeFactory.Create("developer","Николай", 21.0, 2),
    EmployeeFactory.Create("intern","Алексей", 15.0, 0),
    EmployeeFactory.Create("intern", "Дмитрий", 15.0, 0),
    EmployeeFactory.Create("freelancer","Артем", 20.0, 3),
    EmployeeFactory.Create("freelancer","Владислав", 20.0, 2)
};

foreach (var worker in list)
{
    worker.work();
    if (worker is IPrintable PrintInfo)
    {
        PrintInfo.PrintInfo();
    }
}