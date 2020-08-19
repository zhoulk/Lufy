
using UnityEditor;
using UnityEngine;

namespace Lufy
{
    public static class Copy2Clipboard
    {
        [MenuItem("Lufy/Example/2. 复制到剪切板", false, 2)]
        public static void Copy()
        {
            GUIUtility.systemCopyBuffer = "复制到剪切板";
        }
    }
}
