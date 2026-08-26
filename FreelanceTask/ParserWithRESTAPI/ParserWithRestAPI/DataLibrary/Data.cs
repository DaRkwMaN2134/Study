using System.ComponentModel.DataAnnotations;

namespace DataLibrary
{
    public class cache
    {
        [Key]
        public string key { get; set; }
        public string value { get; set; }
        public DateTime updated_at { get; set; }
        public cache() { }
        public cache(string Key, string Value, DateTime Updated_at)
        {
            key = Key;
            value = Value;
            updated_at = Updated_at;
        }
    }
}
