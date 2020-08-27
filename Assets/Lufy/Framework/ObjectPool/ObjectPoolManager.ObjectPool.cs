// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-11 15:26:06
// ========================================================
using System;
using System.Collections.Generic;

namespace LF.Pool
{
    public sealed partial class ObjectPoolManager
    {
        /// <summary>
        /// 对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private sealed class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
        {
            private readonly Dictionary<string, List<Object<T>>> m_Objects;
            private readonly Dictionary<object, Object<T>> m_ObjectMap;
            private readonly ReleaseObjectFilterCallback<T> m_DefaultReleaseObjectFilterCallback;
            private readonly List<T> m_CachedCanReleaseObjects;
            private readonly List<T> m_CachedToReleaseObjects;
            private readonly bool m_AllowMultiSpawn;
            private float m_AutoReleaseInterval;
            private int m_Capacity;
            private float m_ExpireTime;
            private int m_Priority;
            private float m_AutoReleaseTime;

            /// <summary>
            /// 初始化对象池的新实例。
            /// </summary>
            /// <param name="name">对象池名称。</param>
            /// <param name="allowMultiSpawn">是否允许对象被多次获取。</param>
            /// <param name="autoReleaseInterval">对象池自动释放可释放对象的间隔秒数。</param>
            /// <param name="capacity">对象池的容量。</param>
            /// <param name="expireTime">对象池对象过期秒数。</param>
            /// <param name="priority">对象池的优先级。</param>
            public ObjectPool(string name, bool allowMultiSpawn, float autoReleaseInterval, int capacity, float expireTime, int priority)
                : base(name)
            {
                m_Objects = new Dictionary<string, List<Object<T>>>();
                m_ObjectMap = new Dictionary<object, Object<T>>();
                m_DefaultReleaseObjectFilterCallback = DefaultReleaseObjectFilterCallback;
                m_CachedCanReleaseObjects = new List<T>();
                m_CachedToReleaseObjects = new List<T>();
                m_AllowMultiSpawn = allowMultiSpawn;
                m_AutoReleaseInterval = autoReleaseInterval;
                Capacity = capacity;
                ExpireTime = expireTime;
                m_Priority = priority;
                m_AutoReleaseTime = 0f;
            }

            public override Type ObjectType => typeof(T);

            public override int Count => m_ObjectMap.Count;

            public override int CanReleaseCount {
                get
                {
                    GetCanReleaseObjects(m_CachedCanReleaseObjects);
                    return m_CachedCanReleaseObjects.Count;
                }
            }

            public override bool AllowMultiSpawn => m_AllowMultiSpawn;

            public override float AutoReleaseInterval { get => m_AutoReleaseInterval; set => m_AutoReleaseInterval = value; }
            public override int Capacity {
                get
                {
                    return m_Capacity;
                }
                set
                {
                    if (value < 0)
                    {
                        throw new LufyException("Capacity is invalid.");
                    }

                    if (m_Capacity == value)
                    {
                        return;
                    }

                    m_Capacity = value;
                    Release();
                }
            }
            /// <summary>
            /// 获取或设置对象池对象过期秒数。
            /// </summary>
            public override float ExpireTime
            {
                get
                {
                    return m_ExpireTime;
                }

                set
                {
                    if (value < 0f)
                    {
                        throw new LufyException("ExpireTime is invalid.");
                    }

                    if (ExpireTime == value)
                    {
                        return;
                    }

                    m_ExpireTime = value;
                    Release();
                }
            }

            /// <summary>
            /// 获取或设置对象池的优先级。
            /// </summary>
            public override int Priority
            {
                get
                {
                    return m_Priority;
                }
                set
                {
                    m_Priority = value;
                }
            }

            /// <summary>
            /// 检查对象。
            /// </summary>
            /// <returns>要检查的对象是否存在。</returns>
            public bool CanSpawn()
            {
                return CanSpawn(string.Empty);
            }

            public bool CanSpawn(string name)
            {
                List<Object<T>> objectRange = default(List<Object<T>>);
                if (m_Objects.TryGetValue(name, out objectRange))
                {
                    foreach (Object<T> internalObject in objectRange)
                    {
                        if (m_AllowMultiSpawn || !internalObject.IsInUse)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            public override ObjectInfo[] GetAllObjectInfos()
            {
                List<ObjectInfo> results = new List<ObjectInfo>();
                foreach (KeyValuePair<string, List<Object<T>>> objectRanges in m_Objects)
                {
                    foreach (Object<T> internalObject in objectRanges.Value)
                    {
                        results.Add(new ObjectInfo(internalObject.Name, internalObject.Locked, internalObject.CustomCanReleaseFlag, internalObject.LastUseTime, internalObject.SpawnCount));
                    }
                }

                return results.ToArray();
            }

            /// <summary>
            /// 创建对象。
            /// </summary>
            /// <param name="obj">对象。</param>
            /// <param name="spawned">对象是否已被获取。</param>
            public void Register(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw new LufyException("Object is invalid.");
                }

                Object<T> internalObject = Object<T>.Create(obj, spawned);

                List<Object<T>> objectRange = default(List<Object<T>>);
                if (m_Objects.TryGetValue(obj.Name, out objectRange))
                {
                    objectRange.Add(internalObject);
                }
                else
                {
                    objectRange = new List<Object<T>>();
                    objectRange.Add(internalObject);
                    m_Objects.Add(obj.Name, objectRange);
                }
                m_ObjectMap.Add(obj.Target, internalObject);

                if (Count > m_Capacity)
                {
                    Release();
                }
            }

            public override void Release()
            {
                Release(Count - m_Capacity, m_DefaultReleaseObjectFilterCallback);
            }

            public override void Release(int toReleaseCount)
            {
                Release(toReleaseCount, m_DefaultReleaseObjectFilterCallback);
            }

            public void Release(ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                Release(Count - m_Capacity, releaseObjectFilterCallback);
            }

            public void Release(int toReleaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                if (releaseObjectFilterCallback == null)
                {
                    throw new LufyException("Release object filter callback is invalid.");
                }

                if (toReleaseCount < 0)
                {
                    toReleaseCount = 0;
                }

                DateTime expireTime = DateTime.MinValue;
                if (m_ExpireTime < float.MaxValue)
                {
                    expireTime = DateTime.Now.AddSeconds(-m_ExpireTime);
                }

                m_AutoReleaseTime = 0f;
                GetCanReleaseObjects(m_CachedCanReleaseObjects);
                //Log.Debug("m_CachedCanReleaseObjects.size {0} ", m_CachedCanReleaseObjects.Count);

                List<T> toReleaseObjects = releaseObjectFilterCallback(m_CachedCanReleaseObjects, toReleaseCount, expireTime);
                if (toReleaseObjects == null || toReleaseObjects.Count <= 0)
                {
                    return;
                }

                //Log.Debug("toReleaseObjects.size {0} ", toReleaseObjects.Count);
                foreach (T toReleaseObject in toReleaseObjects)
                {
                    ReleaseObject(toReleaseObject);
                }
            }

            public override void ReleaseAllUnused()
            {
                m_AutoReleaseTime = 0f;
                GetCanReleaseObjects(m_CachedCanReleaseObjects);
                foreach (T toReleaseObject in m_CachedCanReleaseObjects)
                {
                    ReleaseObject(toReleaseObject);
                }
            }

            public void SetLocked(T obj, bool locked)
            {
                if (obj == null)
                {
                    throw new LufyException("Object is invalid.");
                }

                SetLocked(obj.Target, locked);
            }

            public void SetLocked(object target, bool locked)
            {
                if (target == null)
                {
                    throw new LufyException("Target is invalid.");
                }

                Object<T> internalObject = GetObject(target);
                if (internalObject != null)
                {
                    internalObject.Locked = locked;
                }
                else
                {
                    throw new LufyException(Utility.Text.Format("Can not find target in object pool '{0}.{1}', target type is '{2}', target value is '{3}'.", typeof(T).FullName, Name, target.GetType().FullName, target.ToString()));
                }
            }

            /// <summary>
            /// 获取对象。
            /// </summary>
            /// <returns>要获取的对象。</returns>
            public T Spawn()
            {
                return Spawn(string.Empty);
            }

            public T Spawn(string name)
            {
                List<Object<T>> objectRange = default(List<Object<T>>);
                if (m_Objects.TryGetValue(name, out objectRange))
                {
                    foreach (Object<T> internalObject in objectRange)
                    {
                        //Log.Debug("spawn {0} {1} {2}", name, m_AllowMultiSpawn, internalObject.IsInUse);

                        if (m_AllowMultiSpawn || !internalObject.IsInUse)
                        {
                            return internalObject.Spawn();
                        }
                    }
                }
                //foreach (var kv in m_Objects)
                //{
                //    Log.Debug("{0} {1}", kv.Key, kv.Value);
                //}

                //Log.Debug("spawn {0}", name);

                return null;
            }

            public void Unspawn(T obj)
            {
                if (obj == null)
                {
                    throw new LufyException("Object is invalid.");
                }

                Unspawn(obj.Target);
            }

            public void Unspawn(object target)
            {
                if (target == null)
                {
                    throw new LufyException("Target is invalid.");
                }

                Object<T> internalObject = GetObject(target);
                if (internalObject != null)
                {
                    internalObject.Unspawn();
                    if (Count > m_Capacity && internalObject.SpawnCount <= 0)
                    {
                        Release();
                    }
                }
                else
                {
                    throw new LufyException(Utility.Text.Format("Can not find target in object pool '{0}.{1}', target type is '{2}', target value is '{3}'.", typeof(T).FullName, Name, target.GetType().FullName, target.ToString()));
                }
            }

            internal override void Shutdown()
            {
                foreach (KeyValuePair<object, Object<T>> objectInMap in m_ObjectMap)
                {
                    objectInMap.Value.Release(true);
                    //ReferencePool.Release(objectInMap.Value);
                }

                m_Objects.Clear();
                m_ObjectMap.Clear();
                m_CachedCanReleaseObjects.Clear();
                m_CachedToReleaseObjects.Clear();
            }

            internal override void Update(float elapseSeconds, float realElapseSeconds)
            {
                m_AutoReleaseTime += realElapseSeconds;
                if (m_AutoReleaseTime < m_AutoReleaseInterval)
                {
                    return;
                }

                Release();
            }

            private Object<T> GetObject(object target)
            {
                if (target == null)
                {
                    throw new LufyException("Target is invalid.");
                }

                //foreach(var kv in m_ObjectMap)
                //{
                //    Log.Debug("{0} {1}", kv.Key, kv.Value);
                //}

                Object<T> internalObject = null;
                if (m_ObjectMap.TryGetValue(target, out internalObject))
                {
                    return internalObject;
                }

                return null;
            }

            private void ReleaseObject(T obj)
            {
                if (obj == null)
                {
                    throw new LufyException("Object is invalid.");
                }

                Object<T> internalObject = GetObject(obj.Target);
                if (internalObject == null)
                {
                    throw new LufyException("Can not release object which is not found.");
                }

                List<Object<T>> objectRange = default(List<Object<T>>);
                if (m_Objects.TryGetValue(obj.Name, out objectRange))
                {
                    objectRange.Remove(internalObject);
                }

                //if (objectRange.Count <= 0)
                //{
                    m_ObjectMap.Remove(obj.Target);
                //}

                //Log.Debug("m_ObjectMap.Size {0} ", m_ObjectMap.Count);

                internalObject.Release(false);
                //ReferencePool.Release(internalObject);
            }

            private void GetCanReleaseObjects(List<T> results)
            {
                if (results == null)
                {
                    throw new LufyException("Results is invalid.");
                }

                results.Clear();
                foreach (KeyValuePair<object, Object<T>> objectInMap in m_ObjectMap)
                {
                    Object<T> internalObject = objectInMap.Value;
                    if (internalObject.IsInUse || internalObject.Locked || !internalObject.CustomCanReleaseFlag)
                    {
                        continue;
                    }

                    results.Add(internalObject.Peek());
                }
            }

            private List<T> DefaultReleaseObjectFilterCallback(List<T> candidateObjects, int toReleaseCount, DateTime expireTime)
            {
                m_CachedToReleaseObjects.Clear();

                if (expireTime > DateTime.MinValue)
                {
                    for (int i = candidateObjects.Count - 1; i >= 0; i--)
                    {
                        if (candidateObjects[i].LastUseTime <= expireTime)
                        {
                            m_CachedToReleaseObjects.Add(candidateObjects[i]);
                            candidateObjects.RemoveAt(i);
                            continue;
                        }
                    }

                    toReleaseCount -= m_CachedToReleaseObjects.Count;
                }

                for (int i = 0; toReleaseCount > 0 && i < candidateObjects.Count; i++)
                {
                    for (int j = i + 1; j < candidateObjects.Count; j++)
                    {
                        if (candidateObjects[i].LastUseTime > candidateObjects[j].LastUseTime)
                        {
                            T temp = candidateObjects[i];
                            candidateObjects[i] = candidateObjects[j];
                            candidateObjects[j] = temp;
                        }
                    }

                    m_CachedToReleaseObjects.Add(candidateObjects[i]);
                    toReleaseCount--;
                }

                return m_CachedToReleaseObjects;
            }

            public void SetPriority(T obj, int priority)
            {
                
            }

            public void SetPriority(object target, int priority)
            {
                
            }
        }
    }
}

