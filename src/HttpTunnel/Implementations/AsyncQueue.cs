using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HttpTunnel.Implementations
{
    public class AsyncQueue<T>
    {
        private readonly object lockObject;
        private readonly Queue<TaskCompletionSource<T>> asks;
        private readonly Queue<T> backlog;

        public AsyncQueue()
        {
            this.lockObject = new object();
            this.asks = new Queue<TaskCompletionSource<T>>();
            this.backlog = new Queue<T>();
        }

        public void Enqueue(T item)
        {
            lock (this.lockObject)
            {
                if (this.asks.TryDequeue(out TaskCompletionSource<T> ask))
                {
                    // There is a pending ask for the item, just return it.
                    ask.SetResult(item);
                }
                else
                {
                    // Nobody is asking for the item now, put it into backlog.
                    this.backlog.Enqueue(item);
                }
            }
        }
        public Task<T> Dequeue()
        {
            return this.Dequeue(TimeSpan.Zero, CancellationToken.None);
        }

        public Task<T> Dequeue(TimeSpan timeout, CancellationToken cancellationToken)
        {
            lock (this.lockObject)
            {
                if (this.backlog.TryDequeue(out T result))
                {
                    return Task.FromResult(result);
                }
                else
                {
                    var ask = new TaskCompletionSource<T>();

                    if (timeout != TimeSpan.Zero)
                    {
                        CancellationTokenSource cts = new CancellationTokenSource(timeout);
                        cts.Token.Register(() => { ask.TrySetCanceled(cancellationToken); });
                    }

                    this.asks.Enqueue(ask);

                    return ask.Task;
                }
            }
        }
    }
}
