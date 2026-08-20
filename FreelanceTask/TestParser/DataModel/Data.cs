namespace DataModel
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public Post() { }
        public Post(int id, int userid, string title, bool completed)
        {
            Id = id;
            UserId = userid;
            Title = title;
            Completed = completed;
        }
    }
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
        public Book(string title, string price, string availability, string rating, string description, string reviewscount,string detailurl)
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

    public class Quote
    {
        public string Text { get; set; }
        public string Author { get; set; }
        public List<string> Tags { get; set; }
        public Quote() { }
        public Quote(string text, string author, List<string> tags)
        {
            Text = text;
            Author = author;
            Tags = tags;
        }
    }
}
