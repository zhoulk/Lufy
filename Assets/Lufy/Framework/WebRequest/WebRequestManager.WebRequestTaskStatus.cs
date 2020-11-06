// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-11-06 14:54:34
// ========================================================

namespace LF.WebRequest
{
    public partial class WebRequestManager : LufyManager
    {
        private enum WebRequestTaskStatus
        {
            /// <summary>
            /// 准备请求。
            /// </summary>
            Todo = 0,

            /// <summary>
            /// 请求中。
            /// </summary>
            Doing,

            /// <summary>
            /// 请求完成。
            /// </summary>
            Done,

            /// <summary>
            /// 请求错误。
            /// </summary>
            Error
        }
    }
}

