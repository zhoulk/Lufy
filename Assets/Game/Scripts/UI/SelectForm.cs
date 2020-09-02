// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 15:19:04
// ========================================================
using UnityEngine;

public class SelectForm : GameUILogic
{
    public GameObject P1Btn;
    public GameObject P2Btn;

    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);

        P1Btn.AddNaviRight(P2Btn).AddSelected(OnBtnSelected).AddUnSelected(OnBtnUnSelected);
        P2Btn.AddNaviLeft(P1Btn).AddSelected(OnBtnSelected).AddUnSelected(OnBtnUnSelected);

        GameEntry.UIEvent.AddOnClickHandler(P1Btn, (obj) =>
        {
            Open(UIFormId.detail);
        });

        GameEntry.UIEvent.AddOnClickHandler(P2Btn, (obj) =>
        {
            Close();
        });
    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        //Log.Debug("select open");

        P1Btn.SetAsDefaultNavi();
    }

    protected internal override void OnClose(object userData)
    {
        base.OnClose(userData);

        //Log.Debug("select close");
    }

    protected internal override void OnResume()
    {
        base.OnResume();

        //Log.Debug("select resume");
    }

    protected internal override void OnPause()
    {
        base.OnPause();

        //Log.Debug("select pause");
    }

    protected internal override void OnReveal()
    {
        base.OnReveal();

        //Log.Debug("select reveal");
    }

    protected internal override void OnRecycle()
    {
        base.OnRecycle();

        //Log.Debug("select recycle");
    }

    protected internal override void OnCover()
    {
        base.OnCover();

        //Log.Debug("select cover");
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        //Log.Debug("select update " + elapseSeconds);
    }
}
