using DataModel;
using HtmlAgilityPack;


namespace Parser
{
    public class HTML_Parser
    {
        public List<Book> BookParse(string html, string url)
        {
            List<Book> bookList = new List<Book>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);


            var bookNodes = doc.DocumentNode.SelectNodes("//ol/li");
            if (bookNodes == null)
            {
                Console.WriteLine("⚠️ Книги не найдены. Возможно, изменилась структура сайта.");
                return null;
            }
            else
            {
                foreach (var bookNode in bookNodes)
                {

                    var titleNode = bookNode.SelectSingleNode(".//h3/a");
                    var priceNode = bookNode.SelectSingleNode(".//p[@class='price_color']");
                    var availabilityNode = bookNode.SelectSingleNode(".//p[@class='instock availability']");


                    string title = titleNode?.InnerText?.Trim() ?? "Название не найдено";
                    string price = priceNode?.InnerText?.Trim() ?? "Цена не найдена";
                    string availability = availabilityNode?.InnerText?.Trim() ?? "Нет в наличии";
                    bookList.Add(new Book { Title = title, Price = price, Availability = availability });
                }
                return bookList;
            }
        }

        public List<string> ParseBookUrl(string html, string url)
        {
            List<string> urlBookList = new List<string>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            string bookUrl = " ";
            var bookNodes = doc.DocumentNode.SelectNodes("//ol/li");
            if (bookNodes == null)
            {
                Console.WriteLine("⚠️ Книга не найдена. Возможно, изменилась структура сайта.");
            }
            else
            {
                foreach (var bookNode in bookNodes)
                {
                    var currentBook = bookNode.SelectSingleNode(".//h3/a");
                    string bookReference = currentBook.GetAttributeValue("href", "");
                    Uri fullUri = new Uri(new Uri(url), bookReference);
                    bookUrl = fullUri.ToString();
                    //Console.WriteLine(bookUrl);
                    urlBookList.Add(bookUrl);
                }
            }
            return urlBookList;
        }

        public Book ParseBookDetail(string html, string detailurl)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            Book book = new Book();
            var titleNode = doc.DocumentNode.SelectSingleNode("//h1");
            var priceNode = doc.DocumentNode.SelectSingleNode(".//p[@class='price_color']");
            var availabilityNode = doc.DocumentNode.SelectSingleNode(".//p[@class='instock availability']");
            var ratingNode = doc.DocumentNode.SelectSingleNode("//p[contains(@class, 'star-rating')]");
            string currentRatingNode = ratingNode.GetAttributeValue("class", "");

            var descriptionNode = doc.DocumentNode.SelectSingleNode("//div[@id='product_description']/following-sibling::p");
            var reviewsCountNode = doc.DocumentNode.SelectSingleNode("//*[@id='content_inner']//th[contains(text(),'Number of reviews')]/following-sibling::td");


            string title = titleNode?.InnerText?.Trim() ?? "Название не найдено";
            string price = priceNode?.InnerText?.Trim() ?? "Цена не найдена";
            string availability = availabilityNode?.InnerText?.Trim() ?? "Нет в наличии";
            string rating = "";
            if (!string.IsNullOrEmpty(currentRatingNode))
            {
                rating = currentRatingNode?.Replace("star-rating ", "").Trim() ?? "Рейтинг отсутствует";
            }
            string description = descriptionNode?.InnerText?.Trim() ?? "Описание не найдено";
            string reviewsCount = reviewsCountNode?.InnerText?.Trim() ?? "Оценки не найдены";
            string detailUrl = detailurl;

            book = new Book(title, price, availability, rating, description, reviewsCount, detailUrl);

            return book;
        }
    }
}
