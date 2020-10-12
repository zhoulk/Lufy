// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-10 17:31:38
// ========================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UIEvent 管理
/// </summary>
/// 
namespace LF.UINavi
{
    public delegate void OnClick(GameObject obj);

    [DisallowMultipleComponent]
    [AddComponentMenu("Lufy/UIEvent")]
    public class UIEventManager : LufyManager
    {
        Dictionary<GameObject, OnClick> m_ClickHandlerDic = new Dictionary<GameObject, OnClick>();

        GameObject m_SelectedGameObject;
        public GameObject SelectedGameObject
        {
            get
            {
                return m_SelectedGameObject;
            }
            set
            {
                m_SelectedGameObject = value;
            }
        }

        bool isPause = false;

        private IUIInput m_UIInput = null;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="input"></param>
        public void Initialize(IUIInput input)
        {
            m_UIInput = input;
        }

        /// <summary>
        /// 添加点击事件处理
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="onClick"></param>
        public void AddOnClickHandler(GameObject obj, OnClick onClick)
        {
            if (m_ClickHandlerDic.ContainsKey(obj))
            {
                m_ClickHandlerDic.Remove(obj);
            }
            m_ClickHandlerDic.Add(obj, onClick);
            UIEventLisner lisner = obj.AddComponent<UIEventLisner>();
            lisner.PointerClickHandler += OnPointerClickHandler;
        }

        /// <summary>
        /// 移除点击事件处理
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveClickHandler(GameObject obj)
        {
            if (m_ClickHandlerDic.ContainsKey(obj))
            {
                m_ClickHandlerDic.Remove(obj);
            }
            UIEventLisner lisner = obj.GetComponent<UIEventLisner>();
            lisner.PointerClickHandler -= OnPointerClickHandler;
        }

        void OnPointerClickHandler(PointerEventData data)
        {
            if (data.pointerPress)
            {
                if (m_ClickHandlerDic.ContainsKey(data.pointerPress))
                {
                    m_ClickHandlerDic[data.pointerPress](data.pointerPress);
                }
            }
        }

        public void OnUpdate()
        {
            if (isPause)
            {
                return;
            }

            if(m_UIInput == null)
            {
                return;
            }

            if (m_SelectedGameObject == null)
            {
                //m_SelectedGameObject = UINaviManager.Instance.DefaultObject;
                //m_SelectedGameObject.AddOutLine();
                return;
            }
            if (m_UIInput.ClickUpDown())
            {
                if (m_SelectedGameObject && m_SelectedGameObject.GetUpNavi())
                {
                    if (m_SelectedGameObject.GetUnSelectedAction() != null)
                    {
                        m_SelectedGameObject.GetUnSelectedAction()(m_SelectedGameObject);
                    }
                    m_SelectedGameObject = m_SelectedGameObject.GetUpNavi();
                    if (m_SelectedGameObject.GetSelectedAction() != null)
                    {
                        m_SelectedGameObject.GetSelectedAction()(m_SelectedGameObject);
                    }
                }
            }
            else if (m_UIInput.ClickLeftDown())
            {
                if (m_SelectedGameObject && m_SelectedGameObject.GetLeftNavi())
                {
                    if (m_SelectedGameObject.GetUnSelectedAction() != null)
                    {
                        m_SelectedGameObject.GetUnSelectedAction()(m_SelectedGameObject);
                    }
                    m_SelectedGameObject = m_SelectedGameObject.GetLeftNavi();
                    if (m_SelectedGameObject.GetSelectedAction() != null)
                    {
                        m_SelectedGameObject.GetSelectedAction()(m_SelectedGameObject);
                    }
                }
            }
            else if (m_UIInput.ClickRightDown())
            {
                if (m_SelectedGameObject && m_SelectedGameObject.GetRightNavi())
                {
                    if (m_SelectedGameObject.GetUnSelectedAction() != null)
                    {
                        m_SelectedGameObject.GetUnSelectedAction()(m_SelectedGameObject);
                    }
                    m_SelectedGameObject = m_SelectedGameObject.GetRightNavi();
                    if (m_SelectedGameObject.GetSelectedAction() != null)
                    {
                        m_SelectedGameObject.GetSelectedAction()(m_SelectedGameObject);
                    }
                }
            }
            else if (m_UIInput.ClickDownDown())
            {
                if (m_SelectedGameObject && m_SelectedGameObject.GetDownNavi())
                {
                    if (m_SelectedGameObject.GetUnSelectedAction() != null)
                    {
                        m_SelectedGameObject.GetUnSelectedAction()(m_SelectedGameObject);
                    }
                    m_SelectedGameObject = m_SelectedGameObject.GetDownNavi();
                    if (m_SelectedGameObject.GetSelectedAction() != null)
                    {
                        m_SelectedGameObject.GetSelectedAction()(m_SelectedGameObject);
                    }
                }
            }
            else if (m_UIInput.ClickEnterDown())
            {
                //Debug.Log("KeyCode2.A  click" + Time.frameCount);
                if (m_SelectedGameObject)
                {
                    if (m_ClickHandlerDic.ContainsKey(m_SelectedGameObject))
                    {
                        m_ClickHandlerDic[m_SelectedGameObject](m_SelectedGameObject);
                    }
                }
            }
        }

        public void Pause()
        {
            isPause = true;
        }

        public void Resume()
        {
            isPause = false;
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            OnUpdate();
        }

        internal override void Shutdown()
        {
            m_SelectedGameObject = null;
            m_ClickHandlerDic.Clear();
        }
    }
}
