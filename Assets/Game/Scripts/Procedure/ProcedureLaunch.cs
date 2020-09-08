// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 09:18:53
// ========================================================
using LF;
using LF.Fsm;
using LF.Procedure;

public class ProcedureLaunch : GameProcedure
{
    protected internal override void OnInit(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);

        Log.Debug("procedure main init");
    }

    protected internal override void OnEnter(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);

        Log.Debug("procedure main enter");
    }

    protected internal override void OnLeave(IFsm<ProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        Log.Debug("procedure main leave");
    }

    protected internal override void OnUpdate(IFsm<ProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        //Log.Debug("procedure main update " + elapseSeconds + "  " + realElapseSeconds);

        //ChangeState<ProcedurePreload>(procedureOwner);
        ChangeState<ProcedureBasketball>(procedureOwner);
        //ChangeState<ProcedureBowling>(procedureOwner);
    }

    protected internal override void OnDestroy(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnDestroy(procedureOwner);

        Log.Debug("procedure main destroy");
    }
}
