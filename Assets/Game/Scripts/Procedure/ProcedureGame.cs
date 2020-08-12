// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 10:14:34
// ========================================================

using LF;
using LF.Event;
using LF.Fsm;
using LF.Procedure;
using UnityEngine.SceneManagement;

public class ExitGameEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(ExitGameEventArgs).GetHashCode();

    public override int Id => EventId;

    /// <summary>
    /// 游戏等级
    /// </summary>
    public int Level;

    public static ExitGameEventArgs Create(int level)
    {
        ExitGameEventArgs e = new ExitGameEventArgs();
        e.Level = level;
        return e;
    }
}

public class ProcedureGame : GameProcedure
{
    bool exitGame = false;

    protected override void OnInit(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);

        //Log.Debug("procedure preload init");

        GameEntry.Event.Subscribe(ExitGameEventArgs.EventId, ExitGameHandler);
    }

    protected override void OnEnter(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);

        //Log.Debug("procedure preload enter");

        Open(UIFormId.Fight);
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
        if (exitGame)
        {
            exitGame = false;
            ChangeState<ProcedureMain>(procedureOwner);
            SceneManager.UnloadSceneAsync("Game");
        }
    }

    protected override void OnDestroy(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnDestroy(procedureOwner);

        //Log.Debug("procedure preload destroy");
    }

    void ExitGameHandler(object sender, GameEventArgs e)
    {
        ExitGameEventArgs ne = (ExitGameEventArgs)e;

        exitGame = true;
    }
}
