// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 15:19:04
// ========================================================
using LF;
using System.Collections.Generic;
using UnityEngine;

public class LoadingForm : GameUILogic
{
    public GameObject shopBtn;
    public GameObject selectBtn;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        Log.Debug("loading init");

        shopBtn.AddNaviRight(selectBtn).AddSelected(OnBtnSelected).AddUnSelected(OnBtnUnSelected);
        selectBtn.AddNaviLeft(shopBtn).AddSelected(OnBtnSelected).AddUnSelected(OnBtnUnSelected);

        GameEntry.UIEvent.AddOnClickHandler(shopBtn, (obj) =>
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("key1", 100);
            param.Add("key2", "23232");
            param.Add("key3", this);
            Open(UIFormId.Shop, param);
        });

        GameEntry.UIEvent.AddOnClickHandler(selectBtn, (obj) =>
        {
            Open(UIFormId.Select);
        });
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        shopBtn.SetAsDefaultNavi();

        Log.Debug("loading open");

        GameEntry.Timer.doOnce(3000, () =>
        {
            Log.Debug("3 seconds later");
        });

        GameEntry.Timer.doFrameOnce(10, () =>
        {
            Log.Debug("10 frame later");
        });

        GameEntry.Timer.doLoop(10000, () =>
        {
            Log.Debug("10 seconds later");
        });

        GameEntry.Timer.doFrameLoop(20, () =>
        {
            Log.Debug("20 frame later " + Time.frameCount);
        });
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

        shopBtn.SetAsDefaultNavi();

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

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        Log.Debug("loading update " + elapseSeconds);
    }
}
