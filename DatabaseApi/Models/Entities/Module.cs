using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Nodes;

namespace DatabaseApi.Models.Entities
{
    public class Module
    {
        [BsonId]
        public string Id { get; set; }
        public ModuleVersion ModuleData { get; set; }
    }
}
