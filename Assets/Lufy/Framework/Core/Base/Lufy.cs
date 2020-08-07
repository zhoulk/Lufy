// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 11:37:47
// ========================================================
using System;
using System.Collections.Generic;

namespace LF
{
    /// <summary>
    /// 程序入口类
    /// </summary>
    public static class Lufy
    {
        private static readonly LinkedList<LufyManager> s_LufyManagers = new LinkedList<LufyManager>();

        /// <summary>
        /// 获取游戏框架组件。
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架组件类型。</typeparam>
        /// <returns>要获取的游戏框架组件。</returns>
        public static T GetManager<T>() where T : LufyManager
        {
            return (T)GetManager(typeof(T));
        }

        /// <summary>
        /// 获取游戏框架组件。
        /// </summary>
        /// <param name="type">要获取的游戏框架组件类型。</param>
        /// <returns>要获取的游戏框架组件。</returns>
        public static LufyManager GetManager(Type type)
        {
            LinkedListNode<LufyManager> current = s_LufyManagers.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    return current.Value;
                }

                current = current.Next;
            }

            return null;
        }

        /// <summary>
        /// 注册游戏框架组件。
        /// </summary>
        /// <param name="manager">要注册的游戏管理组件。</param>
        internal static void RegisterManager(LufyManager manager)
        {
            if (manager == null)
            {
                Log.Error("Game Framework component is invalid.");
                return;
            }

            Type type = manager.GetType();

            LinkedListNode<LufyManager> current = s_LufyManagers.First;
            while (current != null)
            {
                if (current.Value.GetType() == type)
                {
                    Log.Error("Game Framework component type '{0}' is already exist.", type.FullName);
                    return;
                }

                current = current.Next;
            }

            s_LufyManagers.AddLast(manager);
        }

        /// <summary>
        /// 轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        internal static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach(var m in s_LufyManagers)
            {
                m.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }
    }
}

