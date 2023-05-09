using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Dtos.Entities
{
    public class Scenario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string ScenarioName { get; set; } = null!;

        public ScenarioData Data { get; set; }
    }
}
