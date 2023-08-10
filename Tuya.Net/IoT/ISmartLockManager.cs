using Tuya.Net.Data;
using Tuya.Net.Data.Request;

namespace Tuya.Net.IoT
{
    public interface ISmartLockManager
    {
        /// <summary>
        /// Get Temporary Passwords
        /// </summary>
        /// <param name="deviceId">Tuya device ID.</param>
        /// <param name="name">The name of a specified temporary password.</param>
        /// <param name="password">The length of the original password is seven digits for Wi-Fi locks and six digits for Zigbee locks and Bluetooth locks. The password is encrypted by using the AES-128 algorithm with ECB mode and PKCS7Padding. To get the original key, decrypt the temporary key ticket_key with AES using the accessKey that is issued by the platform. The output format is hex.	</param>
        /// <param name="effective_time">The 10-digit timestamp of the effective time. Unit: seconds (s).	</param>
        /// <param name="invalid_time">The 10-digit timestamp of the expiration time. Unit: seconds (s).	</param>
        /// <param name="password_type">The password is encrypted using a ticket.	</param>
        /// <param name="ticket_id">The ID of a specified temporary key.	</param>
        public Task<TemporaryPassword?> CreateTemporaryPasswordAsync(CreateTemporaryPasswordRequest createTemporaryPasswordRequest, CancellationToken ct = default);

        /// <summary>
        /// Get Temporary Password using params
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="pin"></param>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<TemporaryPassword?> CreateTemporaryPasswordAsync(string deviceId, string pin, string name, long startTime, long endTime, CancellationToken ct = default);

        /// <summary>
        /// Get a temporary key for password encryption
        /// </summary>
        /// <param name="deviceId">Tuya device ID.</param>
        public Task<PasswordTicket?> GetPasswordTicketAsync(PasswordTicketRequest passwordTicketRequest, CancellationToken ct = default);
    }
}
