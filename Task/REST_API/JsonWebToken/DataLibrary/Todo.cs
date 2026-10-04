namespace DataLibrary
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public Todo(int id, string title, bool isdone)
        {
            Id = id;
            Title = title;
            IsDone = isdone;
        }
    }
}
