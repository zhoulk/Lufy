// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 15:19:04
// ========================================================
using LF;
using LF.UI;
using UnityEngine.UI;

public class ShopForm : UIFormLogic
{
    public Button backBtn;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        Log.Debug("shop init");

        backBtn.onClick.AddListener(() =>
        {
            GameEntry.UI.CloseUIForm(this);
        });
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        Log.Debug("shop open");
    }

    protected override void OnClose(object userData)
    {
        base.OnClose(userData);

        Log.Debug("shop close");
    }

    protected override void OnResume()
    {
        base.OnResume();

        Log.Debug("shop resume");
    }

    protected override void OnPause()
    {
        base.OnPause();

        Log.Debug("shop pause");
    }

    protected override void OnReveal()
    {
        base.OnReveal();

        Log.Debug("shop reveal");
    }

    protected override void OnRecycle()
    {
        base.OnRecycle();

        Log.Debug("shop recycle");
    }

    protected override void OnCover()
    {
        base.OnCover();

        Log.Debug("shop cover");
    }

    protected override void OnRefocus(object userData)
    {
        base.OnRefocus(userData);

        Log.Debug("shop refocus");
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        Log.Debug("shop update " + elapseSeconds);
    }
}
