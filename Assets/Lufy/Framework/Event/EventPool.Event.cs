﻿// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 11:54:28
// ========================================================

namespace LF.Event
{
    internal sealed partial class EventPool<T> where T : GameEventArgs
    {
        /// <summary>
        /// 事件结点。
        /// </summary>
        private sealed class Event : IReference
        {
            private object m_Sender;
            private T m_EventArgs;

            public Event()
            {
                m_Sender = null;
                m_EventArgs = null;
            }

            public object Sender
            {
                get
                {
                    return m_Sender;
                }
            }

            public T EventArgs
            {
                get
                {
                    return m_EventArgs;
                }
            }

            public static Event Create(object sender, T e)
            {
                Event eventNode = ReferencePool.Acquire<Event>();
                eventNode.m_Sender = sender;
                eventNode.m_EventArgs = e;
                return eventNode;
            }

            public void Clear()
            {
                m_Sender = null;
                m_EventArgs = null;
            }
        }
    }
}
