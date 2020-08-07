// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-07 09:52:57
// ========================================================
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LF.Editor
{
    public static class ExportPackage
    {
        [MenuItem("Lufy/Framework/导出UnityPackage", false, 0)]
        public static void MenuClick()
        {
            var assetPathName = "Assets/Lufy/Framework";
            var fileName = "Lufy_" + DateTime.Now.ToString("yyyyMMdd_hh") + ".unitypackage";
            AssetDatabase.ExportPackage(assetPathName, fileName, ExportPackageOptions.Recurse);
            Application.OpenURL("file:///" + Path.Combine(Application.dataPath, "../"));
        }
    }
}

