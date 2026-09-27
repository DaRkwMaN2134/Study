namespace DataLibrary
{
    public class Post
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Post() { }
        public Post(int userid, int id, string title, string body)
        {
            UserId = userid;
            Id = id;
            Title = title;
            Body = body;
        }
    }

    public class PostRequest
    {
        public int UserId { get; set; }
        public int id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public PostRequest() { }
        public PostRequest(int userid, int Id, string title, string body)
        {
            UserId = userid;
            id = Id;
            Title = title;
            Body = body;
        }
    }
}
