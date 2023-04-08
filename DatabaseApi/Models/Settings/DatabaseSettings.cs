namespace DatabaseApi.Models.Settings
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null;

        public string DatabaseName { get; set; } = null!;

        public string ScenarioCollectionName { get; set; } = null!;

        public string UserCollectionName { get; set; } = null!;
    }
}
