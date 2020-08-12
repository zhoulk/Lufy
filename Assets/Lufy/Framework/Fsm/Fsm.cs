// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 19:49:04
// ========================================================
using System;
using System.Collections.Generic;

namespace LF.Fsm
{
    internal sealed class Fsm<T> : IFsm<T> where T : class
    {
        private string m_Name;
        private T m_Owner;
        private readonly Dictionary<Type, FsmState<T>> m_States;
        private readonly Dictionary<string, object> m_Datas;
        private FsmState<T> m_CurrentState;
        private float m_CurrentStateTime;
        private bool m_IsDestroyed;

        /// <summary>
        /// 初始化有限状态机的新实例。
        /// </summary>
        public Fsm()
        {
            m_Name = string.Empty;
            m_Owner = null;
            m_States = new Dictionary<Type, FsmState<T>>();
            m_Datas = new Dictionary<string, object>();
            m_CurrentState = null;
            m_CurrentStateTime = 0;
            m_IsDestroyed = true;
        }

        public FsmState<T> CurrentState => m_CurrentState;

        /// <summary>
        /// 获取有限状态机名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value ?? string.Empty;
            }
        }

        /// <summary>
        /// 获取有限状态机完整名称。
        /// </summary>
        public string FullName
        {
            get
            {
                return Utility.Text.Format("{0}.{1}", OwnerType.FullName, m_Name);
            }
        }

        /// <summary>
        /// 获取有限状态机持有者。
        /// </summary>
        public T Owner
        {
            get
            {
                return m_Owner;
            }
        }

        /// <summary>
        /// 获取有限状态机持有者类型。
        /// </summary>
        public Type OwnerType
        {
            get
            {
                return typeof(T);
            }
        }

        public int FsmStateCount => m_States.Count;

        public bool IsRunning => m_CurrentState != null;

        public bool IsDestroyed => m_IsDestroyed;

        /// <summary>
        /// 获取当前有限状态机状态名称。
        /// </summary>
        public string CurrentStateName
        {
            get
            {
                return m_CurrentState != null ? m_CurrentState.GetType().FullName : null;
            }
        }

        /// <summary>
        /// 获取当前有限状态机状态持续时间。
        /// </summary>
        public float CurrentStateTime
        {
            get
            {
                return m_CurrentStateTime;
            }
        }

        public FsmState<T>[] GetAllStates()
        {
            int index = 0;
            FsmState<T>[] results = new FsmState<T>[m_States.Count];
            foreach (KeyValuePair<Type, FsmState<T>> state in m_States)
            {
                results[index++] = state.Value;
            }

            return results;
        }

        public void GetAllStates(List<FsmState<T>> results)
        {
            if (results == null)
            {
                throw new LufyException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<Type, FsmState<T>> state in m_States)
            {
                results.Add(state.Value);
            }
        }

        public bool HasState<TState>() where TState : FsmState<T>
        {
            return m_States.ContainsKey(typeof(TState));
        }

        public bool HasState(Type stateType)
        {
            if (stateType == null)
            {
                throw new LufyException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new LufyException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            return m_States.ContainsKey(stateType);
        }

        public TState GetState<TState>() where TState : FsmState<T>
        {
            FsmState<T> state = null;
            if (m_States.TryGetValue(typeof(TState), out state))
            {
                return (TState)state;
            }

            return null;
        }

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        /// <param name="stateType">要获取的有限状态机状态类型。</param>
        /// <returns>要获取的有限状态机状态。</returns>
        public FsmState<T> GetState(Type stateType)
        {
            if (stateType == null)
            {
                throw new LufyException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new LufyException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            FsmState<T> state = null;
            if (m_States.TryGetValue(stateType, out state))
            {
                return state;
            }

            return null;
        }

        public void Start<TState>() where TState : FsmState<T>
        {
            if (IsRunning)
            {
                throw new LufyException("FSM is running, can not start again.");
            }

            FsmState<T> state = GetState<TState>();
            if (state == null)
            {
                throw new LufyException(Utility.Text.Format("FSM '{0}.{1}' can not start state '{2}' which is not exist.", typeof(T).FullName, Name, typeof(TState).FullName));
            }

            m_CurrentStateTime = 0;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new LufyException("FSM is running, can not start again.");
            }

            if (stateType == null)
            {
                throw new LufyException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new LufyException(Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new LufyException(Utility.Text.Format("FSM '{0}.{1}' can not start state '{2}' which is not exist.", typeof(T).FullName, Name, stateType.FullName));
            }

            m_CurrentStateTime = 0;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        /// <summary>
        /// 清理有限状态机。
        /// </summary>
        public void Clear()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnLeave(this, true);
            }

            foreach (KeyValuePair<Type, FsmState<T>> state in m_States)
            {
                state.Value.OnDestroy(this);
            }

            Name = null;
            m_Owner = null;
            m_States.Clear();
            m_Datas.Clear();
            m_CurrentState = null;
            m_CurrentStateTime = 0f;
            m_IsDestroyed = true;
        }

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <param name="name">有限状态机名称。</param>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>创建的有限状态机。</returns>
        public static Fsm<T> Create(string name, T owner, params FsmState<T>[] states)
        {
            if (owner == null)
            {
                throw new LufyException("FSM owner is invalid.");
            }

            if (states == null || states.Length < 1)
            {
                throw new LufyException("FSM states is invalid.");
            }

            Fsm<T> fsm = new Fsm<T>();
            fsm.Name = name;
            fsm.m_Owner = owner;
            fsm.m_IsDestroyed = false;
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new LufyException("FSM states is invalid.");
                }

                Type stateType = state.GetType();
                if (fsm.m_States.ContainsKey(stateType))
                {
                    throw new LufyException(Utility.Text.Format("FSM '{0}.{1}' state '{2}' is already exist.", typeof(T).FullName, name, stateType));
                }

                fsm.m_States.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        /// <summary>
        /// 创建有限状态机。
        /// </summary>
        /// <param name="name">有限状态机名称。</param>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        /// <returns>创建的有限状态机。</returns>
        public static Fsm<T> Create(string name, T owner, List<FsmState<T>> states)
        {
            if (owner == null)
            {
                throw new LufyException("FSM owner is invalid.");
            }

            if (states == null || states.Count < 1)
            {
                throw new LufyException("FSM states is invalid.");
            }

            Fsm<T> fsm = new Fsm<T>();
            fsm.Name = name;
            fsm.m_Owner = owner;
            fsm.m_IsDestroyed = false;
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new LufyException("FSM states is invalid.");
                }

                Type stateType = state.GetType();
                if (fsm.m_States.ContainsKey(stateType))
                {
                    throw new LufyException(Utility.Text.Format("FSM '{0}.{1}' state '{2}' is already exist.", typeof(T).FullName, name, stateType));
                }

                fsm.m_States.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        /// <summary>
        /// 有限状态机轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_CurrentState == null)
            {
                return;
            }

            m_CurrentStateTime += elapseSeconds;
            m_CurrentState.OnUpdate(this, elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理有限状态机。
        /// </summary>
        public void Shutdown()
        {
            //ReferencePool.Release(this);
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要切换到的有限状态机状态类型。</typeparam>
        public void ChangeState<TState>() where TState : FsmState<T>
        {
            ChangeState(typeof(TState));
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <param name="stateType">要切换到的有限状态机状态类型。</param>
        public void ChangeState(Type stateType)
        {
            if (m_CurrentState == null)
            {
                throw new LufyException("Current state is invalid.");
            }

            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new LufyException(Utility.Text.Format("FSM '{0}.{1}' can not change state to '{2}' which is not exist.", typeof(T).FullName, Name, stateType.FullName));
            }

            m_CurrentState.OnLeave(this, false);
            m_CurrentStateTime = 0;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        public bool HasData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new LufyException("Data name is invalid.");
            }

            return m_Datas.ContainsKey(name);
        }

        public TData GetData<TData>(string name)
        {
            return (TData)GetData(name);
        }

        public object GetData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new LufyException("Data name is invalid.");
            }

            object data = null;
            if (m_Datas.TryGetValue(name, out data))
            {
                return data;
            }

            return null;
        }

        public void SetData<TData>(string name, TData data)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new LufyException("Data name is invalid.");
            }

            m_Datas[name] = data;
        }

        public void SetData(string name, object data)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new LufyException("Data name is invalid.");
            }

            m_Datas[name] = data;
        }

        public bool RemoveData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new LufyException("Data name is invalid.");
            }

            return m_Datas.Remove(name);
        }
    }
}

