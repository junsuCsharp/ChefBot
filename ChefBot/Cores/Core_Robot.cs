using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cores
{
    using Externs;
    using System.Collections.Concurrent;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Core_Robot 
    {
        /*
            * 
            * Usage :
              IP : Robot controller IP address
              port : 502

              Function Code
              0x01 READ_COILS(read output bits)
              0x03 READ_HOLDING_REGISTERS(read output registers)
              0x05 WRITE_SINGLE_COIL(write output bit)
              0x06 WRITE_SINGLE_REGISTER(write output register)
              0x0F WRITE_MULTIPLE_COILS(write multiple output bits)
              0x10 WRITE_MULTIPLE_REGISTERS(write multiple output registers)
              Error Code
              MODBUS_EXCEPTION_ILLEGAL_FUNCTION(1)
              MODBUS_EXCEPTION_ILLEGAL_DATA_ADDRESS(2)
              MODBUS_EXCEPTION_ILLEGAL_DATA_VALUE(3)                

            * Modbus Frame ASKII mode : STX 1byte, DEV 2byte, FC(Function Code) 2byte, QD(Query Data) Nbyte, EC(Error Check) 2byte, ETX 2byte 0x0d, 0x0a
            * 
            * setting file
            * 슬레이브 아이디, 아이피, 포트, 동기 타입
            * 
            * 모드버스 테이블
            *
            * 테스트 바이트
            * 020300000006ff03010e0001
            * 020300000006ff03010f0001
            * 000300000006ff0301100001
            * 000300000006ff0301120001
            * 000300000006ff0301110001
            * 000300000006ff0301230001
            * 000300000006ff03012c0001
            * 000300000006ff0301360001
            * 02 03 00 00 00 06 ff 03 01 0e 00 01
            *
        */

        string strHostIpAddress = null;
        string strClientIpAddress = null;

        NetSoket.Server mHost = new NetSoket.Server();
        ConcurrentQueue<byte> QRecvBuff = new ConcurrentQueue<byte>();
     
        NetSoket.Client mClient = new NetSoket.Client();

        static object lockObject = new object();

        public bool IsConneted = false;

        public enum EFlag
        {
            TransId, Protocol, Length, UnitId, Code, Data, Max

                //public int iTransId = 0;
                //public int iComId = 0;
                //public int iLength = 0;
                //public int iUnitId = 0;
                //public int iCode = 0;
                //public int[] iArryData = new int[] { };
        }

        //통신 데이터 패킷 풀이용
        EFlag comFlag = EFlag.TransId;
        int[] iBuffCount = new int[(int)EFlag.Max];
        //int[] iBuffData = new int[(int)EFlag.Max];
        const int iLen = 2;//통신 데이터 길이 옵셋
        byte[] byteTransId = new byte[iLen];
        byte[] byteComId = new byte[iLen];
        byte[] byteLength = new byte[iLen];

        //통신 데이터 버퍼
        int iBuffTransId = 0;
        int iBuffComId = 0;
        int iBuffLength = 0;
        int iBuffUnitId = 0;
        int iBuffCode = 0;
        int iBuffData = 0;
        byte[] iBuffArryData = new byte[] { };

        public Core_Robot(string strServerIp, string strClientIp)
        {
            //strHostIpAddress = strServerIp;
            //mHost.OnClientAccepted += MHost_OnClientAccepted;
            //mHost.OnClientDisconnect += MHost_OnClientDisconnect;
            //mHost.OnDataReceived += MHost_OnDataReceived;
            //mHost.Start(502);

            strClientIpAddress = strClientIp;
            mClient.OnConnect += MClient_OnConnect;
            mClient.OnDataReceived += MClient_OnDataReceived;
            mClient.OnServerDisconnect += MClient_OnServerDisconnect;
            mClient.Connect(strClientIpAddress, 502);

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = 1;
            //timer.SynchronizingObject = this;
            timer.Elapsed += Timer_Elapsed; 
            timer.Start();
        }

        public void ReConnection()
        {
            if (mClient.IsConnected == false)
            {
                mClient = new NetSoket.Client();
                mClient.OnConnect += MClient_OnConnect;
                mClient.OnDataReceived += MClient_OnDataReceived;
                mClient.OnServerDisconnect += MClient_OnServerDisconnect;
                mClient.Connect(strClientIpAddress, 502);
            }
        }

        private void MClient_OnServerDisconnect()
        {
            //throw new NotImplementedException();
            IsConneted = false;
        }

        private void MClient_OnDataReceived(object[] data)
        {
            foreach (byte tmp in (byte[])data[0])
            {
                QRecvBuff.Enqueue(tmp);
            }
            IsConneted = true;
        }

        private void MClient_OnConnect(bool connected)
        {
            //throw new NotImplementedException();
            IsConneted = connected;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();

            #region 모드버스 서버 수신 처리 부분
            lock (lockObject)
            {
                if (QRecvBuff.IsEmpty == false)
                {
                    int QCount = QRecvBuff.Count;
                    byte[] arrayByte = new byte[QCount];
                    string log = null;
                    for (int idx = 0; idx < QCount; idx++)
                    {
                        if (QRecvBuff.TryDequeue(out byte dat))
                        {
                            arrayByte[idx] = dat;
                            log += $"0x{dat:X2} ";
                        }
                    }
                    //Console.WriteLine($"{DateTime.Now} >>> RECV ::: {log}");
                    //Console.WriteLine();
                    for (int idx = 0; idx < arrayByte.Length; idx++)
                    {
                        if (RecvModbus(arrayByte[idx]))
                        {
                            iBuffUnitId = iBuffArryData[0];
                            iBuffCode = iBuffArryData[1];
                            int tmpLength = iBuffArryData[2];

                            if (tmpLength != 0 && (tmpLength == 4 || tmpLength == 2))
                            {
                                iBuffData = (iBuffArryData[3] << 8) | iBuffArryData[4];
                                ModData item = new ModData(iBuffTransId, iBuffComId, iBuffLength, iBuffUnitId, iBuffCode, iBuffArryData);

                                var data = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == iBuffTransId);

                                if (data != null && iBuffCode != (byte)Externs.Robot_Modbus_Table.Func.Write_Coil)
                                {
                                    data.iData = iBuffData;

                                    if ((data.iData & 0x8000) != 0)
                                    {
                                        data.iData = (data.iData - 0x10000);
                                    }

                                    float decmal = 1 / (float)Math.Pow(data.ptDecimal.X, data.ptDecimal.Y);
                                    data.strData = $"{(float)data.iData * decmal}";

                                    if (data.IsUsed == true && (iBuffTransId == 256 || iBuffTransId == 257 || iBuffTransId == 258))
                                    {
                                        data.IsUsed = false;
                                    }

                                    //normal debug
                                    //Console.WriteLine($"{DateTime.Now} >>> RECV ::: {log}");
                                    //Console.WriteLine($"{DateTime.Now} >>> RECV ::: {iBuffTransId}, {iBuffLength}, {iBuffCode}, {data.iData}, {data.strData}");

                                    //if (iBuffTransId == 430)
                                    //{
                                    //    Console.WriteLine($"{DateTime.Now} >>> RECV ::: {iBuffTransId:000} {data.strDesc} [{data.iData}] // {data.strData}");
                                    //    Console.WriteLine();
                                    //}
                                }

                                //TEST
                                //if (data.Address == 259)
                                //{
                                //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Debug,
                                //   $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(10)}" +
                                //   $" | {data.Address} {data.strDesc} {data.iData} {(Externs.Robot_Modbus_Table.RobotState_Ver_1_1)data.iData}");
                                //}
                                //if (data.Address == 260)
                                //{
                                //    devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Debug,
                                //   $"{System.Reflection.MethodBase.GetCurrentMethod().Name.PadLeft(10)}" +
                                //   $" | {data.Address} {data.strDesc} {data.iData}");
                                //}
                            }
                            else if((tmpLength != 0 && tmpLength == 4))
                            {
                                Console.WriteLine($"MODBUS ::: DEBUG ::: {DateTime.Now} | ERROR | ");

                                for (int u =0; u < iBuffArryData.Length; u++)
                                {
                                    Console.WriteLine($"MODBUS ::: DEBUG ::: {DateTime.Now} | {u} | 0x{iBuffArryData[u]:X2} {iBuffArryData[u]} ");
                                }

                                
                            }


                            ComsResetBuffer();
                            

                            //2023.02.09 모드버스 서버 일경우 데이터 줘야 하는 부분 ::: 마스터변경으로 인하여 다시 만들었습니다.
                            //List<byte> iCopyArryData = new List<byte>();
                            //iCopyArryData.Add((byte)(iBuffTransId >> 8));
                            //iCopyArryData.Add((byte)(iBuffTransId));
                            //iCopyArryData.Add((byte)(iBuffComId >> 8));
                            //iCopyArryData.Add((byte)(iBuffComId));
                            //iCopyArryData.Add((byte)((iOffsetLength + iBuffLength) >> 8));
                            //iCopyArryData.Add((byte)((iOffsetLength + iBuffLength)));
                            //iCopyArryData.Add((byte)(iBuffUnitId));
                            //iCopyArryData.Add((byte)(iBuffCode));
                            //iCopyArryData.AddRange(iBuffArryData);
                            //
                            //if (mHost.Send(iCopyArryData.ToArray()))
                            //{
                            //    Console.WriteLine($"Echo Send Meassage ok");
                            //}

                            //DEBUG
                            //if (iBuffTransId == 22)
                            //{
                            //    Console.WriteLine($"{DateTime.Now} >>> RECV ::: {iBuffTransId:000} {data.strDesc} [{data.iData}] // {data.strData}");
                            //    Console.WriteLine();
                            //}

                            //DBUG
                            //if (iBuffTransId >= 420 && iBuffTransId <= 425)
                            //{
                            //    Console.WriteLine($"{DateTime.Now} >>> RECV ::: {iBuffTransId:000} {data.strDesc} [{data.iData}] // {data.strData}");
                            //}

                            //string log = null;
                            //foreach (byte buf in iBuffArryData)
                            //{
                            //    log += $"0x{buf:X2} ";
                            //}
                            ////Console.WriteLine($"{DateTime.Now} >>> {iBuffTransId:000} {iBuffComId} {iBuffLength} {iBuffUnitId} {iBuffCode} {log} [{data.iData}]");
                            //Console.WriteLine($"{DateTime.Now} >>> RECV ::: {iBuffTransId:000} {data.strDesc} [{data.iData}] // {data.strData}");
                            //Console.WriteLine();
                        }
                    }


                }
            }
            
            #endregion

            //SendMeassage
        }

        /// <summary>
        /// 모드버스 서버 수신 프로토콜
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        bool RecvModbus(byte tmp)
        {
            bool IsPars = false;
            //iBuffCount[(int)EFlag.UnitId]++;
            switch (comFlag)
            {
                case EFlag.TransId:
                    byteTransId[iBuffCount[(int)EFlag.TransId]++] = tmp;
                    iBuffCount[(int)EFlag.TransId] = iBuffCount[(int)EFlag.TransId] % iLen;
                    if (iBuffCount[(int)EFlag.TransId] == 0)
                    {
                        //Console.WriteLine($"{DateTime.Now} >>> [{comFlag}] [0x{byteTransId[0]}] [0x{byteTransId[1]}]");
                        iBuffTransId = byteTransId[0] << 8 | byteTransId[1];
                        iBuffCount[(int)EFlag.TransId] = 0;
                        comFlag = EFlag.Protocol;
                    }
                    break;
                case EFlag.Protocol:
                    byteComId[iBuffCount[(int)EFlag.Protocol]++] = tmp;
                    iBuffCount[(int)EFlag.Protocol] = iBuffCount[(int)EFlag.Protocol] % iLen;
                    if (iBuffCount[(int)EFlag.Protocol] == 0)
                    {
                        //Console.WriteLine($"{DateTime.Now} >>> [{comFlag}] [0x{byteComId[0]}] [0x{byteComId[1]}]");
                        iBuffComId = byteComId[0] << 8 | byteComId[1];
                        iBuffCount[(int)EFlag.Protocol] = 0;                        
                        if (iBuffComId != 0)
                        {
                            Console.WriteLine($"{DateTime.Now} >>> RecvModbus Error [{comFlag}] [{iBuffComId}]");

                            Console.WriteLine($"{DateTime.Now} >>> RecvModbus Error [{comFlag}] [0x{byteComId[0]}] [0x{byteComId[1]}]");
                            comFlag = EFlag.TransId;
                            return IsPars;
                        }
                        comFlag = EFlag.Length;
                    }
                    break;
                case EFlag.Length:
                    byteLength[iBuffCount[(int)EFlag.Length]++] = tmp;
                    iBuffCount[(int)EFlag.Length] = iBuffCount[(int)EFlag.Length] % iLen;
                    if (iBuffCount[(int)EFlag.Length] == 0)
                    {
                        //Console.WriteLine($"{DateTime.Now} >>> [{comFlag}] [0x{byteLength[0]}] [0x{byteLength[1]}]");
                        iBuffLength = byteLength[0] << 8 | byteLength[1];
                        iBuffCount[(int)EFlag.Length] = 0;
                        iBuffArryData = new byte[iBuffLength];                        
                        if (iBuffLength == 0)
                        {
                            Console.WriteLine($"{DateTime.Now} >>> RecvModbus Error [{comFlag}] [{iBuffLength}]");

                            Console.WriteLine($"{DateTime.Now} >>> RecvModbus Error [{comFlag}] [0x{byteLength[0]}] [0x{byteLength[1]}]");
                            comFlag = EFlag.TransId;
                            return IsPars;
                        }
                        comFlag = EFlag.Data;
                    }
                    break;                          
                case EFlag.Data:
                    iBuffArryData[iBuffCount[(int)EFlag.Data]++] = tmp;
                    if (iBuffCount[(int)EFlag.Data] == iBuffLength)
                    {
                        iBuffCount[(int)EFlag.Data] = 0;
                        comFlag = EFlag.TransId;
                        IsPars = true;
                    }
                    if (iBuffArryData[0] != 0xff)
                    {
                        iBuffCount[(int)EFlag.Data] = 0;
                        comFlag = EFlag.TransId;
                    }
                    break;
            }
            //Console.WriteLine($"{DateTime.Now} >>> RecvModbus tmp Byte 0x{tmp} ::: {comFlag}");
            return IsPars;
        }

        public void SendMeassage(byte[] array)
        {
            //if (mClient.IsConnected)
            //{
            //    mClient.Send(array);
            //}

            //2023.02.15
            if (mClient.IsConnected)
            {
                mClient.Send(array);
            }
            else
            {
                //mClient.Connect(strClientIpAddress,502, true);
            }
        }

        void ComsResetBuffer()
        {
            comFlag = EFlag.TransId;
            byteTransId = new byte[iLen];
            byteComId = new byte[iLen];
            byteLength = new byte[iLen];
            iBuffCount = new int[(int)EFlag.Max];

            //통신 데이터 버퍼
            iBuffTransId = 0;
            iBuffComId = 0;
            iBuffLength = 0;
            iBuffUnitId = 0;
            iBuffCode = 0;
            iBuffArryData = new byte[] { };
        }
      

        //void Run()
        //{
        //    while (true)
        //    {
        //        System.Threading.Thread.Sleep(1);

        //    }
        //}

        private void MHost_OnDataReceived(object[] data)
        {
            //throw new NotImplementedException();
            //foreach (byte tmp in (byte[])data[0])
            //{
            //    QRecvBuff.Enqueue(tmp);
            //}
        }

        private void MHost_OnClientDisconnect()
        {
            //throw new NotImplementedException();
        }

        private void MHost_OnClientAccepted()
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (mHost.IsConnected)
            {
                mHost.Close();
            }

            //if (mClient.IsConnected)
            //{
            //    mClient.();
            //}
        }

        //const byte STX = 2;
        //const byte DEV = 5;
        const byte DUM = 0;
        const byte LEN = 6;
        const byte SLV = 255;
        const byte DAT = 1;

        /// <summary>
        /// 2023.02.10 모드버스 데이터 동기화, ALL
        /// </summary>
        void Modbus_Sender()
        {
            foreach (Externs.Robot_Modbus_Table.Data dat in Externs.Robot_Modbus_Table.lstModbusData)
            {
                //Trans id 2byte, Protocal id 2byte(Modbus TCP : 0), Length 2byte, slave address 1byte, fucntion code 1byte, data 2byte)

                if (dat.IsUsed)
                {
                    byte[] paket = null;
                    paket = new byte[] { (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DUM, DUM, LEN, SLV, (byte)dat.func, (byte)(dat.Address >> 8), (byte)dat.Address, DUM, DAT };                
                    if (paket != null)
                    {
                        SendMeassage(paket);
                    }
                }
            }

            //Console.WriteLine();
        }

        /// <summary>
        /// 디지털 출력, 툴 출력
        /// </summary>
        /// <param name="manu"></param>
        /// <param name="bitNumber"></param>
        public void Modbus_Sender(Externs.Robot_Modbus_Table.Robot_Write manu, int bitNumber)
        {
            int address = -1;
            int writeAddress = -1;




            if (manu == Externs.Robot_Modbus_Table.Robot_Write.TOOL_IO)
            {
                address = 22; //read address
                var item = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == address);

                int buff = 1;
                int dat = item.iData >> (bitNumber - 1);
                dat = (~dat) & 1;

                writeAddress = 38 + bitNumber - 1;
                byte[] paket = new byte[] { (byte)(writeAddress >> 8), (byte)writeAddress,
                    DUM, DUM,
                    DUM, LEN,
                    SLV, (byte)Externs.Robot_Modbus_Table.Func.Write_Coil,
                    (byte)(writeAddress >> 8), (byte)writeAddress,
                    Externs.Robot_Modbus_Table.SetBit(dat) , DUM };


                //TEST                
                //string log = null;
                //foreach (byte buf in paket)
                //{
                //    log += $"0x{buf:X2} ";
                //}
                //Console.WriteLine($"{DateTime.Now} >>> SEND ::: {log} {writeAddress} {dat}");


                SendMeassage(paket);
            }

            if (manu == Externs.Robot_Modbus_Table.Robot_Write.GPIO)
            {
                address = 1;
                var item = Externs.Robot_Modbus_Table.lstModbusData.Find(x => x.Address == address);

                int dat = item.iData >> (bitNumber - 1);
                dat = (~dat) & 1;
                writeAddress = 16 + bitNumber - 1;
                byte[] paket = new byte[] { (byte)(writeAddress >> 8), (byte)writeAddress,
                    DUM, DUM,
                    DUM, LEN,
                    SLV, (byte)Externs.Robot_Modbus_Table.Func.Write_Coil,
                    (byte)(writeAddress >> 8), (byte)writeAddress,
                    Externs.Robot_Modbus_Table.SetBit(dat) , DUM };

                //TEST                
                //string log = null;
                //foreach (byte buf in paket)
                //{
                //    log += $"0x{buf:X2} ";
                //}
                //Console.WriteLine($"{DateTime.Now} >>> SEND ::: {log} {writeAddress} {dat}");

                SendMeassage(paket);
            }

        }

        /// <summary>
        /// 16bit 레지스터 128 ~ 255까지 
        /// </summary>
        /// <param name="regNumber"></param>
        /// <param name="data"></param>
        public void Modbus_Sender(int regNumber, int data)
        {
            byte[] paket = new byte[] { (byte)(regNumber >> 8), (byte)regNumber, DUM, DUM, DUM, LEN, SLV, (byte)Externs.Robot_Modbus_Table.Func.Write_Register, (byte)(regNumber >> 8), (byte)regNumber, (byte)(data >> 8), (byte)data };
            SendMeassage(paket);
        }
   
    }

    public partial class ModData
    {
        public ModData(int trans, int proto, int length, int unit, int code, byte[] dats)
        {
            iTransId = trans;
            iComId = proto;
            iLength = length;
            iUnitId = code;
            iArryData = dats.ToArray();
        }

        public int iTransId = 0;
        public int iComId = 0;
        public int iLength = 0;
        public int iUnitId = 0;
        public int iCode = 0;
        public byte[] iArryData = new byte[] { };
    }

    
}
