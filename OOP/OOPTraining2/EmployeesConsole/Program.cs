List<Employee> employee = new List<Employee>()
{
    new Manager("Антон", 20.5, 3),
    new Manager("Илья", 20.5, 3),
    new Manager("Владимир", 20.5, 3),
    new Developer("Александр", 21.0, 2),
    new Developer("Владислав", 21.0, 3),
    new Developer("Николай", 21.0, 2)
};

foreach(Employee worker in employee)
{
    worker.PrintInfo();
    if(worker is IWorkable work)
    {
        work.Work();
    }
}
