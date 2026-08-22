namespace DataLibrary
{
    public class Book
    {
        public string Title { get; set; }
        public string Price { get; set; }
        public string Availability { get; set; }
        public string Rating { get; set; }
        public string Description { get; set; }
        public string ReviewsCount { get; set; }
        public string DetailUrl { get; set; }
        public Book() { }
        public Book(string title, string price, string availability, string rating, string description, string reviewscount, string detailurl)
        {
            Title = title;
            Price = price;
            Availability = availability;
            Rating = rating;
            Description = description;
            ReviewsCount = reviewscount;
            DetailUrl = detailurl;
        }
    }
}
