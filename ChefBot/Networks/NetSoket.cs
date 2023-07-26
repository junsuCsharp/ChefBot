using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;


public class NetSoket
{
    #region Server

    public class Server
    {
        #region Delegates

        public delegate void ClientAcceptedEventHandler();
        public delegate void DataReceivedEventHandler(object[] data);
        public delegate void ClientDisconnectedEventHandler();

        #endregion

        #region Events

        public event ClientAcceptedEventHandler OnClientAccepted;
        public event DataReceivedEventHandler OnDataReceived;
        public event ClientDisconnectedEventHandler OnClientDisconnect;

        #endregion

        public Socket ServerSocket { get; set; }
        public byte[]  DataBuffer { get; set; }

        public SocketEncryption EncryptionSettings { get; set; }
        public string EncryptionKey { get; set; }

        public bool UseEncryption { get; set; }

        public bool usrConnected { get; set; }

        private int iPaketBuffLength = 1024000;

        public int BufferSize
        {
            get
            {
                return DataBuffer.Length;
            }
            set
            {
                if (!ServerSocket.Connected)
                    DataBuffer = new byte[value];
            }
        }

        private void init()
        {
            EncryptionSettings = new SocketEncryption();
            EncryptionKey = "key";
            UseEncryption = false;
        }

        public Server()
        {
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            DataBuffer = new byte[iPaketBuffLength];
            //init();
        }

        public Server(Socket sock)
        {
            ServerSocket = sock;
            DataBuffer = new byte[iPaketBuffLength];
            //init();
        }

        public Server(Socket sock, int bufferSize)
        {
            ServerSocket = sock;
            DataBuffer = new byte[bufferSize];
            //init();
        }

        #region Socket Functions

        public void Start(int port)
        {
            if (!ServerSocket.Connected)
            {
                ServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                ServerSocket.Listen(5);
                ServerSocket.BeginAccept(new AsyncCallback(OnAccept), ServerSocket);
            }
        }

        public void Start(int port, int backlog)
        {
            if (!ServerSocket.Connected)
            {
                ServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                ServerSocket.Listen(backlog);
                ServerSocket.BeginAccept(new AsyncCallback(OnAccept), ServerSocket);
            }
        }

        public void Close()
        {
            try
            {
                ServerSocket.Dispose();
                //ServerSocket.Disconnect(true);
                
            }
            catch
            { }
        }

        public bool Send(byte[] data)
        {
            if (SendWork != null && SendWork.ClientSocket.Connected)
            {
                try
                {
                    //2023.02.08 ::: 임시 적용
                    SendWork.ClientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnSend), SendWork.ClientSocket);
                }
                catch
                { }
                

                return true;
            }

            return false;
        }

        public bool IsConnected
        {
            //return true;
            get
            {
                bool rtn = false;
                try
                {
                    //return !((ServerSocket.Poll(1000, SelectMode.SelectRead) && (ServerSocket.Available == 0)) || !ServerSocket.Connected);

                    //OnClientDisconnect()
                    if (SendWork != null)
                    {
                        rtn = SendWork.ClientSocket.Connected;
                    }
                }
                catch
                {
                    return false;
                }
                return rtn;
            }
        }

        #endregion

        #region CallBacks

        NetSoket.Client SendWork;
        private void OnAccept(IAsyncResult ar)
        {
            //canSend = true;
            try
            {
                Socket sock = ServerSocket.EndAccept(ar);

                NetSoket.Client client = new NetSoket.Client(sock, DataBuffer.Length);
                SendWork = client;
                //client.EncryptionKey = EncryptionKey;
                //client.EncryptionSettings = EncryptionSettings;
                //client.UseEncryption = UseEncryption;

                if (OnClientAccepted != null)
                    OnClientAccepted();

                client.ClientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), client);

                ServerSocket.BeginAccept(new AsyncCallback(OnAccept), sock);

                usrConnected = true;
            }
            catch
            { }
          
        }

        private void OnReceive(IAsyncResult ar)
        {
            NetSoket.Client sock = ar.AsyncState as NetSoket.Client;

            int receivedLength = 0;

            try
            {
                receivedLength = sock.ClientSocket.EndReceive(ar);
                if (receivedLength != 0)
                {
                    //if (UseEncryption)
                    //{
                    //    byte[] dataPacket = new byte[receivedLength];
                    //    Buffer.BlockCopy(DataBuffer, 0, dataPacket, 0, receivedLength);

                    //    byte[] decrypted = Decompress(EncryptionSettings.Decrypt(dataPacket, EncryptionKey));

                    //    if (OnDataReceived != null)
                    //        OnDataReceived(sock, DataFormatter.ConvertToObject(decrypted));

                    //    sock.ClientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), sock);
                    //}
                    //else
                    //{
                    byte[] dataPacket = new byte[receivedLength];
                    Buffer.BlockCopy(DataBuffer, 0, dataPacket, 0, receivedLength);

                    if (OnDataReceived != null)
                        OnDataReceived(DataFormatter.ConvertToObject(dataPacket));

                    sock.ClientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), sock);
                    //}
                }
            }
            catch (SocketException sockEx)
            {
                if (OnClientDisconnect != null)
                    OnClientDisconnect();
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            Socket sock = ar.AsyncState as Socket;
            sock.EndSend(ar);
        }

        #endregion
    }

    #endregion

    #region Client

    public class Client
    {
        #region Delegates

        public delegate void ConnectedEventHandler(bool connected);
        public delegate void DataReceivedEventHandler(object[] data);
        public delegate void ServerDisconnectedEventHanlder();

        #endregion

        #region Events

        public event ConnectedEventHandler OnConnect;
        public event DataReceivedEventHandler OnDataReceived;
        public event ServerDisconnectedEventHanlder OnServerDisconnect;

        #endregion

        public Socket ClientSocket { get; set; }
        private byte[] DataBuffer { get; set; }

        public SocketEncryption EncryptionSettings { get; set; }
        public string EncryptionKey { get; set; }

        public bool UseEncryption { get; set; }

        private void init()
        {
            EncryptionSettings = new SocketEncryption();
            EncryptionKey = "key";
            UseEncryption = false;
        }

        public int BufferSize
        {
            get
            {
                return DataBuffer.Length;
            }
            set
            {
                if (!ClientSocket.Connected)
                    DataBuffer = new byte[value];
            }
        }

        public Client()
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            DataBuffer = new byte[1024];
            //init();
        }

        public Client(Socket sock)
        {
            ClientSocket = sock;
            DataBuffer = new byte[1024];
            //init();
        }

        public Client(Socket sock, int bufferSize)
        {
            ClientSocket = sock;
            DataBuffer = new byte[bufferSize];
            //init();
        }

        #region Socket Functions

        public void Connect(string ip, int port)
        {
            try
            {
                IPAddress IP = IPAddress.Parse(ip);
                ClientSocket.Connect(IP, port);
            }
            catch (SocketException sockEx)
            {
                if (string.Equals(sockEx.Message, "No connection could be made because the target machine actively refused it"))
                {
                    if (OnConnect != null)
                        OnConnect(false);
                }
            }

            if (ClientSocket.Connected)
            {
                //var sock = new LIB_Soket.Server(ClientSocket, DataBuffer.Length);
                //sock.ServerSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), sock);

                ClientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), ClientSocket);
            }

            if (OnConnect != null)
                OnConnect(ClientSocket.Connected);
        }

        public void Connect(string ip, int port, bool IsAsync)
        {
            try
            {
                IPAddress IP = IPAddress.Parse(ip);

                if (IsAsync)
                {
                    //ClientSocket.BeginConnect(IP, port, new AsyncCallback(OnReceive), ClientSocket);
                }
                else
                {
                    ClientSocket.Connect(IP, port);
                }

                //ClientSocket.Connect(IP, port);



            }
            catch (SocketException sockEx)
            {
                if (string.Equals(sockEx.Message, "No connection could be made because the target machine actively refused it"))
                {
                    if (OnConnect != null)
                        OnConnect(false);
                }
            }

            if (ClientSocket.Connected)
            {
                //var sock = new LIB_Soket.Server(ClientSocket, DataBuffer.Length);
                //sock.ServerSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), sock);

                ClientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), ClientSocket);
            }

            if (OnConnect != null)
                OnConnect(ClientSocket.Connected);
        }

        // The following method is called when each asynchronous operation completes.
        void ProcessDnsInformation(IAsyncResult result)
        {
            int requestCounter = 0;
            ArrayList hostData = new ArrayList();
            StringCollection hostNames = new StringCollection();

            string hostName = (string)result.AsyncState;
            hostNames.Add(hostName);
            try
            {
                // Get the results.
                IPHostEntry host = Dns.EndGetHostEntry(result);
                hostData.Add(host);
            }
            // Store the exception message.
            catch (SocketException e)
            {
                hostData.Add(e.Message);
            }
            finally
            {
                // Decrement the request counter in a thread-safe manner.
                System.Threading.Interlocked.Decrement(ref requestCounter);
            }
        }

        //public bool Send(object[] data)
        //{
        //    if (ClientSocket.Connected)
        //    {
        //        if (UseEncryption)
        //        {
        //            byte[] dataPacket = EncryptionSettings.Encrypt(Compress(DataFormatter.ConvertToByte(data)), EncryptionKey);
        //            ClientSocket.BeginSend(dataPacket, 0, dataPacket.Length, SocketFlags.None, new AsyncCallback(OnSend), ClientSocket);
        //        }
        //        else
        //        {
        //            byte[] dataPacket = Compress(DataFormatter.ConvertToByte(data));
        //            ClientSocket.BeginSend(dataPacket, 0, dataPacket.Length, SocketFlags.None, new AsyncCallback(OnSend), ClientSocket);
        //        }

        //        return true;
        //    }

        //    return false;
        //}

        public bool IsConnected
        {
            //return true;
            get
            {
                bool rtn = false;
                try
                {
                    //return !((ServerSocket.Poll(1000, SelectMode.SelectRead) && (ServerSocket.Available == 0)) || !ServerSocket.Connected);

                    //OnClientDisconnect()
                    if (ClientSocket != null)
                    {
                        rtn = ClientSocket.Connected;
                    }
                }
                catch
                {
                    return false;
                }
                return rtn;
            }
        }

        public bool Send(byte[] data)
        {
            if (ClientSocket != null && ClientSocket.Connected)
            {
                if (data != null)
                {
                    try
                    {
                        ClientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnSend), ClientSocket);

                        return true;
                    }
                    catch
                    { }
                    
                }
            }

            return false;
        }

        #endregion

        #region CallBacks

        private void OnReceive(IAsyncResult ar)
        {
            //LIB_Soket.Client sock = ar.AsyncState as LIB_Soket.Client;

            int receivedLength = 0;

            try
            {
                receivedLength = ClientSocket.EndReceive(ar);
            }
            catch (SocketException sockEx)
            {
                if (OnServerDisconnect != null)
                    OnServerDisconnect();
            }

            if (receivedLength != 0)
            {
                //if (UseEncryption)
                //{
                //    byte[] dataPacket = new byte[receivedLength];
                //    Buffer.BlockCopy(DataBuffer, 0, dataPacket, 0, receivedLength);

                //    byte[] decrypted = Decompress(EncryptionSettings.Decrypt(dataPacket, EncryptionKey));

                //    if (OnDataReceived != null)
                //        OnDataReceived(sock, DataFormatter.ConvertToObject(decrypted));

                //    sock.ClientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), sock);
                //}
                //else
                //{
                byte[] dataPacket = new byte[receivedLength];
                Buffer.BlockCopy(DataBuffer, 0, dataPacket, 0, receivedLength);

                if (OnDataReceived != null)
                    OnDataReceived(DataFormatter.ConvertToObject(dataPacket));

                ClientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), ClientSocket);
                //}
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            Socket sock = ar.AsyncState as Socket;
            sock.EndSend(ar);
        }

        #endregion
    }

    #endregion

    #region Data Formatting

    private class DataFormatter
    {
        public static byte[] ConvertToByte(object[] obj)
        {
            var bf = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static object[] ConvertToObject(byte[] arrBytes)
        {
            //var bf = new BinaryFormatter();

            //using (var ms = new MemoryStream())
            //{
            //    ms.Write(arrBytes, 0, arrBytes.Length);
            //    ms.Seek(0, SeekOrigin.Begin);

            //    return (object[])bf.Deserialize(ms);
            //}

            object[] obj = new object[1];
            obj[0] = arrBytes;
            return obj;
        }
    }

    #endregion

    #region Encryption

    public class SocketEncryption
    {
        public EncryptionMethod Method { get; set; }

        public SocketEncryption()
        {
            Method = new DefaultEncryption();
        }

        public SocketEncryption(EncryptionMethod method)
        {
            Method = method;
        }

        public string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }

        public byte[] Encrypt(byte[] input, string key)
        {
            return Method.Encrypt(input, key);
        }

        public byte[] Decrypt(byte[] input, string key)
        {
            return Method.Decrypt(input, key);
        }
    }

    public interface EncryptionMethod
    {
        byte[] Encrypt(byte[] input, string key);
        byte[] Decrypt(byte[] input, string key);
    }

    private class DefaultEncryption : EncryptionMethod
    {
        public byte[] Encrypt(byte[] input, string key)
        {
            using (var ms = new MemoryStream())
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                    byte[] rijndaelKey = md5.ComputeHash(keyBytes, 0, keyBytes.Length);

                    using (var r = new RijndaelManaged())
                    {
                        r.Key = rijndaelKey;
                        r.IV = rijndaelKey;

                        r.Mode = CipherMode.CBC;
                        r.Padding = PaddingMode.PKCS7;

                        using (var cs = new CryptoStream(ms, r.CreateEncryptor(), CryptoStreamMode.Write))
                            cs.Write(input, 0, input.Length);

                        return ms.ToArray();
                    }
                }
            }
        }

        public byte[] Decrypt(byte[] input, string key)
        {
            using (var ms = new MemoryStream())
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                    byte[] rijndaelKey = md5.ComputeHash(keyBytes, 0, keyBytes.Length);

                    using (var r = new RijndaelManaged())
                    {
                        r.Key = rijndaelKey;
                        r.IV = rijndaelKey;

                        r.Mode = CipherMode.CBC;
                        r.Padding = PaddingMode.PKCS7;

                        using (var cs = new CryptoStream(ms, r.CreateDecryptor(), CryptoStreamMode.Write))
                            cs.Write(input, 0, input.Length);

                        return ms.ToArray();
                    }
                }
            }
        }
    }

    #endregion

    #region Compression / Decompression

    public static byte[] Compress(byte[] input)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (GZipStream _gz = new GZipStream(ms, CompressionMode.Compress))
            {
                _gz.Write(input, 0, input.Length);
            }
            return ms.ToArray();
        }
    }

    public static byte[] Decompress(byte[] input)
    {
        using (var ms = new MemoryStream(input))
        {
            using (var ms2 = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    byte[] payload = new byte[1000];
                    int count = 0;

                    while ((count = gzip.Read(payload, 0, payload.Length)) > 0)
                    {
                        ms2.Write(payload, 0, count);
                    }

                    return ms2.ToArray();
                }
            }
        }
    }

    #endregion
}