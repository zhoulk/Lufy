// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 14:57:11
// ========================================================
using UnityEngine;

namespace LF
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Lufy/Base")]
    public sealed class BaseManager : LufyManager
    {
        [SerializeField]
        [HideInInspector]
        private int m_FrameRate = 30;

        /// <summary>
        /// 获取或设置游戏帧率。
        /// </summary>
        public int FrameRate
        {
            get
            {
                return m_FrameRate;
            }
            set
            {
                Application.targetFrameRate = m_FrameRate = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            Application.targetFrameRate = m_FrameRate;
        }

        internal override void Shutdown()
        {
            throw new System.NotImplementedException();
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            
        }

        private void Update()
        {
            Lufy.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }
    }
}

