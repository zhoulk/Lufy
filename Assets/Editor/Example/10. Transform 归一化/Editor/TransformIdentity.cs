
using UnityEditor;
using UnityEngine;

namespace Lufy
{
    public static class TransformIdentity
    {
        [MenuItem("Lufy/Example/10. Transform 归一化", false, 10)]
        private static void MenuClicked()
        {
            var transform = new GameObject("transform").transform;
            Identity(transform);
        }
        /// <summary>
        /// 重置操作
        /// </summary>
        /// <param name="trans">Trans.</param>
        public static void Identity(Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }
    }
}

