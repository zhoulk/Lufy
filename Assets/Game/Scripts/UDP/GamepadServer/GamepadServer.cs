/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/12/11
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using LF.Net;
using LF;

namespace LF.GamepadServer
{
    /// <summary>
    /// 手柄Server
    /// </summary>
    public class GamepadServer : IGamepadServer
    {
        private KService kServer;
        private UdpClient udpClient;
        private IPackager packager;
        private MonoBehaviour monoDriver;

        private List<KChannel> removeChannels;
        /// <summary>
        /// 构造函数
        /// </summary>
        public GamepadServer(MonoBehaviour monoDriver)
        {
            this.monoDriver = monoDriver;
            this.removeChannels = new List<KChannel>();

            InitKcpServer();
            InitUdpServer();
        }

        /// <summary>
        /// 初始化UdpServer
        /// </summary>
        private void InitUdpServer()
        {
            string msg = $"{NetworkHelper.LocalAddress()}-3900-Unity-Unity {UnityEngine.Application.productName}";

            this.udpClient = new UdpClient();
            this.monoDriver.StartCoroutine(OnBroadcastMessage(msg, new IPEndPoint(IPAddress.Broadcast, 7777)));
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        IEnumerator OnBroadcastMessage(string content, IPEndPoint remoteEndPoint)
        {
            while (true)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(content);
                udpClient.Send(bytes, bytes.Length, remoteEndPoint);
                yield return new WaitForSeconds(2f);
            }
        }

        private void InitKcpServer()
        {
            this.packager = new CShaperPackager();
            this.kServer = new KService(new IPEndPoint(IPAddress.Any, 3900))
            {
                OnReadCallback = this.OnServiceReadCallback,
                OnErrorCallback = this.OnServiceErrorCallback
            };
        }

        //读取
        private void OnServiceReadCallback(KChannel channel, byte[] buf)
        {
            IMessage msg = packager.Decode(buf);
            //Debug.LogError("Server:" + msg.GetMessageType() + " size:" + buf.Length);

            //数据回传客户端
            channel.Send(buf);
            //application.Trigger(MultiInputEvents.GamepadMessage, msg);
            OnGamepadMessage(msg);
        }

        private void OnServiceErrorCallback(KChannel channel, ErrorCode error)
        {
            Debug.Log($"Service channel:{channel.Conv}  error:{error}");
            this.removeChannels.Add(channel);
        }

        /// <summary>
        /// 广播数据到所有已连接手柄
        /// </summary>
        /// <param name="json">数据</param>
        public void BroadcastToGamepad(byte[] bytes)
        {
            kServer.BoardcastSend(bytes);
        }

        /// <summary>
        /// Monobehvior Update
        /// </summary>
        public void Update()
        {

            if (kServer != null)
            {
                kServer.Update();

                foreach (var c in removeChannels)
                {
                    kServer.RemoveChannel(c.Conv);
                }

                removeChannels.Clear();
            }
        }

        /// <summary>
        /// Monobehvior OnDestroy
        /// </summary>
        public void OnDestroy()
        {
            kServer?.Dispose();
            udpClient?.Close();
        }

        /// <summary>
        /// 输入手柄消息
        /// </summary>
        /// <param name="msg">消息</param>
        private void OnGamepadMessage(IMessage msg)
        {
            Log.Debug("msgType = {0}", msg.GetMessageType());

            switch (msg.GetMessageType())
            {
                case MessageType.Keyboard:
                    MessageKeyboard kb = msg as MessageKeyboard;

                    //Debug.Log($"Keyboard  Hid:{kb.Hid} KeyCode:{kb.KeyCode} State:{kb.State}");
                    //singlePool.Cache(kb.KeyCode, kb.State);
                    //multiPool.Cache(kb.KeyCode, kb.State);
                    break;

                case MessageType.Rocker:
                    MessageRocker r = msg as MessageRocker;
                    //if (Rockers.TryGetValue(r.Hid, out IRocker rocker))
                    //{
                    //    rocker.SetMessage(msg);
                    //}

                    ////Debug.Log($"Rocker  Hid:{r.Hid} KeyCode:{r.KeyCode} State:{r.State}");

                    //singlePool.Cache(r.KeyCode, r.State);
                    //multiPool.Cache(r.KeyCode, r.State);
                    break;

                case MessageType.Gyro:
                    MessageGyro g = msg as MessageGyro;
                    //if (Gyros.TryGetValue(g.Hid, out IGyro gyro))
                    //{
                    //    gyro.SetMessage(msg);
                    //}
                    break;

                case MessageType.BasketBall:
                    MessageBasketBall b = msg as MessageBasketBall;

                    //Log.Debug("power = {0} XAngle = {1}", b.Velocity, b.Torque);

                    GameEntry.Event.Fire(this, BasketBallEventArgs.Create(b));

                    break;
            }
        }
    }
}