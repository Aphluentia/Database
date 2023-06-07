namespace DatabaseApi.Models.Entities
{
    public class Patient
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public int Age { get; set; }
        public string ConditionName { get; set; }
        public DateTime ConditionAcquisitionDate { get; set; }
        public string ProfilePicture { get; set; }
        public WebPlatform WebPlatform { get; set; }
        public string AssignedTherapist{ get; set; }

    }
}
