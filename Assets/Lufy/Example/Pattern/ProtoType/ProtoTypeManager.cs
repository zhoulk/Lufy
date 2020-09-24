// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-24 14:28:23
// ========================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LF.Pattern.ProtoType
{
    public class ProtoTypeManager : LufyManager
    {
        private Dictionary<string, object> m_ProtoDic = new Dictionary<string, object>();

        /// <summary>
        /// 注册一个原型
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="proto">定义</param>
        public void RegisteProtoType(string name, object proto)
        {
            if (m_ProtoDic.ContainsKey(name))
            {
                Log.Error("already exist proto name {0}", name);
                return;
            }
            m_ProtoDic.Add(name, proto);
        }

        /// <summary>
        /// 从一个实体进行克隆
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public object CloneFrom(string name)
        {
            object proto = null;
            if(m_ProtoDic.TryGetValue(name, out proto))
            {
                string target = Serializable(proto);
                return Derializable(target);
            }
            Log.Error("cannot find proto name {0}", name);
            return null;
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            
        }

        internal override void Shutdown()
        {
            
        }

        private string Serializable(object target)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, target);

                return Convert.ToBase64String(stream.ToArray());
            }
        }

        private object Derializable(string target)
        {
            byte[] targetArray = Convert.FromBase64String(target);

            using (MemoryStream stream = new MemoryStream(targetArray))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }

    }
}

