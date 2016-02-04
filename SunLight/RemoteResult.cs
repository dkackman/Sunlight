using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sunlight
{
    sealed class RemoteResult<T> where T : class
    {
        private sealed class Scope : IDisposable
        {
            private readonly RemoteResult<T> _parent;
            private readonly bool _entered;

            public Scope(RemoteResult<T> parent)
            {
                _parent = parent;
                _entered = _parent.Enter();
            }

            public bool Entered => _entered;

            public void Dispose()
            {
                if (_entered)
                {
                    _parent.Exit();
                }
            }
        }

        private const int WAITING = 1;
        private const int NOT_WAITING = 0;

        private int _waiting = NOT_WAITING;

        private T _result;
        private readonly T _notSetSentinal;
        private AggregateException _exceptions;
        private readonly Func<Task<T>> _func;
        private readonly Action _continuation;

        public RemoteResult(Func<Task<T>> func, Action continuation,T notSetSentinal)
        {
            Debug.Assert(func != null);
            Debug.Assert(continuation != null);

            _func = func;
            _continuation = continuation;
            _result = notSetSentinal;
            _notSetSentinal = notSetSentinal;
        }

        public T Result => _result;

        public AggregateException Exceptions => _exceptions;

        public bool IsFaulted => _exceptions != null;

        public bool IsSet => !object.ReferenceEquals(_result, _notSetSentinal);

        public void Reset()
        {
            using (var scope = new Scope(this))
            {
                if (scope.Entered)
                {
                    Interlocked.Exchange<T>(ref _result, _notSetSentinal);
                    Interlocked.Exchange(ref _exceptions, null);
                }
            }
        }

        public async Task Execute()
        {
            using (var scope = new Scope(this))
            {
                if (scope.Entered && !IsSet && !IsFaulted)
                {
                    await Task.Run<T>(
                        async () => await _func())
                        .ContinueWith((antecedent) =>
                        {
                            try
                            {
                                Interlocked.Exchange<T>(ref _result, antecedent.Result);
                            }
                            catch (AggregateException e)
                            {
                                Debug.Assert(false, e.Message);
                                Interlocked.Exchange<T>(ref _result, _notSetSentinal);
                                Interlocked.Exchange(ref _exceptions, e);
                            }
                        })
                        .ContinueWith((antecedent) => _continuation());
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
