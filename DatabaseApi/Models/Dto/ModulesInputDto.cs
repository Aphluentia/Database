using MongoDB.Bson;

namespace DatabaseApi.Models.Dto
{
    public class ModulesInputDto
    {
        public int ModuleType { get; set; }
        public string Id { get; set; }
        public string Data { get; set; }
    }
}
