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
        private readonly List<Entity> m_Entities;
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
            m_Entities = new List<Entity>();
        }

        public EntityInstanceObject SpawnEntityInstanceObject(string name)
        {
            return m_InstancePool.Spawn(name);
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
            m_Entities.Add(entity);
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

