// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-23 17:48:22
// ========================================================
using LF.Pattern.Cmd;
using UnityEngine;

class ACmd : ICommand
{
    public void Execute(object userData)
    {
        Debug.LogFormat("excute A {0}", userData);
    }

    public void Undo(object preUserData, object userData)
    {
        Debug.LogFormat("undo A {0} {1}", preUserData, userData);
    }
}

class BCmd : ICommand
{
    public void Execute(object userData)
    {
        Debug.Log("excute B");
    }

    public void Undo(object preUserData, object userData)
    {
        Debug.Log("undo B");
    }
}

class CCmd : ICommand
{
    public void Execute(object userData)
    {
        Debug.Log("excute C");
    }

    public void Undo(object preUserData, object userData)
    {
        Debug.Log("undo C");
    }
}

public class CommandTest : MonoBehaviour
{
    CommandManager m_CommandManager = null;

    // Start is called before the first frame update
    void Start()
    {
        m_CommandManager = GetComponent<CommandManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_CommandManager.ExecuteCmd(new ACmd(), "A Param");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_CommandManager.ExecuteCmd(new ACmd(), "Q Param");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            m_CommandManager.ExecuteCmd(new BCmd(), "B Param");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            m_CommandManager.ExecuteCmd(new CCmd(), "C Param");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_CommandManager.Undo("Z Param");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //m_CommandManager.ExecuteCmd(EmptyCmd.Create(), null);
            m_CommandManager.ExecuteEmptyCmd();
        }
    }
}
