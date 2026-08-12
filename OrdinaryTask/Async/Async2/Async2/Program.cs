using System.Linq;
class Programm
{
    static async Task Main()
    {
        Func<int, int> squareNum = x => x * x;
        Predicate<int> evenNum = x => x % 2 == 0;
        Action<string> toUpper = x => Console.WriteLine(x.ToUpper());
        List<int> numList = new List<int>()
        {
            1,2,3,4,5,6,7,8,9,10
        };

        ////////////////////////////////////////////////////////////////////

        List<string> nameList = new List<string>()
        {
            "Анна", "Борис", "Виктор"
        };

        ////////////////////////////////////////////////////////////////////

        var tasks = new List<Func<Task<int>>>
        {
        async () => { await Task.Delay(1000); return 5; },
        async () => { await Task.Delay(500); return 3; },
        async () => { await Task.Delay(1500); return 7; }
        };

        ////////////////////////////////////////////////////////////////////

        Console.WriteLine($"Числа возведенные в квадрат - {string.Join(", ", await ProcessDataAsync(numList, squareNum))}");
        Console.WriteLine($"Четные числа - {string.Join(", ", await FilterDataAsync(numList, evenNum))}");
        await ProcessItemsAsync(nameList, toUpper);
        Console.WriteLine($"Сумма чисел метода - {await RunInParallelAsync(tasks)}");
    }

    ////////////////////////////////////////////////////////////////////

    static async Task<List<int>> ProcessDataAsync(List<int> numList, Func<int, int> func)
    {
        List<int> newNumList = new List<int>();
        foreach (var item in numList)
        {
            newNumList.Add(func(item));
            await Task.Delay(100);
        }
        return newNumList;
    }

    ////////////////////////////////////////////////////////////////////

    static async Task<List<int>> FilterDataAsync(List<int> evenNum, Predicate<int> pred)
    {
        List<int> newNumList = new List<int>();
        foreach (var item in evenNum)
        {
            if (pred(item))
            {
                newNumList.Add(item);
            }
            await Task.Delay(50);
        }
        return newNumList;
    }

    ////////////////////////////////////////////////////////////////////

    static async Task ProcessItemsAsync(List<string> name, Action<string> act)
    {
        foreach (var item in name)
        {
            act(item);
            await Task.Delay(200);
        }
    }

    ////////////////////////////////////////////////////////////////////

    static async Task<int> RunInParallelAsync(List<Func<Task<int>>> list)
    {
        List<Task<int>> taskList = new List<Task<int>>();
        foreach (var item in list)
        {
            taskList.Add(item());
        }
        int[] result = await Task.WhenAll(taskList);
        return result.Sum();

    }
}
