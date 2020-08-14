using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Lufy
{
    public class ReuseMenuItem
    {
        [MenuItem("Lufy/Example/6. MenuItem复用", false, 6)]
        public static void MenuClick()
        {
            EditorApplication.ExecuteMenuItem("Lufy/4. 导出UnityPackage");
            Application.OpenURL("file:///" + Path.Combine(Application.dataPath, "../"));
        }
    }
}

