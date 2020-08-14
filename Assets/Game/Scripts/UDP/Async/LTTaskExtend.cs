/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/23
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Threading;

namespace LT
{
    /// <summary>
    /// LTTask扩展
    /// </summary>
	public partial struct LTTask
    {
        public static LTTask New() => new LTTask();
        public static LTTask<T> New<T>() => new LTTask<T>();

        public static LTTask FromException(Exception ex)
        {
            LTTaskCompletionSource tcs = new LTTaskCompletionSource();
            tcs.TrySetException(ex);
            return tcs.Task;
        }

        public static LTTask<T> FromException<T>(Exception ex)
        {
            var tcs = new LTTaskCompletionSource<T>();
            tcs.TrySetException(ex);
            return tcs.Task;
        }

        public static LTTask<T> FromResult<T>(T value)
        {
            return new LTTask<T>(value);
        }

        public static LTTask FromCanceled(CancellationToken token)
        {
            LTTaskCompletionSource tcs = new LTTaskCompletionSource();
            tcs.TrySetException(new OperationCanceledException(token));
            return tcs.Task;
        }

        public static LTTask<T> FromCanceled<T>(CancellationToken token)
        {
            var tcs = new LTTaskCompletionSource<T>();
            tcs.TrySetException(new OperationCanceledException(token));
            return tcs.Task;
        }
    }
}