namespace PjoterParker.Api.Database.Entities
{
    public class UniquenessTable
    {
        public UniquenessTable(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}