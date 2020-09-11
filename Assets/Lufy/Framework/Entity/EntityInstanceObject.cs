// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-11 10:26:50
// ========================================================
using LF.Pool;
using LF.Res;
using UnityEngine;

namespace LF.Entity
{
    public class EntityInstanceObject : ObjectBase
    {
        private object m_EntityAsset;
        private IResManager m_ResManager;

        public EntityInstanceObject()
        {
            m_EntityAsset = null;
            m_ResManager = null;
        }

        public static EntityInstanceObject Create(string name, object entityAsset, object entityInstance, IResManager resManager)
        {
            if (entityAsset == null)
            {
                throw new LufyException("Entity asset is invalid.");
            }

            EntityInstanceObject entityInstanceObject = ReferencePool.Acquire<EntityInstanceObject>();
            entityInstanceObject.Initialize(name, entityInstance);
            entityInstanceObject.m_EntityAsset = entityAsset;
            entityInstanceObject.m_ResManager = resManager;
            return entityInstanceObject;
        }

        public override void Clear()
        {
            base.Clear();
            m_EntityAsset = null;
            m_ResManager = null;
        }

        protected internal override void Release(bool isShutdown)
        {
            ReleaseEntity(m_EntityAsset, Target);
        }

        /// <summary>
        /// 释放实体。
        /// </summary>
        /// <param name="entityAsset">要释放的实体资源。</param>
        /// <param name="entityInstance">要释放的实体实例。</param>
        private void ReleaseEntity(object entityAsset, object entityInstance)
        {
            m_ResManager.UnloadAsset(entityAsset);
            GameObject.Destroy((Object)entityInstance);
        }
    }
}

