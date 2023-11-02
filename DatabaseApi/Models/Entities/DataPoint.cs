using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class DataPoint
    {
        public string SectionName { get; set; }
        public string ContextName { get; set; }
        public bool isDataEditable { get; set; }
        public string Content { get; set; }
    }
}
