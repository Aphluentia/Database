using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class ModuleTemplate
    {
        [BsonId]
        public string ModuleType { get; set; }
        public string HtmlDashboard { get; set; }
        public string HtmlCard { get; set; }
    }
}
