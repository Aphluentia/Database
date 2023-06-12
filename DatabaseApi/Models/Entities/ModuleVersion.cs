using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class ModuleVersion
    {
        [BsonId]
        public string VersionId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? DataStructure { get; set; }
        public string? HtmlCard { get; set; }
        public string? HtmlDashboard { get; set; }
    }
}
