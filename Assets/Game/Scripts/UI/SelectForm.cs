// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 15:19:04
// ========================================================
using LF;
using LF.UI;
using UnityEngine.UI;

public class SelectForm : GameUILogic
{
    public Button P1Btn;
    public Button P2Btn;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        Log.Debug("select init");

        P1Btn.onClick.AddListener(() =>
        {
            Open(UIFormId.Detail);
        });

        P2Btn.onClick.AddListener(() =>
        {
            Close();
        });
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        Log.Debug("select open");
    }

    protected override void OnClose(object userData)
    {
        base.OnClose(userData);

        Log.Debug("select close");
    }

    protected override void OnResume()
    {
        base.OnResume();

        Log.Debug("select resume");
    }

    protected override void OnPause()
    {
        base.OnPause();

        Log.Debug("select pause");
    }

    protected override void OnReveal()
    {
        base.OnReveal();

        Log.Debug("select reveal");
    }

    protected override void OnRecycle()
    {
        base.OnRecycle();

        Log.Debug("select recycle");
    }

    protected override void OnCover()
    {
        base.OnCover();

        Log.Debug("select cover");
    }

    protected override void OnRefocus(object userData)
    {
        base.OnRefocus(userData);

        Log.Debug("select refocus");
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        Log.Debug("select update " + elapseSeconds);
    }
}
