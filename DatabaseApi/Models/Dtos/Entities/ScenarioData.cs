using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DatabaseApi.Models.Dtos.Entities
{
    public class ScenarioData
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public IList<string> Descriptions { get; set; }
        public IList<string> Names { get; set; }
        public IList<string> People { get; set; }
        public IList<string> Social { get; set; }
        public IList<string> Verbs { get; set; }
    }
}
