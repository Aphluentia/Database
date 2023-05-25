using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using DatabaseApi.Models.Entities;

namespace DatabaseApi.Models.Dtos.Entities
{
    public class User
    {
       
        public Guid? WebPlatformId { get; set; }
        [BsonId]
        public string Email { get; set; }
        public string Name { get; set; }
        public ISet<string>? Modules { get; set; }
        public ISet<string>? ActiveScenariosIds { get; set; }

        public string Password { get; set; }
        public int? PermissionLevel { get; set; }

    }
}
