public abstract class Employee
{
    public abstract string Name { get; set; }
    public abstract double BaseSalary { get; set; }
    public abstract double CalculateBonus();
    public Employee(string name, double basesalary)
    {
        Name = name;
        BaseSalary = basesalary;
    }
    public virtual void PrintInfo()
    {
        Console.WriteLine($"Имя: {Name}; Зарплата: {BaseSalary}");
    }
}

public class Manager : Employee, IWorkable
{
    public override string Name { get; set; }
    public override double BaseSalary { get; set; }
    public override double CalculateBonus() => BaseSalary * 1.20;
    public int TeamSize;
    public Manager(string name, double basesalary, int teamsize) : base(name, basesalary)
    {
        TeamSize = teamsize;
    }
    public override void PrintInfo() => Console.WriteLine($"Имя: {Name}; Зарплата: {CalculateBonus()}; Размер команды: {TeamSize}");
    public void Work()
    {
        Console.WriteLine("Manager is managing");
    }
}



public class Developer : Employee, IWorkable
{
    public override string Name { get; set; }
    public override double BaseSalary { get; set; }
    public int ProjectsCount;
    public override double CalculateBonus() => BaseSalary * 1.10;
    public Developer(string name, double baselasary, int projectscount) : base(name, baselasary)
    {
        ProjectsCount = projectscount;
    }
    public override void PrintInfo() => Console.WriteLine($"Имя: {Name}; Зарплата: {CalculateBonus()}; Количество проектов: {ProjectsCount}");
    public void Work()
    {
        Console.WriteLine("Developer is coding");
    }
}

public interface IWorkable
{
    void Work();
}