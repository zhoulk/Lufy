// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 10:19:03
// ========================================================
namespace LF
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _Instance;

        static object mLock = new object();

        public static T Instance
        {
            get
            {
                lock (mLock)
                {
                    if (_Instance == null)
                    {
                        _Instance = new T();

                        _Instance.OnSingletonInit();
                    }
                }

                return _Instance;
            }
        }

        public virtual void OnSingletonInit()
        {

        }
    }
}

