/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/06/01
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using LT;
using LT.Net;
using LF;

/// <summary>
/// 会话
/// </summary>
public class Session
{
    private MonoBehaviour monoDriver;

    private UdpClient udpClient;
    private KChannel channel;
    private MessageHeader header = new MessageHeader();
    private LTTaskCompletionSource<ErrorCode> tcs;
    private Coroutine coroutineSendHeart;
    private Coroutine coroutineRefreshPing;
    private List<uint> timeStampList = new List<uint>();

    public Action<byte[]> OnReadEventHandler { get; set; }
    public Action<ErrorCode> OnErrorEventHandler { get; set; }
    public IPEndPoint RemoteEndPoint { get; private set; }
    public bool IsConnected { get; private set; }
    public uint Ping { get; private set; }

    public Session(MonoBehaviour monoDriver)
    {
        this.monoDriver = monoDriver;
    }

    public void Update()
    {
        if (channel != null)
        {
            //Debug.Log("update");
            channel.Update();
        }
    }

    public LTTask<ErrorCode> Connect(IPEndPoint remoteEndPoint)
    {
        this.tcs = new LTTaskCompletionSource<ErrorCode>();
        this.IsConnected = false;
        this.RemoteEndPoint = remoteEndPoint;
        uint conv = ConvGenerator.Conv();

        Log.Debug($"connect:new conv:{conv}");

        //#if IPV4
        this.udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
        //#elif IPV6
        //        this.udpClient = new UdpClient(new IPEndPoint(IPAddress.IPv6Any, 0));
        //#endif
        this.udpClient.BeginReceive(OnBeginReceive, null);
        this.channel = new KChannel(conv, udpClient, remoteEndPoint) { Name = "Client" };
        this.channel.OnReadCallback = OnKcpRead;
        this.channel.OnErrorCallback = OnKcpError;

        //立刻发送连接请求
        byte hid = 1;
        IMessage msg = MessagesFactory.Connect(hid);
        Send(msg.Encode());

        this.monoDriver.StartCoroutine(SimulationRecvConnected());
        return this.tcs.Task;
    }

    /// <summary>
    /// 模拟收到连接包，为了兼容1.3.1旧版本
    /// </summary>
    IEnumerator SimulationRecvConnected()
    {
        yield return new WaitForSeconds(1f);
        OnConnectedSuccess();
    }

    /// <summary>
    /// udp接收方法
    /// </summary>
    private void OnBeginReceive(IAsyncResult ar)
    {
        try
        {
            IPEndPoint remoteEndPoint = null;
            byte[] cache = udpClient.EndReceive(ar, ref remoteEndPoint);
            if (cache.Length < KCP.IKCP_OVERHEAD)
            {
                udpClient.BeginReceive(OnBeginReceive, null);
                return;
            }

            this.channel.HandleRecv(cache);
            this.udpClient.BeginReceive(OnBeginReceive, null);
        }
        catch (Exception e)
        {
            //ingored
            Log.Debug($"可忽略的日志堆栈:{e}");
        }
    }

    /// <summary>
    /// 重连
    /// </summary>
    public async void Reconnect()
    {
        Log.Debug("Reconnect.");
        await Connect(RemoteEndPoint);
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void Disconnect()
    {
        if (!IsConnected) return;

        this.IsConnected = false;
        byte hid = 1;
        IMessage msg = MessagesFactory.Disconnect(hid);
        Send(msg.Encode());
    }

    /// <summary>
    /// 连接成功
    /// </summary>
    private void OnConnectedSuccess()
    {
        if (tcs.Task.IsCompleted)
        {
            Log.Debug("OnKcpRead task is completed.");
            return;
        }

        Log.Debug("Connect success.");

        this.IsConnected = true;
        this.tcs.SetResult(ErrorCode.KcpConnectSuccess);

        Loom.QueueOnMainThread(() =>
        {
            if(coroutineSendHeart != null)
            {
                monoDriver.StopCoroutine(coroutineSendHeart);
            }
            if(coroutineRefreshPing != null)
            {
                monoDriver.StopCoroutine(coroutineRefreshPing);
            }

            coroutineSendHeart = monoDriver.StartCoroutine(SendHeart());
            coroutineRefreshPing = monoDriver.StartCoroutine(RefreshPing());
        });
    }

    /// <summary>
    /// 内部解析
    /// </summary>
    private void OnKcpRead(KChannel channel, byte[] bytes)
    {
        //处理连接
        if (!this.IsConnected)
        {
            //兼容1.3.1旧版本时，改用SimulationRecvConnected
            //OnConnectedSuccess();
        }
        else
        {
            //解析数据
            header.Clear();
            header.Decode(bytes, 0);

            if (header.GetMessageType() != MessageType.Msg)
            {
                int ts = (SystemTime.LowClientNow() - header.TimeStamp) / 2;

                Log.Debug("当前延迟:" + ts);

                if (ts < 0) ts = 10;

                timeStampList.Add((uint)ts);
            }

            OnReadEventHandler?.Invoke(bytes);
        }
    }

    private void OnKcpError(KChannel channel, ErrorCode error)
    {
        Log.Error($"error code:{error}");

        //兼容旧版本时，不考虑是否可链接成功
        //if (error == ErrorCode.ERR_KcpCantConnect)
        //{
        //    if (tcs.Task.IsCompleted)
        //    {
        //        Log.Error("OnKcpError task is completed.");
        //        return;
        //    }

        //    tcs.SetResult(error);

        //    Shutdown();
        //    OnErrorEventHandler?.Invoke(error);
        //}
        if (error == ErrorCode.ERR_SocketError)
        {
            if (!tcs.Task.IsCompleted)
            {
                tcs.SetResult(error);
            }

            Shutdown();
            OnErrorEventHandler?.Invoke(error);
        }
        else
        {
            //非指定错误，执行重连
            Shutdown();
            Reconnect();
        }
    }

    #region 协程方法
    IEnumerator SendHeart()
    {
        while (IsConnected)
        {
            //Log.Debug("客户端发送了心跳.");
            byte hid = 1;
            IMessage msg = MessagesFactory.Heart(hid);
            Send(msg.Encode());
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator RefreshPing()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            uint total = 0;
            if (timeStampList.Count > 0)
            {
                foreach (var ts in timeStampList)
                {
                    total += ts;
                }
                uint value = (uint)((float)total / timeStampList.Count);

                //Log.Debug($"刷新延迟计算：total:{total}  count:{timeStampList.Count}  result:{value} ");
                Ping = value > 460 ? 460 : value;
            }
            else
            {
                //Log.Debug("------------------->460");
                Ping = 460;
            }

            timeStampList.Clear();
        }
    }
    #endregion

    public void Send(byte[] buffer)
    {
        if (channel != null)
        {
            channel.Send(buffer);
        }
    }

    public void Shutdown()
    {
        Log.Debug("Session Shutdown.");

        monoDriver.StopCoroutine(this.coroutineSendHeart);
        monoDriver.StopCoroutine(this.coroutineRefreshPing);

        Disconnect();
        IsConnected = false;
        channel?.Dispose();
        channel = null;

        udpClient?.Dispose();
        udpClient = null;
    }

    public void OnDestroy()
    {
        Shutdown();
    }
}