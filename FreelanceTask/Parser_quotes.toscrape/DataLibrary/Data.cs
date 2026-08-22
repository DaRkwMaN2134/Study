namespace DataLibrary
{
    public class Quote
    {
        public int Id { get; set; }
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