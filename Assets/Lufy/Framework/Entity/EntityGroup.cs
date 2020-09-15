// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-11 10:25:24
// ========================================================
using LF.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace LF.Entity
{
    public class EntityGroup
    {
        private readonly string m_Name;
        private readonly IObjectPool<EntityInstanceObject> m_InstancePool;
        private readonly LinkedList<Entity> m_Entities;
        private LinkedListNode<Entity> m_CachedNode;

        private GameObject m_RootInstance = null;

        public EntityGroup(string name, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime, GameObject rootInstance, ObjectPoolManager objectPoolManager)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new LufyException("Entity group name is invalid.");
            }

            m_Name = name;
            m_RootInstance = rootInstance;
            m_InstancePool = objectPoolManager.CreateSingleSpawnObjectPool<EntityInstanceObject>(Utility.Text.Format("Entity Instance Pool ({0})", name), instanceCapacity, instanceExpireTime);
            m_InstancePool.AutoReleaseInterval = instanceAutoReleaseInterval;
            m_Entities = new LinkedList<Entity>();
            m_CachedNode = null;
        }

        public EntityInstanceObject SpawnEntityInstanceObject(string name)
        {
            return m_InstancePool.Spawn(name);
        }

        public void UnspawnEntity(Entity entity)
        {
            m_InstancePool.Unspawn(entity.Handle);
        }

        public void RegisterEntityInstanceObject(EntityInstanceObject obj, bool spawned)
        {
            m_InstancePool.Register(obj, spawned);
        }

        /// <summary>
        /// 往实体组增加实体。
        /// </summary>
        /// <param name="entity">要增加的实体。</param>
        public void AddEntity(Entity entity)
        {
            m_Entities.AddLast(entity);
        }

        /// <summary>
        /// 从实体组移除实体。
        /// </summary>
        /// <param name="entity">要移除的实体。</param>
        public void RemoveEntity(Entity entity)
        {
            if (m_CachedNode != null && m_CachedNode.Value == entity)
            {
                m_CachedNode = m_CachedNode.Next;
            }

            m_Entities.Remove(entity);
        }

        /// <summary>
        /// 实体组轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            LinkedListNode<Entity> current = m_Entities.First;
            while (current != null)
            {
                m_CachedNode = current.Next;
                current.Value.OnUpdate(elapseSeconds, realElapseSeconds);
                current = m_CachedNode;
                m_CachedNode = null;
            }
        }

        public GameObject RootInstance
        {
            get
            {
                return m_RootInstance;
            }
        }
    }
}

