// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-11-06 15:53:53
// ========================================================

using System.Collections.Generic;

namespace LF
{
    /// <summary>
    /// 任务池。
    /// </summary>
    /// <typeparam name="T">任务类型。</typeparam>
    public class TaskPool<T> where T : TaskBase
    {
        private ITaskAgent<T> m_Agent;
        private readonly LinkedList<T> m_WaitingTasks;
        private bool m_Paused;

        /// <summary>
        /// 初始化任务池的新实例。
        /// </summary>
        public TaskPool()
        {
            m_WaitingTasks = new LinkedList<T>();
            m_Paused = false;
        }

        /// <summary>
        /// 获取或设置任务池是否被暂停。
        /// </summary>
        public bool Paused
        {
            get
            {
                return m_Paused;
            }
            set
            {
                m_Paused = value;
            }
        }

        /// <summary>
        /// 获取等待任务数量。
        /// </summary>
        public int WaitingTaskCount
        {
            get
            {
                return m_WaitingTasks.Count;
            }
        }

        /// <summary>
        /// 任务池轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_Paused)
            {
                return;
            }

            ProcessRunningTasks(elapseSeconds, realElapseSeconds);
            ProcessWaitingTasks(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理任务池。
        /// </summary>
        public void Shutdown()
        {
            RemoveAllTasks();
        }

        /// <summary>
        /// 设置任务代理。
        /// </summary>
        /// <param name="agent">要增加的任务代理。</param>
        public void SetAgent(ITaskAgent<T> agent)
        {
            if (agent == null)
            {
                throw new LufyException("Task agent is invalid.");
            }

            agent.Initialize();
            m_Agent = agent;
        }

        /// <summary>
        /// 增加任务。
        /// </summary>
        /// <param name="task">要增加的任务。</param>
        public void AddTask(T task)
        {
            m_WaitingTasks.AddFirst(task);
        }

        /// <summary>
        /// 移除任务。
        /// </summary>
        /// <param name="serialId">要移除任务的序列编号。</param>
        /// <returns>移除任务是否成功。</returns>
        public bool RemoveTask(int serialId)
        {
            foreach (T task in m_WaitingTasks)
            {
                if (task.SerialId == serialId)
                {
                    m_WaitingTasks.Remove(task);
                    ReferencePool.Release(task);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 移除所有任务。
        /// </summary>
        public void RemoveAllTasks()
        {
            foreach (T task in m_WaitingTasks)
            {
                ReferencePool.Release(task);
            }

            m_WaitingTasks.Clear();
        }

        public TaskInfo[] GetAllTaskInfos()
        {
            List<TaskInfo> results = new List<TaskInfo>();

            foreach (T waitingTask in m_WaitingTasks)
            {
                results.Add(new TaskInfo(waitingTask.SerialId, 0, TaskStatus.Todo, waitingTask.Description));
            }

            return results.ToArray();
        }

        private void ProcessRunningTasks(float elapseSeconds, float realElapseSeconds)
        {
            if (m_Agent != null)
            {
                T task = m_Agent.Task;
                if (!task.Done)
                {
                    m_Agent.Update(elapseSeconds, realElapseSeconds);
                    return;
                }

                m_Agent.Reset();
                ReferencePool.Release(task);
            }
        }

        private void ProcessWaitingTasks(float elapseSeconds, float realElapseSeconds)
        {
            LinkedListNode<T> current = m_WaitingTasks.First;
            while (current != null && m_Agent != null)
            {
                T task = current.Value;
                LinkedListNode<T> next = current.Next;
                StartTaskStatus status = m_Agent.Start(task);
                if (status == StartTaskStatus.Done || status == StartTaskStatus.HasToWait || status == StartTaskStatus.UnknownError)
                {
                    m_Agent.Reset();
                }

                if (status == StartTaskStatus.Done || status == StartTaskStatus.CanResume || status == StartTaskStatus.UnknownError)
                {
                    m_WaitingTasks.Remove(current);
                }

                if (status == StartTaskStatus.Done || status == StartTaskStatus.UnknownError)
                {
                    ReferencePool.Release(task);
                }

                current = next;
            }
        }
    }
}

