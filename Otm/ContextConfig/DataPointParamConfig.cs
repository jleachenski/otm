using System.Text.Json;
using System.Text.Json.Serialization;
using System;

namespace Otm.ContextConfig
{
    public class DataPointParamConfig
    {
        public string Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TypeCode TypeCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Modes Mode { get; set; }

        public Object Value { get; set; }
    }
}