using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class ModuleRegistry
    {
        [BsonId]
        public string ModuleName { get; set; }
        public ICollection<ModuleVersion> Versions { get; set; }
    }
}
