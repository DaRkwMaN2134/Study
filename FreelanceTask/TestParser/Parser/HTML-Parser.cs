using DataModel;
using HtmlAgilityPack;


namespace Parser
{
    public class HTML_Parser
    {
        public List<Book> BookParse(string html)
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
}
