using LF;
using LF.UDP;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace Lufy
{
    public static class UDPFunc
    {
        static IPEndPoint targetEndPoint = null;
        static Session session = null;

        [MenuItem("Lufy/Example/12. UDP/Search", false, 0)]
        private static void SearchClicked()
        {
            UdpRecv.Instance.Init(new IPEndPoint(IPAddress.Any, 7777));
            UdpRecv.Instance.ReceiveEventHandler = (bytes, endPoint) =>
            {
                string info = System.Text.Encoding.UTF8.GetString(bytes);
                Log.Debug("{0} {1} ----- {2}", endPoint.Address.ToString(), endPoint, info);

                if (endPoint.Address.ToString().Equals("172.16.4.112"))
                {
                    targetEndPoint = endPoint;
                    Log.Debug("search finish");
                    StopClicked();
                }
            };
        }

        [MenuItem("Lufy/Example/12. UDP/Stop", false, 1)]
        private static void StopClicked()
        {
            UdpRecv.Instance.Dispose();
        }

        [MenuItem("Lufy/Example/12. UDP/Connect", false, 2)]
        private static void ConnectClicked()
        {
            //session = new Session(this);
            //session.Connect(targetEndPoint);
        }
    }
}

