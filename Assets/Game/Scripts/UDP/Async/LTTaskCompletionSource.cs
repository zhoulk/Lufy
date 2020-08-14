/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/23
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace LT
{
    /// <summary>
    /// 任务完成源，用于等待由用户决定返回结果时机的地方
    /// </summary>
    public partial class LTTaskCompletionSource : IAwaiter
    {
        private AwaiterStatus state;
        private Action continuation;
        private ExceptionDispatchInfo exception;

        #region 显式实现接口
        AwaiterStatus IAwaiter.Status => state;
        bool IAwaiter.IsCompleted => state != AwaiterStatus.Pending;

        void IAwaiter.GetResult()
        {
            switch (this.state)
            {
                case AwaiterStatus.Succeeded:
                    return;
                case AwaiterStatus.Faulted:
                    this.exception?.Throw();
                    this.exception = null;
                    return;
                case AwaiterStatus.Canceled:
                    // guranteed operation canceled exception.
                    this.exception?.Throw();
                    this.exception = null;
                    throw new OperationCanceledException();
                default:
                    throw new NotSupportedException("LTTask does not allow call GetResult directly when task not completed. Please use 'await'.");
            }
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            ((ICriticalNotifyCompletion)this).UnsafeOnCompleted(continuation);
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
        {
            this.continuation = continuation;
            if (state != AwaiterStatus.Pending)
            {
                TryInvokeContinuation();
            }
        }
        #endregion

        public LTTask Task => new LTTask(this);

        private void TryInvokeContinuation()
        {
            this.continuation?.Invoke();
            this.continuation = null;
        }

        public void SetResult()
        {
            if (this.TrySetResult())
            {
                return;
            }

            throw new InvalidOperationException("Task transition to final already completed");
        }

        public void SetException(Exception e)
        {
            if (this.TrySetException(e))
            {
                return;
            }

            throw new InvalidOperationException("Task transition to final already completed");
        }

        public bool TrySetResult()
        {
            if (this.state != AwaiterStatus.Pending)
            {
                return false;
            }

            this.state = AwaiterStatus.Succeeded;
            this.TryInvokeContinuation();
            return true;

        }

        public bool TrySetException(Exception e)
        {
            if (this.state != AwaiterStatus.Pending)
            {
                return false;
            }

            this.state = AwaiterStatus.Faulted;
            this.exception = ExceptionDispatchInfo.Capture(e);
            this.TryInvokeContinuation();
            return true;

        }

        public bool TrySetCanceled()
        {
            if (this.state != AwaiterStatus.Pending)
            {
                return false;
            }

            this.state = AwaiterStatus.Canceled;
            this.TryInvokeContinuation();
            return true;
        }

        public bool TrySetCanceled(OperationCanceledException e)
        {
            if (this.state != AwaiterStatus.Pending)
            {
                return false;
            }

            this.state = AwaiterStatus.Canceled;
            this.exception = ExceptionDispatchInfo.Capture(e);
            this.TryInvokeContinuation();
            return true;
        }
    }

    /// <summary>
    /// 任务完成源，用于等待由用户决定返回结果时机的地方
    /// </summary>
    public sealed class LTTaskCompletionSource<TResult> : IAwaiter<TResult>
    {
        private AwaiterStatus state;
        private Action continuation;
        private ExceptionDispatchInfo exception;
        private TResult result;

        AwaiterStatus IAwaiter<TResult>.Status => state;
        bool IAwaiter<TResult>.IsCompleted => state != AwaiterStatus.Pending;

        TResult IAwaiter<TResult>.GetResult()
        {
            switch (this.state)
            {
                case AwaiterStatus.Succeeded:
                    return this.result;
                case AwaiterStatus.Faulted:
                    this.exception?.Throw();
                    this.exception = null;
                    return default;
                case AwaiterStatus.Canceled:
                    // guranteed operation canceled exception.
                    this.exception?.Throw();
                    this.exception = null;
                    throw new OperationCanceledException();
                default:
                    throw new NotSupportedException("LTTask does not allow call GetResult directly when task not completed. Please use 'await'.");
            }
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            ((ICriticalNotifyCompletion)this).UnsafeOnCompleted(continuation);
        }

        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
        {
            this.continuation = continuation;
            if (state != AwaiterStatus.Pending)
            {
                TryInvokeContinuation();
            }
        }

        public LTTask<TResult> Task => new LTTask<TResult>(this);

        private void TryInvokeContinuation()
        {
            this.continuation?.Invoke();
            this.continuation = null;
        }

        public void SetResult(TResult result)
        {
            if (this.TrySetResult(result))
            {
                return;
            }

            throw new InvalidOperationException("Task transition to final already completed");
        }

        public void SetException(Exception e)
        {
            if (this.TrySetException(e))
            {
                return;
            }

            throw new InvalidOperationException("Task transition to final already completed");
        }

        public bool TrySetResult(TResult result)
        {
            if (this.state != AwaiterStatus.Pending)
            {
                return false;
            }

            this.result = result;
            this.state = AwaiterStatus.Succeeded;
            this.TryInvokeContinuation();
            return true;
        }

        public bool TrySetException(Exception e)
        {
            if (this.state != AwaiterStatus.Pending)
            {
                return false;
            }

            this.state = AwaiterStatus.Faulted;
            this.exception = ExceptionDispatchInfo.Capture(e);
            this.TryInvokeContinuation();
            return true;
        }

        public bool TrySetCanceled()
        {
            if (this.state != AwaiterStatus.Pending)
            {
                return false;
            }

            this.state = AwaiterStatus.Canceled;
            this.TryInvokeContinuation();
            return true;
        }

        public bool TrySetCanceled(OperationCanceledException e)
        {
            if (this.state != AwaiterStatus.Pending)
            {
                return false;
            }

            this.state = AwaiterStatus.Canceled;
            this.exception = ExceptionDispatchInfo.Capture(e);
            this.TryInvokeContinuation();
            return true;
        }
    }
}