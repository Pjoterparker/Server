using System;

namespace PjoterParker.Api.Database.Entities
{
    public class UniquenessTable
    {
        public UniquenessTable(string key, string value, Guid aggrageteId)
        {
            Key = key;
            Value = value;
            AggrageteId = aggrageteId;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public Guid AggrageteId { get; set; }
    }
}