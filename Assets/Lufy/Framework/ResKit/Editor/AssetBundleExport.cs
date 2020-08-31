// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-25 11:33:04
// ========================================================
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace LF.Editor
{
    public static class AssetBundlerExport
    {
        private static string m_BunleTargetPath = Application.dataPath + "/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString();
        private static string ABCONFIGPATH = "Assets/Lufy/Framework/ResKit/Editor/ABConfig.asset";
        private static string ABBYTEPATH = "Assets/Game/Res/AssetBundleConfig.bytes";

        //key是ab包名，value是路径，所有文件夹ab包dic
        private static Dictionary<string, string> m_AllFileDir = new Dictionary<string, string>();
        //过滤的list
        private static List<string> m_AllFileAB = new List<string>();
        //单个prefab的ab包
        private static Dictionary<string, List<string>> m_AllPrefabDir = new Dictionary<string, List<string>>();
        //储存所有有效路径
        private static List<string> m_ConfigFil = new List<string>();

        [MenuItem("Lufy/Framework/AssetBundle/Build", false, 0)]
        public static void MenuClick1()
        {
            m_ConfigFil.Clear();
            m_AllFileAB.Clear();
            m_AllFileDir.Clear();
            m_AllPrefabDir.Clear();

            ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCONFIGPATH);
            foreach (var fileDir in abConfig.m_AllFileDirAB)
            {
                if (m_AllFileDir.ContainsKey(fileDir.ABName))
                {
                    Log.Error("AB包配置名字重复，请检查！");
                }
                else
                {
                    m_AllFileDir.Add(fileDir.ABName, fileDir.Path);
                    m_AllFileAB.Add(fileDir.Path);
                    m_ConfigFil.Add(fileDir.Path);
                }
            }

            string[] allStr = AssetDatabase.FindAssets("t:Prefab", abConfig.m_AllPrefabPath.ToArray());
            for (int i = 0; i < allStr.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
                EditorUtility.DisplayProgressBar("查找Prefab", "Prefab:" + path, i * 1.0f / allStr.Length);
                m_ConfigFil.Add(path);
                if (!ContainAllFileAB(path))
                {
                    GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    string[] allDepend = AssetDatabase.GetDependencies(path);
                    List<string> allDependPath = new List<string>();
                    for (int j = 0; j < allDepend.Length; j++)
                    {
                        Debug.Log(path + " " + allDepend[j]);
                        if (!ContainAllFileAB(allDepend[j]) && !allDepend[j].EndsWith(".cs"))
                        {
                            Debug.Log("add  " + path + " " + allDepend[j]);
                            m_AllFileAB.Add(allDepend[j]);
                            allDependPath.Add(allDepend[j]);
                        }
                    }
                    if (m_AllPrefabDir.ContainsKey(obj.name))
                    {
                        Debug.LogError("存在相同名字的Prefab！名字：" + obj.name);
                    }
                    else
                    {
                        m_AllPrefabDir.Add(obj.name, allDependPath);
                    }
                }
            }

            foreach (string name in m_AllFileDir.Keys)
            {
                SetABName(name, m_AllFileDir[name]);
            }

            foreach (string name in m_AllPrefabDir.Keys)
            {
                SetABName(name, m_AllPrefabDir[name]);
            }

            BunildAssetBundle();

            string[] oldABNames = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < oldABNames.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(oldABNames[i], true);
                EditorUtility.DisplayProgressBar("清除AB包名", "名字：" + oldABNames[i], i * 1.0f / oldABNames.Length);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }

        static void SetABName(string name, string path)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            if (assetImporter == null)
            {
                Log.Error("不存在此路径文件：{0}", path);
            }
            else
            {
                assetImporter.assetBundleName = name;
            }
            //Log.Debug("setABName {0} {1}", name, path);
        }

        static void SetABName(string name, List<string> paths)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                SetABName(name, paths[i]);
            }
        }

        static void BunildAssetBundle()
        {
            string[] allBundles = AssetDatabase.GetAllAssetBundleNames();
            //key为全路径，value为包名
            Dictionary<string, string> resPathDic = new Dictionary<string, string>();
            for (int i = 0; i < allBundles.Length; i++)
            {
                string[] allBundlePath = AssetDatabase.GetAssetPathsFromAssetBundle(allBundles[i]);
                for (int j = 0; j < allBundlePath.Length; j++)
                {
                    if (allBundlePath[j].EndsWith(".cs"))
                        continue;

                    Debug.Log("此AB包：" + allBundles[i] + "下面包含的资源文件路径：" + allBundlePath[j]);
                    resPathDic.Add(allBundlePath[j], allBundles[i]);
                }
            }

            if (!Directory.Exists(m_BunleTargetPath))
            {
                Directory.CreateDirectory(m_BunleTargetPath);
            }

            DeleteAB();
            //生成自己的配置表
            WriteData(resPathDic);

            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(m_BunleTargetPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
            if (manifest == null)
            {
                Log.Error("AssetBundle 打包失败！");
            }
            else
            {
                Log.Debug("AssetBundle 打包完毕");
                Application.OpenURL("file:///" + m_BunleTargetPath);
            }
        }

        /// <summary>
        /// 是否包含在已经有的AB包里，做来做AB包冗余剔除
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static bool ContainAllFileAB(string path)
        {
            for (int i = 0; i < m_AllFileAB.Count; i++)
            {
                if (path == m_AllFileAB[i] || (path.Contains(m_AllFileAB[i]) && (path.Replace(m_AllFileAB[i], "")[0] == '/')))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 删除无用AB包
        /// </summary>
        static void DeleteAB()
        {
            DirectoryInfo direction = new DirectoryInfo(m_BunleTargetPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (File.Exists(files[i].FullName))
                {
                    File.Delete(files[i].FullName);
                }
                if (File.Exists(files[i].FullName + ".manifest"))
                {
                    File.Delete(files[i].FullName + ".manifest");
                }
            }
        }

        static void WriteData(Dictionary<string, string> resPathDic)
        {
            AssetBundleConfig config = new AssetBundleConfig();
            config.ABList = new List<ABBase>();
            foreach (string path in resPathDic.Keys)
            {
                if (!ValidPath(path))
                    continue;

                ABBase abBase = new ABBase();
                abBase.Path = path;
                abBase.Crc = Crc32.GetCrc32(path);
                abBase.ABName = resPathDic[path];
                abBase.AssetName = path.Remove(0, path.LastIndexOf("/") + 1);
                abBase.ABDependce = new List<string>();
                string[] resDependce = AssetDatabase.GetDependencies(path);
                for (int i = 0; i < resDependce.Length; i++)
                {
                    string tempPath = resDependce[i];
                    if (tempPath == path || path.EndsWith(".cs"))
                        continue;

                    string abName = "";
                    if (resPathDic.TryGetValue(tempPath, out abName))
                    {
                        if (abName == resPathDic[path])
                            continue;

                        if (!abBase.ABDependce.Contains(abName))
                        {
                            abBase.ABDependce.Add(abName);
                        }
                    }
                }
                config.ABList.Add(abBase);
            }

            //写入xml
            string xmlPath = Application.dataPath + "/AssetbundleConfig.xml";
            if (File.Exists(xmlPath)) File.Delete(xmlPath);
            FileStream fileStream = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
            XmlSerializer xs = new XmlSerializer(config.GetType());
            xs.Serialize(sw, config);
            sw.Close();
            fileStream.Close();

            //写入二进制
            foreach (ABBase abBase in config.ABList)
            {
                abBase.Path = "";
            }
            FileStream fs = new FileStream(ABBYTEPATH, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs.Seek(0, SeekOrigin.Begin);
            fs.SetLength(0);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, config);
            fs.Close();
            AssetDatabase.Refresh();
            SetABName("assetbundleconfig", ABBYTEPATH);
        }

        /// <summary>
        /// 遍历文件夹里的文件名与设置的所有AB包进行检查判断
        /// </summary>
        /// <param name="name"></param>
        /// <param name="strs"></param>
        /// <returns></returns>
        static bool ConatinABName(string name, string[] strs)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                if (name == strs[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 是否有效路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static bool ValidPath(string path)
        {
            for (int i = 0; i < m_ConfigFil.Count; i++)
            {
                if (path.Contains(m_ConfigFil[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
