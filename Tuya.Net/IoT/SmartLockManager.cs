using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Xml.Linq;
using Tuya.Net.Data;
using Tuya.Net.Data.Request;
using Tuya.Net.Security;

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
        private readonly ILogger? _logger;

        private PasswordTicket? _passwordTicket = null;

        public SmartLockManager(ITuyaClient client, ILogger? logger)
        {
            this._logger = logger;
            this._client = client;
        }

        public async Task<TemporaryPassword?> CreateTemporaryPasswordAsync(string deviceId, string pin, string name, long startTime, long endTime, CancellationToken ct = default)
        {
            _logger.LogInformation($"Getting temporary key for device: {deviceId}");
            //Check if password ticket is expired
            if (_passwordTicket == null || _passwordTicket.ExpireTime < (new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()))
            {
                _logger.LogInformation("Password ticket expired, getting new ticket");
                _passwordTicket = await GetPasswordTicketAsync(new PasswordTicketRequest
                {
                    deviceId = deviceId,
                }, ct);
            }
            _logger.LogInformation($"Using password ticket: {_passwordTicket?.TicketKey}");
            if(_passwordTicket == null)
            {
                throw new Exception("Password ticket is null");
            }
            var key = EncryptionUtils.DecryptAES128("21924dc0b7aa495fa731f84715b2ac85", _passwordTicket.TicketKey);
            var encryptedPassword = EncryptionUtils.EncryptAES128(key, pin);

            CreateTemporaryPasswordRequest createTemporaryPasswordRequest = new CreateTemporaryPasswordRequest()
            {
                deviceId = deviceId,
                name = name,
                password = encryptedPassword,
                effective_time = startTime,
                invalid_time = endTime,
                password_type = "ticket",
                ticket_id = _passwordTicket?.TicketId
            };

            return await CreateTemporaryPasswordAsync(createTemporaryPasswordRequest, ct);
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
 