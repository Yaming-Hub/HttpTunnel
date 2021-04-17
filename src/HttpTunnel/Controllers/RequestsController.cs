using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.AspNetCore.Mvc;

namespace HttpTunnel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RequestsController : ControllerBase
    {
        private readonly IForwardSender forwardSender;
        private readonly IAsyncQueue<RequestData> backwardRequestQueue;

        public RequestsController(IForwardSender forwardSender, IAsyncQueue<RequestData> backwardRequestQueue)
        {
            this.forwardSender = forwardSender;
            this.backwardRequestQueue = backwardRequestQueue;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var (succeeded, requestData) = await this.backwardRequestQueue.TryDequeue(TimeSpan.FromMinutes(1));
            if (succeeded)
            {
                return this.Ok(requestData);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        public Task<ResponseData> Post([FromBody] RequestData request)
        {
            return this.forwardSender.Send(request);
        }
    }
}
