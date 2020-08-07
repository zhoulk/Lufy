using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lufy
{
    public static class CustomShortCut
    {
        [MenuItem("Lufy/Example/7.导出UnityPackage %e", false, 7)]
        public static void MenuClick()
        {
            EditorApplication.ExecuteMenuItem("Lufy/6. MenuItem复用");
        }
    }
}

