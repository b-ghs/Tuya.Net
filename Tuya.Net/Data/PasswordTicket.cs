using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuya.Net.Data
{
    public class PasswordTicket
    {
        [JsonProperty("expire_time", NullValueHandling = NullValueHandling.Ignore)]
        public long? ExpireTime { get; set; }

        [JsonProperty("ticket_key", NullValueHandling = NullValueHandling.Ignore)]
        public string? TicketKey { get; set; }

        [JsonProperty("ticket_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? TicketId { get; set; }
    }
}
