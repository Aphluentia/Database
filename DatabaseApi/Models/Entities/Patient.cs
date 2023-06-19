using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class Patient
    {
        [BsonId]
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public int Age { get; set; }
        public string ConditionName { get; set; }
        public DateTime ConditionAcquisitionDate { get; set; }
        public string ProfilePicture { get; set; }
        public WebPlatform WebPlatform { get; set; }
        public HashSet<string> AcceptedTherapists { get; set; }
        public HashSet<string> RequestedTherapists { get; set; }



    }
}
