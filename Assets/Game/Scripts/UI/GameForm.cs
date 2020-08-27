// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 15:19:04
// ========================================================
using LF;
using LF.UDP;
using LF;
using LF.Net;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class GameForm : GameUILogic
{
    public GameObject shopBtn;
    public GameObject selectBtn;
   
    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);

        //Log.Debug("loading init");

        GameEntry.UIEvent.AddOnClickHandler(shopBtn, (obj) =>
        {
            Open(UIFormId.Shop);
        });

        GameEntry.UIEvent.AddOnClickHandler(selectBtn, (obj) =>
        {
            Open(UIFormId.Select);
        });
    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        //Log.Debug("loading open");

        //GameEntry.Timer.doOnce(3000, () =>
        //{
        //    Log.Debug("3 seconds later");
        //});

        //GameEntry.Timer.doFrameOnce(10, () =>
        //{
        //    Log.Debug("10 frame later");
        //});

        //GameEntry.Timer.doLoop(10000, () =>
        //{
        //    Log.Debug("10 seconds later");
        //});

        //GameEntry.Timer.doFrameLoop(20, () =>
        //{
        //    Log.Debug("20 frame later " + Time.frameCount);
        //});
    }

    protected internal override void OnClose(object userData)
    {
        base.OnClose(userData);

        Log.Debug("loading close");

        UdpRecv.Instance.Dispose();
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

        //Log.Debug("loading reveal");
    }

    protected internal override void OnRecycle()
    {
        base.OnRecycle();

        Log.Debug("loading recycle");
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

    private void OnDestroy()
    {
        UdpRecv.Instance.Dispose();
    }
}
