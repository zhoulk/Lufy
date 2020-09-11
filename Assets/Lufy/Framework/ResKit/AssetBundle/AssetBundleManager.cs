using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LF.Res
{
    public partial class AssetBundleManager
    {
        private Dictionary<uint, AssetBundleItem> m_AssetBundleItemDic = new Dictionary<uint, AssetBundleItem>();

        protected string ABLoadPath
        {
            get
            {
                return Application.streamingAssetsPath + "/";
            }
        }

        public AssetBundleManager()
        {

        }

        /// <summary>
        /// 根据路径的crc加载中间类ResoucItem
        /// </summary>
        /// <param name="crc"></param>
        /// <returns></returns>
        protected internal AssetBundle LoadAssetBundle(uint crc)
        {
            ConfigItem configItem = null;
            if (!m_ConfigItemDic.TryGetValue(crc, out configItem) || configItem == null)
            {
                Log.Error("LoadResourceAssetBundle error: can not find crc {0} in AssetBundleConfig", crc.ToString());
                return null;
            }

            AssetBundleItem bundleItem = null;
            if (m_AssetBundleItemDic.TryGetValue(crc, out bundleItem))
            {
                return bundleItem.AssetBundle;
            }

            AssetBundle bundle = LoadAssetBundle(configItem.ABName);
            if (configItem.DependAssetBundle != null)
            {
                for (int i = 0; i < configItem.DependAssetBundle.Count; i++)
                {
                    LoadAssetBundle(configItem.DependAssetBundle[i]);
                }
            }
            return bundle;
        }

        /// <summary>
        /// 加载单个assetbundle根据名字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private AssetBundle LoadAssetBundle(string name)
        {
            AssetBundleItem item = null;
            uint crc = Crc32.GetCrc32(name);

            if (!m_AssetBundleItemDic.TryGetValue(crc, out item))
            {
                AssetBundle assetBundle = null;
                string fullPath = ABLoadPath + name;
                assetBundle = AssetBundle.LoadFromFile(fullPath);

                if (assetBundle == null)
                {
                    Debug.LogError(" Load AssetBundle Error:" + fullPath);
                }

                item = ReferencePool.Acquire<AssetBundleItem>();
                item.AssetBundle = assetBundle;
                item.RefCount++;
                m_AssetBundleItemDic.Add(crc, item);
            }
            else
            {
                item.RefCount++;
            }
            return item.AssetBundle;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="item"></param>
        protected internal void ReleaseAsset(ConfigItem item)
        {
            if (item == null)
            {
                return;
            }

            if (item.DependAssetBundle != null && item.DependAssetBundle.Count > 0)
            {
                for (int i = 0; i < item.DependAssetBundle.Count; i++)
                {
                    UnLoadAssetBundle(item.DependAssetBundle[i]);
                }
            }
            UnLoadAssetBundle(item.ABName);
        }

        private void UnLoadAssetBundle(string name)
        {
            AssetBundleItem item = null;
            uint crc = Crc32.GetCrc32(name);
            if (m_AssetBundleItemDic.TryGetValue(crc, out item) && item != null)
            {
                item.RefCount--;
                if (item.RefCount <= 0 && item.AssetBundle != null)
                {
                    Log.Debug("release " + name);
                    item.AssetBundle.Unload(true);
                    ReferencePool.Release(item);
                    m_AssetBundleItemDic.Remove(crc);
                }
            }
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            
        }
    }

    public class AssetBundleItem : IReference
    {
        public AssetBundle AssetBundle;
        public int RefCount;

        public void Clear()
        {
            AssetBundle = null;
            RefCount = 0;
        }
    }

    public class ResouceItem
    {
        //资源路径的CRC
        public uint m_Crc = 0;
        //该资源的文件名
        public string m_AssetName = string.Empty;
        //该资源所在的AssetBundle
        public string m_ABName = string.Empty;
        //该资源所依赖的AssetBundle
        public List<string> m_DependAssetBundle = null;
        //该资源加载完的AB包
        public AssetBundle m_AssetBundle = null;

        //资源对象
        public Object m_Obj = null;
        //资源唯一标识
        public int m_Guid = 0;
        //资源最后所使用的时间
        public float m_LastUseTime = 0.0f;
        //引用计数
        protected int m_RefCount = 0;

        public int RefCount
        {
            get { return m_RefCount; }
            set
            {
                m_RefCount = value;
                if (m_RefCount < 0)
                {
                    Debug.LogError("refcount < 0" + m_RefCount + " ," + (m_Obj != null ? m_Obj.name : "name is null"));
                }
            }
        }
    }
}

