// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-31 15:24:32
// ========================================================
using LF.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LF.Res
{
    public class AssetBundleLoader : IResLoader
    {
        protected string m_ABConfigABName = "assetbundleconfig";
        //资源关系依赖配表，可以根据crc来找到对应资源块
        protected Dictionary<uint, ResouceItem> m_ResouceItemDic = new Dictionary<uint, ResouceItem>();
        private Dictionary<object, string> m_Asset4Path = new Dictionary<object, string>();

        private ObjectPoolManager m_PoolManager = null;
        private IObjectPool<ResouceItemObject> m_AssetBundleItemPool = null;

        protected string ABLoadPath
        {
            get
            {
                return Application.streamingAssetsPath + "/";
            }
        }

        public void SetObjectPoolManager(ObjectPoolManager objectPoolManager)
        {
            m_PoolManager = objectPoolManager;
            m_AssetBundleItemPool = m_PoolManager.CreateSingleSpawnObjectPool<ResouceItemObject>("AssetBundle", 1024, 10);
        }

        public void Init()
        {
            if(m_PoolManager == null)
            {
                throw new LufyException("m_PoolManager is null");
            }

            LoadAssetBundleConfig();
        }

        public void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks)
        {
            uint crc = Crc32.GetCrc32(assetName);
            Debug.Log(assetName + "  " + crc);
            ResouceItem item = LoadResouceAssetBundle(crc);
            AssetBundleRequest bundleRequest = item.m_AssetBundle.LoadAssetAsync(item.m_AssetName);
            bundleRequest.completed += (op) =>
            {
                m_Asset4Path.Add(bundleRequest.asset, assetName);
                if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
                {
                    loadAssetCallbacks.LoadAssetSuccessCallback(assetName, bundleRequest.asset, 0, null);
                }
            };
        }

        /// <summary>
        /// 加载配置表
        /// </summary>
        bool LoadAssetBundleConfig()
        {
            m_ResouceItemDic.Clear();
            string configPath = ABLoadPath + m_ABConfigABName;
            AssetBundle configAB = AssetBundle.LoadFromFile(configPath);
            TextAsset textAsset = configAB.LoadAsset<TextAsset>(m_ABConfigABName);
            if (textAsset == null)
            {
                Log.Error("AssetBundleConfig is no exist!");
                return false;
            }

            MemoryStream stream = new MemoryStream(textAsset.bytes);
            BinaryFormatter bf = new BinaryFormatter();
            AssetBundleConfig config = (AssetBundleConfig)bf.Deserialize(stream);
            stream.Close();

            for (int i = 0; i < config.ABList.Count; i++)
            {
                ABBase abBase = config.ABList[i];
                ResouceItem item = new ResouceItem();
                item.m_Crc = abBase.Crc;
                item.m_AssetName = abBase.AssetName;
                item.m_ABName = abBase.ABName;
                item.m_DependAssetBundle = abBase.ABDependce;
                if (m_ResouceItemDic.ContainsKey(item.m_Crc))
                {
                    Debug.LogError("重复的Crc 资源名:" + item.m_AssetName + " ab包名：" + item.m_ABName);
                }
                else
                {
                    m_ResouceItemDic.Add(item.m_Crc, item);
                }
            }

            return true;
        }

        /// <summary>
        /// 根据路径的crc加载中间类ResoucItem
        /// </summary>
        /// <param name="crc"></param>
        /// <returns></returns>
        private ResouceItem LoadResouceAssetBundle(uint crc)
        {
            ResouceItem item = null;
            if (!m_ResouceItemDic.TryGetValue(crc, out item) || item == null)
            {
                Log.Error("LoadResourceAssetBundle error: can not find crc {0} in AssetBundleConfig", crc.ToString());
                return item;
            }

            if (item.m_AssetBundle != null)
            {
                return item;
            }

            item.m_AssetBundle = LoadAssetBundle(item.m_ABName);

            if (item.m_DependAssetBundle != null)
            {
                for (int i = 0; i < item.m_DependAssetBundle.Count; i++)
                {
                    LoadAssetBundle(item.m_DependAssetBundle[i]);
                }
            }

            return item;
        }

        /// <summary>
        /// 加载单个assetbundle根据名字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private AssetBundle LoadAssetBundle(string name)
        {
            ResouceItemObject item = null;
            uint crc = Crc32.GetCrc32(name);

            item = m_AssetBundleItemPool.Spawn(crc.ToString());
            if (item == null)
            {
                AssetBundle assetBundle = null;
                string fullPath = ABLoadPath + name;
                assetBundle = AssetBundle.LoadFromFile(fullPath);

                item = ResouceItemObject.Create(crc, assetBundle);
                m_AssetBundleItemPool.Register(item, true);
            }

            ResouceItem resouceItem = item.Target as ResouceItem;

            return resouceItem.m_AssetBundle;
        }

        public void UnLoadAsset(object asset)
        {
            string assetName = null;
            m_Asset4Path.TryGetValue(asset, out assetName);
            if (string.IsNullOrEmpty(assetName))
            {
                Log.Error("unload asset not exist {0}", asset);
                return;
            }

            uint crc = Crc32.GetCrc32(assetName);
            ResouceItem item = null;
            if (!m_ResouceItemDic.TryGetValue(crc, out item) || item == null)
            {
                Log.Error("UnLoadAsset error: can not find crc {0} in AssetBundleConfig", crc.ToString());
                return;
            }

            ReleaseAsset(item);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="item"></param>
        private void ReleaseAsset(ResouceItem item)
        {
            if (item == null)
            {
                return;
            }

            if (item.m_DependAssetBundle != null && item.m_DependAssetBundle.Count > 0)
            {
                for (int i = 0; i < item.m_DependAssetBundle.Count; i++)
                {
                    UnLoadAssetBundle(item.m_DependAssetBundle[i]);
                }
            }
            UnLoadAssetBundle(item.m_ABName);
        }

        private void UnLoadAssetBundle(string name)
        {
            AssetBundleItem item = null;
            uint crc = Crc32.GetCrc32(name);
            if (m_AssetBundleItemDic.TryGetValue(crc, out item) && item != null)
            {
                item.RefCount--;
                if (item.RefCount <= 0 && item.assetBundle != null)
                {
                    item.assetBundle.Unload(true);
                    item.Rest();
                    m_AssetBundleItemPool.Recycle(item);
                    m_AssetBundleItemDic.Remove(crc);
                }
            }

            m_AssetBundleItemPool.Unspawn()
        }
    }

    public class ResouceItemObject : ObjectBase
    {
        public static ResouceItemObject Create(uint crc, AssetBundle assetBundle)
        {
            ResouceItem resouceItem = new ResouceItem();
            resouceItem.m_AssetBundle = assetBundle;
            ResouceItemObject uiInstanceObject = ReferencePool.Acquire<ResouceItemObject>();
            uiInstanceObject.Initialize(crc.ToString(), resouceItem);
            return uiInstanceObject;
        }

        protected internal override void Release(bool isShutdown)
        {
            ReferencePool.Release(this);
        }

        public override void Clear()
        {
            base.Clear();
            ((ResouceItem)Target).Clear();
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

        public void Clear()
        {
            m_AssetBundle = null;
        }
    }
}

