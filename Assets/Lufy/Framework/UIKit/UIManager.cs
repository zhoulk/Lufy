// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 16:31:35
// ========================================================

using LF.Pool;
using LF.Res;
using System.Collections.Generic;
using UnityEngine;

namespace LF.UI
{
    /// <summary>
    /// 页面管理
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Lufy/UI")]
    public sealed class UIManager : LufyManager
    {
        private readonly Dictionary<string, UIFormLogic> m_cachedForms = new Dictionary<string, UIFormLogic>();
        private readonly LinkedList<UIFormLogic> m_formList = new LinkedList<UIFormLogic>();
        private readonly Queue<UIFormLogic> m_RecycleQueue = new Queue<UIFormLogic>();

        private List<string> removeKeyList = new List<string>();

        [SerializeField]
        [HideInInspector]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        [HideInInspector]
        private int m_InstanceCapacity = 16;
        [SerializeField]
        [HideInInspector]
        private float m_InstanceExpireTime = 60f;

        private IResManager m_ResManager = null;

        private ObjectPoolManager m_PoolManager = null;
        private IObjectPool<UIFormObject> m_InstancePool = null;

        private LoadAssetCallbacks m_LoadAssetCallbacks;

        public int Count
        {
            get
            {
                return m_cachedForms.Count;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池的容量。
        /// </summary>
        public int InstanceCapacity
        {
            get
            {
                return m_InstanceCapacity;
            }
            set
            {
                m_InstanceCapacity = value;
            }
        }

        /// <summary>
        /// 获取或设置界面实例对象池对象过期秒数。
        /// </summary>
        public float InstanceExpireTime
        {
            get
            {
                return m_InstanceExpireTime;
            }
            set
            {
                m_InstanceExpireTime = value;
            }
        }

        public void SetResManager(IResManager resManager)
        {
            m_ResManager = resManager;
        }

        public void SetObjectPoolManager(ObjectPoolManager objectPoolManager)
        {
            m_PoolManager = objectPoolManager;
            m_InstancePool = m_PoolManager.CreateSingleSpawnObjectPool<UIFormObject>("UIDefault", InstanceCapacity, InstanceExpireTime);
        }

        public UIManager()
        {
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccess, LoadAssetFail);
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>要获取的界面。</returns>
        public UIFormLogic GetUIForm(string uiFormAssetName)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new LufyException("UI form asset name is invalid.");
            }

            UIFormLogic uiFormLogic = null;
            m_cachedForms.TryGetValue(uiFormAssetName, out uiFormLogic);

            return uiFormLogic;
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>界面的序列编号。</returns>
        public void OpenUIForm(string uiFormAssetName)
        {
            OpenUIForm(uiFormAssetName, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public void OpenUIForm(string uiFormAssetName, object userData)
        {
            if (string.IsNullOrEmpty(uiFormAssetName))
            {
                throw new LufyException("UI form asset name is invalid.");
            }

            if (m_ResManager == null)
            {
                throw new LufyException("ResManager is invalid.");
            }

            if (m_PoolManager == null)
            {
                throw new LufyException("ObjectPoolManager is invalid.");
            }

            UIFormLogic uiFormInstanceObject = null;
            m_cachedForms.TryGetValue(uiFormAssetName, out uiFormInstanceObject);
            if (uiFormInstanceObject == null)
            {
                UIFormObject obj = m_InstancePool.Spawn(uiFormAssetName);
                uiFormInstanceObject = obj?.Target as UIFormLogic;
                if(uiFormInstanceObject == null)
                {
                    m_ResManager.LoadAsset(uiFormAssetName, m_LoadAssetCallbacks);
                }
                else
                {
                    m_cachedForms.Add(uiFormAssetName, uiFormInstanceObject);

                    uiFormInstanceObject.OnOpen(userData);
                    m_formList.AddFirst(uiFormInstanceObject);
                    Refresh();
                }
            }
            else
            {
                m_formList.Remove(uiFormInstanceObject);
                uiFormInstanceObject.OnOpen(userData);
                m_formList.AddFirst(uiFormInstanceObject);
                Refresh();
            }
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiFormAssetName">要关闭界面的名字</param>
        public void CloseUIForm(string uiFormAssetName)
        {
            CloseUIForm(uiFormAssetName, null);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiFormAssetName">要关闭界面的名字</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(string uiFormAssetName, object userData)
        {
            UIFormLogic uiForm = GetUIForm(uiFormAssetName);
            if (uiForm == null)
            {
                throw new LufyException(Utility.Text.Format("Can not find UI form '{0}'.", uiFormAssetName));
            }

            CloseUIForm(uiForm, userData);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        public void CloseUIForm(UIFormLogic uiForm)
        {
            CloseUIForm(uiForm, null);
        }

        /// <summary>
        /// 关闭界面。
        /// </summary>
        /// <param name="uiForm">要关闭的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void CloseUIForm(UIFormLogic uiForm, object userData)
        {
            if (uiForm == null)
            {
                throw new LufyException("UI form is invalid.");
            }

            uiForm.OnClose(userData);

            m_formList.Remove(uiForm);

            removeKeyList.Clear();
            foreach (var kv in m_cachedForms)
            {
                if(kv.Value == uiForm)
                {
                    removeKeyList.Add(kv.Key);
                }
            }
            foreach(var key in removeKeyList)
            {
                m_cachedForms.Remove(key);
            }
            m_RecycleQueue.Enqueue(uiForm);

            Refresh();
        }

        /// <summary>
        /// 重新计算UI层次
        /// </summary>
        public void Refresh()
        {
            LinkedListNode<UIFormLogic> current = m_formList.First;
            bool pause = false;
            bool cover = false;
            int depth = m_formList.Count;
            while (current != null && current.Value != null)
            {
                LinkedListNode<UIFormLogic> next = current.Next;
                current.Value.OnDepthChanged(depth--);
                if (current.Value == null)
                {
                    return;
                }

                if (pause)
                {
                    if (!current.Value.Covered)
                    {
                        current.Value.Covered = true;
                        current.Value.OnCover();
                        if (current.Value == null)
                        {
                            return;
                        }
                    }

                    if (!current.Value.Paused)
                    {
                        current.Value.Paused = true;
                        current.Value.OnPause();
                        if (current.Value == null)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (current.Value.Paused)
                    {
                        current.Value.Paused = false;
                        current.Value.OnResume();
                        if (current.Value == null)
                        {
                            return;
                        }
                    }

                    if (current.Value.PauseCoveredUIForm)
                    {
                        pause = true;
                    }

                    if (cover)
                    {
                        if (!current.Value.Covered)
                        {
                            current.Value.Covered = true;
                            current.Value.OnCover();
                            if (current.Value == null)
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (current.Value.Covered)
                        {
                            current.Value.Covered = false;
                            current.Value.OnReveal();
                            if (current.Value == null)
                            {
                                return;
                            }
                        }

                        cover = true;
                    }
                }

                current = next;
            }
        }

        void LoadAssetSuccess(string assetName, object asset, float duration, object userData){
            GameObject obj = GameObject.Instantiate(asset as GameObject);
            obj.name = obj.name.Substring(0, obj.name.Length - 7);
            obj.transform.SetParent(m_InstanceRoot);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            UIFormLogic uiFormInstanceObject = obj.GetComponent<UIFormLogic>();
            m_cachedForms.Add(assetName, uiFormInstanceObject);
            m_InstancePool.Register(UIFormObject.Create(assetName, uiFormInstanceObject), true);

            uiFormInstanceObject.OnInit(userData);

            uiFormInstanceObject.OnOpen(userData);
            m_formList.AddFirst(uiFormInstanceObject);
            Refresh();
        }

        void LoadAssetFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {

        }

        protected override void Awake()
        {
            base.Awake();

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = (new GameObject("UI Form Instances")).transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            m_InstanceRoot.gameObject.layer = LayerMask.NameToLayer("UI");
        }

        internal override void Shutdown()
        {
            m_cachedForms.Clear();
            m_formList.Clear();
            removeKeyList.Clear();
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            while (m_RecycleQueue.Count > 0)
            {
                UIFormLogic obj = m_RecycleQueue.Dequeue();
                obj.OnRecycle();
                m_InstancePool.Unspawn(obj);
            }

            foreach (var uiForm in m_formList)
            {
                uiForm.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }
    }
}
