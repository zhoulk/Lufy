// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-27 11:09:19
// ========================================================
using LF.Pool;
using UnityEngine;

namespace LF.UI
{
    public class UIFormObject : ObjectBase
    {
        private object m_UIFormAsset;

        public static UIFormObject Create(string name, object uiFormInstance, object uiFormAsset)
        {
            if (uiFormAsset == null)
            {
                throw new LufyException("UI form asset is invalid.");
            }

            UIFormObject uiInstanceObject = ReferencePool.Acquire<UIFormObject>();
            uiInstanceObject.Initialize(name, uiFormInstance);
            uiInstanceObject.m_UIFormAsset = uiFormAsset;
            return uiInstanceObject;
        }

        protected internal override void Release(bool isShutdown)
        {
            //Debug.Log("uiform release " + Target);
            if (Target != null)
            {
                UIFormLogic t = Target as UIFormLogic;
                t.OnRealse(m_UIFormAsset, t.gameObject);
            }
            ReferencePool.Release(this);
        }
    }
}
