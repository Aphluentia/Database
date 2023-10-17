using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class DataPoint
    {
        [BsonId]
        public string SectionName { get; set; }
        public bool isDataEditable { get; set; }
        public string Content { get; set; }
    }
}
