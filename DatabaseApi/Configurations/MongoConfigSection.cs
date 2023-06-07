namespace DatabaseApi.Configurations
{
    public class MongoConfigSection
    {

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TherapistCollectionName { get; set; }
        public string PatientsCollectionName { get; set; }
        public string ModuleTemplatesCollectionName { get; set; }
    }
}
