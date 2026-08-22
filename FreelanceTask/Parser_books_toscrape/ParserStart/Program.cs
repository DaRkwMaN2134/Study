using DataLibrary;
using ParserLibrary;
using FileIOLibrary;
using HtmlAgilityPack;

class Program()
{
    HTTP_Client clientRequest = new HTTP_Client();
    JsonSerialize jsonInput = new JsonSerialize();
    HTML_Parser htmlParser = new HTML_Parser();
    string file = "Book.json";

    async Task Parser()
    {
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
        await jsonInput.JsonBookWriteAsync(file, allBooks);
        Console.WriteLine($"Всего собрано книг: {allBooks.Count}");
    }

    static public async Task Main()
    {
        Program start = new Program();
        await start.Parser();


        Console.WriteLine("\nДля выхода нажмите любую кнопку");
        Console.ReadKey();
    }
}