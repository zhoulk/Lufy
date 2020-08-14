using System;
using UnityEditor;
using UnityEngine;

namespace Lufy
{
    public static class LogFileName
    {
        [MenuItem("Lufy/Example/1.生成Package名字", false, 1)]
        public static void GeneratePackageName()
        {
            Debug.Log("Lufy_" + DateTime.Now.ToString("yyyyMMdd_hh"));
        }
    }
}
