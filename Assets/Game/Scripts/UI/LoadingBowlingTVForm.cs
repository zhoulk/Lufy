// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 15:19:04
// ========================================================
using LF;
using UnityEngine;

public class LoadingBowlingTVForm : GameUILogic
{
    public GameObject basketBallBtn;

    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);

        basketBallBtn.AddUnSelected(OnBtnUnSelected).AddSelected(OnBtnSelected);

        GameEntry.UIEvent.AddOnClickHandler(basketBallBtn, (obj) =>
        {
            GameEntry.Event.Fire(this, EnterGameEventArgs.Create(SceneId.BowlingPhoneTV));
        });
    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        basketBallBtn.SetAsDefaultNavi();
    }

    protected internal override void OnClose(object userData)
    {
        base.OnClose(userData);

        //Log.Debug("loading close");
    }

    protected internal override void OnResume()
    {
        base.OnResume();

        //Log.Debug("loading resume");
    }

    protected internal override void OnPause()
    {
        base.OnPause();

        //Log.Debug("loading pause");
    }

    protected internal override void OnReveal()
    {
        base.OnReveal();

        basketBallBtn.SetAsDefaultNavi();

        Log.Debug("loading reveal");
    }

    protected internal override void OnRecycle()
    {
        base.OnRecycle();

        //Log.Debug("loading recycle");
    }

    protected internal override void OnCover()
    {
        base.OnCover();

        //Log.Debug("loading cover");
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        //Log.Debug("loading update " + elapseSeconds);
    }
}
