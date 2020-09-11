// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-11 11:43:51
// ========================================================
using LF.Entity;

namespace Test.Entity
{
    public class Bullet : EntityLogic
    {
        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);

            BulletData data = userData as BulletData;
            CachedTransform.position = data.Pos;
        }
    }
}

