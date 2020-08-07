using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lufy
{
    public class ResolutionCheck
    {
        [MenuItem("Lufy/Example/8. 判断屏幕宽高比", false, 8)]
        public static void MenuClick()
        {
            Debug.Log(IsPadResolution() ? "是 Pad 分辨率" : "不是 Pad 分辨率");
            Debug.Log(IsPhoneResolution() ? "是 Phone 分辨率" : "不是 Phone 分辨率");
            Debug.Log(IsiPhoneXResolution() ? "是 iPhone X 分辨率" : "不是iPhone X 分辨率");
        }

        public static float GetAspectRatio()
        {
            return Screen.width > Screen.height ? ((float)Screen.width / Screen.height) : ((float)Screen.height / Screen.width);
        }

        /// <summary>
        /// 是否是 Pad 分辨率 4 : 3
        /// </summary>
        /// <returns></returns>
        public static bool IsPadResolution()
        {
            var aspect = GetAspectRatio();
            return aspect > 4.0f / 3 - 0.05 && aspect < 4.0f / 3 + 0.05;
        }
        /// <summary>
        /// 是否是⼿手机分辨率 16:9
        /// </summary>
        /// <returns></returns>
        public static bool IsPhoneResolution()
        {
            var aspect = GetAspectRatio();
            return aspect > 16.0f / 9 - 0.05 && aspect < 16.0f / 9 + 0.05;
        }
        /// <summary>
        /// 是否是iPhone X 分辨率 2436:1125
        /// </summary>
        /// <returns></returns>
        public static bool IsiPhoneXResolution()
        {
            var aspect = GetAspectRatio();
            return aspect > 2436.0f / 1125 - 0.05 && aspect < 2436.0f / 1125 +
            0.05;
        }
    }
}

