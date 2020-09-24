// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-24 09:13:58
// ========================================================

using LF;

namespace LF.Pattern.Cmd
{
    /// <summary>
    /// 空命令
    /// </summary>
    public class EmptyCmd : ICommand, IReference
    {
        public void Execute(object userData)
        {

        }

        public void Undo(object preUserData, object userData)
        {

        }

        public static EmptyCmd Create()
        {
            EmptyCmd emptyCmd = ReferencePool.Acquire<EmptyCmd>();
            return emptyCmd;
        }

        public void Clear()
        {

        }
    }
}

