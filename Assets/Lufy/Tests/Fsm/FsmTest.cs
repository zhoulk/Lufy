// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 11:50:47
// ========================================================
using System.Collections;
using LF;
using LF.Fsm;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests
{
    public class FsmTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void FsmTestSimplePasses()
        {
            // Use the Assert class to test conditions

            Hero hero = new Hero();
            hero.Initialize();
            hero.Start();
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator FsmTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }

    class Hero
    {
        FsmManager m_manager;
        IFsm<Hero> m_fsm;

        public void Initialize()
        {
            m_manager = new FsmManager();
            Action[] actions = new Action[2];
            actions[0] = new AttackAct();
            actions[1] = new IdleAct();
            m_fsm = m_manager.CreateFsm<Hero>(this, actions);
        }

        public void Start()
        {
            m_fsm.Start<IdleAct>();
        }
    }

    class Action : FsmState<Hero>
    {

    }

    class AttackAct : Action
    {
        protected override void OnInit(IFsm<Hero> fsm)
        {
            base.OnInit(fsm);

            Log.Info("attack init");
        }

        protected override void OnEnter(IFsm<Hero> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("attack enter");
        }

        protected override void OnUpdate(IFsm<Hero> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<Hero> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            Log.Info("attack leave");
        }
    }

    class IdleAct : Action
    {
        protected override void OnInit(IFsm<Hero> fsm)
        {
            base.OnInit(fsm);

            Log.Info("idle init");
        }

        protected override void OnEnter(IFsm<Hero> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("idle enter");

            ChangeState<AttackAct>(fsm);
        }

        protected override void OnUpdate(IFsm<Hero> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            Log.Info("idle enter " + elapseSeconds);
        }

        protected override void OnLeave(IFsm<Hero> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            Log.Info("idle leave");
        }
    }
}
