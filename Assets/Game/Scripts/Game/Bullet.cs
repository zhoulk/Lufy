// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-11 18:18:30
// ========================================================
using LF.Pool;

public class Bullet : ObjectBase
{
    public static Bullet Create(string name, object uiFormInstance)
    {
        Bullet bulletInstanceObject = new Bullet();
        bulletInstanceObject.Initialize(name, uiFormInstance);
        return bulletInstanceObject;
    }

    protected override void Release(bool isShutdown)
    {
        
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
    }

    protected override void OnUnspawn()
    {
        base.OnUnspawn();
    }
}
