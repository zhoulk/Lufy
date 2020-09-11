// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-11 11:51:46
// ========================================================
using LF;
using UnityEngine;

namespace Test.Entity
{
    public class BulletData : IReference
    {
        private Vector3 m_Pos;

        public Vector3 Pos
        {
            get
            {
                return m_Pos;
            }
        }

        public void Clear()
        {
            m_Pos = Vector3.zero;
        }

        public static BulletData Create(Vector3 pos)
        {
            BulletData data = ReferencePool.Acquire<BulletData>();
            data.m_Pos = pos;
            return data;
        }
    }
}

