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
        public static UIFormObject Create(string name, object uiFormInstance)
        {
            UIFormObject uiInstanceObject = ReferencePool.Acquire<UIFormObject>();
            uiInstanceObject.Initialize(name, uiFormInstance);
            ((UIFormLogic)uiFormInstance).Handler = uiInstanceObject;
            return uiInstanceObject;
        }

        protected internal override void Release(bool isShutdown)
        {
            //Debug.Log("uiform release " + Target);
            if (Target != null)
            {
                UIFormLogic t = Target as UIFormLogic;
                GameObject.Destroy(t.gameObject);
            }
            ReferencePool.Release(this);
        }
    }
}
