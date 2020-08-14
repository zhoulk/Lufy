using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lufy
{
    public static class TransformSetPos
    {
        [MenuItem("Lufy/Example/9. Transform 赋值优化", false, 9)]
        private static void GenerateUnityPackageName()
        {
            var transform = new GameObject("transform").transform;
            SetLocalPosX(transform, 5.0f);
            SetLocalPosY(transform, 5.0f);
            SetLocalPosZ(transform, 5.0f);
        }

        public static void SetLocalPosX(Transform transform, float x)
        {
            var localPos = transform.localPosition;
            localPos.x = x;
            transform.localPosition = localPos;
        }
        public static void SetLocalPosY(Transform transform, float y)
        {
            var localPos = transform.localPosition;
            localPos.y = y;
            transform.localPosition = localPos;
        }
        public static void SetLocalPosZ(Transform transform, float z)
        {
            var localPos = transform.localPosition;
            localPos.z = z;
            transform.localPosition = localPos;
        }
        public static void SetLocalPosXY(Transform transform, float x, float
        y)
        {
            var localPos = transform.localPosition;
            localPos.x = x;
            localPos.y = y;
            transform.localPosition = localPos;
        }
        public static void SetLocalPosXZ(Transform transform, float x, float
        z)
        {
            var localPos = transform.localPosition;
            localPos.x = x;
            localPos.z = z;
            transform.localPosition = localPos;
        }

        public static void SetLocalPosYZ(Transform transform, float y, float z)
        {
            var localPos = transform.localPosition;
            localPos.y = y;
            localPos.z = z;
            transform.localPosition = localPos;
        }
    }
}

