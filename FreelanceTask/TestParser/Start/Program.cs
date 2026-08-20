using DataModel;
using FileIO;
using HtmlAgilityPack;
using Parser;
using static System.Net.WebRequestMethods;

class Program()
{
    HTTP_Client clientRequest = new HTTP_Client();
    JsonSerialize jsonInput = new JsonSerialize();
    HTML_Parser htmlParser = new HTML_Parser();
    string file1 = "post1.json";
    string file2 = "post2.json";

    async Task pars1Async()
    {
        var jsonString1 = await clientRequest.HTTPJsonplaceholderRequestAsync();
        var post1 = await jsonInput.JsonPostWriteAsync(file1, jsonString1);
        Console.WriteLine($"Title: {post1.Title}");
        Console.WriteLine($"Completed: {post1.Completed}\n\n");
    }




    async Task pars2Async()
    {
        Console.WriteLine("Введите url страницы");
        string url = "https://books.toscrape.com";
        List<Book> allBooks = new List<Book>();
        List<string> urlBookList = new List<string>();
        while (true)
        {
            var htmlRoot = await clientRequest.HTTPBookstoscrapeRequestAsync(url);
            List<Book> booksFromPage = htmlParser.BookParse(htmlRoot, url);
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlRoot);

            urlBookList = htmlParser.ParseBookUrl(htmlRoot, url);

            var semaphore = new SemaphoreSlim(5); // максимум 5 запросов одновременно
            var tasks = new List<Task>();

            foreach (var currentUrlBook in urlBookList)
            {
                await semaphore.WaitAsync();
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var htmlSub = await clientRequest.HTTPBookstoscrapeRequestAsync(currentUrlBook);
                        Book book = htmlParser.ParseBookDetail(htmlSub, currentUrlBook);
                        {
                            lock (allBooks) allBooks.Add(book);
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }
            await Task.WhenAll(tasks);

            var nextNode = doc.DocumentNode.SelectSingleNode("//li[@class='next']/a");
            if (nextNode != null)
            {
                string nextPage = nextNode.GetAttributeValue("href", "");
                Uri fullUri = new Uri(new Uri(url), nextPage);
                url = fullUri.ToString();


                if (booksFromPage != null)
                {
                    foreach (var book in booksFromPage)
                    {
                        Console.WriteLine($"{book.Title} — {book.Price}");
                    }
                }
                else
                {
                    Console.WriteLine($"Парсинг пустой");
                    break;
                }
            }
            else
            {
                Console.WriteLine($"Следующей страницы нет");
                break;
            }
        }
        await jsonInput.JsonBookWriteAsync(file2, allBooks);
        Console.WriteLine($"Всего собрано книг: {allBooks.Count}");
    }

    async Task pars3Async()
    {
        await clientRequest.HTTPLoginRequestAsync("https://the-internet.herokuapp.com");
    }

    async Task pars4Async()
    {
        await clientRequest.HTTPCSRFRequestAsync("http://quotes.toscrape.com/login");
    }
    static async Task Main()
    {
        Program start = new Program();
        await start.pars1Async();
        await start.pars2Async();
        await start.pars3Async();
        await start.pars4Async();

        Console.WriteLine("\nДля выхода нажмите любую кнопку");
        Console.ReadKey();
    }
}