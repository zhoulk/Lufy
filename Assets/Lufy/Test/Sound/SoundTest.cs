// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-23 14:21:05
// ========================================================
using LF;
using LF.Event;
using LF.Res;
using LF.Sound;
using LF.Timer;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    SoundManager m_SoundManager = null;
    ResManager m_ResManager = null;
    EventManager m_EventManager = null;
    TimerManager m_TimerManager = null;

    // Start is called before the first frame update
    void Start()
    {
        m_TimerManager = Lufy.GetManager<TimerManager>();
        m_EventManager = Lufy.GetManager<EventManager>();
        m_ResManager = Lufy.GetManager<ResManager>();
        m_ResManager.SetResLoader(new AssetBundleLoader());
        m_SoundManager = Lufy.GetManager<SoundManager>();
        m_SoundManager.SetResManager(m_ResManager);
        m_SoundManager.SetEventManager(m_EventManager);
        m_SoundManager.SetTimerManager(m_TimerManager);

        m_EventManager.Subscribe(PlayMusicCompleteEventArgs.EventId, OnMusicPlayCompleteHander);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_SoundManager.MusicParams.Loop = true;
            m_SoundManager.MusicParams.Interval = 5;
            m_SoundManager.PlaySound("Assets/Lufy/Test/Sound/bgm3.mp3", false);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            m_SoundManager.MusicParams.Loop = true;
            m_SoundManager.MusicParams.Interval = 0;
            m_SoundManager.PlaySound("Assets/Lufy/Test/Sound/bgm.mp3", false);
        }
    }

    void OnMusicPlayCompleteHander(object sender, GameEventArgs args)
    {
        PlayMusicCompleteEventArgs ne = args as PlayMusicCompleteEventArgs;
        Log.Debug("{0} {1} {2}", ne.SoundAssetName, ne.SoundLength, ne.UserData);
    }
}
