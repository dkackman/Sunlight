using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sunlight
{
    sealed class NonblockingCriticalSection
    {
        private const int WAITING = 1;
        private const int NOT_WAITING = 0;

        private int _waiting = NOT_WAITING;

        public async Task Execute(Func<Task> func, Action continuation = null)
        {
            if (Enter())
            {
                try
                {
                    await func();
                }
                finally
                {
                    Exit();
                }

                if (continuation != null)
                {
                    continuation();
                }
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
