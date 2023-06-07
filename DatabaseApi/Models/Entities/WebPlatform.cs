namespace DatabaseApi.Models.Entities
{
    public class WebPlatform
    {

        public Guid WebPlatformId { get; set; }
        public ISet<Module> Modules { get; set; } = new HashSet<Module>();
        public ISet<string> ActiveScenarios { get; set; } = new HashSet<string>();
    }
}
