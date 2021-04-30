using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HttpTunnel.Diagnostics
{
    public static class LogEvents
    {
        public static EventId RequestReceived = new EventId(id: 0x01, name: "RequestReceived");

        public static EventId ResponseReturned = new EventId(id: 0x02, name: "ResponseReturned");

        public static EventId RequestSent = new EventId(id: 0x03, name: "RequestSent");

        public static EventId ResponseReceived = new EventId(id: 0x04, name: "ResponseReceived");
    }
}
