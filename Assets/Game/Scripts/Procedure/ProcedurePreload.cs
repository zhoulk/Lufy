﻿// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 10:14:34
// ========================================================

using LF;
using LF.Fsm;
using LF.Procedure;

public class ProcedurePreload : GameProcedure
{
    protected internal override void OnInit(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);

        Log.Debug("procedure preload init");
    }

    protected internal override void OnEnter(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);

        Log.Debug("procedure preload enter");

        Open(UIFormId.game);
        ChangeState<ProcedureMMain>(procedureOwner);
    }

    protected internal override void OnLeave(IFsm<ProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        Log.Debug("procedure preload leave");
    }

    protected internal override void OnUpdate(IFsm<ProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        //Log.Debug("procedure preload update " + elapseSeconds + "  " + realElapseSeconds);
    }

    protected internal override void OnDestroy(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnDestroy(procedureOwner);

        Log.Debug("procedure preload destroy");
    }
}
