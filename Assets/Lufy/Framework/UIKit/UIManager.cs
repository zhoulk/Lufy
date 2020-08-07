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
        Dictionary<string, UIFormLogic> m_cachedForms = new Dictionary<string, UIFormLogic>();

        Transform rootTrans = null;

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>界面的序列编号。</returns>
        public void OpenUIForm(string uiFormAssetName)
        {
            OpenUIForm(uiFormAssetName, false, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <returns>界面的序列编号。</returns>
        public void OpenUIForm(string uiFormAssetName, bool pauseCoveredUIForm)
        {
            OpenUIForm(uiFormAssetName, pauseCoveredUIForm, null);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public void OpenUIForm(string uiFormAssetName, object userData)
        {
            OpenUIForm(uiFormAssetName, false, userData);
        }

        /// <summary>
        /// 打开界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>界面的序列编号。</returns>
        public void OpenUIForm(string uiFormAssetName, bool pauseCoveredUIForm, object userData)
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
                obj.transform.SetParent(rootTrans);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                uiFormInstanceObject = obj.GetComponent<UIFormLogic>();
                m_cachedForms.Add(uiFormAssetName, uiFormInstanceObject);

                uiFormInstanceObject.OnInit(userData);
            }
            uiFormInstanceObject.OnOpen(userData);
        }

        protected override void Awake()
        {
            base.Awake();

            rootTrans = transform.Find("Root");
        }

        internal override void Shutdown()
        {
            
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            
        }
    }
}
