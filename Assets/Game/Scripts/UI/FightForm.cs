// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 15:19:04
// ========================================================
using LF;
using LF.UI;
using UnityEngine.UI;

public class FightForm : GameUILogic
{
    public Button backBtn;

    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);

        backBtn.gameObject.SetActive(false);
        backBtn.onClick.AddListener(() =>
        {
            Close();
            GameEntry.Event.Fire(this, ExitGameEventArgs.Create(1));
        });
    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        //Log.Debug("detail open");
    }

    protected internal override void OnClose(object userData)
    {
        base.OnClose(userData);

        //Log.Debug("detail close");
    }

    protected internal override void OnResume()
    {
        base.OnResume();

        //Log.Debug("detail resume");
    }

    protected internal override void OnPause()
    {
        base.OnPause();

        //Log.Debug("detail pause");
    }

    protected internal override void OnReveal()
    {
        base.OnReveal();

        //Log.Debug("detail reveal");
    }

    protected internal override void OnRecycle()
    {
        base.OnRecycle();

        //Log.Debug("detail recycle");
    }

    protected internal override void OnCover()
    {
        base.OnCover();

        //Log.Debug("detail cover");
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        //Log.Debug("detail update " + elapseSeconds);
    }
}
