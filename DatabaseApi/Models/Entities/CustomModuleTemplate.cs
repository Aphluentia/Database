using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class CustomModuleTemplate: ModuleVersion
    {
        public string ModuleName { get; set; }
        public static CustomModuleTemplate FromModuleTemplate(Application template, ModuleVersion version)
        {
            return new CustomModuleTemplate
            {
                ModuleName = template.ModuleName,
                VersionId = version.VersionId,
                Timestamp = DateTime.UtcNow,
                DataStructure = version.DataStructure,
                HtmlCard = version.HtmlCard,
                HtmlDashboard = version.HtmlDashboard

            };
        }
        public static CustomModuleTemplate FromExistingModule(Module module)
        {
            return new CustomModuleTemplate
            {
                ModuleName = module.ModuleTemplate.ModuleName,
                VersionId = module.ModuleTemplate.VersionId,
                Timestamp = module.ModuleTemplate.Timestamp,
                DataStructure = module.ModuleTemplate.DataStructure,
                HtmlCard = module.ModuleTemplate.HtmlCard,
                HtmlDashboard = module.ModuleTemplate.HtmlDashboard

            };
        }
    }
}
