// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-14 14:50:58
// ========================================================
using LF;
using LF.Net;
using System.Net;

public class UDPManager : MonoSingleton<UDPManager>
{
    public Session session;

    public void Connect(IPEndPoint targetEndPoint)
    {
        if(session == null)
        {
            session = new Session(this);
        }
        session.Connect(targetEndPoint);
    }

    public void Send(IMessage msg)
    {
        if(session != null)
        {
            session.Send(msg.Encode());
        }
    }

    public void DisConnect()
    {
        if(session != null)
        {
            session.Shutdown();
        }
        session = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (session != null)
        {
            session.Update();
        }

        Loom.Current.Update();
    }
}
