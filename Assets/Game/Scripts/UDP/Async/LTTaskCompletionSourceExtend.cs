/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/23
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/


namespace LT
{
    /// <summary>
    /// LTTaskCompletionSource扩展
    /// </summary>
    public partial class LTTaskCompletionSource
    {
        public static LTTaskCompletionSource New() => new LTTaskCompletionSource();
        public static LTTaskCompletionSource<T> New<T>() => new LTTaskCompletionSource<T>();
    }
}