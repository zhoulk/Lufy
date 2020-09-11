// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-11 11:29:27
// ========================================================

using UnityEngine;

namespace LF.Entity
{
    public abstract class EntityLogic : MonoBehaviour
    {
        private int m_OriginalLayer = 0;
        private Transform m_OriginalTransform = null;

        /// <summary>
        /// 获取已缓存的 Transform。
        /// </summary>
        public Transform CachedTransform
        {
            get;
            private set;
        }

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnInit(object userData)
        {
            if (CachedTransform == null)
            {
                CachedTransform = transform;
            }

            m_OriginalLayer = gameObject.layer;
            m_OriginalTransform = CachedTransform.parent;
        }
    }
}

