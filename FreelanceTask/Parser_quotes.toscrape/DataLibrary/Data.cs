namespace DataLibrary
{
    public class Quote
    {
        public int id { get; set; }
        public string text { get; set; }
        public string author { get; set; }
        public List<string> tags { get; set; }
        public Quote() { }
        public Quote(string Text, string Author, List<string> Tags)
        {
            text = Text;
            author = Author;
            tags = Tags;
        }
    }
}