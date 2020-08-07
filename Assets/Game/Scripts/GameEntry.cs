// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 11:51:44
// ========================================================

using LF;
using LF.Fsm;
using LF.Procedure;
using LF.UI;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    static BaseManager m_base;
    static UIManager m_ui;
    static ProcedureManager m_procedure;
    static FsmManager m_fsm;

    private void Awake()
    {
        m_base = Lufy.GetManager<BaseManager>();
        m_ui = Lufy.GetManager<UIManager>();
        m_procedure = Lufy.GetManager<ProcedureManager>();
        m_fsm = Lufy.GetManager<FsmManager>();
    }

    private void Start()
    {
        ProcedureBase[] procedures = new ProcedureBase[2];
        procedures[0] = new ProcedureLaunch();
        procedures[1] = new ProcedurePreload();

        m_procedure.Initialize(m_fsm, procedures);
        m_procedure.StartProcedure(typeof(ProcedureLaunch));
    }

    public static BaseManager Base
    {
        get
        {
            return m_base;
        }
        set { }
    }

    public static UIManager UI
    {
        get
        {
            return m_ui;
        }
        set { }
    }
}

