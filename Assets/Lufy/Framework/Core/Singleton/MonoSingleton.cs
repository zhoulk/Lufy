// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 10:41:28
// ========================================================

using UnityEngine;

namespace LF
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;
        private static object syncRoot = new object();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            T[] instance1 = FindObjectsOfType<T>();
                            if (instance1 != null)
                            {
                                for (var i = 0; i < instance1.Length; i++)
                                {
                                    Destroy(instance1[i].gameObject);
                                }
                            }
                        }
                    }
                    GameObject go = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(go);
                    instance = go.AddComponent<T>();
                }
                return instance;
            }
        }

        public virtual void Awake()
        {
            T t = gameObject.GetComponent<T>();
            if (t == null)
                t = gameObject.AddComponent<T>();
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = t;

                OnSingletonInit();
            }
            if (instance != t)
            {
                MonoBehaviour[] monos = gameObject.GetComponents<MonoBehaviour>();
                if (monos.Length > 1)
                {
                    Destroy(t);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }


        public virtual void OnSingletonInit()
        {

        }
    }
}
