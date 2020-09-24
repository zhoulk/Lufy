// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-23 17:48:34
// ========================================================

namespace LF.Pattern.Cmd
{
    public interface ICommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="userData">执行时候的参数</param>
        void Execute(object userData);

        /// <summary>
        /// 重做
        /// </summary>
        /// <param name="preUserData">执行时候的参数</param>
        /// <param name="userData">重做时候的参数</param>
        void Undo(object preUserData, object userData);
    }
}

