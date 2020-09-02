// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-02 09:26:09
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LF.Res
{
    public class ResourceManager
    {
        private AssetBundleManager m_AssetBundleManager = null;

        //缓存使用的资源列表
        public Dictionary<uint, ResouceItem> AssetDic { get; set; } = new Dictionary<uint, ResouceItem>();

        private bool m_EditorResource = false;

        public ResourceManager()
        {
            m_AssetBundleManager = new AssetBundleManager();
            m_AssetBundleManager.LoadAssetBundleConfig();

            m_EditorResource = Lufy.GetManager<ResManager>().EditorResource;
        }

        /// <summary>
        /// 同步资源加载，外部直接调用，仅加载不需要实例化的资源，例如Texture,音频等等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadResource<T>(string path) where T : UnityEngine.Object
        {
            if (m_EditorResource)
            {
                return LoadResourceFromEditor<T>(path);
            }
            else
            {
                return LoadResourceFromAssetBundle<T>(path);
            }
        }

        /// <summary>
        /// 从编辑器读取资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        private T LoadResourceFromEditor<T>(string path) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            uint crc = Crc32.GetCrc32(path);
            ResouceItem item = GetCacheResouceItem(crc);
            if (item == null)
            {
                item = new ResouceItem();
                item.m_Crc = crc;
                T obj = LoadAssetByEditor<T>(path);
                item.m_Obj = obj;
            }

            CacheResource(path, item);
            return item.m_Obj as T;
        }

        /// <summary>
        /// 从AssetBundle读取资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        private T LoadResourceFromAssetBundle<T>(string path) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            uint crc = Crc32.GetCrc32(path);
            ResouceItem item = GetCacheResouceItem(crc);
            if (item != null)
            {
                return item.m_Obj as T;
            }

            T obj = null;
            item = m_AssetBundleManager.LoadResouceAssetBundle(crc);
            if (item != null && item.m_AssetBundle != null)
            {
                if (item.m_Obj != null)
                {
                    obj = item.m_Obj as T;
                }
                else
                {
                    obj = item.m_AssetBundle.LoadAsset<T>(item.m_AssetName);
                }
                item.m_Crc = crc;
                item.m_Obj = obj;
            }
            CacheResource(path, item);
            return obj;
        }

        protected T LoadAssetByEditor<T>(string path) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#else
            return null;
#endif
        }

        /// <summary>
        /// 缓存加载的资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="item"></param>
        /// <param name="addrefcount"></param>
        void CacheResource(string path, ResouceItem item, int addrefcount = 1)
        {
            //缓存太多，清除最早没有使用的资源
            //WashOut();

            if (item == null)
            {
                Debug.LogError("ResouceItem is null, path: " + path);
            }

            if (item.m_Obj == null)
            {
                Debug.LogError("object is null, path: " + path);
            }

            item.m_Guid = item.m_Obj.GetInstanceID();
            item.m_LastUseTime = Time.realtimeSinceStartup;
            item.RefCount += addrefcount;
            if (AssetDic.ContainsKey(item.m_Crc))
            {
                AssetDic[item.m_Crc] = item;
            }
            else
            {
                AssetDic.Add(item.m_Crc, item);
            }
        }

        /// <summary>
        /// 从资源池获取缓存资源
        /// </summary>
        /// <param name="crc"></param>
        /// <param name="addrefcount"></param>
        /// <returns></returns>
        ResouceItem GetCacheResouceItem(uint crc, int addrefcount = 1)
        {
            ResouceItem item = null;
            if (AssetDic.TryGetValue(crc, out item))
            {
                if (item != null)
                {
                    item.RefCount += addrefcount;
                    item.m_LastUseTime = Time.realtimeSinceStartup;
                }
            }

            return item;
        }
    }
}
