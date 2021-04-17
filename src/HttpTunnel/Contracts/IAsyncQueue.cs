using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HttpTunnel.Contracts
{
    public interface IAsyncQueue<T>
    {
        public void Enqueue(T item);

        Task<(bool, T)> TryDequeue(TimeSpan timeout);
    }
}
