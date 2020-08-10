// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 15:19:04
// ========================================================
using LF;
using LF.UI;
using UnityEngine.UI;

public class LoadingForm : GameUILogic
{
    public Button shopBtn;
    public Button selectBtn;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        Log.Debug("loading init");

        shopBtn.onClick.AddListener(() =>
        {
            Open(UIFormId.Shop);
        });

        selectBtn.onClick.AddListener(() =>
        {
            Open(UIFormId.Select);
        });
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        Log.Debug("loading open");
    }

    protected override void OnClose(object userData)
    {
        base.OnClose(userData);

        Log.Debug("loading close");
    }

    protected override void OnResume()
    {
        base.OnResume();

        Log.Debug("loading resume");
    }

    protected override void OnPause()
    {
        base.OnPause();

        Log.Debug("loading pause");
    }

    protected override void OnReveal()
    {
        base.OnReveal();

        Log.Debug("loading reveal");
    }

    protected override void OnRecycle()
    {
        base.OnRecycle();

        Log.Debug("loading recycle");
    }

    protected override void OnCover()
    {
        base.OnCover();

        Log.Debug("loading cover");
    }

    protected override void OnRefocus(object userData)
    {
        base.OnRefocus(userData);

        Log.Debug("loading refocus");
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        Log.Debug("loading update " + elapseSeconds);
    }
}
