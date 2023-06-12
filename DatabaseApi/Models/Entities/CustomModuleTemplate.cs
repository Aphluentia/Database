using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseApi.Models.Entities
{
    public class CustomModuleTemplate: ModuleVersion
    {
        public string ModuleName { get; set; }
        public static CustomModuleTemplate FromModuleTemplate(ModuleRegistry template, ModuleVersion version)
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
    }
}
