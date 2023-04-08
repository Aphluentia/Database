using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DatabaseApi.Models.Dtos.Entities
{
    public class User
    {
       
        public string? WebPlatformId { get; set; }
        [BsonId]
        public string Email { get; set; }
        public string Name { get; set; }
        public ISet<string> ActiveScenariosIds { get; set; }

        public ISet<Connection> Connections { get; set; }

    }
}
