// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-25 11:33:04
// ========================================================
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LF.Editor
{
    public static class AssetBundlerExport
    {
        [MenuItem("Lufy/Framework/AssetBundle/Build", false, 0)]
        public static void MenuClick1()
        {
            string outputPath = Application.streamingAssetsPath + "/" + "android";
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);

            AssetDatabase.Refresh();
        }
    }
}
