using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tuya.Net.Data;
using Tuya.Net.Data.Request;

namespace Tuya.Net.IoT
{
    internal class SmartLockManager : ISmartLockManager
    {
        /// <summary>
        /// Tuya client instance.
        /// </summary>
        private readonly ITuyaClient _client;

        /// <summary>
        /// Logger instance.
        /// </summary>
        private readonly ILogger _logger;

        public SmartLockManager(ITuyaClient client, ILogger logger)
        {
            this._logger = logger;
            this._client = client;
        }

        public async Task<TemporaryPassword?> CreateTemporaryPasswordAsync(CreateTemporaryPasswordRequest createTemporaryPasswordRequest, CancellationToken ct = default)
        {
            _logger.LogInformation($"Getting temporary key for device: {createTemporaryPasswordRequest.deviceId}");
            return await _client.RequestAsync<TemporaryPassword>(HttpMethod.Post, $"/v1.0/devices/{createTemporaryPasswordRequest.deviceId}/door-lock/temp-password", JsonConvert.SerializeObject(createTemporaryPasswordRequest), cancellationToken: ct);
        }

        public async Task<PasswordTicket?> GetPasswordTicketAsync(PasswordTicketRequest passwordTicketRequest, CancellationToken ct = default)
        {
            _logger.LogInformation("");
            return await _client.RequestAsync<PasswordTicket>(HttpMethod.Post, $"/v1.0/devices/{passwordTicketRequest.deviceId}/door-lock/password-ticket", JsonConvert.SerializeObject(passwordTicketRequest), cancellationToken: ct);
        }
    }
}
 