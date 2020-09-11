// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-11 11:11:28
// ========================================================
using System;

namespace LF.Entity
{
    public class ShowEntityInfo : IReference
    {
        private int m_SerialId;
        private int m_EntityId;
        private EntityGroup m_EntityGroup;
        private Type m_EntityLogicType;
        private object m_UserData;

        public ShowEntityInfo()
        {
            m_SerialId = 0;
            m_EntityId = 0;
            m_EntityGroup = null;
            m_UserData = null;
            m_EntityLogicType = null;
        }

        public int SerialId
        {
            get
            {
                return m_SerialId;
            }
        }

        public int EntityId
        {
            get
            {
                return m_EntityId;
            }
        }

        public EntityGroup EntityGroup
        {
            get
            {
                return m_EntityGroup;
            }
        }

        public Type EntityLogicType
        {
            get
            {
                return m_EntityLogicType;
            }
            set
            {
                m_EntityLogicType = value;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public static ShowEntityInfo Create(int serialId, int entityId, EntityGroup entityGroup, Type logicType, object userData)
        {
            ShowEntityInfo showEntityInfo = ReferencePool.Acquire<ShowEntityInfo>();
            showEntityInfo.m_SerialId = serialId;
            showEntityInfo.m_EntityId = entityId;
            showEntityInfo.m_EntityGroup = entityGroup;
            showEntityInfo.m_UserData = userData;
            showEntityInfo.m_EntityLogicType = logicType;
            return showEntityInfo;
        }

        public void Clear()
        {
            m_SerialId = 0;
            m_EntityId = 0;
            m_EntityGroup = null;
            m_UserData = null;
            m_EntityLogicType = null;
        }
    }
}

