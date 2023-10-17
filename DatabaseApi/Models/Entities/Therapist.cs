using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class Therapist
    {
        [BsonId]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Credentials { get; set; }
        public string Description { get; set; }
        public string ProfilePicture { get; set; }
        public HashSet<string> AcceptedPatients { get; set; }
        public HashSet<string> RequestedPatients { get; set; }

    }
}
