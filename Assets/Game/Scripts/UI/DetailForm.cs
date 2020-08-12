// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 15:19:04
// ========================================================

using UnityEngine.UI;

public class DetailForm : GameUILogic
{
    public Button fightBtn;
    public Button backBtn;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        //Log.Debug("detail init");

        fightBtn.onClick.AddListener(() =>
        {
            GameEntry.Event.Fire(this, EnterGameEventArgs.Create(1));
        });

        backBtn.onClick.AddListener(() =>
        {
            Close();
        });
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        //Log.Debug("detail open");
    }

    protected override void OnClose(object userData)
    {
        base.OnClose(userData);

        //Log.Debug("detail close");
    }

    protected override void OnResume()
    {
        base.OnResume();

        //Log.Debug("detail resume");
    }

    protected override void OnPause()
    {
        base.OnPause();

        //Log.Debug("detail pause");
    }

    protected override void OnReveal()
    {
        base.OnReveal();

        //Log.Debug("detail reveal");
    }

    protected override void OnRecycle()
    {
        base.OnRecycle();

        //Log.Debug("detail recycle");
    }

    protected override void OnCover()
    {
        base.OnCover();

        //Log.Debug("detail cover");
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        //Log.Debug("detail update " + elapseSeconds);
    }
}
