using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Nodes;

namespace DatabaseApi.Models.Entities
{
    public class Module
    {
        public int ModuleType { get; set; }
        [BsonId]
        public string Id { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; }
        public string Checksum { get; set; }
    }
}
