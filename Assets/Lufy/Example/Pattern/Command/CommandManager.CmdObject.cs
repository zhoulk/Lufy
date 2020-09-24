// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-24 08:55:36
// ========================================================
using LF;

namespace LF.Pattern.Cmd
{
    public partial class CommandManager : LufyManager
    {
        public class CmdObject : IReference
        {
            private ICommand m_Cmd;
            private object m_userData;

            public CmdObject()
            {
                Cmd = null;
                UserData = null;
            }

            public ICommand Cmd
            {
                get;
                private set;
            }

            public object UserData
            {
                get;
                private set;
            }

            public static CmdObject Create(ICommand cmd, object userData)
            {
                CmdObject cmdObject = ReferencePool.Acquire<CmdObject>();
                cmdObject.Cmd = cmd;
                cmdObject.UserData = userData;
                return cmdObject;
            }

            public void Clear()
            {
                Cmd = null;
                UserData = null;
            }
        }
    }
}

