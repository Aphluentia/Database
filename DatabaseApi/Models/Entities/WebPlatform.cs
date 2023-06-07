using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class WebPlatform
    {

        [BsonId]
        public Guid WebPlatformId { get; set; }
        public ISet<Module> Modules { get; set; } = new HashSet<Module>();
        public ISet<string> ActiveScenarios { get; set; } = new HashSet<string>();
    }
}
