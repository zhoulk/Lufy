// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 17:48:17
// ========================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LF.Fsm
{
    /// <summary>
    /// 状态机管理
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Lufy/Fsm")]
    public sealed class FsmManager : LufyManager
    {
        private readonly Dictionary<string, IFsm> m_Fsms = new Dictionary<string, IFsm>();

        /// <summary>
        /// 检查是否存在有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <returns>是否存在有限状态机。</returns>
        public bool HasFsm<T>() where T : class
        {
            return InternalHasFsm(typeof(T), string.Empty);
        }

        /// <summary>
        /// 检查是否存在有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <returns>是否存在有限状态机。</returns>
        public bool HasFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new LufyException("Owner type is invalid.");
            }

            return InternalHasFsm(ownerType, string.Empty);
        }

        /// <summary>
        /// 检查是否存在有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="name">有限状态机名称。</param>
        /// <returns>是否存在有限状态机。</returns>
        public bool HasFsm<T>(string name) where T : class
        {
            return InternalHasFsm(typeof(T), name);
        }

        /// <summary>
        /// 检查是否存在有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <param name="name">有限状态机名称。</param>
        /// <returns>是否存在有限状态机。</returns>
        public bool HasFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new LufyException("Owner type is invalid.");
            }

            return InternalHasFsm(ownerType, name);
        }

        /// <summary>
        /// 获取有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <returns>要获取的有限状态机。</returns>
        public IFsm GetFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new LufyException("Owner type is invalid.");
            }

            return InternalGetFsm(ownerType, string.Empty);
        }

        /// <summary>
        /// 获取有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <param name="name">有限状态机名称。</param>
        /// <returns>要获取的有限状态机。</returns>
        public IFsm GetFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new LufyException("Owner type is invalid.");
            }

            return InternalGetFsm(ownerType, name);
        }

        /// <summary>
        /// 获取所有有限状态机。
        /// </summary>
        /// <returns>所有有限状态机。</returns>
        public IFsm[] GetAllFsms()
        {
            int index = 0;
            IFsm[] results = new IFsm[m_Fsms.Count];
            foreach (KeyValuePair<string, IFsm> fsm in m_Fsms)
            {
                results[index++] = fsm.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有有限状态机。
        /// </summary>
        /// <param name="results">所有有限状态机。</param>
        public void GetAllFsms(List<IFsm> results)
        {
            if (results == null)
            {
                throw new LufyException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<string, IFsm> fsm in m_Fsms)
            {
                results.Add(fsm.Value);
            }
        }

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="name">有限状态机名称。</param>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>要创建的有限状态机。</returns>
        public IFsm<T> CreateFsm<T>(string name, T owner, params FsmState<T>[] states) where T : class
        {
            string key = Utility.Text.Format("{0}.{1}", typeof(T).FullName, name);
            if (HasFsm<T>(name))
            {
                throw new LufyException(Utility.Text.Format("Already exist FSM '{0}'.", key));
            }

            Fsm<T> fsm = Fsm<T>.Create(name, owner, states);
            m_Fsms.Add(key, fsm);
            return fsm;
        }

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>要创建的有限状态机。</returns>
        public IFsm<T> CreateFsm<T>(T owner, List<FsmState<T>> states) where T : class
        {
            return CreateFsm(string.Empty, owner, states);
        }

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>要创建的有限状态机。</returns>
        public IFsm<T> CreateFsm<T>(T owner, params FsmState<T>[] states) where T : class
        {
            return CreateFsm(string.Empty, owner, states);
        }

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="name">有限状态机名称。</param>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>要创建的有限状态机。</returns>
        public IFsm<T> CreateFsm<T>(string name, T owner, List<FsmState<T>> states) where T : class
        {
            string key = Utility.Text.Format("{0}.{1}", typeof(T).FullName, name);
            if (HasFsm<T>(name))
            {
                throw new LufyException(Utility.Text.Format("Already exist FSM '{0}'.", key));
            }

            Fsm<T> fsm = Fsm<T>.Create(name, owner, states);
            m_Fsms.Add(key, fsm);
            return fsm;
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm<T>() where T : class
        {
            return InternalDestroyFsm(typeof(T), string.Empty);
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new LufyException("Owner type is invalid.");
            }

            return InternalDestroyFsm(ownerType, string.Empty);
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="name">要销毁的有限状态机名称。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm<T>(string name) where T : class
        {
            return InternalDestroyFsm(typeof(T), name);
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="ownerType">有限状态机持有者类型。</param>
        /// <param name="name">要销毁的有限状态机名称。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm(Type ownerType, string name)
        {
            if (ownerType == null)
            {
                throw new LufyException("Owner type is invalid.");
            }

            return InternalDestroyFsm(ownerType, name);
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <typeparam name="T">有限状态机持有者类型。</typeparam>
        /// <param name="fsm">要销毁的有限状态机。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm<T>(IFsm<T> fsm) where T : class
        {
            if (fsm == null)
            {
                throw new LufyException("FSM is invalid.");
            }

            return InternalDestroyFsm(typeof(T), fsm.Name);
        }

        /// <summary>
        /// 销毁有限状态机。
        /// </summary>
        /// <param name="fsm">要销毁的有限状态机。</param>
        /// <returns>是否销毁有限状态机成功。</returns>
        public bool DestroyFsm(IFsm fsm)
        {
            if (fsm == null)
            {
                throw new LufyException("FSM is invalid.");
            }

            return InternalDestroyFsm(fsm.OwnerType, fsm.Name);
        }

        private bool InternalHasFsm(Type ownerType, string name)
        {
            string key = Utility.Text.Format("{0}.{1}", ownerType.FullName, name);
            return m_Fsms.ContainsKey(key);
        }

        private IFsm InternalGetFsm(Type ownerType, string name)
        {
            string key = Utility.Text.Format("{0}.{1}", ownerType.FullName, name);
            IFsm fsm = null;
            if (m_Fsms.TryGetValue(key, out fsm))
            {
                return fsm;
            }

            return null;
        }

        private bool InternalDestroyFsm(Type ownerType, string name)
        {
            string key = Utility.Text.Format("{0}.{1}", ownerType.FullName, name);
            IFsm fsm = null;
            if (m_Fsms.TryGetValue(key, out fsm))
            {
                fsm.Shutdown();
                return m_Fsms.Remove(key);
            }

            return false;
        }

        /// <summary>
        /// 有限状态机管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (m_Fsms.Count <= 0)
            {
                return;
            }

            foreach (var kv in m_Fsms)
            {
                if (kv.Value.IsDestroyed)
                {
                    continue;
                }

                kv.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        internal override void Shutdown()
        {
            foreach (var fsm in m_Fsms)
            {
                fsm.Value.Shutdown();
            }

            m_Fsms.Clear();
        }
    }
}

