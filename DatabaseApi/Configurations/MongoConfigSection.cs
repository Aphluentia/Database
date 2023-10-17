namespace DatabaseApi.Configurations
{
    public class MongoConfigSection
    {

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TherapistCollectionName { get; set; }
        public string PatientsCollectionName { get; set; }
        public string ApplicationsCollectionName { get; set; }
        public string ModulesCollectionName { get; set; }
    }
}
