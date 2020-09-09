// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-09 16:11:46
// ========================================================

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LF.Res
{
    public partial class AssetBundleManager
    {
        protected string m_ABConfigABName = "assetbundleconfig";
        //资源关系依赖配表，可以根据crc来找到对应资源块
        protected Dictionary<uint, ConfigItem> m_ConfigItemDic = new Dictionary<uint, ConfigItem>();

        /// <summary>
        /// 加载配置表
        /// </summary>
        protected internal bool LoadAssetBundleConfig()
        {
            m_ConfigItemDic.Clear();
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
                ConfigItem item = new ConfigItem();
                item.Crc = abBase.Crc;
                item.AssetName = abBase.AssetName;
                item.ABName = abBase.ABName;
                item.DependAssetBundle = abBase.ABDependce;
                if (m_ConfigItemDic.ContainsKey(item.Crc))
                {
                    Log.Error("重复的Crc 资源名:" + item.AssetName + " ab包名：" + item.ABName);
                }
                else
                {
                    m_ConfigItemDic.Add(item.Crc, item);
                }
            }

            return true;
        }

        /// <summary>
        /// 查询配置
        /// </summary>
        /// <param name="crc"></param>
        /// <returns></returns>
        protected internal ConfigItem LoadConfigItem(uint crc)
        {
            ConfigItem configItem = null;
            if (!m_ConfigItemDic.TryGetValue(crc, out configItem) || configItem == null)
            {
                Log.Error("LoadResourceAssetBundle error: can not find crc {0} in AssetBundleConfig", crc.ToString());
                return null;
            }
            return configItem;
        }
    }

    public class ConfigItem
    {
        //资源路径的CRC
        public uint Crc = 0;
        //该资源的文件名
        public string AssetName = string.Empty;
        //该资源所在的AssetBundle
        public string ABName = string.Empty;
        //该资源所依赖的AssetBundle
        public List<string> DependAssetBundle = null;
    }
}
