using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Management;
using System.Windows.Forms;
using System.Threading;
using System.Net.NetworkInformation;

namespace Networks
{
    public class NetFunc
    {
        #region IP 주소 변경하기 - ChangeIPAddress(sourceDescription, sourceIPAddress, sourceSubnetMask, sourceGateway)
        //bool result = ChangeIPAddress("802.11n Wireless LAN Card", "192.168.0.10", "255.255.255.0", "192.168.0.1");  
        /// <summary>
        /// IP 주소 변경하기
        /// </summary>
        /// <param name="sourceDescription">소스 설명</param>
        /// <param name="sourceIPAddress">소스 IP 주소</param>
        /// <param name="sourceSubnetMask">소스 서브넷 마스크</param>
        /// <param name="sourceGateway">소스 게이트웨이</param>
        /// <returns>처리 결과</returns>
        public static bool ChangeIPAddress(string sourceDescription, string sourceIPAddress, string sourceSubnetMask, string sourceGateway)
        {
            ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
            foreach (ManagementObject managementObject in managementObjectCollection)
            {
                string description = managementObject["Description"] as string;
                if (string.Compare(description, sourceDescription, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    try
                    {
                        ManagementBaseObject setGatewaysManagementBaseObject = managementObject.GetMethodParameters("SetGateways");
                        setGatewaysManagementBaseObject["DefaultIPGateway"] = new string[] { sourceGateway };
                        setGatewaysManagementBaseObject["GatewayCostMetric"] = new int[] { 1 };
                        ManagementBaseObject enableStaticManagementBaseObject = managementObject.GetMethodParameters("EnableStatic");
                        enableStaticManagementBaseObject["IPAddress"] = new string[] { sourceIPAddress };
                        enableStaticManagementBaseObject["SubnetMask"] = new string[] { sourceSubnetMask };
                        managementObject.InvokeMethod("EnableStatic", enableStaticManagementBaseObject, null);
                        managementObject.InvokeMethod("SetGateways", setGatewaysManagementBaseObject, null);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        public static List<string> GetLocalIP()
        {
            string localIP = "Not available, please check your network setings!";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            List<string> lstIp = new List<string>();
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    lstIp.Add(localIP);
                    //break;
                }
            }
            return lstIp;
        }

        public static List<string> GetMacAddress()
        {
            List<string> lstMacAddress = new List<string>();

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                string mac = nic.GetPhysicalAddress().ToString();
                int macCnt = mac.Length;
                string dat = null;
                for (int i = 0; i < macCnt; i++)
                {
                    dat += mac[i];
                    if (i % 2 == 1 && i < macCnt - 1)
                    {
                        dat += ':';
                    }

                }
                lstMacAddress.Add(dat);
            }
            return lstMacAddress;
        }


        public static bool Ping(string ip)
        {
            try
            {
                System.Net.NetworkInformation.Ping Pping = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
                options.DontFragment = true; //전송할 데이터를 입력
                string strData = "ECO";
                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(strData);
                int iTimeout = 255;               
                System.Net.NetworkInformation.PingReply PRreply = Pping.Send(System.Net.IPAddress.Parse(ip), iTimeout, buffer, options);
                if (PRreply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    return true;
                }
            }
            catch
            { }
            return false;
        }
    }

    /// <summary>
    /// Ping 관리자
    /// </summary>
    public class PingManager
    {
        #region Field
        /// <summary>
        /// 소켓
        /// </summary>
        protected Socket socket;
        /// <summary>
        /// 열기 여부
        /// </summary>
        protected bool isOpen;
        /// <summary>
        /// 수동 리셋 이벤트
        /// </summary>
        /// <remarks>읽기 완료 여부용</remarks>
        protected ManualResetEvent manualResetEvent;
        /// <summary>
        /// 최근 시퀀스
        /// </summary>
        protected byte lastSequence = 0;
        /// <summary>
        /// Ping 명령
        /// </summary>
        protected byte[] pingCommandByteArray;
        /// <summary>
        /// Ping 결과
        /// </summary>
        protected byte[] pingResultByteArray;
        #endregion
        #region 생성자 - PingManager()
        /// <summary>
        /// 생성자
        /// </summary>
        public PingManager()
        {
            this.pingCommandByteArray = new byte[8];
            this.pingCommandByteArray[0] = 8; // Type
            this.pingCommandByteArray[1] = 0; // Subtype
            this.pingCommandByteArray[2] = 0; // Checksum
            this.pingCommandByteArray[3] = 0;
            this.pingCommandByteArray[4] = 1; // Identifier
            this.pingCommandByteArray[5] = 0;
            this.pingCommandByteArray[6] = 0; // Sequence number
            this.pingCommandByteArray[7] = 0;
            // returned IP Header + optional data
            this.pingResultByteArray = new byte[this.pingCommandByteArray.Length + 1000];
        }
        #endregion
        #region 열기 - Open()
        /// <summary>
        /// 열기
        /// </summary>
        public void Open()
        {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            this.isOpen = true;
            this.manualResetEvent = new ManualResetEvent(false);
        }
        #endregion
        #region 전송하기 - Send(ipAddress, timeOut)
        /// <summary>
        /// 전송하기
        /// </summary>
        /// <param name="ipAddress">IP 주소</param>
        /// <param name="timeOut">타임아웃</param>
        /// <returns>경과 시간</returns>
        public TimeSpan Send(string ipAddress, TimeSpan timeOut)
        {
            while (this.socket.Available > 0)
            {
                this.socket.Receive
                (
                    this.pingResultByteArray,
                    Math.Min(this.socket.Available, this.pingResultByteArray.Length),
                    SocketFlags.None
                );
            }
            this.manualResetEvent.Reset();
            DateTime sendDateTime = DateTime.Now;
            this.pingCommandByteArray[6] = this.lastSequence++; // complete ping command
            SetChecksum(this.pingCommandByteArray);
            int countSent = this.socket.SendTo(this.pingCommandByteArray, new IPEndPoint(IPAddress.Parse(ipAddress), 0));
            this.socket.BeginReceive
            (
                this.pingResultByteArray,
                0,
                this.pingResultByteArray.Length,
                SocketFlags.None,
                new AsyncCallback(Socket_EndReceive),
                null
            );
            if (this.manualResetEvent.WaitOne(timeOut, false)) // 데이터를 수신할 때까지 대기한다.
            {
                if
                (
                    (this.pingResultByteArray[20] == 0) &&
                    (this.pingCommandByteArray[4] == this.pingResultByteArray[24]) &&
                    (this.pingCommandByteArray[5] == this.pingResultByteArray[25]) &&
                    (this.pingCommandByteArray[6] == this.pingResultByteArray[26]) &&
                    (this.pingCommandByteArray[7] == this.pingResultByteArray[27])
                )
                {
                    return DateTime.Now.Subtract(sendDateTime); // 경과 시간 반환
                }
            }
            return TimeSpan.MaxValue;
        }
        #endregion
        #region 닫기 - Close()
        /// <summary>
        /// 닫기
        /// </summary>
        public void Close()
        {
            this.isOpen = false;
            this.socket.Close();
            this.manualResetEvent.Close();
        }
        #endregion
        #region 체크썸 설정하기 - SetChecksum(sourceByteArray)
        /// <summary>
        /// 체크썸 설정하기
        /// </summary>
        /// <param name="sourceByteArray">소스 바이트 배열</param>
        protected void SetChecksum(byte[] sourceByteArray)
        {
            sourceByteArray[2] = 0;
            sourceByteArray[3] = 0;
            uint checksum = 0;
            for (int i = 0; i < this.pingCommandByteArray.Length; i = i + 2)
            {
                checksum += BitConverter.ToUInt16(this.pingCommandByteArray, i);
            }
            checksum = ~((checksum & 0xffffu) + (checksum >> 16));
            sourceByteArray[2] = (byte)checksum;
            sourceByteArray[3] = (byte)(checksum >> 8);
        }
        #endregion
        #region 소켓 수신 완료시 처리하기 - Socket_EndReceive(asyncResult)
        /// <summary>
        /// 소켓 수신 완료시 처리하기
        /// </summary>
        /// <param name="asyncResult">비동기 결과</param>
        protected void Socket_EndReceive(IAsyncResult asyncResult)
        {
            if (this.isOpen)
            {
                try
                {
                    this.socket.EndReceive(asyncResult);
                }
                catch (Exception)
                {
                }
                this.manualResetEvent.Set();
            }
        }
        #endregion
    }
}
