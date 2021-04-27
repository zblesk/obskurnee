using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Obskurnee.Models;
using System;
using System.Threading.Tasks;

namespace Obskurnee.Services
{
    public class MatrixService
    {
        private readonly ILogger<MatrixService> _logger;
        private readonly MatrixConfig _config;

        public MatrixService(
            ILogger<MatrixService> logger,
            IConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config
                .GetSection(MatrixConfig.ConfigName)
                .Get<MatrixConfig>();
        }

        public async Task SendMessage(string message)
        {
            if (_config == null || !_config.Enabled)
            {
                _logger.LogInformation("Matrix notification disabled or not configured; not sending");
                return;
            }
            try
            {
                _logger.LogInformation("Sending a notification to Matrix");
                await $"{_config.Homeserver}/_matrix/client/r0/rooms/{_config.RoomId}/send/m.room.message?access_token={_config.AccessToken}"
                    .PostJsonAsync(new
                    {
                        msgtype = "m.text",
                        body = message,
                        format = "org.matrix.custom.html",
                        formatted_body = message.RenderMarkdown(),
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sending a message to Matrix failed");
            }
        }

        public string GetEnabledMatrixRoomLink()
        {
            if (!_config.Enabled)
            {
                return null;
            }
            if (!string.IsNullOrWhiteSpace(_config.RoomDisplayAddress))
            {
                return _config.RoomDisplayAddress;
            }
            return _config.RoomId;
        }
    }
}