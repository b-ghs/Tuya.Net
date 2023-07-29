using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuya.Net.Data.Request
{
    public class PasswordTicketRequest
    {
        [JsonProperty("device_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? deviceId { get; set; }
    }
}
