﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HttpTunnel.Models
{
    public static class Base64StreamReader
    {
        public static async Task<string> ReadStreamAsBase64String(Stream stream)
        {
            byte[] bytes = null;

            using (var memoryStream = new MemoryStream(capacity: 4096))
            {
                await stream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }
    }
}
