using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lufy
{
    public class PercentFun : MonoBehaviour
    {
        [MenuItem("Lufy/Example/11. 概率函数", false, 11)]
        private static void MenuClicked()
        {
            Debug.Log(Percent(50));
        }

        /// <summary>
        /// 输⼊入百分⽐比返回是否命中概率
        /// </summary>
        public static bool Percent(int percent)
        {
            return Random.Range(0, 100) < percent;
        }
    }
}

