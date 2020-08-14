/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/15
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

namespace LT
{
    /// <summary>
    /// 等待接口
    /// </summary>
    /// <typeparam name="TAwaiter">等待器</typeparam>
    public interface IAwaitable<out TAwaiter> where TAwaiter : IAwaiter
    {
        TAwaiter GetAwaiter();
    }

    /// <summary>
    /// 等待接口
    /// </summary>
    /// <typeparam name="TAwaiter">等待器</typeparam>
    /// <typeparam name="TResult">等待返回结果</typeparam>
    public interface IAwaitable<out TAwaiter, TResult> where TAwaiter : IAwaiter<TResult>
    {
        TAwaiter GetAwaiter();
    }
}