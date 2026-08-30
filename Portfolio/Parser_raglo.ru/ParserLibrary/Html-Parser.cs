using DataLibrary;
using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace ParserLibrary
{
    public class Html_Parser
    {
        static Http_Client client = new Http_Client();
        public async Task<List<Card>> ParseCategoryAsync(string html, string baseUrl)
        {
            var mainDoc = new HtmlDocument();
            mainDoc.LoadHtml(html);
            var allCardNode = mainDoc.DocumentNode.SelectNodes("//div[contains(@class, 'product-items-block')]//div[contains(@class, 'item-wrap col')]");
            if (allCardNode != null)
            {

                var categoryNameNone = mainDoc.DocumentNode.SelectSingleNode("/html/head/title");
                var categoryName = categoryNameNone?.InnerText?.Trim();
                categoryName = categoryName?.Replace("Основной каталог", "").Replace("Raglo", "").Trim() ?? "";

                var cards = new ConcurrentBag<Card>();

                var options = new ParallelOptions { MaxDegreeOfParallelism = 20 };
                await Parallel.ForEachAsync(allCardNode, options, async (cardNode, token) =>
                {
                    try
                    {
                        string article = "";
                        string urlimage = "";
                        string price = "";
                        string description = "";
                        string currentCardHtml = "";


                        var articleNode = cardNode.SelectSingleNode(".//div[contains(@class, 'product-info')]/a");
                        var pictureUrlNode = cardNode.SelectSingleNode(".//div[contains(@class, 'product-image-block')]//a//div");
                        var priceNode = cardNode.SelectSingleNode(".//div[contains(@class, 'product-info')]//div[contains(@class, 'price-block')]//div[(@class='price font-body bold')]");

                        if (articleNode != null)
                        {
                            article = articleNode.GetAttributeValue("href", "");
                            article = article.Trim('/').Split('/').LastOrDefault() ?? "";

                        }
                        else
                        {
                            Console.WriteLine("Артикль пуст");
                            article = "-";
                        }


                        if (pictureUrlNode != null)
                        {
                            urlimage = "https://raglo.ru" + pictureUrlNode.GetAttributeValue("data-src", "");
                        }
                        else
                        {
                            Console.WriteLine("Изображение пусто");
                            urlimage = "-";
                        }

                        if (priceNode != null)
                        {
                            price = priceNode?.InnerText?.Trim();
                            var numberMatch = System.Text.RegularExpressions.Regex.Match(price.Replace(" ", "").Replace("&nbsp;", ""), @"\d+[\d,.]*");
                            price = numberMatch.ToString();
                        }
                        else
                        {
                            Console.WriteLine("Цена пуста");
                            price = "-";
                        }

                        if (article != "-")
                        {
                            var cardUrl = baseUrl + article + "/";
                            try
                            {
                                currentCardHtml = await client.HttpRequestAsync(cardUrl);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            if (currentCardHtml != null)
                            {
                                var cardDoc = new HtmlDocument();
                                cardDoc.LoadHtml(currentCardHtml);


                                var currentCardNode = cardDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'text-block')]");
                                if (currentCardNode != null)
                                {
                                    var descriptionNode = currentCardNode.SelectSingleNode(".//div[@itemprop='description']");

                                    if (descriptionNode != null)
                                    {
                                        description = descriptionNode?.InnerText?.Trim();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Описание пустое");
                                        description = "-";
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Описание пустое");
                                    description = "-";
                                }
                            }
                        }
                        else
                        {
                            description = "-";
                        }

                        Card currentCard = new Card(categoryName, article, urlimage, price, description);
                        cards.Add(currentCard);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
                return cards.ToList();
            }
            else
            {
                return new List<Card>();
            }
        }

        public string ParseUrl(string html, string url)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            string nextpageUrl = " ";
            var nextNode = doc.DocumentNode.SelectSingleNode("//div[@class='page-item ']//a");

            if (nextNode != null)
            {
                string nexPageReference = nextNode.GetAttributeValue("href", "");
                Uri fullUri = new Uri(new Uri(url), nexPageReference);
                nextpageUrl = fullUri.ToString();
                return nextpageUrl;
            }
            else
            {
                return null;
            }
        }
    }
}
