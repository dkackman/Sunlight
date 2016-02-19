﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sunlight
{
    sealed class RemoteResult<T> where T : class
    {
        private const int WAITING = 1;
        private const int NOT_WAITING = 0;

        private int _waiting = NOT_WAITING;

        private T _result;
        private readonly T _notSetSentinal;
        private AggregateException _exceptions;
        private readonly Func<Task<T>> _func;
        private readonly Action _continuation;

        public RemoteResult(Func<Task<T>> func, Action continuation, T notSetSentinal)
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
            if (Enter())
            {
                Interlocked.Exchange(ref _result, _notSetSentinal);
                Interlocked.Exchange(ref _exceptions, null);
                Exit();
                Task.Run(_continuation);
            }
        }

        public void Execute()
        {
            if (!IsSet && !IsFaulted && Enter())
            {
                Task.Run<T>(async () => await _func())
                    .ContinueWith((antecedent) =>
                    {
                        try
                        {
                            Interlocked.Exchange<T>(ref _result, antecedent.Result);
                        }
                        catch (AggregateException e)
                        {
                            Debug.Assert(false, e.Message);
                            Interlocked.Exchange(ref _result, _notSetSentinal);
                            Interlocked.Exchange(ref _exceptions, e);
                        }
                        finally
                        {
                            Exit();
                        }
                    })
                    .ContinueWith((antecedent) => _continuation());
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
