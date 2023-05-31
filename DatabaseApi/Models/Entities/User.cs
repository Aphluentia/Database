using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class User
    {
        [BsonId]
        public string Email { get; set; }
        public Guid WebPlatformId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public ISet<string> Modules { get; set; } = new HashSet<string>();
        public ISet<string> ActiveScenarios { get; set; } = new HashSet<string>();
        public int PermissionLevel { get; set; } = 0;

    }
}
