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
    [Route("[controller]")]
    public class ResponsesController : ControllerBase
    {
        private readonly IBackwardRequestHandler backwardRequestHandler;

        public ResponsesController(IBackwardRequestHandler backwardRequestHandler)
        {
            this.backwardRequestHandler = backwardRequestHandler;
        }

        [HttpPut("{requestId}")]
        public void Put(int requestId, [FromBody] ResponseData response)
        {
            this.backwardRequestHandler.SetResponse(requestId, response);
        }
    }
}
