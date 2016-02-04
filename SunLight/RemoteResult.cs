using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sunlight
{
    sealed class RemoteResult<T>
    {
        private sealed class Scope : IDisposable
        {
            private readonly RemoteResult<T> _parent;

            public Scope(RemoteResult<T> parent)
            {
                _parent = parent;
                Entered = _parent.Enter();
            }

            public bool Entered { get; private set; }

            public void Dispose()
            {
                if (Entered)
                {
                    _parent.Exit();
                }
            }
        }

        private const int WAITING = 1;
        private const int NOT_WAITING = 0;

        private int _waiting = NOT_WAITING;

        private readonly T _sentinal;

        public RemoteResult(T sentinal)
        {
            Result = sentinal;
            _sentinal = sentinal;
        }

        public T Result { get; private set; }

        public AggregateException Exceptions { get; private set; }

        public bool IsFaulted => Exceptions != null;

        public void Reset()
        {
            using (var scope = new Scope(this))
            {
                if (scope.Entered)
                {
                    Result = _sentinal;
                    Exceptions = null;
                }
            }
        }

        public async Task Execute(Func<bool> executeIf, Func<Task<T>> func, Action continuation = null)
        {
            using (var scope = new Scope(this))
            {
                if (scope.Entered && object.ReferenceEquals(Result, _sentinal) && executeIf())
                {
                    await Task.Run<T>(async () => await func()).ContinueWith((antecedent) =>
                    {
                        try
                        {
                            Result = antecedent.Result;
                        }
                        catch (AggregateException e)
                        {
                            Debug.Assert(false, e.Message);
                            Result = _sentinal;
                            Exceptions = e;
                        }

                        if (continuation != null)
                        {
                            continuation();
                        }
                    });
                }
            }
        }

        private bool Enter()
        {
            return Interlocked.CompareExchange(ref _waiting, WAITING, NOT_WAITING) == NOT_WAITING;
        }

        private void Exit()
        {
            if (Interlocked.Exchange(ref _waiting, NOT_WAITING) == NOT_WAITING)
                throw new InvalidOperationException("Critical section exited but is not entered");
        }
    }
}
