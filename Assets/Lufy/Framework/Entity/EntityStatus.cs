// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-11 10:42:44
// ========================================================
namespace LF.Entity
{
    /// <summary>
    /// 实体状态。
    /// </summary>
    public enum EntityStatus
    {
        Unknown = 0,
        WillInit,
        Inited,
        WillShow,
        Showed,
        WillHide,
        Hidden,
        WillRecycle,
        Recycled
    }
}
