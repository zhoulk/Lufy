// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 09:18:41
// ========================================================
using LF;
using LF.Pool;
using UnityEngine;

public class BulletHelper : Singleton<BulletHelper>
{
    int m_serial = 1;

    IObjectPool<Bullet> m_BulletPool;
    GameObject m_prefab;

    public override void OnSingletonInit()
    {
        m_BulletPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<Bullet>("bullet", 10, 3);
        m_prefab = Resources.Load<GameObject>("bullet/bullet");
    }

    public Bullet Spawn()
    {
        Bullet bullet = m_BulletPool.Spawn();
        if(bullet == null)
        {
            string name = "bullet" + m_serial++;
            GameObject obj = GameObject.Instantiate(m_prefab);
            obj.name = name;
            bullet = Bullet.Create(name, obj);
            m_BulletPool.Register(bullet, true);
        }
        return bullet;
    }

    public void UnSpawn(Bullet bullet)
    {
        m_BulletPool.Unspawn(bullet);
    }
}
