// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-21 10:51:58
// ========================================================
using LF;
using LF.Event;
using LF.Pool;
using LF.Res;
using LF.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    UIManager m_UIManager = null;
    ResManager m_ResManager = null;
    ObjectPoolManager m_ObjectPoolManager = null;
    EventManager m_EventManager = null;

    // UI1 -> UI2
    // UI1 -> Pop1
    // UI1 -> UI2 -> UI3
    // UI1 -> Pop1 -> UI2
    // UI1 -> UI2 -> UI3 -> UI1
    // UI1 -> Pop1 -> UI2 -> UI1

    // Start is called before the first frame update
    void Start()
    {
        m_EventManager = Lufy.GetManager<EventManager>();
        m_ObjectPoolManager = Lufy.GetManager<ObjectPoolManager>();
        m_ResManager = Lufy.GetManager<ResManager>();
        m_ResManager.SetResLoader(new AssetBundleLoader());
        m_UIManager = Lufy.GetManager<UIManager>();
        m_UIManager.SetResManager(m_ResManager);
        m_UIManager.SetObjectPoolManager(m_ObjectPoolManager);
        m_UIManager.SetEventManager(m_EventManager);

        m_EventManager.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUISuccess);
        m_EventManager.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFail);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("key1", "value1");
            param.Add("key2", 100);
            param.Add("key3", new string[] { "ada", "adsd"});
            m_UIManager.OpenUIForm("Assets/Lufy/Test/UI/Prefab/test.prefab", param);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("key1", "value1");
            param.Add("key2", 100);
            param.Add("key3", new string[] { "ada", "adsd" });
            m_UIManager.OpenUIForm("Assets/Lufy/Test/UI/Prefab/test11111.prefab", param);
        }
    }

    void OnOpenUISuccess(object sender, GameEventArgs args)
    {
        OpenUIFormSuccessEventArgs ne = args as OpenUIFormSuccessEventArgs;

        Log.Debug(ne);
        Log.Debug(ne.UserData);

        Dictionary<string, object> param = ne.UserData as Dictionary<string, object>;
        foreach(var kv in param)
        {
            Log.Debug("{0} {1}", kv.Key, kv.Value);
        }
    }

    void OnOpenUIFail(object sender, GameEventArgs args)
    {
        OpenUIFormFailureEventArgs ne = args as OpenUIFormFailureEventArgs;

        Log.Debug(ne);
        Log.Debug(ne.UIFormAssetName);
        Log.Debug(ne.ErrorMessage);
        Log.Debug(ne.UserData);

        Dictionary<string, object> param = ne.UserData as Dictionary<string, object>;
        foreach (var kv in param)
        {
            Log.Debug("{0} {1}", kv.Key, kv.Value);
        }
    }
}
