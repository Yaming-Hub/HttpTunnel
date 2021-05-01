using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpTunnel.Models
{
    internal static class MediaTypeExtensions
    {
        public static bool IsTextContent(this MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            if (mediaTypeHeaderValue == null)
            {
                return false;
            }

            return mediaTypeHeaderValue.MediaType.IsTextContent();

        }

        public static bool IsTextContent(this string mediaType)
        {
            if (mediaType == null)
            {
                return false;
            }

            mediaType = mediaType.ToLower();

            if (mediaType.StartsWith("text/"))
            {
                return true;
            }

            if (mediaType.StartsWith("application/json"))
            {
                return true;
            }

            return false;
        }
    }
}
