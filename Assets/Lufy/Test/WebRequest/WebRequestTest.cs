// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-11-06 15:03:55
// ========================================================
using LF;
using LF.Event;
using LF.WebRequest;
using UnityEngine;

public class WebRequestTest : MonoBehaviour
{
    WebRequestManager webRequestManager;
    EventManager eventManager;

    // Start is called before the first frame update
    void Start()
    {
        webRequestManager = Lufy.GetManager<WebRequestManager>();
        eventManager = Lufy.GetManager<EventManager>();
        webRequestManager.SetEventManager(eventManager);
        webRequestManager.Initialize();

        eventManager.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebSuccessHander);
        eventManager.Subscribe(WebRequestStartEventArgs.EventId, OnWebStartHander);
        eventManager.Subscribe(WebRequestFailureEventArgs.EventId, OnWebFailHander);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("click A");
            webRequestManager.AddWebRequest("http://113.108.110.61:62181/easyview-mxly-api//isg/token", new byte[] { }, "load token");
        }
    }

    void OnWebSuccessHander(object sender, GameEventArgs args)
    {
        WebRequestSuccessEventArgs ne = args as WebRequestSuccessEventArgs;
        string response = Utility.Converter.GetString(ne.GetWebResponseBytes());
        Debug.Log(ne.UserData + "  " + response);
    }

    void OnWebStartHander(object sender, GameEventArgs args)
    {
        WebRequestStartEventArgs ne = args as WebRequestStartEventArgs;
        Debug.Log(ne.UserData);
    }

    void OnWebFailHander(object sender, GameEventArgs args)
    {
        WebRequestFailureEventArgs ne = args as WebRequestFailureEventArgs;
        Debug.Log(ne.UserData + " " + ne.ErrorMessage);
    }
}
