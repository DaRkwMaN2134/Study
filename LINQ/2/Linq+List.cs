using System.Linq;
Random rand = new Random();
List<int> list = new List<int>();
for(int i = 0; i < 50; i++)
{
    list.Add(rand.Next(0, 101));
}
Console.WriteLine($"Все числа: {string.Join(",", list)}");

//////////////////////////////////////////////////////

var divide = list.Where(n => n%15==0);
var order = divide.OrderByDescending(n => n).Take(5);

Console.WriteLine($"Числа, которые делятся на 3 и на 5, первые 5: {string.Join(", ", order)}");

//////////////////////////////////////////////////////

var sum = list.Where(n => n > 50).Sum();

Console.WriteLine(string.Join($",", $"Сумма всех чисел >50: {sum}"));

//////////////////////////////////////////////////////

var unique = list.Distinct().Count();
Console.WriteLine($"Уникальные числа {unique}");

