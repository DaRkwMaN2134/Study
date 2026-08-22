namespace DataLibrary
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
}
