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
        GameEntry.UI.OpenUIForm(formId.ToString().ToLower(), param);
    }

    public void Close(UIFormId formId)
    {
        GameEntry.UI.CloseUIForm(formId.ToString().ToLower());
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

    protected override void OnReveal()
    {
        base.OnReveal();
        resumeUINavi();
    }

    protected override void OnCover()
    {
        base.OnCover();
        pauseUINavi();
    }

    protected override void OnResume()
    {
        base.OnResume();
        resumeUINavi();
    }

    protected override void OnPause()
    {
        base.OnPause();
        pauseUINavi();
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);
        resumeUINavi();
    }

    protected override void OnClose(object userData)
    {
        base.OnClose(userData);
        pauseUINavi();
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
