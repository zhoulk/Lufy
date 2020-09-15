// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-10 19:56:26
// ========================================================
using System;
using UnityEngine;

namespace LF.Entity
{
    public class Entity : MonoBehaviour
    {
        private int m_Id;
        private string m_EntityAssetName;
        private EntityGroup m_EntityGroup;
        private EntityLogic m_EntityLogic;

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName
        {
            get
            {
                return m_EntityAssetName;
            }
        }

        /// <summary>
        /// 获取实体所属的实体组。
        /// </summary>
        public EntityGroup EntityGroup
        {
            get
            {
                return m_EntityGroup;
            }
        }

        /// <summary>
        /// 获取实体实例。
        /// </summary>
        public object Handle
        {
            get
            {
                return gameObject;
            }
        }

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="isNewInstance">是否是新实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnInit(int entityId, string entityAssetName, EntityGroup entityGroup, bool isNewInstance, Type logicType, object userData)
        {
            m_Id = entityId;
            m_EntityAssetName = entityAssetName;
            if (isNewInstance)
            {
                m_EntityGroup = entityGroup;
            }
            else if (m_EntityGroup != entityGroup)
            {
                Log.Error("Entity group is inconsistent for non-new-instance entity.");
                return;
            }

            if (logicType == null)
            {
                Log.Error("Entity logic type is invalid.");
                return;
            }

            if (m_EntityLogic != null)
            {
                if (m_EntityLogic.GetType() == logicType)
                {
                    m_EntityLogic.enabled = true;
                    return;
                }

                Destroy(m_EntityLogic);
                m_EntityLogic = null;
            }

            m_EntityLogic = gameObject.AddComponent(logicType) as EntityLogic;
            if (m_EntityLogic == null)
            {
                Log.Error("Entity '{0}' can not add entity logic.", entityAssetName);
                return;
            }

            m_EntityLogic.OnInit(userData);
        }

        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void OnShow(object userData)
        {
            m_EntityLogic.OnShow(userData);
        }

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnHide(bool isShutdown, object userData)
        {
            m_EntityLogic.OnHide(isShutdown, userData);
        }

        /// <summary>
        /// 实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            m_EntityLogic.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 实体回收。
        /// </summary>
        public void OnRecycle()
        {
            m_EntityLogic.OnRecycle();
            m_EntityLogic.enabled = false;

            m_Id = 0;
        }
    }
}

