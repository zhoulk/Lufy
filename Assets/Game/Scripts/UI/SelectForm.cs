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

    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);

        //Log.Debug("select init");

        P1Btn.onClick.AddListener(() =>
        {
            Open(UIFormId.Detail);
        });

        P2Btn.onClick.AddListener(() =>
        {
            Close();
        });
    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        //Log.Debug("select open");
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
