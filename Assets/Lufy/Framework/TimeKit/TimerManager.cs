// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-10 19:00:47
// ========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LF.Timer
{
    public delegate void Handler();
    public delegate void Handler<T1>(T1 param1);
    public delegate void Handler<T1, T2>(T1 param1, T2 param2);
    public delegate void Handler<T1, T2, T3>(T1 param1, T2 param2, T3 param3);

    /// <summary>
    /// 时间管理类
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Lufy/Timer")]
    public sealed class TimerManager : LufyManager
    {
        private List<TimerHandler> m_Pool = new List<TimerHandler>();
        /** 用数组保证按放入顺序执行*/
        private List<TimerHandler> m_Handlers = new List<TimerHandler>();
        private int m_CurrFrame = 0;

        /// /// <summary>
        /// 定时执行一次(基于毫秒)
        /// </summary>
        /// <param name="delay">延迟时间(单位毫秒)</param>
        /// <param name="method">结束时的回调方法</param>
        /// <param name="args">回调参数</param>
        public void doOnce(int delay, Handler method)
        {
            create(false, false, delay, method);
        }
        public void doOnce<T1>(int delay, Handler<T1> method, params object[] args)
        {
            create(false, false, delay, method, args);
        }
        public void doOnce<T1, T2>(int delay, Handler<T1, T2> method, params object[] args)
        {
            create(false, false, delay, method, args);
        }
        public void doOnce<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, params object[] args)
        {
            create(false, false, delay, method, args);
        }

        /// /// <summary>
        /// 定时重复执行(基于毫秒)
        /// </summary>
        /// <param name="delay">延迟时间(单位毫秒)</param>
        /// <param name="method">结束时的回调方法</param>
        /// <param name="args">回调参数</param>
        public void doLoop(int delay, Handler method)
        {
            create(false, true, delay, method);
        }
        public void doLoop<T1>(int delay, Handler<T1> method, params object[] args)
        {
            create(false, true, delay, method, args);
        }
        public void doLoop<T1, T2>(int delay, Handler<T1, T2> method, params object[] args)
        {
            create(false, true, delay, method, args);
        }
        public void doLoop<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, params object[] args)
        {
            create(false, true, delay, method, args);
        }


        /// <summary>
        /// 定时执行一次(基于帧率)
        /// </summary>
        /// <param name="delay">延迟时间(单位为帧)</param>
        /// <param name="method">结束时的回调方法</param>
        /// <param name="args">回调参数</param>
        public void doFrameOnce(int delay, Handler method)
        {
            create(true, false, delay, method);
        }
        public void doFrameOnce<T1>(int delay, Handler<T1> method, params object[] args)
        {
            create(true, false, delay, method, args);
        }
        public void doFrameOnce<T1, T2>(int delay, Handler<T1, T2> method, params object[] args)
        {
            create(true, false, delay, method, args);
        }
        public void doFrameOnce<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, params object[] args)
        {
            create(true, false, delay, method, args);
        }

        /// <summary>
        /// 定时重复执行(基于帧率)
        /// </summary>
        /// <param name="delay">延迟时间(单位为帧)</param>
        /// <param name="method">结束时的回调方法</param>
        /// <param name="args">回调参数</param>
        public void doFrameLoop(int delay, Handler method)
        {
            create(true, true, delay, method);
        }
        public void doFrameLoop<T1>(int delay, Handler<T1> method, params object[] args)
        {
            create(true, true, delay, method, args);
        }
        public void doFrameLoop<T1, T2>(int delay, Handler<T1, T2> method, params object[] args)
        {
            create(true, true, delay, method, args);
        }
        public void doFrameLoop<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, params object[] args)
        {
            create(true, true, delay, method, args);
        }

        /// <summary>
        /// 清理定时器
        /// </summary>
        /// <param name="method">method为回调函数本身</param>
        public void clearTimer(Handler method)
        {
            clear(method);
        }
        public void clearTimer<T1>(Handler<T1> method)
        {
            clear(method);
        }
        public void clearTimer<T1, T2>(Handler<T1, T2> method)
        {
            clear(method);
        }
        public void clearTimer<T1, T2, T3>(Handler<T1, T2, T3> method)
        {
            clear(method);
        }

        private object create(bool useFrame, bool repeat, int delay, Delegate method, params object[] args)
        {
            if (method == null)
            {
                return null;
            }

            //如果执行时间小于1，直接执行
            if (delay < 1)
            {
                method.DynamicInvoke(args);
                return -1;
            }
            TimerHandler handler;
            if (m_Pool.Count > 0)
            {
                handler = m_Pool[m_Pool.Count - 1];
                m_Pool.Remove(handler);
            }
            else
            {
                handler = new TimerHandler();
            }
            handler.userFrame = useFrame;
            handler.repeat = repeat;
            handler.delay = delay;
            handler.method = method;
            handler.args = args;
            handler.exeTime = delay + (useFrame ? m_CurrFrame : currentTime);
            m_Handlers.Add(handler);
            return method;
        }

        private void clear(Delegate method)
        {
            TimerHandler handler = m_Handlers.FirstOrDefault(t => t.method == method);
            if (handler != null)
            {
                m_Handlers.Remove(handler);
                handler.clear();
                m_Pool.Add(handler);
            }
        }

        /// <summary>
        /// 游戏自启动运行时间，毫秒
        /// </summary>
        long currentTime
        {
            get { return (long)(Time.time * 1000); }
        }

        void advanceTime()
        {
            m_CurrFrame++;
            for (int i = 0; i < m_Handlers.Count; i++)
            {
                TimerHandler handler = m_Handlers[i];
                long t = handler.userFrame ? m_CurrFrame : currentTime;
                if (t >= handler.exeTime)
                {
                    Delegate method = handler.method;
                    object[] args = handler.args;
                    if (handler.repeat)
                    {
                        while (t >= handler.exeTime)
                        {
                            handler.exeTime += handler.delay;
                            method.DynamicInvoke(args);
                        }
                    }
                    else
                    {
                        clear(handler.method);
                        method.DynamicInvoke(args);
                    }
                }
            }
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            advanceTime();
        }

        internal override void Shutdown()
        {
            // 清理所有定时器
            foreach (TimerHandler handler in m_Handlers)
            {
                clear(handler.method);
            }
            m_Handlers.Clear();
            foreach (TimerHandler handler in m_Pool)
            {
                clear(handler.method);
            }
            m_Pool.Clear();
        }

        /**定时处理器*/
        class TimerHandler
        {
            /**执行间隔*/
            public int delay;
            /**是否重复执行*/
            public bool repeat;
            /**是否用帧率*/
            public bool userFrame;

            /**执行时间*/
            public long exeTime;

            /**处理方法*/
            public Delegate method;

            /**参数*/
            public object[] args;

            /**清理*/

            public void clear()
            {
                method = null;
                args = null;
            }
        }
    }
}

