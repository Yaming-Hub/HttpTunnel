using HttpTunnel.Models;

namespace HttpTunnel.Contracts
{
    /// <summary>
    /// This interface defines a component which prevents request from looping 
    /// cross tunnels. 
    /// </summary>
    public interface ILoopBreaker
    {    
        /// <summary>
        /// This method tags the request as being proxied, and if the request is already
        /// tagged, that means a loop is detected and the breaker will throw to stop the loop.
        /// </summary>
        /// <param name="requestData">The request.</param>
        void ValidateAndTag(RequestData requestData);
    }
}
