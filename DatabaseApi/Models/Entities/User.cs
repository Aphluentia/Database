using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using DatabaseApi.Models.Dtos.Enums;

namespace DatabaseApi.Models.Dtos.Entities
{
    public class User
    {
       
        public Guid? WebPlatformId { get; set; }
        [BsonId]
        public string Email { get; set; }
        public string Name { get; set; }
        public ISet<string>? ModuleIds { get; set; }
        public ISet<string>? ActiveScenariosIds { get; set; }

        public string Password { get; set; }
        public PermissionLevel? PermissionLevel { get; set; }

    }
}
