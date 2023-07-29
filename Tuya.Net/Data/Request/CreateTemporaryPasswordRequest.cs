using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuya.Net.Data.Request
{
    public class CreateTemporaryPasswordRequest
    {
        [JsonProperty("device_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? deviceId { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? name { get; set; }

        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string? password { get; set; }

        [JsonProperty("effective_time", NullValueHandling = NullValueHandling.Ignore)]
        public long? effective_time { get; set; }

        [JsonProperty("invalid_time", NullValueHandling = NullValueHandling.Ignore)]
        public long? invalid_time { get; set; }

        [JsonProperty("password_type", NullValueHandling = NullValueHandling.Ignore)]
        public string? password_type { get; set; }

        [JsonProperty("ticket_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? ticket_id { get; set; }
    }
}
