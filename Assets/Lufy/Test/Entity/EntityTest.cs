// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-10 10:27:47
// ========================================================
using LF;
using LF.Entity;
using LF.Pool;
using LF.Res;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Entity
{
    public class EntityTest : MonoBehaviour
    {
        EntityManager m_EntityManager = null;
        ResManager m_ResManager = null;
        ObjectPoolManager m_ObjectPoolManager = null;
        int entityId = 0;

        List<int> bulletIds = new List<int>();

        // Start is called before the first frame update
        void Start()
        {
            m_EntityManager = Lufy.GetManager<EntityManager>();
            m_ResManager = Lufy.GetManager<ResManager>();
            m_ResManager.SetResLoader(new AssetBundleLoader());
            m_ObjectPoolManager = Lufy.GetManager<ObjectPoolManager>();
            m_EntityManager.SetResourceManager(m_ResManager);
            m_EntityManager.SetObjectPoolManager(m_ObjectPoolManager);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 touchPos = Input.mousePosition;
                touchPos.z = 5;
                Vector3 pos = Camera.main.ScreenToWorldPoint(touchPos);
                Debug.Log(pos + "  " + Input.mousePosition);
                //pos.z = 0;
                entityId++;
                m_EntityManager.ShowEntity(entityId, "Assets/Game/Res/Prefabs/bullet/bullet.prefab", "bullet", typeof(Bullet), BulletData.Create(pos));
                bulletIds.Add(entityId);
            }

            if (Input.GetMouseButtonDown(1))
            {
                if(bulletIds.Count > 0)
                {
                    int id = bulletIds[0];
                    m_EntityManager.HideEntity(id, null);
                    bulletIds.RemoveAt(0);
                }
            }
        }
    }
}

