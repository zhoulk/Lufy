// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 16:31:35
// ========================================================

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

        Transform rootTrans = null;

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

            UIFormLogic uiFormInstanceObject = null;
            m_cachedForms.TryGetValue(uiFormAssetName, out uiFormInstanceObject);
            if (uiFormInstanceObject == null)
            {
                GameObject prefab = Resources.Load<GameObject>(uiFormAssetName);
                GameObject obj = GameObject.Instantiate(prefab);
                obj.name = obj.name.Substring(0, obj.name.Length - 7);
                obj.transform.SetParent(rootTrans);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                uiFormInstanceObject = obj.GetComponent<UIFormLogic>();
                m_cachedForms.Add(uiFormAssetName, uiFormInstanceObject);

                uiFormInstanceObject.OnInit(userData);
            }
            else
            {
                m_formList.Remove(uiFormInstanceObject);
            }
            uiFormInstanceObject.OnOpen(userData);

            m_formList.AddFirst(uiFormInstanceObject);

            Refresh();
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

        protected override void Awake()
        {
            base.Awake();

            rootTrans = transform.Find("Root");
        }

        internal override void Shutdown()
        {
            m_cachedForms.Clear();
            m_formList.Clear();
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }
    }
}
