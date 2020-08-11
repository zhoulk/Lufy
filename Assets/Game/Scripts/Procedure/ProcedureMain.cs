// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 10:14:34
// ========================================================

using LF;
using LF.Fsm;
using LF.Procedure;
using UnityEngine.SceneManagement;

public class ProcedureMain : GameProcedure
{
    int targetScene = -1;

    protected override void OnInit(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);

        //Log.Debug("procedure preload init");
    }

    protected override void OnEnter(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);

        //Log.Debug("procedure preload enter");

        //Open(UIFormId.Loading);
    }

    protected override void OnLeave(IFsm<ProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        //Log.Debug("procedure preload leave");
    }

    protected override void OnUpdate(IFsm<ProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        //Log.Debug("procedure preload update " + elapseSeconds + "  " + realElapseSeconds);
        if(targetScene != -1)
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
            ChangeState<ProcedureGame>(procedureOwner);
            targetScene = -1;
        }
    }

    protected override void OnDestroy(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnDestroy(procedureOwner);

        //Log.Debug("procedure preload destroy");
    }

    public void EnterGame()
    {
        targetScene = 1;
    }
}
