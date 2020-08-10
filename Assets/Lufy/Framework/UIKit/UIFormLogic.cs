// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 16:59:12
// ========================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LF.UI
{
    /// <summary>
    /// 页面逻辑基类
    /// </summary>
    public abstract class UIFormLogic : MonoBehaviour
    {
        private bool m_Visible = false;
        private bool m_Paused = false;
        private bool m_Covered = false;

        public Vector2 scalerSize = new Vector2(1280, 720);
        public const int DepthFactor = 100;

        private CanvasGroup m_CanvasGroup = null;
        private CanvasScaler m_CanvasScaler = null;
        private Canvas m_CachedCanvas = null;
        private List<Canvas> m_CachedCanvasContainer = new List<Canvas>();

        public int OriginalDepth
        {
            get;
            private set;
        }

        public int Depth
        {
            get
            {
                return m_CachedCanvas.sortingOrder;
            }
        }

        /// <summary>
        /// 暂停覆盖其他页面
        /// </summary>
        public bool PauseCoveredUIForm = true;

        /// <summary>
        /// 获取或设置界面是否可见。
        /// </summary>
        public bool Visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                if (m_Visible == value)
                {
                    return;
                }

                m_Visible = value;
                InternalSetVisible(value);
            }
        }

        public bool Paused
        {
            get
            {
                return m_Paused;
            }
            set
            {
                m_Paused = value;
            }
        }

        public bool Covered
        {
            get
            {
                return m_Covered;
            }
            set
            {
                m_Covered = value;
            }
        }

        /// <summary>
        /// 界面初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnInit(object userData)
        {
            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            m_CachedCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            m_CachedCanvas.overrideSorting = true;
            OriginalDepth = m_CachedCanvas.sortingOrder;

            //RectTransform transform = GetComponent<RectTransform>();
            //transform.anchorMin = Vector2.zero;
            //transform.anchorMax = Vector2.one;
            //transform.anchoredPosition = Vector2.zero;
            //transform.sizeDelta = Vector2.zero;

            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.alpha = 0f;

            m_CanvasScaler = gameObject.GetOrAddComponent<CanvasScaler>();
            m_CanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            m_CanvasScaler.referenceResolution = scalerSize;

            gameObject.GetOrAddComponent<GraphicRaycaster>();
        }

        /// <summary>
        /// 界面回收。
        /// </summary>
        protected internal virtual void OnRecycle()
        {
        }

        /// <summary>
        /// 界面打开。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnOpen(object userData)
        {
            Visible = true;
        }

        /// <summary>
        /// 界面关闭。
        /// </summary>
        /// <param name="isShutdown">是否是关闭界面管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnClose(object userData)
        {
            Visible = false;
        }

        /// <summary>
        /// 界面暂停。
        /// </summary>
        protected internal virtual void OnPause()
        {
            Visible = false;
        }

        /// <summary>
        /// 界面暂停恢复。
        /// </summary>
        protected internal virtual void OnResume()
        {
            Visible = true;
        }

        /// <summary>
        /// 界面遮挡。
        /// </summary>
        protected internal virtual void OnCover()
        {
        }

        /// <summary>
        /// 界面遮挡恢复。
        /// </summary>
        protected internal virtual void OnReveal()
        {
        }

        /// <summary>
        /// 界面激活。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected internal virtual void OnRefocus(object userData)
        {

        }

        /// <summary>
        /// 界面轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected internal virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 设置界面的可见性。
        /// </summary>
        /// <param name="visible">界面的可见性。</param>
        protected virtual void InternalSetVisible(bool visible)
        {
            gameObject.SetActive(visible);
            m_CanvasGroup.alpha = visible ? 1 : 0;
            m_CanvasGroup.interactable = visible;
        }

        internal void OnDepthChanged(int uiDepth)
        {
            int oldDepth = Depth;
            int deltaDepth = DepthFactor * uiDepth - oldDepth + OriginalDepth;
            GetComponentsInChildren(true, m_CachedCanvasContainer);
            for (int i = 0; i < m_CachedCanvasContainer.Count; i++)
            {
                m_CachedCanvasContainer[i].sortingOrder += deltaDepth;
            }

            m_CachedCanvasContainer.Clear();
        }
    }
}

