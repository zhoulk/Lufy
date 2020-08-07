using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lufy
{
    public static class OpenInFolder
    {
        [MenuItem("Lufy/Example/5. 打开文件夹", false, 5)]
        public static void MenuClick()
        {
            Application.OpenURL("file:///" + Application.dataPath);
        }
    }
}

