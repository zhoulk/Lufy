// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-10 10:30:50
// ========================================================
using System;
using UnityEngine;

namespace LF.Entity
{
    public partial class EntityManager : LufyManager
    {
        [Serializable]
        private sealed class EntityGroupInfo
        {
            [SerializeField]
            private string m_Name = null;

            [SerializeField]
            private float m_InstanceAutoReleaseInterval = 60f;

            [SerializeField]
            private int m_InstanceCapacity = 16;

            [SerializeField]
            private float m_InstanceExpireTime = 60f;

            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            public float InstanceAutoReleaseInterval
            {
                get
                {
                    return m_InstanceAutoReleaseInterval;
                }
            }

            public int InstanceCapacity
            {
                get
                {
                    return m_InstanceCapacity;
                }
            }

            public float InstanceExpireTime
            {
                get
                {
                    return m_InstanceExpireTime;
                }
            }
        }
    }
}
