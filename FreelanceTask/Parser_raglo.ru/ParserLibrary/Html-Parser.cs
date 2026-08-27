using DataLibrary;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ParserLibrary
{
    public class Html_Parser
    {
        public async Task<List<Card>> ParseCategoryAsync(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var allCardNode = doc.DocumentNode.SelectNodes("//div[contains(@class, 'product-items-block')]//div[contains(@class, 'item-wrap col')]");

            var categoryNameNone = doc.DocumentNode.SelectSingleNode("/html/head/title");
            var categoryName = categoryNameNone?.InnerText?.Trim();

            List<Card> cards = new List<Card>();

            foreach (var card in allCardNode)
            {
                var articleNode = card.SelectSingleNode(".//div[contains(@class, 'product-info')]/a");
                var pictureUrlNode = card.SelectSingleNode(".//div[contains(@class, 'product-image-block')]//a//div");
                var priceNode = card.SelectSingleNode(".//div[contains(@class, 'product-info')]//div[contains(@class, 'price-block')]//div[contains(@class, 'price font-body bold')]");
                //var descriptionNode = card.SelectSingleNode();

                var currenrCardUrl = card.SelectSingleNode(".//div[contains(@class, 'product-info')]//a");
                var cardUrl = currenrCardUrl.GetAttributeValue("href", "");

                //var currentCardNode = ;




                var article = articleNode.GetAttributeValue("href", "");
                var urlimage = "https://raglo.ru" + pictureUrlNode.GetAttributeValue("data-src", "");
                var price = priceNode?.InnerText?.Trim();
                //var description = descriptionNode?.InnerText?.Trim();
                var description = "";

                Card currentCard = new Card(categoryName, article, urlimage, price, description);
                cards.Add(currentCard);
            }
            /*foreach (var card in cards)
            {
                Console.WriteLine($"Категория товаров {card.categoryname}\n Артикль {card.article}\n Ссылка на картинку {card.pictureurl}\n Цена {card.price}\n Описание {card.description}\n\n");
            }*/
            return cards;
        }

        public string ParseUrl(string html, string url)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            string nextpageUrl = " ";
            var nextNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'pagination-nav')]//div[@class='page-item ']//a");

            if (nextNode != null)
            {
                string nexPageReference = nextNode.GetAttributeValue("href", "");
                Uri fullUri = new Uri(new Uri(url), nexPageReference);
                nextpageUrl = fullUri.ToString();
                //Console.WriteLine(cardUrl);
                return nextpageUrl;
            }
            else
            {
                return null;
            }
        }
    }
}
