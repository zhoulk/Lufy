
using System;
using UnityEditor;
using UnityEngine;

namespace Lufy
{
    public static class LogName2Clipboard
    {
        [MenuItem("Lufy/Example/3. 生成包名到剪切板", false, 3)]
        public static void GenerateName2Clipboard()
        {
            GUIUtility.systemCopyBuffer = "Lufy_" + DateTime.Now.ToString("yyyyMMdd_hh");
        }
    }
}
