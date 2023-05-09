using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using DatabaseApi.Models.Dtos.Enums;

namespace DatabaseApi.Models.Dtos.Entities
{
    public class Connection
    {
        public DateTime? Creation { get; set; } = DateTime.UtcNow;
        public string ModuleId { get; set; } = null!;

        public ApplicationType ApplicationType{ get; set; }

    }
}
