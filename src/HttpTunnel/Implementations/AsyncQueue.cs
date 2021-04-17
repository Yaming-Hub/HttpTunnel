using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HttpTunnel.Contracts;

namespace HttpTunnel.Implementations
{
    public class AsyncQueue<T> : IAsyncQueue<T>
    {
        private readonly object lockObject;
        private readonly Queue<TaskCompletionSource<(bool, T)>> asks;
        private readonly Queue<T> backlog;

        public AsyncQueue()
        {
            this.lockObject = new object();
            this.asks = new Queue<TaskCompletionSource<(bool, T)>>();
            this.backlog = new Queue<T>();
        }

        public void Enqueue(T item)
        {
            lock (this.lockObject)
            {
                if (this.asks.TryDequeue(out TaskCompletionSource<(bool, T)> ask))
                {
                    // There is a pending ask for the item, just return it.
                    ask.SetResult((true, item));
                }
                else
                {
                    // Nobody is asking for the item now, put it into backlog.
                    this.backlog.Enqueue(item);
                }
            }
        }

        public Task<(bool, T)> TryDequeue(TimeSpan timeout)
        {
            lock (this.lockObject)
            {
                if (this.backlog.TryDequeue(out T item))
                {
                    return Task.FromResult((true, item));
                }
                else
                {
                    var ask = new TaskCompletionSource<(bool, T)>();

                    if (timeout != TimeSpan.Zero)
                    {
                        CancellationTokenSource cts = new CancellationTokenSource(timeout);
                        cts.Token.Register(() => { ask.SetResult((false, default(T))); });
                    }

                    this.asks.Enqueue(ask);

                    return ask.Task;
                }
            }
        }
    }
}
