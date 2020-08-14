// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-11 18:18:30
// ========================================================
using LF;
using LF.Pool;
using UnityEngine;

public class Bullet : ObjectBase
{
    public static Bullet Create(string name, object uiFormInstance)
    {
        Bullet bulletInstanceObject = new Bullet();
        bulletInstanceObject.Initialize(name, uiFormInstance);
        return bulletInstanceObject;
    }

    protected internal override void Release(bool isShutdown)
    {
        Log.Debug("bullet release");

        GameEntry.Event.Fire(this, BulletReleaseEventArgs.Create(this));

        if(Target != null)
        {
            GameObject.Destroy(Target as GameObject);
        }
    }

    protected internal override void OnSpawn()
    {
        base.OnSpawn();

        if (Target != null)
        {
            GameObject obj = Target as GameObject;
            obj.SetActive(true);
        }
    }

    protected internal override void OnUnspawn()
    {
        base.OnUnspawn();

        GameObject obj = Target as GameObject;

        if (obj != null)
        {
            obj.SetActive(false);
        }
    }
}
