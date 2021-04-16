using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpTunnel.Models
{
    public class RequestMessage
    {
        private const string LF = "337da65f2f124c3197b2474cd958e359"; // \n
        private const string CR = "37b531635fb74d1fb77aedb9949901a6"; // \r

        public string Data { get; set; }

        public RequestMessage Create(RequestData request)
        {
            var json = JsonSerializer.Serialize(request);

            // Encode new line characters
            json = json.Replace("\r", CR).Replace("\n", LF);

            return new RequestMessage { Data = json };
        }

        public RequestData Request
        {
            get
            {
                var json = this.Data.Replace(CR, "\r").Replace(LF, "\n");
                return JsonSerializer.Deserialize<RequestData>(json);
            }
        }
    }
}
