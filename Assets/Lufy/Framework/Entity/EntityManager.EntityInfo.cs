// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-10 10:30:50
// ========================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LF.Entity
{
    public partial class EntityManager : LufyManager
    {
        private sealed class EntityInfo : IReference
        {
            private Entity m_Entity;
            private EntityStatus m_Status;
            private Entity m_ParentEntity;
            private List<Entity> m_ChildEntities;

            public EntityInfo()
            {
                m_Entity = null;
                m_Status = EntityStatus.Unknown;
                m_ParentEntity = null;
                m_ChildEntities = new List<Entity>();
            }

            public Entity Entity
            {
                get
                {
                    return m_Entity;
                }
            }

            public EntityStatus Status
            {
                get
                {
                    return m_Status;
                }
                set
                {
                    m_Status = value;
                }
            }

            public Entity ParentEntity
            {
                get
                {
                    return m_ParentEntity;
                }
                set
                {
                    m_ParentEntity = value;
                }
            }

            public static EntityInfo Create(Entity entity)
            {
                if (entity == null)
                {
                    throw new LufyException("Entity is invalid.");
                }

                EntityInfo entityInfo = ReferencePool.Acquire<EntityInfo>();
                entityInfo.m_Entity = entity;
                entityInfo.m_Status = EntityStatus.WillInit;
                return entityInfo;
            }

            public void Clear()
            {
                m_Entity = null;
                m_Status = EntityStatus.Unknown;
                m_ParentEntity = null;
                m_ChildEntities.Clear();
            }

            public Entity[] GetChildEntities()
            {
                return m_ChildEntities.ToArray();
            }

            public void GetChildEntities(List<Entity> results)
            {
                if (results == null)
                {
                    throw new LufyException("Results is invalid.");
                }

                results.Clear();
                foreach (Entity childEntity in m_ChildEntities)
                {
                    results.Add(childEntity);
                }
            }

            public void AddChildEntity(Entity childEntity)
            {
                if (m_ChildEntities.Contains(childEntity))
                {
                    throw new LufyException("Can not add child entity which is already exist.");
                }

                m_ChildEntities.Add(childEntity);
            }

            public void RemoveChildEntity(Entity childEntity)
            {
                if (!m_ChildEntities.Remove(childEntity))
                {
                    throw new LufyException("Can not remove child entity which is not exist.");
                }
            }
        }
    }
}
