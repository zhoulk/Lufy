// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-11 18:11:46
// ========================================================
using LF;
using LF.Pool;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private IObjectPool<Bullet> m_BulletPool;

    List<Bullet> bullets = new List<Bullet>();

    Transform bgTrans;

    private void Awake()
    {
        bgTrans = GameObject.Find("bg").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_BulletPool = GameEntry.ObjectPool.CreateMultiSpawnObjectPool<Bullet>("bullet", 10);

        GameObject prefab = Resources.Load<GameObject>("bullet/bullet");
        GameObject obj = GameObject.Instantiate(prefab);
        obj.name = "bullet1";
        m_BulletPool.Register(Bullet.Create("bullet1", obj), false);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionInWorld.z = -1;

            Log.Debug("click {0}", mousePositionInWorld);

            Bullet bullet = m_BulletPool.Spawn("bullet1");
            bullets.Add(bullet);

            Debug.Log(bullet);

            GameObject obj = bullet.Target as GameObject;
            obj.transform.SetParent(bgTrans);
            obj.transform.position = mousePositionInWorld;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Log.Debug("click right");

            if(bullets.Count > 0)
            {
                Bullet bullet = bullets[0];
                m_BulletPool.Unspawn(bullet);
                bullets.RemoveAt(0);
            }
        }
    }
}
