// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 11:59:33
// ========================================================

using UnityEngine;

namespace LF
{
    /// <summary>
    /// 游戏框架管理抽象类。
    /// </summary>
    public abstract class LufyManager : MonoBehaviour
    {
        /// <summary>
        /// 游戏框架管理初始化。
        /// </summary>
        protected virtual void Awake()
        {
            Lufy.RegisterManager(this);
        }

        /// <summary>
        /// 游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal abstract void OnUpdate(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 关闭并清理游戏框架模块。
        /// </summary>
        internal abstract void Shutdown();
    }
}

