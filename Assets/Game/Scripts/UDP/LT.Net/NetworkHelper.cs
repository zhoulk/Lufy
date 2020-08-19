/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/05/30
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

using System.Runtime.InteropServices;

namespace LF
{
    public static class NetworkHelper
    {
        [DllImport("__Internal")]
        private static extern void _getUUIDInKeychain();

        public static void getUUIDInKeychain()
        {
            _getUUIDInKeychain();
        }

        public static IPEndPoint ToIPEndPoint(string host, int port)
        {
            return new IPEndPoint(IPAddress.Parse(host), port);
        }

        public static IPEndPoint ToIPEndPoint(string address)
        {
            int index = address.LastIndexOf(':');
            string host = address.Substring(0, index);
            string p = address.Substring(index + 1);
            int port = int.Parse(p);
            return ToIPEndPoint(host, port);
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        public static string LocalAddress()
        {
            string address = "";
#if UNITY_2017
            address = Network.player.ipAddress;
#else
            address = LocalAddress2018OrNewer();
#endif
            return address;
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns>本机IP地址</returns>
        private static string LocalAddress2018OrNewer()
        {
            try
            {
                string hostName = System.Net.Dns.GetHostName();
                var ipEntry = System.Net.Dns.GetHostEntry(hostName);
                for (int i = 0; i < ipEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (ipEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ipEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string PhysicalAddress = "";

        /// <summary>
        /// 获取本机Mac地址
        /// </summary>
        /// <returns>返回mac地址</returns>
        public static string GetMacAddress()
        {
#if !UNITY_IOS
            //PhysicalAddress = App.Make<ISetting>().GetString("mac", "");

            if (PhysicalAddress == "")
            {
                NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adaper in nice)
                {
                    //Debug.LogError("111|" + adaper.Description + "|" + adaper.GetPhysicalAddress().ToString());

                    if (adaper.Description == "en0")
                    {
                        PhysicalAddress = adaper.GetPhysicalAddress().ToString();
                        break;
                    }
                    else
                    {
                        PhysicalAddress = adaper.GetPhysicalAddress().ToString();

                        if (PhysicalAddress != "")
                        {
                            break;
                        }
                    }
                }

                //App.Make<ISetting>().SetString("mac", PhysicalAddress);
            }

            return PhysicalAddress;
#else
            return PhysicalAddress;
#endif
        }
    }
}