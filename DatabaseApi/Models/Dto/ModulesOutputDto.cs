using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace DatabaseApi.Models.Dto
{
    public class ModulesOutputDto
    {
        public int ModuleType { get; set; }
        public string Id { get; set; }
        public string Data { get; set; }
        public DateTime DateTime { get; set; }
        public string Checksum { get; set; }
    }
}
