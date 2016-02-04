using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sunlight
{
    sealed class RemoteResult<T>
    {
        private const int WAITING = 1;
        private const int NOT_WAITING = 0;

        private int _waiting = NOT_WAITING;

        public RemoteResult()
        {
            Result = default(T);
        }

        public T Result { get; private set; }

        public async Task Execute(Func<bool> executeIf, Func<T> func, Action continuation = null)
        {
            if (Enter() && executeIf())
            {
                await Task.Run<T>(() => func()).ContinueWith((antecedent) =>
                {
                    try
                    {
                        Result = antecedent.Result;
                    }
                    catch (AggregateException e)
                    {
                    }

                    if (continuation != null)
                    {
                        continuation();
                    }
                });
            }
        }
        public async Task Execute<T>(Func<Task<T>> func, Action<T> continuation = null)
        {
            if (Enter())
            {
                await Task.Run(func).ContinueWith(t =>
                {
                    Exit();
                    if (continuation != null)
                    {
                        continuation(t.Result);
                    }
                });
            }
        }

        public bool Enter()
        {
            return Interlocked.CompareExchange(ref _waiting, WAITING, NOT_WAITING) == NOT_WAITING;
        }

        public void Exit()
        {
            if (Interlocked.Exchange(ref _waiting, NOT_WAITING) == NOT_WAITING)
                throw new InvalidOperationException("Critical section exited but is not entered");
        }
    }
}
