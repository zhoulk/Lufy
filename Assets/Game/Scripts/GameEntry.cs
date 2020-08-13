// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 11:51:44
// ========================================================

using LF.Timer;
using LF;
using LF.Fsm;
using LF.Procedure;
using LF.UI;
using LF.UINavi;
using UnityEngine;
using LF.Pool;
using LF.Event;
using LF.Sound;

public class GameEntry : MonoBehaviour
{
    static BaseManager m_Base;
    static UIManager m_UI;
    static ProcedureManager m_Procedure;
    static FsmManager m_Fsm;
    static UIEventManager m_UIEvent;
    static TimerManager m_Timer;
    static ObjectPoolManager m_ObjectPool;
    static EventManager m_Event;
    static SoundManager m_Sound;

    private void Start()
    {
        initManagers();

        ProcedureBase[] procedures = new ProcedureBase[4];
        procedures[0] = new ProcedureLaunch();
        procedures[1] = new ProcedurePreload();
        procedures[2] = new ProcedureMain();
        procedures[3] = new ProcedureGame();

        m_Procedure.Initialize(m_Fsm, procedures);
        m_Procedure.StartProcedure(typeof(ProcedureLaunch));
    }

    public static BaseManager Base
    {
        get
        {
            return m_Base;
        }
        set { }
    }

    public static UIManager UI
    {
        get
        {
            return m_UI;
        }
        set { }
    }

    public static UIEventManager UIEvent
    {
        get
        {
            return m_UIEvent;
        }
        set { }
    }

    public static TimerManager Timer
    {
        get
        {
            return m_Timer;
        }
        set { }
    }

    public static ProcedureManager Procedure
    {
        get
        {
            return m_Procedure;
        }
        set { }
    }

    public static ObjectPoolManager ObjectPool
    {
        get
        {
            return m_ObjectPool;
        }
        set { }
    }

    public static EventManager Event
    {
        get
        {
            return m_Event;
        }
        set { }
    }

    public static SoundManager Sound
    {
        get
        {
            return m_Sound;
        }
        set { }
    }

    void initManagers()
    {
        m_Base = Lufy.GetManager<BaseManager>();
        m_UI = Lufy.GetManager<UIManager>();
        m_Procedure = Lufy.GetManager<ProcedureManager>();
        m_Fsm = Lufy.GetManager<FsmManager>();
        m_Timer = Lufy.GetManager<TimerManager>();

        m_UIEvent = Lufy.GetManager<UIEventManager>();
        m_UIEvent.Initialize(new GameInput());

        m_ObjectPool = Lufy.GetManager<ObjectPoolManager>();
        m_Event = Lufy.GetManager<EventManager>();

        m_Sound = Lufy.GetManager<SoundManager>();
    }
}

class GameInput : IUIInput
{
    public bool ClickDownDown()
    {
        return Input.GetKeyDown(KeyCode.DownArrow);
    }

    public bool ClickEnterDown()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }

    public bool ClickLeftDown()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow);
    }

    public bool ClickRightDown()
    {
        return Input.GetKeyDown(KeyCode.RightArrow);
    }

    public bool ClickUpDown()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }
}

