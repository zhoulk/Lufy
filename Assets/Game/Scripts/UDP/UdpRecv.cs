// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-13 11:18:21
// ========================================================

using System;
using System.Net;
using System.Net.Sockets;

namespace LF.UDP
{
    public class UdpRecv : Singleton<UdpRecv>
    {
        private UdpClient client;

        public UdpRecv()
        {

        }

        public void Init(IPEndPoint endPoint)
        {
            client = new UdpClient(endPoint);
            client.BeginReceive(OnRequestReceive, null);
        }

        private void OnRequestReceive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint remoteEndPoint = null;
                byte[] bytes = client.EndReceive(ar, ref remoteEndPoint);
                ReceiveEventHandler?.Invoke(bytes, remoteEndPoint);

                client.BeginReceive(OnRequestReceive, null);
            }
            catch (Exception)
            {
                //ingored
            }
        }

        public void Dispose()
        {
            if(client != null)
            {
                client.Close();
                client = null;
            }
        }

        public Action<byte[], IPEndPoint> ReceiveEventHandler { get; set; }
    }
}

