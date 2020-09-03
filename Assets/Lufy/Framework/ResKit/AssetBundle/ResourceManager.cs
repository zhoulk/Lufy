// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-02 09:26:09
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LF.Res
{
    public class ResourceManager
    {
        private AssetBundleManager m_AssetBundleManager = null;

        //缓存使用的资源列表
        public Dictionary<uint, ResouceItem> AssetDic { get; set; } = new Dictionary<uint, ResouceItem>();
        //缓存引用计数为零的资源列表，达到缓存最大的时候释放这个列表里面最早没用的资源
        protected CMapList<ResouceItem> m_NoRefrenceAssetMapList = new CMapList<ResouceItem>();

        private bool m_EditorResource = false;

        //最大缓存个数
        private int m_MaxCacheCount = 500;

        public ResourceManager()
        {
            m_AssetBundleManager = new AssetBundleManager();
            m_AssetBundleManager.LoadAssetBundleConfig();

            m_EditorResource = Lufy.GetManager<ResManager>().EditorResource;
            m_MaxCacheCount = Lufy.GetManager<ResManager>().MaxCacheCount;
        }

        /// <summary>
        /// 同步资源加载，外部直接调用，仅加载不需要实例化的资源，例如Texture,音频等等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadResource<T>(string path) where T : UnityEngine.Object
        {
            if (m_EditorResource)
            {
                return LoadResourceFromEditor<T>(path);
            }
            else
            {
                return LoadResourceFromAssetBundle<T>(path);
            }
        }

        /// <summary>
        /// 不需要实例化的资源的卸载，根据对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="destoryObj"></param>
        /// <returns></returns>
        public bool ReleaseResouce(Object obj, bool destoryObj = false)
        {
            if (obj == null)
            {
                return false;
            }

            ResouceItem item = null;
            foreach (ResouceItem res in AssetDic.Values)
            {
                if (res.m_Guid == obj.GetInstanceID())
                {
                    item = res;
                }
            }

            if (item == null)
            {
                Debug.LogError("AssetDic里不存在改资源：" + obj.name + "  可能释放了多次");
                return false;
            }

            item.RefCount--;

            DestoryResouceItem(item, destoryObj);
            return true;
        }

        /// <summary>
        /// 从编辑器读取资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        private T LoadResourceFromEditor<T>(string path) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            uint crc = Crc32.GetCrc32(path);
            ResouceItem item = GetCacheResouceItem(crc);
            if (item == null)
            {
                item = new ResouceItem();
                item.m_Crc = crc;
                T obj = LoadAssetByEditor<T>(path);
                item.m_Obj = obj;
            }

            CacheResource(path, item);
            return item.m_Obj as T;
        }

        /// <summary>
        /// 从AssetBundle读取资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        private T LoadResourceFromAssetBundle<T>(string path) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            uint crc = Crc32.GetCrc32(path);
            ResouceItem item = GetCacheResouceItem(crc);
            if (item != null)
            {
                return item.m_Obj as T;
            }

            T obj = null;
            item = m_AssetBundleManager.LoadResouceAssetBundle(crc);
            if (item != null && item.m_AssetBundle != null)
            {
                if (item.m_Obj != null)
                {
                    obj = item.m_Obj as T;
                }
                else
                {
                    obj = item.m_AssetBundle.LoadAsset<T>(item.m_AssetName);
                }
                item.m_Crc = crc;
                item.m_Obj = obj;
            }
            CacheResource(path, item);
            return obj;
        }

        protected T LoadAssetByEditor<T>(string path) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#else
            return null;
#endif
        }

        /// <summary>
        /// 缓存加载的资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="item"></param>
        /// <param name="addrefcount"></param>
        void CacheResource(string path, ResouceItem item, int addrefcount = 1)
        {
            //缓存太多，清除最早没有使用的资源
            WashOut();

            if (item == null)
            {
                Debug.LogError("ResouceItem is null, path: " + path);
            }

            if (item.m_Obj == null)
            {
                Debug.LogError("object is null, path: " + path);
            }

            item.m_Guid = item.m_Obj.GetInstanceID();
            item.m_LastUseTime = Time.realtimeSinceStartup;
            item.RefCount += addrefcount;
            if (AssetDic.ContainsKey(item.m_Crc))
            {
                AssetDic[item.m_Crc] = item;
            }
            else
            {
                AssetDic.Add(item.m_Crc, item);
            }
        }

        /// <summary>
        /// 从资源池获取缓存资源
        /// </summary>
        /// <param name="crc"></param>
        /// <param name="addrefcount"></param>
        /// <returns></returns>
        ResouceItem GetCacheResouceItem(uint crc, int addrefcount = 1)
        {
            ResouceItem item = null;
            if (AssetDic.TryGetValue(crc, out item))
            {
                if (item != null)
                {
                    item.RefCount += addrefcount;
                    item.m_LastUseTime = Time.realtimeSinceStartup;
                }
            }

            return item;
        }

        /// <summary>
        /// 回收一个资源
        /// </summary>
        /// <param name="item"></param>
        /// <param name="destroy"></param>
        protected void DestoryResouceItem(ResouceItem item, bool destroyCache = false)
        {
            if (item == null || item.RefCount > 0)
            {
                return;
            }

            if (!destroyCache)
            {
                m_NoRefrenceAssetMapList.InsertToHead(item);
                return;
            }

            if (!AssetDic.Remove(item.m_Crc))
            {
                return;
            }

            m_NoRefrenceAssetMapList.Remove(item);

            //释放assetbundle引用
            m_AssetBundleManager.ReleaseAsset(item);

            if (item.m_Obj != null)
            {
                item.m_Obj = null;
#if UNITY_EDITOR
                Resources.UnloadUnusedAssets();
#endif
            }
        }

        /// <summary>
        /// 缓存太多，清除最早没有使用的资源
        /// </summary>
        protected void WashOut()
        {
            //当大于缓存个数时，进行一半释放
            Debug.Log("washout " + m_NoRefrenceAssetMapList.Size() + "  " + m_MaxCacheCount);
            while (m_NoRefrenceAssetMapList.Size() >= m_MaxCacheCount)
            {
                for (int i = 0; i < m_MaxCacheCount / 2; i++)
                {
                    ResouceItem item = m_NoRefrenceAssetMapList.Back();
                    DestoryResouceItem(item, true);
                }
            }
        }
    }

    //双向链表结构节点
    public class DoubleLinkedListNode<T> : IReference where T : class, new()
    {
        //前一个节点
        public DoubleLinkedListNode<T> prev = null;
        //后一个节点
        public DoubleLinkedListNode<T> next = null;
        //当前节点
        public T t = null;

        public void Clear()
        {
            prev = null;
            next = null;
            t = null;
        }
    }

    //双向链表结构
    public class DoubleLinedList<T> where T : class, new()
    {
        //表头
        public DoubleLinkedListNode<T> Head = null;
        //表尾
        public DoubleLinkedListNode<T> Tail = null;
        //个数
        protected int m_Count = 0;
        public int Count
        {
            get { return m_Count; }
        }

        /// <summary>
        /// 添加一个节点到头部
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public DoubleLinkedListNode<T> AddToHeader(T t)
        {
            DoubleLinkedListNode<T> pList = ReferencePool.Acquire<DoubleLinkedListNode<T>>();
            pList.next = null;
            pList.prev = null;
            pList.t = t;
            return AddToHeader(pList);
        }

        /// <summary>
        /// 添加一个节点到头部
        /// </summary>
        /// <param name="pNode"></param>
        /// <returns></returns>
        public DoubleLinkedListNode<T> AddToHeader(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null)
                return null;

            pNode.prev = null;
            if (Head == null)
            {
                Head = Tail = pNode;
            }
            else
            {
                pNode.next = Head;
                Head.prev = pNode;
                Head = pNode;
            }
            m_Count++;
            return Head;
        }

        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public DoubleLinkedListNode<T> AddToTail(T t)
        {
            DoubleLinkedListNode<T> pList = ReferencePool.Acquire<DoubleLinkedListNode<T>>();
            pList.next = null;
            pList.prev = null;
            pList.t = t;
            return AddToTail(pList);
        }

        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        /// <param name="pNode"></param>
        /// <returns></returns>
        public DoubleLinkedListNode<T> AddToTail(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null)
                return null;

            pNode.next = null;
            if (Tail == null)
            {
                Head = Tail = pNode;
            }
            else
            {
                pNode.prev = Tail;
                Tail.next = pNode;
                Tail = pNode;
            }
            m_Count++;
            return Tail;
        }

        /// <summary>
        /// 移除某个节点
        /// </summary>
        /// <param name="pNode"></param>
        public void RemoveNode(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null)
                return;

            if (pNode == Head)
                Head = pNode.next;

            if (pNode == Tail)
                Tail = pNode.prev;

            if (pNode.prev != null)
                pNode.prev.next = pNode.next;

            if (pNode.next != null)
                pNode.next.prev = pNode.prev;

            ReferencePool.Release(pNode);
            m_Count--;
        }

        /// <summary>
        /// 把某个节点移动到头部
        /// </summary>
        /// <param name="pNode"></param>
        public void MoveToHead(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null || pNode == Head)
                return;

            if (pNode.prev == null && pNode.next == null)
                return;

            if (pNode == Tail)
                Tail = pNode.prev;

            if (pNode.prev != null)
                pNode.prev.next = pNode.next;

            if (pNode.next != null)
                pNode.next.prev = pNode.prev;

            pNode.prev = null;
            pNode.next = Head;
            Head.prev = pNode;
            Head = pNode;
            if (Tail == null)
            {
                Tail = Head;
            }
        }
    }

    public class CMapList<T> where T : class, new()
    {
        DoubleLinedList<T> m_DLink = new DoubleLinedList<T>();
        Dictionary<T, DoubleLinkedListNode<T>> m_FindMap = new Dictionary<T, DoubleLinkedListNode<T>>();

        ~CMapList()
        {
            Clear();
        }

        /// <summary>
        /// 情况列表
        /// </summary>
        public void Clear()
        {
            while (m_DLink.Tail != null)
            {
                Remove(m_DLink.Tail.t);
            }
        }

        /// <summary>
        /// 插入一个节点到表头
        /// </summary>
        /// <param name="t"></param>
        public void InsertToHead(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (m_FindMap.TryGetValue(t, out node) && node != null)
            {
                m_DLink.AddToHeader(node);
                return;
            }
            m_DLink.AddToHeader(t);
            m_FindMap.Add(t, m_DLink.Head);
        }

        /// <summary>
        /// 从表尾弹出一个结点
        /// </summary>
        public void Pop()
        {
            if (m_DLink.Tail != null)
            {
                Remove(m_DLink.Tail.t);
            }
        }

        /// <summary>
        /// 删除某个节点
        /// </summary>
        /// <param name="t"></param>
        public void Remove(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (!m_FindMap.TryGetValue(t, out node) || node == null)
            {
                return;
            }
            m_DLink.RemoveNode(node);
            m_FindMap.Remove(t);
        }

        /// <summary>
        /// 获取到尾部节点
        /// </summary>
        /// <returns></returns>
        public T Back()
        {
            return m_DLink.Tail == null ? null : m_DLink.Tail.t;
        }

        /// <summary>
        /// 返回节点个数
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return m_FindMap.Count;
        }

        /// <summary>
        /// 查找是否存在该节点
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Find(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (!m_FindMap.TryGetValue(t, out node) || node == null)
                return false;

            return true;
        }

        /// <summary>
        /// 刷新某个节点，把节点移动到头部
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Reflesh(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (!m_FindMap.TryGetValue(t, out node) || node == null)
                return false;

            m_DLink.MoveToHead(node);
            return true;
        }
    }
}
