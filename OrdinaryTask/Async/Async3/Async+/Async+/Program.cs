class Programm
{
    static async Task Main()
    {
        var token = new CancellationTokenSource(2000);
        var token2 = new CancellationTokenSource();
        string str = await DownloadDataAsync(3000, token.Token);
        Console.WriteLine(str);


        Task<string> AnyTask = await Task.WhenAny(Source1Async(3000, token2.Token), Source2Async(2000, token2.Token), Source3Async(1000, token2.Token));
        token2.Cancel();
        string res = await AnyTask;
        Console.WriteLine(res);

        Task<string> AllTask = AnalyzeBadTask();
        string res2 = await AllTask;
        Console.WriteLine(res2);


    }

    static public async Task<string> DownloadDataAsync(int delay, CancellationToken token)
    {
        try
        {
            await Task.Delay(delay, token);
            return "Данные загружены";
        }
        catch (OperationCanceledException)
        {
            return "Операция отменена";
        }
    }



    static public async Task<string> Source1Async(int delay, CancellationToken token)
    {
        try
        {
            await Task.Delay(delay, token);
            return "Результат от источника 1";
        }
        catch(TaskCanceledException)
        {
            return "Слишком долго 1";
        }
    }

    static public async Task<string> Source2Async(int delay, CancellationToken token)
    {
        try
        {
            await Task.Delay(delay, token);
            return "Результат от источника 2";
        }
        catch (TaskCanceledException)
        {
            return "Слишком долго 2";
        }
    }

    static public async Task<string> Source3Async(int delay, CancellationToken token)
    {
        try
        {
            await Task.Delay(delay, token);
            return "Результат от источника 3";
        }
        catch (TaskCanceledException)
        {
            return "Слишком долго";
        }
    }



    static public async Task BadTask1()
    {
        throw new InvalidOperationException();
    }

    static public async Task BadTask2()
    {
        throw new DivideByZeroException();
    }

    static public async Task<string> BadTask3()
    {
        await Task.Delay(1000);
        return "Успешно";
    }

    static public async Task<string> AnalyzeBadTask()
    {
        Task task1 = BadTask1();
        Task task2 = BadTask2();
        Task<string> task3 = BadTask3();
        try
        {
            await Task.WhenAll(task1, task2, task3);
            return "Успешно";
        }
        catch (InvalidOperationException)
        {
            return "Ошибка";
        }
        catch (DivideByZeroException)
        {
            return "Ошибка";
        }
        finally
        {
            if(task1.IsFaulted)
            {
                Console.WriteLine(task1.Exception.Message);
            }

            if (task2.IsFaulted)
            {
                Console.WriteLine(task2.Exception.Message);
            }

            if (task3.IsCompletedSuccessfully)
            {
                Console.WriteLine(task3.Result);
            }
        }
    }


}
