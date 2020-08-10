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

public class GameEntry : MonoBehaviour
{
    static BaseManager m_Base;
    static UIManager m_UI;
    static ProcedureManager m_Procedure;
    static FsmManager m_Fsm;
    static UIEventManager m_UIEvent;
    static TimerManager m_Timer;

    private void Awake()
    {
        m_Base = Lufy.GetManager<BaseManager>();
        m_UI = Lufy.GetManager<UIManager>();
        m_Procedure = Lufy.GetManager<ProcedureManager>();
        m_Fsm = Lufy.GetManager<FsmManager>();
        m_Timer = Lufy.GetManager<TimerManager>();

        m_UIEvent = Lufy.GetManager<UIEventManager>();
        m_UIEvent.Initialize(new GameInput());
    }

    private void Start()
    {
        ProcedureBase[] procedures = new ProcedureBase[2];
        procedures[0] = new ProcedureLaunch();
        procedures[1] = new ProcedurePreload();

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

