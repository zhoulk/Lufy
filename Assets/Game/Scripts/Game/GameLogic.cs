// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-11 18:11:46
// ========================================================
using LF;
using LF.Event;
using LF.Pool;
using LT;
using LT.Net;
using System.Collections.Generic;
using UnityEngine;

public sealed class BulletReleaseEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(BulletReleaseEventArgs).GetHashCode();

    public override int Id => EventId;

    public Bullet bullet;

    public static BulletReleaseEventArgs Create(Bullet bullet)
    {
        BulletReleaseEventArgs e = ReferencePool.Acquire<BulletReleaseEventArgs>(); //new BulletReleaseEventArgs();
        e.bullet = bullet;
        return e;
    }
}

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
        TestManager.Instance.Step1();

        GameEntry.Event.Subscribe(BulletReleaseEventArgs.EventId, OnBulletReleaseHandler);

        GameEntry.Sound.PlayMusic(MusicId.bgm2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionInWorld.z = -1;

            //Log.Debug("click {0}", mousePositionInWorld);

            Bullet bullet = BulletHelper.Instance.Spawn();
            bullets.Add(bullet);

            //Debug.Log(bullet);

            GameObject obj = bullet.Target as GameObject;
            obj.transform.SetParent(bgTrans);
            obj.transform.position = mousePositionInWorld;

            //GameEntry.Sound.PlaySound(SoundId.UI_click);

            //GameEntry.Sound.ResumeMusic();

        }

        if (Input.GetMouseButtonUp(0))
        {
            //IMessage msg = MessagesFactory.BasketBall(1, 10, 90);
            //UDPManager.Instance.Send(msg);
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Log.Debug("click right");

            if (bullets.Count > 0)
            {
                Bullet bullet = bullets[0];
                BulletHelper.Instance.UnSpawn(bullet);
                bullets.RemoveAt(0);
            }

            //GameEntry.Sound.PauseMusic();
            //GameEntry.Sound.MuteSound(true);
        }
    }

    private void OnDestroy()
    {
        foreach(var b in bullets)
        {
            BulletHelper.Instance.UnSpawn(b);
        }
        bullets.Clear();
    }

    void OnBulletReleaseHandler(object sender, GameEventArgs args)
    {
        BulletReleaseEventArgs ne = args as BulletReleaseEventArgs;
        if (ne != null)
        {
            Log.Debug("release bullet {0}", ne.bullet.Name);
        }
    }
}
