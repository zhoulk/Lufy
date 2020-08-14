/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/23
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Diagnostics;

namespace LT
{
    /// <summary>
    /// LTTask等待器
    /// </summary>
    public struct LTTaskAwaiter : IAwaiter
    {
        private readonly LTTask task;

        [DebuggerHidden]
        public LTTaskAwaiter(LTTask task)
        {
            this.task = task;
        }

        [DebuggerHidden]
        public bool IsCompleted => task.IsCompleted;

        [DebuggerHidden]
        public AwaiterStatus Status => task.Status;

        [DebuggerHidden]
        public void GetResult()
        {
            task.GetResult();
        }

        [DebuggerHidden]
        public void OnCompleted(Action continuation)
        {
            if (task.Awaiter != null)
            {
                task.Awaiter.OnCompleted(continuation);
            }
            else
            {
                continuation();
            }
        }

        [DebuggerHidden]
        public void UnsafeOnCompleted(Action continuation)
        {
            if (task.Awaiter != null)
            {
                task.Awaiter.UnsafeOnCompleted(continuation);
            }
            else
            {
                continuation();
            }
        }
    }

    /// <summary>
    /// LTTask等待器
    /// </summary>
    public struct LTTaskAwaiter<TResult> : IAwaiter<TResult>
    {
        private readonly LTTask<TResult> task;

        [DebuggerHidden]
        public LTTaskAwaiter(LTTask<TResult> task)
        {
            this.task = task;
        }

        [DebuggerHidden]
        public bool IsCompleted => task.IsCompleted;

        [DebuggerHidden]
        public AwaiterStatus Status => task.Status;

        [DebuggerHidden]
        public TResult GetResult()
        {
            return task.GetResult();
        }

        [DebuggerHidden]
        public void OnCompleted(Action continuation)
        {
            if (task.Awaiter != null)
            {
                task.Awaiter.OnCompleted(continuation);
            }
            else
            {
                continuation();
            }
        }

        [DebuggerHidden]
        public void UnsafeOnCompleted(Action continuation)
        {
            if (task.Awaiter != null)
            {
                task.Awaiter.UnsafeOnCompleted(continuation);
            }
            else
            {
                continuation();
            }
        }
    }
}