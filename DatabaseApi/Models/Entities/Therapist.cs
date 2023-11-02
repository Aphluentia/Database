using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class Therapist: User
    {
        public string Credentials { get; set; }
        public string Description { get; set; }
        public HashSet<string> AcceptedPatients { get; set; }
        public HashSet<string> RequestedPatients { get; set; }

    }
}
