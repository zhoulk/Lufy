// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 09:18:41
// ========================================================
using LF;
using LF.Event;
using LF.Pool;
using LF.Res;
using UnityEngine;

public class BulletHelper : Singleton<BulletHelper>
{
    int m_serial = 1;

    IObjectPool<Bullet> m_BulletPool;
    GameObject m_prefab;
    string bulletAssetName = "Assets/Game/Res/Prefabs/bullet/bullet.prefab";

    public override void OnSingletonInit()
    {
        m_BulletPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<Bullet>("bullet", 10, 100);
        //m_prefab = Resources.Load<GameObject>("bullet/bullet");

        GameEntry.Res.LoadAsset(bulletAssetName, new LoadAssetCallbacks(
            (string assetName, object asset, float duration, object userData) =>
            {
                m_prefab = asset as GameObject;
            }));

        //GameEntry.Event.Subscribe(BulletReleaseEventArgs.EventId, OnBulletReleaseHandler);
    }

    public Bullet Spawn()
    {
        Bullet bullet = m_BulletPool.Spawn();
        if (bullet == null)
        {
            string name = "bullet" + m_serial++;
            GameObject obj = GameObject.Instantiate(m_prefab);
            obj.name = name;
            bullet = Bullet.Create(bulletAssetName, obj);
            m_BulletPool.Register(bullet, true);
        }
        return bullet;
    }

    public void UnSpawn(Bullet bullet)
    {
        m_BulletPool.Unspawn(bullet);
    }

    //void OnBulletReleaseHandler(object sender, GameEventArgs args)
    //{
    //    BulletReleaseEventArgs ne = args as BulletReleaseEventArgs;
    //    if (ne != null)
    //    {
    //        Log.Debug("helper release bullet {0}", ne.bullet.Name);
    //    }
    //}
}
