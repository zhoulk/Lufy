// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 15:19:04
// ========================================================
using LF;
using LF.UDP;
using LT;
using LT.Net;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class LoadingForm : GameUILogic
{
    public GameObject shopBtn;
    public GameObject selectBtn;

    public GameObject searchBtn;
    public GameObject connectBtn;
    public GameObject disConnectBtn;
    public GameObject basketBallBtn;

    public Text statusText;

    IPEndPoint targetEndPoint;

    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);

        //Log.Debug("loading init");

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

        GameEntry.UIEvent.AddOnClickHandler(searchBtn, (obj) =>
        {
            statusText.text = "搜索中...";
            UdpRecv.Instance.Init(new IPEndPoint(IPAddress.Any, 7777));
            UdpRecv.Instance.ReceiveEventHandler = (bytes, endPoint) =>
            {
                string info = System.Text.Encoding.UTF8.GetString(bytes);
                Log.Debug("{0} {1} ----- {2}", endPoint.Address.ToString(), endPoint, info);

                if (endPoint.Address.ToString().Equals("172.16.4.112"))
                {
                    string[] array = info.Split('-');

                    string ip = array[0];
                    string port = array[1];
                    //string type = array[2]  //盒子型号

                    targetEndPoint = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));

                    Log.Debug("search finish");
                    Loom.QueueOnMainThread(() =>
                    {
                        statusText.text = "搜索成功";
                    });
                    UdpRecv.Instance.Dispose();
                }
            };
        });

        GameEntry.UIEvent.AddOnClickHandler(connectBtn, (obj) =>
        {
            statusText.text = "";
            UDPManager.Instance.Connect(targetEndPoint);
        });

        GameEntry.UIEvent.AddOnClickHandler(disConnectBtn, (obj) =>
        {
            UDPManager.Instance.DisConnect();
        });

        GameEntry.UIEvent.AddOnClickHandler(basketBallBtn, (obj) =>
        {
            //IMessage msg = MessagesFactory.BasketBall(1, 10, 90);
            //UDPManager.Instance.Send(msg);
        });
    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        shopBtn.SetAsDefaultNavi();

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

        shopBtn.SetAsDefaultNavi();

        //Log.Debug("loading reveal");
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
