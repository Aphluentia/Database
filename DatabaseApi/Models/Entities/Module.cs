using MongoDB.Bson;
using System.Text.Json.Nodes;

namespace DatabaseApi.Models.Entities
{
    public class Module
    {
        public int ModuleType { get; set; }
        public string Id { get; set; }
        public string Data { get; set; }
        public DateTime DateTime { get; set; }
        public string Checksum { get; set; }
    }
}
