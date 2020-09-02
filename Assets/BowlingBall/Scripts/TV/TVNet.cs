// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-02 17:20:56
// ========================================================
using LF;
using LF.Event;
using LF.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bowling
{
    public class TVNet : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (Define.platForm.Equals(PlatForm.TV))
            {
                GameEntry.Event.Subscribe(BowlingBallEventArgs.EventId, OnBowlingBallHandler);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnBowlingBallHandler(object sender, GameEventArgs args)
        {
            BowlingBallEventArgs ne = args as BowlingBallEventArgs;
            if (ne != null)
            {
                Log.Debug("second {0} {1}", ne.msg.Velocity, ne.msg.Torque);
                
            }
        }
    }

    public sealed class BowlingBallEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(BowlingBallEventArgs).GetHashCode();

        public override int Id => EventId;

        public MessageBowlingBall msg;

        public static BowlingBallEventArgs Create(MessageBowlingBall msg)
        {
            //Log.Debug("middle {0} {1}", msg.Velocity, msg.Torque);

            BowlingBallEventArgs e = new BowlingBallEventArgs();
            e.msg = msg;
            return e;
        }
    }
}

