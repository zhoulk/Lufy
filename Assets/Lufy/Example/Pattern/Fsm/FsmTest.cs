// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-24 17:53:59
// ========================================================
using LF;
using LF.Fsm;
using UnityEngine;

class HeroRun : FsmState<FsmTest>
{
    protected internal override void OnDestroy(IFsm<FsmTest> fsm)
    {
        base.OnDestroy(fsm);

        Log.Debug("HeroRun destroy");
    }

    protected internal override void OnEnter(IFsm<FsmTest> fsm)
    {
        base.OnEnter(fsm);

        Log.Debug("HeroRun enter");
    }

    protected internal override void OnInit(IFsm<FsmTest> fsm)
    {
        base.OnInit(fsm);

        Log.Debug("HeroRun init");
    }

    protected internal override void OnLeave(IFsm<FsmTest> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);

        Log.Debug("HeroRun leave");
    }

    protected internal override void OnUpdate(IFsm<FsmTest> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        //Log.Debug("HeroRun update");

        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeState<HeroIdle>(fsm);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeState<HeroAttack>(fsm);
        }
    }
}

class HeroIdle : FsmState<FsmTest>
{
    protected internal override void OnDestroy(IFsm<FsmTest> fsm)
    {
        base.OnDestroy(fsm);

        Log.Debug("HeroIdle destroy");
    }

    protected internal override void OnEnter(IFsm<FsmTest> fsm)
    {
        base.OnEnter(fsm);

        Log.Debug("HeroIdle enter");
    }

    protected internal override void OnInit(IFsm<FsmTest> fsm)
    {
        base.OnInit(fsm);

        Log.Debug("HeroIdle init");
    }

    protected internal override void OnLeave(IFsm<FsmTest> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);

        Log.Debug("HeroIdle leave");
    }

    protected internal override void OnUpdate(IFsm<FsmTest> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeState<HeroRun>(fsm);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeState<HeroAttack>(fsm);
        }
    }
}

class HeroAttack : FsmState<FsmTest>
{
    protected internal override void OnDestroy(IFsm<FsmTest> fsm)
    {
        base.OnDestroy(fsm);

        Log.Debug("HeroAttack destroy");
    }

    protected internal override void OnEnter(IFsm<FsmTest> fsm)
    {
        base.OnEnter(fsm);

        Log.Debug("HeroAttack enter");
    }

    protected internal override void OnInit(IFsm<FsmTest> fsm)
    {
        base.OnInit(fsm);

        Log.Debug("HeroAttack init");
    }

    protected internal override void OnLeave(IFsm<FsmTest> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);

        Log.Debug("HeroAttack leave");
    }

    protected internal override void OnUpdate(IFsm<FsmTest> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeState<HeroIdle>(fsm);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeState<HeroRun>(fsm);
        }
    }
}

public class FsmTest : MonoBehaviour
{
    FsmManager m_FsmManager = null;
    private IFsm<FsmTest> m_HeroFsm;

    // Start is called before the first frame update
    void Start()
    {
        m_FsmManager = GetComponent<FsmManager>();

        FsmState<FsmTest>[] states = new FsmState<FsmTest>[3];
        states[0] = new HeroRun();
        states[1] = new HeroIdle();
        states[2] = new HeroAttack();
        m_HeroFsm = m_FsmManager.CreateFsm(this, states);
        m_HeroFsm.Start(typeof(HeroRun));
    }

    // Update is called once per frame
    void Update()
    {
        m_FsmManager.OnUpdate(Time.deltaTime, Time.unscaledDeltaTime);
    }
}
