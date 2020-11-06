// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-11-06 15:49:42
// ========================================================

namespace LF
{
    /// <summary>
    /// 任务基类。
    /// </summary>
    public class TaskBase : IReference
    {
        private int m_SerialId;
        private bool m_Done;

        /// <summary>
        /// 初始化任务基类的新实例。
        /// </summary>
        public TaskBase()
        {
            m_SerialId = 0;
            m_Done = false;
        }

        /// <summary>
        /// 获取任务的序列编号。
        /// </summary>
        public int SerialId
        {
            get
            {
                return m_SerialId;
            }
        }

        /// <summary>
        /// 获取或设置任务是否完成。
        /// </summary>
        public bool Done
        {
            get
            {
                return m_Done;
            }
            set
            {
                m_Done = value;
            }
        }

        /// <summary>
        /// 获取任务描述。
        /// </summary>
        public virtual string Description
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 初始化任务基类。
        /// </summary>
        /// <param name="serialId">任务的序列编号。</param>
        internal void Initialize(int serialId)
        {
            m_SerialId = serialId;
            m_Done = false;
        }

        /// <summary>
        /// 清理任务基类。
        /// </summary>
        public virtual void Clear()
        {
            m_SerialId = 0;
            m_Done = false;
        }
    }
}

