// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-23 17:51:17
// ========================================================

using LF;
using System.Collections.Generic;

namespace LF.Pattern.Cmd
{
    /// <summary>
    /// 命令管理
    /// </summary>
    public partial class CommandManager : LufyManager
    {
        Dictionary<string, LinkedList<CmdObject>> m_CmdDic = new Dictionary<string, LinkedList<CmdObject>>();

        public void ExecuteEmptyCmd()
        {
            ExecuteEmptyCmd(string.Empty);
        }

        public void ExecuteEmptyCmd(string group)
        {
            ExecuteCmd(EmptyCmd.Create(), group, null);
        }

        public void ExecuteCmd(ICommand cmd, object userData)
        {
            ExecuteCmd(cmd, string.Empty, userData);
        }

        public void ExecuteCmd(ICommand cmd, string group, object userData)
        {
            LinkedList<CmdObject> cmdList;
            if (!m_CmdDic.TryGetValue(group, out cmdList))
            {
                cmdList = new LinkedList<CmdObject>();
                m_CmdDic.Add(group, cmdList);
            }
            CmdObject cmdObject = CmdObject.Create(cmd, userData);
            cmdList.AddLast(cmdObject);

            cmd.Execute(userData);
        }

        public void Undo(object userData)
        {
            Undo(string.Empty, userData);
        }

        public void Undo(string group, object userData)
        {
            LinkedList<CmdObject> cmdList;
            if (!m_CmdDic.TryGetValue(group, out cmdList))
            {
                return;
            }
            if (cmdList.Count > 0)
            {
                CmdObject cmdObject = cmdList.Last.Value;
                cmdObject.Cmd.Undo(cmdObject.UserData, userData);
                cmdList.RemoveLast();
                ReferencePool.Release(cmdObject);
            }
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }

        internal override void Shutdown()
        {

        }
    }
}
