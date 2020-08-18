// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 11:52:28
// ========================================================

namespace LF.Event
{
    public abstract class GameEventArgs : IReference
    {
        /// <summary>
        /// 获取类型编号。
        /// </summary>
        public abstract int Id
        {
            get;
        }

        public virtual void Clear()
        {
            
        }
    }
}

