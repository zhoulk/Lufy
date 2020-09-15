// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-11 11:43:51
// ========================================================
using LF;
using LF.Entity;

namespace Test.Entity
{
    public class Bullet : EntityLogic
    {
        protected internal override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            Log.Debug("bullet on hide {0} {1}", isShutdown, userData );
        }

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);

            BulletData data = userData as BulletData;
            CachedTransform.position = data.Pos;
        }

        protected internal override void OnRecycle()
        {
            base.OnRecycle();
            Log.Debug("bullet on recycle ");
        }

        protected internal override void OnShow(object userData)
        {
            base.OnShow(userData);

            BulletData data = userData as BulletData;
            Log.Debug("bullet on show " + data);
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            //Log.Debug("bullet on update ");
        }
    }
}

