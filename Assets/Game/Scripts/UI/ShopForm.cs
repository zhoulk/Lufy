// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 15:19:04
// ========================================================
using LF;
using LF.UI;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopForm : GameUILogic
{
    public Button backBtn;

    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);

        if(userData != null)
        {
            Dictionary<string, object> param = userData as Dictionary<string, object>;
            Log.Debug("shop param {0} {1} {2} ", param["key1"], param["key2"], param["key3"]);
        }

        //Log.Debug("shop init");

        backBtn.onClick.AddListener(() =>
        {
            Close();
        });
    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        //Log.Debug("shop open");
    }

    protected internal override void OnClose(object userData)
    {
        base.OnClose(userData);

        //Log.Debug("shop close");
    }

    protected internal override void OnResume()
    {
        base.OnResume();

        //Log.Debug("shop resume");
    }

    protected internal override void OnPause()
    {
        base.OnPause();

        //Log.Debug("shop pause");
    }

    protected internal override void OnReveal()
    {
        base.OnReveal();

        //Log.Debug("shop reveal");
    }

    protected internal override void OnRecycle()
    {
        base.OnRecycle();

        //Log.Debug("shop recycle");
    }

    protected internal override void OnCover()
    {
        base.OnCover();

        //Log.Debug("shop cover");
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        //Log.Debug("shop update " + elapseSeconds);
    }
}
