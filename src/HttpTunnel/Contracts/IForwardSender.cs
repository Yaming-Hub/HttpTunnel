using System.Threading.Tasks;
using HttpTunnel.Models;

namespace HttpTunnel.Contracts
{
    /// <summary>
    /// This interface defines the component that receives the request data and returns
    /// the response.
    /// </summary>
    public interface IForwardSender
    {
        /// <summary>
        /// Sends the request and return the reponse.
        /// </summary>
        /// <param name="requestData">The request.</param>
        /// <returns>The response.</returns>
        Task<ResponseData> Send(RequestData requestData);
    }
}
