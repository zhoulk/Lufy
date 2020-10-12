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
    public delegate void OnAssetLoadComplete(string path, object obj);

    public class AssetLoadAsyncOp : IReference
    {
        public string Path;
        public string AssetName;
        public OnAssetLoadComplete CompleteAction;
        public bool isLoading;
        public AssetBundleRequest Oper;
        public AssetBundle AssetBundle;

        public void Clear()
        {
            Path = string.Empty;
            AssetName = string.Empty;
            CompleteAction = null;
            isLoading = false;
            Oper = null;
            AssetBundle = null;
        }
    }

    public class AssetManager
    {
        private AssetBundleManager m_AssetBundleManager = null;

        //缓存使用的资源列表
        public Dictionary<uint, AssetItem> AssetDic { get; set; } = new Dictionary<uint, AssetItem>();
        //缓存引用计数为零的资源列表，达到缓存最大的时候释放这个列表里面最早没用的资源
        protected CMapList<AssetItem> m_NoRefrenceAssetMapList = new CMapList<AssetItem>();
        
        protected List<AssetLoadAsyncOp> waitingList = new List<AssetLoadAsyncOp>();
        protected List<AssetLoadAsyncOp> loadingList = new List<AssetLoadAsyncOp>();

        private bool m_EditorResource = false;

        //最大缓存个数
        private int m_MaxCacheCount = 0;

        public AssetManager()
        {
            m_AssetBundleManager = new AssetBundleManager();
            m_AssetBundleManager.LoadAssetBundleConfig();

            m_EditorResource = Lufy.GetManager<ResManager>().EditorResource;
            //m_MaxCacheCount = Lufy.GetManager<ResManager>().MaxCacheCount;
        }

        /// <summary>
        /// 异步资源加载，外部直接调用，仅加载不需要实例化的资源，例如Texture,音频等等
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void LoadAsset(string path, OnAssetLoadComplete complete)
        {
            if (m_EditorResource)
            {
                LoadAssetFromEditor(path, complete);
            }
            else
            {
                //LoadAssetFromAssetBundle(path, complete);
                AssetLoadAsyncOp op = ReferencePool.Acquire<AssetLoadAsyncOp>();
                op.Path = path;
                op.CompleteAction = complete;
                waitingList.Add(op);
            }
        }

        /// <summary>
        /// 不需要实例化的资源的卸载，根据对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ReleaseAsset(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            AssetItem item = null;
            foreach (AssetItem res in AssetDic.Values)
            {
                if (res.Asset == obj)
                {
                    item = res;
                }
            }

            if (item == null)
            {
                Log.Error("AssetDic里不存在改资源：" + obj + "  可能释放了多次");
                return false;
            }

            item.RefCount--;

            DestoryAssetItem(item);
            return true;
        }

        /// <summary>
        /// 从编辑器读取资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        private void LoadAssetFromEditor(string path, OnAssetLoadComplete complete)
        {
            if (string.IsNullOrEmpty(path))
            {
                if(complete != null)
                {
                    complete(path, null);
                }
                return;
            }
            uint crc = Crc32.GetCrc32(path);
            AssetItem item = GetCacheAssetItem(crc);
            if (item == null)
            {
                item = ReferencePool.Acquire<AssetItem>();
#if UNITY_EDITOR
                object obj = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(path);
                item.Asset = obj;
#endif
            }

            CacheAsset(path, item);
            if (complete != null)
            {
                complete(path, item.Asset);
            }
        }

        /// <summary>
        /// 从AssetBundle读取资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private void LoadAssetFromAssetBundle(string path, OnAssetLoadComplete complete)
        {
            if (string.IsNullOrEmpty(path))
            {
                if (complete != null)
                {
                    complete(path, null);
                }
                return;
            }

            uint crc = Crc32.GetCrc32(path);
            AssetItem item = GetCacheAssetItem(crc);
            if (item != null && item.Asset != null)
            {
                if (complete != null)
                {
                    complete(path, item.Asset);
                }
                return;
            }

            ConfigItem configItem = m_AssetBundleManager.LoadConfigItem(crc);
            AssetBundle bundle = m_AssetBundleManager.LoadAssetBundle(crc);
            if (bundle != null)
            {
                //bundle.LoadAssetAsync(configItem.AssetName);
                AssetLoadAsyncOp op = ReferencePool.Acquire<AssetLoadAsyncOp>();
                op.AssetBundle = bundle;
                op.CompleteAction = complete;
                op.isLoading = false;
                op.Oper = null;
                op.Path = path;
                op.AssetName = configItem.AssetName;
                loadingList.Add(op);
            }
        }

        /// <summary>
        /// 缓存加载的资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="item"></param>
        /// <param name="addrefcount"></param>
        void CacheAsset(string path, AssetItem item, int addrefcount = 1)
        {
            if (item == null)
            {
                Log.Error("ResouceItem is null, path: " + path);
            }

            if (item.Asset == null)
            {
                Log.Error("object is null, path: " + path);
            }

            item.Crc = Crc32.GetCrc32(path);
            item.LastUseTime = Time.realtimeSinceStartup;
            item.RefCount += addrefcount;
            if (AssetDic.ContainsKey(item.Crc))
            {
                AssetDic[item.Crc] = item;
            }
            else
            {
                AssetDic.Add(item.Crc, item);
            }
        }

        /// <summary>
        /// 从资源池获取缓存资源
        /// </summary>
        /// <param name="crc"></param>
        /// <param name="addrefcount"></param>
        /// <returns></returns>
        AssetItem GetCacheAssetItem(uint crc, int addrefcount = 1)
        {
            AssetItem item = null;
            if (AssetDic.TryGetValue(crc, out item))
            {
                if (item != null)
                {
                    item.RefCount += addrefcount;
                    item.LastUseTime = Time.realtimeSinceStartup;
                }
            }

            return item;
        }

        /// <summary>
        /// 回收一个资源
        /// </summary>
        /// <param name="item"></param>
        protected void DestoryAssetItem(AssetItem item)
        {
            Debug.Log(item + " count = " + item.RefCount);
            // 判断计数
            if (item == null || item.RefCount > 0)
            {
                return;
            }
            // 移除缓存
            if (!AssetDic.Remove(item.Crc))
            {
                return;
            }

            // 施放
            m_NoRefrenceAssetMapList.Remove(item);

#if !UNITY_EDITOR
            //释放assetbundle引用
            ConfigItem configItem = m_AssetBundleManager.LoadConfigItem(item.Crc);
            m_AssetBundleManager.ReleaseAsset(configItem);
#endif

            if (item.Asset != null)
            {
                item.Asset = null;
#if UNITY_EDITOR
                Resources.UnloadUnusedAssets();
#endif
            }

            ReferencePool.Release(item);
        }

        /// <summary>
        /// 缓存太多，清除最早没有使用的资源
        /// </summary>
        //protected void WashOut()
        //{
        //    //当大于缓存个数时，进行一半释放
        //    Debug.Log("washout " + m_NoRefrenceAssetMapList.Size() + "  " + m_MaxCacheCount);
        //    while (m_NoRefrenceAssetMapList.Size() >= m_MaxCacheCount)
        //    {
        //        for (int i = 0; i < m_MaxCacheCount / 2; i++)
        //        {
        //            AssetItem item = m_NoRefrenceAssetMapList.Back();
        //            DestoryAssetItem(item, true);
        //        }
        //    }
        //}

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            m_AssetBundleManager.OnUpdate(elapseSeconds, realElapseSeconds);

            if(loadingList.Count > 0)
            {
                AssetLoadAsyncOp op = loadingList[0];
                if(op.isLoading == true)
                {
                    if (op.Oper.isDone)
                    {
                        AssetItem item = ReferencePool.Acquire<AssetItem>();
                        item.Asset = op.Oper.asset;
                        CacheAsset(op.Path, item);
                        if(op.CompleteAction != null)
                        {
                            op.CompleteAction(op.Path, op.Oper.asset);
                        }

                        loadingList.RemoveAt(0);
                        ReferencePool.Release(op);
                    }
                }
                else
                {
                    op.isLoading = true;
                    //Debug.Log("load asset " + op.AssetName + "  " + op.AssetBundle);
                    op.Oper = op.AssetBundle.LoadAssetAsync(op.AssetName);
                }
            }
            else
            {
                if(waitingList.Count > 0)
                {
                    AssetLoadAsyncOp op = waitingList[0];
                    LoadAssetFromAssetBundle(op.Path, op.CompleteAction);
                    waitingList.RemoveAt(0);
                }
            }
        }

        public class AssetItem : IReference
        {
            public uint Crc;
            public object Asset;
            public int RefCount;
            //资源最后所使用的时间
            public float LastUseTime;

            public void Clear()
            {
                Crc = 0;
                Asset = null;
                RefCount = 0;
                LastUseTime = 0;
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
