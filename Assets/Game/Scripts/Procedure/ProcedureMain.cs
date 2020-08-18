// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 10:14:34
// ========================================================

using LF;
using LF.Event;
using LF.Fsm;
using LF.Procedure;
using LF.Scene;
using UnityEngine.SceneManagement;

public class EnterGameEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(EnterGameEventArgs).GetHashCode();

    public override int Id => EventId;

    /// <summary>
    /// 游戏等级
    /// </summary>
    public int Level;

    public static EnterGameEventArgs Create(int level)
    {
        EnterGameEventArgs e = new EnterGameEventArgs();
        e.Level = level;
        return e;
    }
}

public class ProcedureMain : GameProcedure
{
    int targetScene = -1;

    protected internal override void OnInit(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);

        Log.Debug("procedure main init");

        GameEntry.Event.Subscribe(EnterGameEventArgs.EventId, EnterGameHandler);
        GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, LoadSceneSuccessHandler);
        GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, LoadSceneUpdateHandler);
    }

    protected internal override void OnEnter(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);

        Log.Debug("procedure main enter");

        if (procedureOwner.HasData("fromGame"))
        {
            bool fromGame = procedureOwner.GetData<bool>("fromGame");
            if (fromGame)
            {
                Open(UIFormId.Detail);
            }
        }
    }

    protected internal override void OnLeave(IFsm<ProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        //Log.Debug("procedure preload leave");
    }

    protected internal override void OnUpdate(IFsm<ProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        //Log.Debug("procedure preload update " + elapseSeconds + "  " + realElapseSeconds);
        if(targetScene != -1)
        {
            //SceneManager.LoadScene("BasketBall", LoadSceneMode.Additive);
            //SceneManager.LoadScene("Game", LoadSceneMode.Additive);
            targetScene = -1;

            GameEntry.Scene.LoadScene(SceneId.Game);

            ChangeState<ProcedureGame>(procedureOwner);
        }
    }

    protected internal override void OnDestroy(IFsm<ProcedureManager> procedureOwner)
    {
        base.OnDestroy(procedureOwner);

        //Log.Debug("procedure preload destroy");
    }

    void EnterGameHandler(object sender, GameEventArgs e)
    {
        EnterGameEventArgs ne = (EnterGameEventArgs)e;
        if (ne != null)
        {
            targetScene = ne.Level;
        }
    }

    void LoadSceneSuccessHandler(object sender, GameEventArgs e)
    {
        LoadSceneSuccessEventArgs ne = e as LoadSceneSuccessEventArgs;
        Log.Debug("load scene {0} complete", ne.SceneAssetName);
    }

    void LoadSceneUpdateHandler(object sender, GameEventArgs e)
    {
        LoadSceneUpdateEventArgs ne = e as LoadSceneUpdateEventArgs;
        Log.Debug("load scene progress {0}", ne.Progress);
    }
}
