using CompanyLibrary;

List<IWorkable> list = new List<IWorkable>()
{
    new Developer("Антон", 20.5, 3),
    new Developer("Илья", 20.5, 3),
    new Developer("Владимир", 20.5, 3),
    new Manager("Александр", 21.0, 2),
    new Manager("Владислав", 21.0, 3),
    new Manager("Николай", 21.0, 2)
};

foreach(var worker in list)
{
    worker.work();
    if(worker is IPrintable PrintInfo)
    {
        PrintInfo.PrintInfo();
    }
}