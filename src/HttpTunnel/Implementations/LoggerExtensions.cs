using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HttpTunnel.Models;
using Microsoft.Extensions.Logging;

namespace HttpTunnel.Implementations
{
    public static class LoggerExtensions
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static void LogData<T>(this ILogger logger, EventId eventId, T data)
        {
            var message = JsonSerializer.Serialize(data, JsonSerializerOptions);
            logger.LogInformation(eventId, message);
        }

        public static void LogRequestData(this ILogger logger, EventId eventId, RequestData requestData)
            => logger.LogData(eventId, requestData);

        public static void LogResponseData(this ILogger logger, EventId eventId, ResponseData responseData)
            => logger.LogData(eventId, responseData);
    }
}
