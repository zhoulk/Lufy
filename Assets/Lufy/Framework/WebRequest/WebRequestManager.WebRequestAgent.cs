// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-11-06 14:54:34
// ========================================================

using LF.Event;
using UnityEngine.Networking;

namespace LF.WebRequest
{
    public partial class WebRequestManager : LufyManager
    {
        private sealed class WebRequestAgent : ITaskAgent<WebRequestTask>
        {
            private EventManager m_EventManager = null;

            private UnityWebRequest m_UnityWebRequest = null;
            private WebRequestTask m_Task;
            private float m_WaitTime;

            public WebRequestTask Task
            {
                get
                {
                    return m_Task;
                }
            }

            /// <summary>
            /// 获取已经等待时间。
            /// </summary>
            public float WaitTime
            {
                get
                {
                    return m_WaitTime;
                }
            }

            public void SetEventManager(EventManager eventManager)
            {
                m_EventManager = eventManager;
            }

            public void Initialize()
            {
                
            }

            public void Reset()
            {
                throw new System.NotImplementedException();
            }

            public void Shutdown()
            {
                throw new System.NotImplementedException();
            }

            public StartTaskStatus Start(WebRequestTask task)
            {
                if (task == null)
                {
                    throw new LufyException("Task is invalid.");
                }

                m_Task = task;
                m_Task.Status = WebRequestTaskStatus.Doing;

                if (m_EventManager != null)
                {
                    m_EventManager.Fire(this, WebRequestStartEventArgs.Create(m_Task.SerialId, m_Task.WebRequestUri, m_Task.UserData));
                }

                byte[] postData = m_Task.GetPostData();
                if (postData == null)
                {
                    m_UnityWebRequest = UnityWebRequest.Get(m_Task.WebRequestUri);
                    m_UnityWebRequest.SendWebRequest();
                }
                else
                {
                    m_UnityWebRequest = UnityWebRequest.Post(m_Task.WebRequestUri, Utility.Converter.GetString(postData));
                    m_UnityWebRequest.SendWebRequest();
                }

                m_WaitTime = 0f;
                return StartTaskStatus.CanResume;
            }

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (m_Task.Status == WebRequestTaskStatus.Doing)
                {
                    m_WaitTime += realElapseSeconds;
                    if (m_WaitTime >= m_Task.Timeout)
                    {
                        OnWebRequestAgentError(this, "timeOut");
                    }
                }
            }

            private void OnWebRequestAgentError(WebRequestAgent sender, string errMsg)
            {
                m_Task.Status = WebRequestTaskStatus.Error;
                m_Task.Done = true;

                if(m_EventManager != null)
                {
                    m_EventManager.Fire(this, WebRequestFailureEventArgs.Create(sender.Task.SerialId, sender.Task.WebRequestUri, errMsg, sender.Task.UserData));
                }
            }
        }
    }
}

