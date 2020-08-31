using LF.Pool;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LF.Res
{
    public class AssetBundleManager
    {
        protected string m_ABConfigABName = "assetbundleconfig";
        //资源关系依赖配表，可以根据crc来找到对应资源块
        protected Dictionary<uint, ResouceItem> m_ResouceItemDic = new Dictionary<uint, ResouceItem>();
        private Dictionary<uint, AssetBundleItem> m_AssetBundleItemDic = new Dictionary<uint, AssetBundleItem>();

        IObjectPool<AssetBundleItem> m_AssetBundleItemPool = null;

        protected string ABLoadPath
        {
            get
            {
                return Application.streamingAssetsPath + "/";
            }
        }

        public AssetBundleManager()
        {
            m_AssetBundleItemPool = Lufy.GetManager<ObjectPoolManager>().CreateMultiSpawnObjectPool<AssetBundleItem>("AssetBundle", 512);
        }

        /// <summary>
        /// 加载配置表
        /// </summary>
        public bool LoadAssetBundleConfig()
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
        public ResouceItem LoadResouceAssetBundle(uint crc)
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

                item = m_AssetBundleItemPool.Spawn();
                if(item == null)
                {
                    item = AssetBundleItem.Create(assetBundle);
                    m_AssetBundleItemPool.Register(item, true);
                }
                item.assetBundle = assetBundle;
                item.RefCount++;
                m_AssetBundleItemDic.Add(crc, item);
            }
            else
            {
                item.RefCount++;
            }
            return item.assetBundle;
        }
    }

    public class AssetBundleItem : ObjectBase
    {
        public AssetBundle assetBundle;
        public int RefCount;

        public static AssetBundleItem Create(AssetBundle assetBundle)
        {
            AssetBundleItem item = ReferencePool.Acquire<AssetBundleItem>();
            item.Initialize(assetBundle);
            item.assetBundle = assetBundle;
            return item;
        }

        protected internal override void Release(bool isShutdown)
        {
            
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
    }
}

