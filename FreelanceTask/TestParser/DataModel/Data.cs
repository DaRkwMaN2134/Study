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
        public Book() { }
        public Book(string title, string price, string availability)
        {
            Title = title;
            Price = price;
            Availability = availability;
        }

    }
}
