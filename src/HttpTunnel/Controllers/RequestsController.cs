using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.AspNetCore.Mvc;

namespace HttpTunnel.Controllers
{
    [ApiController]
    [Route("tunnel/[controller]")]
    public class RequestsController : ControllerBase
    {
        private readonly IForwardSender forwardSender;

        public RequestsController(IForwardSender forwardSender)
        {
            this.forwardSender = forwardSender;
        }

        [HttpPost]
        public Task<ResponseData> Post([FromBody]RequestData request)
        {
            return this.forwardSender.Send(request);
        }
    }
}
