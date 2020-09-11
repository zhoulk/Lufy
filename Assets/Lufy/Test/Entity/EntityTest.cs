// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-10 10:27:47
// ========================================================
using LF;
using LF.Entity;
using LF.Pool;
using LF.Res;
using UnityEngine;

namespace Test.Entity
{
    public class EntityTest : MonoBehaviour
    {
        EntityManager m_EntityManager = null;
        ResManager m_ResManager = null;
        ObjectPoolManager m_ObjectPoolManager = null;
        int entityId = 0;

        private void Awake()
        {
            m_EntityManager = Lufy.GetManager<EntityManager>();
            m_ResManager = Lufy.GetManager<ResManager>();
            m_ResManager.SetResLoader(new AssetBundleLoader());
            m_ObjectPoolManager = Lufy.GetManager<ObjectPoolManager>();
            m_EntityManager.SetResourceManager(m_ResManager);
            m_EntityManager.SetObjectPoolManager(m_ObjectPoolManager);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                entityId++;
                m_EntityManager.ShowEntity(entityId, "Assets/Game/Res/Prefabs/bullet/bullet.prefab", "bullet", typeof(Bullet), BulletData.Create(pos));
            }
        }
    }
}

