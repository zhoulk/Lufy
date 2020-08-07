using System;
using UnityEditor;

namespace Lufy
{
    public static class ExportPackage
    {
        [MenuItem("Lufy/Example/4. 导出UnityPackage", false, 4)]
        public static void exportPackage()
        {
            var assetPathName = "Assets/Lufy/Framework";
            var fileName = "Lufy_" + DateTime.Now.ToString("yyyyMMdd_hh") + ".unitypackage";
            AssetDatabase.ExportPackage(assetPathName, fileName, ExportPackageOptions.Recurse);
        }
    }
}

