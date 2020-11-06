﻿// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-11-06 16:22:47
// ========================================================

namespace LF
{
    /// <summary>
    /// 开始处理任务的状态。
    /// </summary>
    public enum StartTaskStatus
    {
        /// <summary>
        /// 可以立刻处理完成此任务。
        /// </summary>
        Done = 0,

        /// <summary>
        /// 可以继续处理此任务。
        /// </summary>
        CanResume,

        /// <summary>
        /// 不能继续处理此任务，需等待其它任务执行完成。
        /// </summary>
        HasToWait,

        /// <summary>
        /// 不能继续处理此任务，出现未知错误。
        /// </summary>
        UnknownError
    }
}
