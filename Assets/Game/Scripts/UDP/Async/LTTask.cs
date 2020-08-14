/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/15
 * 模块描述：
 * 1.轻量级Task，目的在于扩展原生Task的使用灵活性。
 * 2.由于原生Task解决耗时任务时的回调时机使用者无法自由掌控。
 * ------------------------------------------------------------------------------*/

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LT
{
    /// <summary>
    /// 轻量级Task
    /// </summary>
    [AsyncMethodBuilder(typeof(AsyncLTTaskMethodBuilder))]
    public partial struct LTTask : IAwaitable<LTTaskAwaiter>, IEquatable<LTTask>
    {
        private IAwaiter awaiter;

        [DebuggerHidden]
        public LTTask(IAwaiter awaiter)
        {
            this.awaiter = awaiter;
        }

        [DebuggerHidden]
        public LTTaskAwaiter GetAwaiter()
        {
            return new LTTaskAwaiter(this);
        }

        [DebuggerHidden]
        public bool IsCompleted => awaiter?.IsCompleted ?? true;

        [DebuggerHidden]
        public AwaiterStatus Status => awaiter?.Status ?? AwaiterStatus.Succeeded;

        [DebuggerHidden]
        public IAwaiter Awaiter { get => awaiter; }

        [DebuggerHidden]
        public void GetResult()
        {
            awaiter?.GetResult();
        }

        [DebuggerHidden]
        public bool Equals(LTTask other)
        {
            if (this.awaiter == null && other.awaiter == null)
                return true;

            if (this.awaiter != null && other.awaiter != null)
                return this.awaiter == other.awaiter;

            return false;
        }
    }

    /// <summary>
    /// 轻量级Task
    /// </summary>
    [AsyncMethodBuilder(typeof(AsyncLTTaskMethodBuilder<>))]
    public struct LTTask<TResult> : IAwaitable<LTTaskAwaiter<TResult>, TResult>, IEquatable<LTTask<TResult>>
    {
        private TResult result;
        private IAwaiter<TResult> awaiter;

        [DebuggerHidden]
        public LTTask(TResult result)
        {
            this.result = result;
            this.awaiter = null;
        }

        [DebuggerHidden]
        public LTTask(IAwaiter<TResult> awaiter)
        {
            this.result = default;
            this.awaiter = awaiter;
        }

        [DebuggerHidden]
        public LTTaskAwaiter<TResult> GetAwaiter()
        {
            return new LTTaskAwaiter<TResult>(this);
        }

        [DebuggerHidden]
        public bool IsCompleted => awaiter?.IsCompleted ?? true;

        [DebuggerHidden]
        public AwaiterStatus Status => awaiter?.Status ?? AwaiterStatus.Succeeded;

        [DebuggerHidden]
        public IAwaiter<TResult> Awaiter { get => awaiter; }

        [DebuggerHidden]
        public TResult GetResult()
        {
            if (awaiter == null)
            {
                return result;
            }

            return this.awaiter.GetResult();
        }

        [DebuggerHidden]
        public bool Equals(LTTask<TResult> other)
        {
            if (this.awaiter == null && other.awaiter == null)
                return true;

            if (this.awaiter != null && other.awaiter != null)
                return this.awaiter == other.awaiter;

            return false;
        }
    }
}