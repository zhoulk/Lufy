// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-10 16:35:45
// ========================================================
using LF.UI;
using System.Collections.Generic;
using UnityEngine;

public class GameUILogic : UIFormLogic
{
    GameObject m_CurrentObj = null;

    public void Open(UIFormId formId, Dictionary<string, object> param = null)
    {
        GameEntry.UI.OpenUIForm(formId, param);
    }

    public void Close(UIFormId formId)
    {
        GameEntry.UI.CloseUIForm(formId);
    }

    public void Close()
    {
        GameEntry.UI.CloseUIForm(this);
    }

    public void OnBtnSelected(GameObject obj)
    {
        m_CurrentObj = obj;
        obj.transform.Find("Selected1P").gameObject.SetActive(true);
    }

    public void OnBtnUnSelected(GameObject obj)
    {
        obj.transform.Find("Selected1P").gameObject.SetActive(false);
    }

    protected internal override void OnReveal()
    {
        base.OnReveal();
        resumeUINavi();
    }

    protected internal override void OnCover()
    {
        base.OnCover();
        pauseUINavi();
    }

    protected internal override void OnResume()
    {
        base.OnResume();
        resumeUINavi();
    }

    protected internal override void OnPause()
    {
        base.OnPause();
        pauseUINavi();
    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        resumeUINavi();
    }

    protected internal override void OnClose(object userData)
    {
        base.OnClose(userData);
        pauseUINavi();
    }

    protected internal override void OnRecycle()
    {
        base.OnRecycle();
        //Debug.Log(this + "  recycle");
    }

    protected internal override void OnRealse(object uiFormAsset, object uiFormInstance)
    {
        base.OnRealse(uiFormAsset, uiFormInstance);

        Debug.Log(this + "  release");
        GameEntry.Res.UnloadAsset(uiFormAsset);
        GameObject.Destroy((GameObject)uiFormInstance);
    }

    void pauseUINavi()
    {
        if (m_CurrentObj != null)
        {
            OnBtnUnSelected(m_CurrentObj);
        }
        // 如果热点正好在当前页面，暂停导航
        if (m_CurrentObj == GameEntry.UIEvent.SelectedGameObject)
        {
            GameEntry.UIEvent.Pause();
        }
    }

    void resumeUINavi()
    {
        // 如果热点正好在当前页面，恢复导航
        if (m_CurrentObj == GameEntry.UIEvent.SelectedGameObject)
        {
            GameEntry.UIEvent.Resume();
        }
    }
}
