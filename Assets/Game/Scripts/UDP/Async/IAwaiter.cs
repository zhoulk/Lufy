/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/12
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System.Runtime.CompilerServices;

namespace LT
{
    /// <summary>
    /// 等待器状态
    /// </summary>
    public enum AwaiterStatus
    {
        /// <summary>The operation has not yet completed.</summary>
        Pending = 0,

        /// <summary>The operation completed successfully.</summary>
        Succeeded = 1,

        /// <summary>The operation completed with an error.</summary>
        Faulted = 2,

        /// <summary>The operation completed due to cancellation.</summary>
        Canceled = 3
    }

    /// <summary>
    /// 等待器
    /// </summary>
    public interface IAwaiter : ICriticalNotifyCompletion
    {
        AwaiterStatus Status { get; }
        bool IsCompleted { get; }
        void GetResult();
    }

    /// <summary>
    /// 等待器
    /// </summary>
    /// <typeparam name="TResult">等待返回结果</typeparam>
    public interface IAwaiter<TResult> : ICriticalNotifyCompletion
    {
        AwaiterStatus Status { get; }
        bool IsCompleted { get; }
        TResult GetResult();
    }

    /// <summary>
    /// 等待器扩展
    /// </summary>
    public static class AwaiterStatusExtensions
    {
        /// <summary>!= Pending.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCompleted(this AwaiterStatus status)
        {
            return status != AwaiterStatus.Pending;
        }

        /// <summary>== Succeeded.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCompletedSuccessfully(this AwaiterStatus status)
        {
            return status == AwaiterStatus.Succeeded;
        }

        /// <summary>== Canceled.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCanceled(this AwaiterStatus status)
        {
            return status == AwaiterStatus.Canceled;
        }

        /// <summary>== Faulted.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFaulted(this AwaiterStatus status)
        {
            return status == AwaiterStatus.Faulted;
        }
    }
}