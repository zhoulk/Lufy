// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-10 10:25:14
// ========================================================
using LF;
using LF.Pool;
using LF.Res;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LF.Entity
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Lufy/Entity")]
    public partial class EntityManager : LufyManager
    {
        [SerializeField]
        private EntityGroupInfo[] m_EntityGroupInfos = null;

        private readonly Dictionary<int, EntityInfo> m_EntityInfos;
        private readonly Dictionary<string, EntityGroup> m_EntityGroups;
        private readonly Dictionary<int, int> m_EntitiesBeingLoaded;
        private readonly HashSet<int> m_EntitiesToReleaseOnLoad;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private int m_Serial;
        private bool m_IsShutdown;
        private readonly Queue<EntityInfo> m_RecycleQueue;

        private IResManager m_ResourceManager;
        private ObjectPoolManager m_ObjectPoolManager;

        public EntityManager()
        {
            m_EntityInfos = new Dictionary<int, EntityInfo>();
            m_EntityGroups = new Dictionary<string, EntityGroup>();
            m_EntitiesBeingLoaded = new Dictionary<int, int>();
            m_EntitiesToReleaseOnLoad = new HashSet<int>();
            m_RecycleQueue = new Queue<EntityInfo>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadEntitySuccessCallback, LoadEntityFailureCallback, LoadEntityUpdateCallback, LoadEntityDependencyAssetCallback);
            m_Serial = 0;
            m_IsShutdown = false;
            m_ResourceManager = null;
            m_ObjectPoolManager = null;
        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IResManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new LufyException("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置对象池管理器。
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器。</param>
        public void SetObjectPoolManager(ObjectPoolManager objectPoolManager)
        {
            if (objectPoolManager == null)
            {
                throw new LufyException("Object pool manager is invalid.");
            }

            m_ObjectPoolManager = objectPoolManager;
        }

        private void Start()
        {
            for (int i = 0; i < m_EntityGroupInfos.Length; i++)
            {
                if (!AddEntityGroup(m_EntityGroupInfos[i].Name, m_EntityGroupInfos[i].InstanceAutoReleaseInterval, m_EntityGroupInfos[i].InstanceCapacity, m_EntityGroupInfos[i].InstanceExpireTime))
                {
                    Log.Error("Add entity group '{0}' failure.", m_EntityGroupInfos[i].Name);
                    continue;
                }
            }
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            while (m_RecycleQueue.Count > 0)
            {
                EntityInfo entityInfo = m_RecycleQueue.Dequeue();
                Entity entity = entityInfo.Entity;
                EntityGroup entityGroup = (EntityGroup)entity.EntityGroup;
                if (entityGroup == null)
                {
                    throw new LufyException("Entity group is invalid.");
                }

                entityInfo.Status = EntityStatus.WillRecycle;
                entity.OnRecycle();
                entityInfo.Status = EntityStatus.Recycled;
                entityGroup.UnspawnEntity(entity);
                ReferencePool.Release(entityInfo);
            }

            foreach (KeyValuePair<string, EntityGroup> entityGroup in m_EntityGroups)
            {
                entityGroup.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        internal override void Shutdown()
        {
            m_IsShutdown = true;
            m_EntityGroups.Clear();
            m_EntitiesBeingLoaded.Clear();
            m_EntitiesToReleaseOnLoad.Clear();
            m_RecycleQueue.Clear();
        }

        /// <summary>
        /// 增加实体组。
        /// </summary>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <param name="instanceAutoReleaseInterval">实体实例对象池自动释放可释放对象的间隔秒数。</param>
        /// <param name="instanceCapacity">实体实例对象池容量。</param>
        /// <param name="instanceExpireTime">实体实例对象池对象过期秒数。</param>
        /// <returns>是否增加实体组成功。</returns>
        public bool AddEntityGroup(string entityGroupName, float instanceAutoReleaseInterval, int instanceCapacity, float instanceExpireTime)
        {
            if (string.IsNullOrEmpty(entityGroupName))
            {
                throw new LufyException("Entity group name is invalid.");
            }

            if (m_ObjectPoolManager == null)
            {
                throw new LufyException("You must set object pool manager first.");
            }

            if (HasEntityGroup(entityGroupName))
            {
                return false;
            }

            GameObject groupInstance = new GameObject();
            groupInstance.name = Utility.Text.Format("Entity Group - {0}", entityGroupName);
            groupInstance.transform.SetParent(transform);
            groupInstance.transform.localScale = Vector3.one;

            m_EntityGroups.Add(entityGroupName, new EntityGroup(entityGroupName, instanceAutoReleaseInterval, instanceCapacity, instanceExpireTime, groupInstance, m_ObjectPoolManager));
            return true;
        }

        /// <summary>
        /// 显示实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ShowEntity(int entityId, string entityAssetName, string entityGroupName, Type logicType, object userData)
        {
            if (m_ResourceManager == null)
            {
                throw new LufyException("You must set resource manager first.");
            }

            if (string.IsNullOrEmpty(entityAssetName))
            {
                throw new LufyException("Entity asset name is invalid.");
            }

            if (string.IsNullOrEmpty(entityGroupName))
            {
                throw new LufyException("Entity group name is invalid.");
            }

            if (HasEntity(entityId))
            {
                throw new LufyException(Utility.Text.Format("Entity id '{0}' is already exist.", entityId.ToString()));
            }

            if (IsLoadingEntity(entityId))
            {
                throw new LufyException(Utility.Text.Format("Entity '{0}' is already being loaded.", entityId.ToString()));
            }

            EntityGroup entityGroup = GetEntityGroup(entityGroupName);
            if (entityGroup == null)
            {
                throw new LufyException(Utility.Text.Format("Entity group '{0}' is not exist.", entityGroupName));
            }

            EntityInstanceObject entityInstanceObject = entityGroup.SpawnEntityInstanceObject(entityAssetName);
            if (entityInstanceObject == null)
            {
                int serialId = ++m_Serial;
                m_EntitiesBeingLoaded.Add(entityId, serialId);
                m_ResourceManager.LoadAsset(entityAssetName, m_LoadAssetCallbacks, ShowEntityInfo.Create(serialId, entityId, entityGroup, logicType, userData));
                return;
            }

            InternalShowEntity(entityId, entityAssetName, entityGroup, entityInstanceObject.Target, false, 0f, logicType, userData);
        }

        /// <summary>
        /// 隐藏实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void HideEntity(int entityId, object userData)
        {
            if (IsLoadingEntity(entityId))
            {
                m_EntitiesToReleaseOnLoad.Add(m_EntitiesBeingLoaded[entityId]);
                m_EntitiesBeingLoaded.Remove(entityId);
                return;
            }

            EntityInfo entityInfo = GetEntityInfo(entityId);
            if (entityInfo == null)
            {
                throw new LufyException(Utility.Text.Format("Can not find entity '{0}'.", entityId.ToString()));
            }

            InternalHideEntity(entityInfo, userData);
        }

        /// <summary>
        /// 是否存在实体组。
        /// </summary>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <returns>是否存在实体组。</returns>
        public bool HasEntityGroup(string entityGroupName)
        {
            if (string.IsNullOrEmpty(entityGroupName))
            {
                throw new LufyException("Entity group name is invalid.");
            }

            return m_EntityGroups.ContainsKey(entityGroupName);
        }

        /// <summary>
        /// 是否存在实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <returns>是否存在实体。</returns>
        public bool HasEntity(int entityId)
        {
            return m_EntityInfos.ContainsKey(entityId);
        }

        /// <summary>
        /// 是否正在加载实体。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <returns>是否正在加载实体。</returns>
        public bool IsLoadingEntity(int entityId)
        {
            return m_EntitiesBeingLoaded.ContainsKey(entityId);
        }

        /// <summary>
        /// 获取实体信息。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <returns>实体信息。</returns>
        private EntityInfo GetEntityInfo(int entityId)
        {
            EntityInfo entityInfo = null;
            if (m_EntityInfos.TryGetValue(entityId, out entityInfo))
            {
                return entityInfo;
            }

            return null;
        }

        private void InternalShowEntity(int entityId, string entityAssetName, EntityGroup entityGroup, object entityInstance, bool isNewInstance, float duration, Type logicType, object userData)
        {
            Entity entity = CreateEntity(entityInstance, entityGroup, userData);
            if (entity == null)
            {
                throw new LufyException("Can not create entity in helper.");
            }

            EntityInfo entityInfo = EntityInfo.Create(entity);
            m_EntityInfos.Add(entityId, entityInfo);
            entityInfo.Status = EntityStatus.WillInit;
            entity.OnInit(entityId, entityAssetName, entityGroup, isNewInstance, logicType, userData);
            entityInfo.Status = EntityStatus.Inited;
            entityGroup.AddEntity(entity);
            entityInfo.Status = EntityStatus.WillShow;
            entity.OnShow(userData);
            entityInfo.Status = EntityStatus.Showed;
        }

        private void InternalHideEntity(EntityInfo entityInfo, object userData)
        {
            Entity entity = entityInfo.Entity;
            Entity[] childEntities = entityInfo.GetChildEntities();
            foreach (Entity childEntity in childEntities)
            {
                HideEntity(childEntity.Id, userData);
            }

            if (entityInfo.Status == EntityStatus.Hidden)
            {
                return;
            }

            entityInfo.Status = EntityStatus.WillHide;
            entity.OnHide(m_IsShutdown, userData);
            entityInfo.Status = EntityStatus.Hidden;

            EntityGroup entityGroup = (EntityGroup)entity.EntityGroup;
            if (entityGroup == null)
            {
                throw new LufyException("Entity group is invalid.");
            }

            entityGroup.RemoveEntity(entity);
            if (!m_EntityInfos.Remove(entity.Id))
            {
                throw new LufyException("Entity info is unmanaged.");
            }

            m_RecycleQueue.Enqueue(entityInfo);
        }

        /// <summary>
        /// 创建实体。
        /// </summary>
        /// <param name="entityInstance">实体实例。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>实体。</returns>
        private Entity CreateEntity(object entityInstance, EntityGroup entityGroup, object userData)
        {
            GameObject gameObject = entityInstance as GameObject;
            if (gameObject == null)
            {
                Log.Error("Entity instance is invalid.");
                return null;
            }

            Transform transform = gameObject.transform;
            transform.SetParent(entityGroup.RootInstance.transform);

            return gameObject.GetOrAddComponent<Entity>();
        }

        /// <summary>
        /// 释放实体。
        /// </summary>
        /// <param name="entityAsset">要释放的实体资源。</param>
        /// <param name="entityInstance">要释放的实体实例。</param>
        private void ReleaseEntity(object entityAsset, object entityInstance)
        {
            m_ResourceManager.UnloadAsset(entityAsset);
            Destroy(entityInstance as UnityEngine.Object);
        }

        /// <summary>
        /// 实例化实体。
        /// </summary>
        /// <param name="entityAsset">要实例化的实体资源。</param>
        /// <returns>实例化后的实体。</returns>
        private object InstantiateEntity(object entityAsset)
        {
            return Instantiate((UnityEngine.Object)entityAsset);
        }

        /// <summary>
        /// 获取实体组。
        /// </summary>
        /// <param name="entityGroupName">实体组名称。</param>
        /// <returns>要获取的实体组。</returns>
        private EntityGroup GetEntityGroup(string entityGroupName)
        {
            if (string.IsNullOrEmpty(entityGroupName))
            {
                throw new LufyException("Entity group name is invalid.");
            }

            EntityGroup entityGroup = null;
            if (m_EntityGroups.TryGetValue(entityGroupName, out entityGroup))
            {
                return entityGroup;
            }

            return null;
        }

        private void LoadEntitySuccessCallback(string entityAssetName, object entityAsset, float duration, object userData)
        {
            Debug.Log(entityAssetName + " " + entityAsset);
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            if (showEntityInfo == null)
            {
                throw new LufyException("Show entity info is invalid.");
            }

            if (m_EntitiesToReleaseOnLoad.Contains(showEntityInfo.SerialId))
            {
                m_EntitiesToReleaseOnLoad.Remove(showEntityInfo.SerialId);
                ReferencePool.Release(showEntityInfo);
                ReleaseEntity(entityAsset, null);
                return;
            }

            m_EntitiesBeingLoaded.Remove(showEntityInfo.EntityId);
            EntityInstanceObject entityInstanceObject = EntityInstanceObject.Create(entityAssetName, entityAsset, InstantiateEntity(entityAsset), m_ResourceManager);
            showEntityInfo.EntityGroup.RegisterEntityInstanceObject(entityInstanceObject, true);

            InternalShowEntity(showEntityInfo.EntityId, entityAssetName, showEntityInfo.EntityGroup, entityInstanceObject.Target, true, duration, showEntityInfo.EntityLogicType, showEntityInfo.UserData);
            ReferencePool.Release(showEntityInfo);
        }

        private void LoadEntityFailureCallback(string entityAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {

        }

        private void LoadEntityUpdateCallback(string entityAssetName, float progress, object userData)
        {
           
        }

        private void LoadEntityDependencyAssetCallback(string entityAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
   
        }
    }
}
