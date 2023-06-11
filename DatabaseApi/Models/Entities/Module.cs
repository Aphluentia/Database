﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Nodes;

namespace DatabaseApi.Models.Entities
{
    public class Module
    {
        [BsonId]
        public string Id { get; set; }
        public string Data { get; set; }
        public string ModuleTemplate { get; set; }
        public bool IsAssigned { get; set; } = false;
        public DateTime Timestamp { get; set; }
        public string Checksum { get; set; }
    }
}
