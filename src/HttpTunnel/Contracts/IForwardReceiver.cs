using System.Threading.Tasks;
using HttpTunnel.Models;

namespace HttpTunnel.Contracts
{
    /// <summary>
    /// This interface defines the component that receives the request data and returns
    /// the response.
    /// </summary>
    public interface IForwardReceiver : IRequestReceiver
    {

    }
}
