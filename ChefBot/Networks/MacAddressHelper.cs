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
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;


namespace Networks
{
    public class MacAddressHelper
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int SendARP(int desIpValue, int srcIpValue, byte[] physicalAddrArray, ref uint physicalAddrArrayLength);


        private static List<MacIpPair> _macIpPairs;

        public static string GetMacByIp(string ip)
        {
            if (_macIpPairs == null)
            {
                GetAllMacAddressesAndIppairs();
            }

            int index = _macIpPairs.FindIndex(x => x.IpAddress == ip);
            if (index >= 0)
            {
                return _macIpPairs[index].MacAddress.ToUpper();
            }
            else
            {
                return string.Empty;
            }
        }

        private static void GetAllMacAddressesAndIppairs()
        {
            List<MacIpPair> mip = new List<MacIpPair>();
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a ";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string cmdOutput = pProcess.StandardOutput.ReadToEnd();
            string pattern = @"(?<ip>([0-9]{1,3}\.?){4})\s*(?<mac>([a-f0-9]{2}-?){6})";

            foreach (Match m in Regex.Matches(cmdOutput, pattern, RegexOptions.IgnoreCase))
            {
                mip.Add(new MacIpPair()
                {
                    MacAddress = m.Groups["mac"].Value,
                    IpAddress = m.Groups["ip"].Value
                });
            }

            _macIpPairs = mip;
        }

        private struct MacIpPair
        {
            public string MacAddress;
            public string IpAddress;
        }

        public static string Get_MACAddress(string ip)
        {
            IPAddress ipAddr = IPAddress.Parse(ip);
            //List<byte> byteIpArray = new List<byte>();
            byte[] byteIpArray = new byte[6];
            uint uiIP = (uint)byteIpArray.Length;
            int iIpValue = BitConverter.ToInt32(ipAddr.GetAddressBytes(), 0);
            int iReturn = SendARP(iIpValue, 0, byteIpArray, ref uiIP);

            if (iReturn != 0)
            {
                return null;
            }

            string[] strIP = new string[(int)uiIP];
            for (int i = 0; i < uiIP; i++)
            {
                strIP[i] = byteIpArray[i].ToString("X2");
            }
            string strMacAddress = string.Join(":", strIP);
            return strMacAddress;
        }
    }
}
