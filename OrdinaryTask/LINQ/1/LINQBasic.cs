using System.Linq;

Random rnd = new Random();
int counst = 20;
List<int> NumList = new List<int>(20);
List<string> NameList = new List<string>(20);
string[] name = { "Анна", "Борис", "Светлана", "Дмитрий", "Екатерина" };


void ListFill()
{
    for (int i = 0; i < counst; i++)
    {
        int random = rnd.Next(101);
        NumList.Add(random);
    }

    foreach (string names in name)
    {
        NameList.Add(names);
    }
}


void ListView()
{
    string result;
    result = string.Join(",", NumList);
    Console.WriteLine($"С писок цифр - {result}");
}

void ListFilter()
{
    string result;
    var Filter = NumList.Where(num => num > 50);
    result = string.Join(",", Filter);
    Console.WriteLine($"Отфильтрованный список - {result}");
}    

void OrderedBy()
{
    string result;
    var FilterOtderBy = NumList.OrderBy(num => num);
    result = string.Join(",", FilterOtderBy);
    Console.WriteLine($"По возрастанию - {result}");

}

void Agregation()
{
    var Max = NumList.Max();
    var Min = NumList.Min();
    var Sum = NumList.Sum();
    var Average = NumList.Average();
    Console.WriteLine($"Макс значение {Max}");
    Console.WriteLine($"Мин значение  {Min}");
    Console.WriteLine($"Сумма {Sum}");
    Console.WriteLine($"Среднее арифметическое {Average}");
}


void Selecting()
{
    var Stroki = NumList.Select(num => $"Число: {num}");
    foreach (var selected in Stroki)
    Console.WriteLine(selected);
}

void Stroki()
{
    var FilterByA = NameList.Where(name => name.Contains("а")).ToList();
    var FilterByLength = NameList.OrderBy(name => name.Length).ToList();
    var FilterByC = NameList.FirstOrDefault(name => name.StartsWith('С'));
    foreach (var containA in FilterByA)
    {
        Console.WriteLine($"Содержит букву А: {containA}");
    }
    foreach (var containLegnth in FilterByLength)
    {
        Console.WriteLine($"По длине: {containLegnth}");
    }
    Console.WriteLine($"Содержит букву С: {FilterByC}");

}


ListFill();
ListView();
ListFilter();
OrderedBy();
Agregation();
Selecting();
Stroki();