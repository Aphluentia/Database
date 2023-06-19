using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class Application
    {
        [BsonId]
        public string ModuleName { get; set; }
        public ICollection<ModuleVersion> Versions { get; set; }
    }
}
