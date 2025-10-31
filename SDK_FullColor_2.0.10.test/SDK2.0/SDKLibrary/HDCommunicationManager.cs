using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace SDKLibrary
{
    public delegate void MsgReportEventHandler(Device devcie, string msg);
    public delegate void ResolveInfoReportEventHandler(Device devcie, ResolveInfo msg);

    /// <summary>
    /// 通讯管理
    /// </summary>
    public class HDCommunicationManager : IDisposable
    {
        #region 
        /// <summary>
        /// 创建一个空对象
        /// </summary>
        public HDCommunicationManager()
        {
            Devices = new List<Device>();
            _timerScan = null;
            _timerCheckTimeoutAndAutoSend = null;
            _timerCheckTimeoutAndAutoSend = new Timer(new TimerCallback(TimerCheckTimeoutAndAutoSendCallback), this, 1000, 3000);
        }

        /// <summary>
        /// Listen with the specified local endpoint.等待设备主动连接。
        /// </summary>
        /// <param name="localEP"></param>
        public void Listen(IPEndPoint localEP)
        {
            _listener = new TcpListener(localEP);
            _listener.AllowNatTraversal(true);
            _listener.Start();
            _listener.BeginAcceptTcpClient(new AsyncCallback(HandleTcpClientAccepted), _listener);
        }
        #endregion


        /// <summary>
        /// 开始扫描局域网设备
        /// </summary>
       public void StartScanLANDevice()
        {
            _udpClient = new UdpClient(0);
            _udpClient.EnableBroadcast = true;
            _udpClient.BeginReceive(UdpReceiveAsyncCallback, _udpClient);
            _timerScan = new Timer(new TimerCallback(ScanTick), this, 1000, 15000);
        }

        /// <summary>
        /// 结束扫描局域网设备
        /// </summary>
        public void EndScanDevice()
        {
            _timerScan = null;
        }


        /// <summary>
        /// 定时扫描设备函数函数
        /// </summary>
        /// <param name="ar"></param>
        private void ScanTick(object state)
        {
            if (state is HDCommunicationManager)
            {
                if (state is HDCommunicationManager)
                {
                    ((HDCommunicationManager)state).ScanDevice();
                }
            }
        }

        /// <summary>
        /// 广播扫描设备
        /// </summary>
       private  void ScanDevice()
        {
            if (_udpClient != null)
            {
                byte[] buff = DataProtocol.GetUdpCmdScan();
                _udpClient.Send(buff, buff.Length, new IPEndPoint(IPAddress.Broadcast, 10001));
            }
        }
        /// <summary>
        /// udp接收数据回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void UdpReceiveAsyncCallback(IAsyncResult ar)
        {
            try
            {
                UdpClient client = ar.AsyncState as UdpClient;
                if (client != null)
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    Byte[] receiveBytes = client.EndReceive(ar, ref remoteEP);
                    client.BeginReceive(UdpReceiveAsyncCallback, client);

                    UdpResolveReceiveData(receiveBytes, remoteEP);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Source + ex.StackTrace + ex.Message;
                Console.WriteLine(msg);
            }
        }

        /// <summary>
        /// 解析下位机反馈的udp数据
        /// </summary>
        /// <param name="srcData"></param>
        /// <returns></returns>
        private ErrorCode UdpResolveReceiveData(byte[] srcData, IPEndPoint remoteEP)
        {
            ErrorCode error = ErrorCode.kUnknown;
            int MIN_RESPOND_UDP_LENGHT = 6;
            if (srcData.Length < MIN_RESPOND_UDP_LENGHT)
            {
                return error;
            }

            // 获取版本与指令
            int version = DataProtocol.GetInt(srcData[0], srcData[1], srcData[2], srcData[3]);
            CmdType cmd = (CmdType)DataProtocol.GetInt(srcData[4], srcData[5]);

            switch (cmd)
            {
                case CmdType.kSearchDeviceAnswer:
                    {
                        ResolveScanAnswer(srcData, remoteEP);
                        error = ErrorCode.kSuccess;
                    }
                    break;

                case CmdType.kErrorAnswer:
                    {
                        if (srcData.Length >= 6)
                        {
                            int errorReturn = DataProtocol.GetInt(srcData[4], srcData[5]);
                            if (errorReturn > (int)ErrorCode.kUnknown && errorReturn < (int)ErrorCode.kCount)
                            {
                                error = (ErrorCode)errorReturn;
                            }
                            else
                            {
                                error = ErrorCode.kUnknown;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            return error;
        }

        /// <summary>
        /// 解析扫描指令
        /// </summary>
        /// <param name="srcData"></param>
        private void ResolveScanAnswer(byte[] srcData, IPEndPoint remoteEP)
        {
            if (srcData.Length < 25)
            {
                return;
            }

            int MAX_DEVICE_ID_LENGHT = 15;
            string deviceID = Encoding.UTF8.GetString(srcData, 6, MAX_DEVICE_ID_LENGHT);
            deviceID = deviceID.Trim('\0'); // 去掉尾部的\0
                                            // 当前设备不存在则添加到Devices
            bool found = false;
            lock (_deviceLock)
            {
                for (int i = Devices.Count - 1; i >= 0; --i)
                {
                    Device obj = Devices[i];
                    DeviceInfo info = obj.GetDeviceInfo();
                    if (info.deviceID == deviceID)
                    {
                        if (obj.Client.TcpClient.Connected)
                        {
                            found = true;
                            info.lastScanTime = DateTime.UtcNow;
                        }
                        break;
                    }
                }
            }

            if (!found)
            {
                string exception;
                string remoteIP = remoteEP.Address.ToString();

                //if (remoteIP == "192.168.1.96")
                {
                    Device device = AddDevice(remoteIP, out exception);
                    if (device != null)
                    {
                        device.GetDeviceInfo().deviceID = deviceID;
                        MsgReport(device, "online");
                    }
                }


            }
        }

        /// <summary>
        /// 获取当前在线设备列表
        /// </summary>
        /// <returns></returns>
        public List<Device> GetDevices()
        {
           // lock (_deviceLock)
            {
                List<Device> device = new List<Device>(Devices);
                return device;
            }
        }

        #region Fields
        /// <summary>
        /// 服务器使用的异步TcpListener
        /// </summary>
        private TcpListener _listener;

        /// <summary>
        /// 客户端会话列表
        /// </summary>
        private List<Device> Devices { get;  set; }
  
        private bool _disposed = false;

        private int _receiveBufferSize = 9 * 1024;

        private Timer _timerCheckTimeoutAndAutoSend;
        private Timer _timerScan;
        private Object _deviceLock = new Object();

        #endregion

        private UdpClient _udpClient;


        /// <summary>
        /// 普通消息通知事件
        /// </summary>
        public event MsgReportEventHandler MsgReport;

        /// 已经解析信息事件
        public event ResolveInfoReportEventHandler ResolvedInfoReport;

        /// <summary>
        /// 与客户端的连接已建立事件
        /// </summary>
        public event EventHandler<AsyncEventArgs> ClientConnected;
        /// <summary>
        /// 与客户端的连接已断开事件
        /// </summary>
        public event EventHandler<AsyncEventArgs> ClientDisconnected;

        /// <summary>
        /// 报告信息
        /// </summary>
        /// <param name="device"></param>
        /// <param name="msg"></param>
        internal void ReportMsg(Device device, string msg)
        {
            if (MsgReport != null)
            {
                MsgReport(device, msg);
            }
        }

        internal static string GetLogMsgString(string msg)
        {
            return DateTime.Now.ToString() + " " + msg;
        }
        internal static string GetLogMsgString(Device device, string msg)
        {
            return GetLogMsgString(device.GetDeviceInfo().deviceID + ":" + msg);
        }

        #region Method


        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="ip">设备IP</param>
        /// <param name="exception">异常信息</param>
        /// <returns>成功返回设备对象，失败返回null</returns>
        public Device AddDevice(string ip, out string exception)
        {
            return AddDevice(ip, 10001, out exception);
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="ip">设备IP</param>
        /// <param name="port">设备端口</param>
        /// <param name="exception">异常信息</param>
        /// <returns>成功返回设备对象，失败返回null</returns>
        private Device AddDevice(string ip, int port, out string exception)
        {
            exception = "";
            try
            {
                TcpClient client = new TcpClient(ip, port);
                Device device = new Device(this, client);
                lock (_deviceLock)
                {
                    Devices.Add(device);

                    // 开始异步接收数据，并主动触发RaiseClientConnected 函数初始化协议版本
                    NetworkStream stream = device.Client.NetworkStream;
                    stream.BeginRead(device.Client.Buffer, 0, device.Client.Buffer.Length, HandleDataReceived, device);
                    RaiseClientConnected(device);
                }

                return device;
            }
            catch (Exception exp)
            {
                exception = exp.Message;
                return null;
            }
        }



        /// <summary>
        /// 定时检查发送超时状态与自动发送数据
        /// </summary>
        /// <param name="state"></param>
        internal void TimerCheckTimeoutAndAutoSendCallback(object state)
        {
            if (state is HDCommunicationManager)
            {
                if (state is HDCommunicationManager)
                {
                    ((HDCommunicationManager)state).CheckTimeoutAndAutoSend();
                }
            }
        }

        /// <summary>
        /// 定时检查发送超时状态与自动发送数据
        /// </summary>
        internal void CheckTimeoutAndAutoSend()
        {
            //return;
            List<Device> offlineDevice = new List<Device>();
            lock (_deviceLock)
            {
                for (int i = Devices.Count - 1; i >= 0; i--)
                {
                    bool hasOffline = Devices[i].CheckTimeOutAndAutoSend();
                    if (hasOffline)
                    {
                        offlineDevice.Add(Devices[i]);
                        Close(Devices[i]);
                    }
                }
            }

            foreach (var device in offlineDevice)
            {
                ReportMsg(device, "offline");
            }
        }





/// <summary>
/// 停止通信
/// </summary>
public void Stop()
        {
            if (_listener != null)
            {
                _listener.Stop();
            }
            lock (_deviceLock)
            {
                //关闭所有客户端连接
                CloseAllClient();
            }
            _timerCheckTimeoutAndAutoSend = null;
            _timerScan = null;
        }



        /// <summary>
        /// 处理客户端连接的函数
        /// </summary>
        /// <param name="ar"></param>
        private void HandleTcpClientAccepted(IAsyncResult ar)
        {
            //TcpListener tcpListener = (TcpListener)ar.AsyncState;

            try
            {

            TcpClient client = _listener.EndAcceptTcpClient(ar);
            client.ReceiveBufferSize = _receiveBufferSize;
            byte[] buffer = new byte[client.ReceiveBufferSize];

            Device device = new Device(this, new TcpClientState(client, buffer));
            lock (_deviceLock)
            {
                for (int i = Devices.Count - 1; i >= 0; --i)
                {
                    if (Devices[i].Client.TcpClient.Client.RemoteEndPoint == device.Client.TcpClient.Client.RemoteEndPoint)
                    {
                        Close(Devices[i]);
                    }
                    }
                    Devices.Add(device);

                NetworkStream stream = device.Client.NetworkStream;
                //开始异步读取数据
                stream.BeginRead(device.Client.Buffer, 0, device.Client.Buffer.Length, HandleDataReceived, device);
                RaiseClientConnected(device);
            }

            _listener.BeginAcceptTcpClient(new AsyncCallback(HandleTcpClientAccepted), ar.AsyncState);
                //触发数据收到事件
                RaiseDataReceived(device);
            }
            catch (Exception exp)
            {
                HDCommunicationManager.GetLogMsgString(exp.Message);
            }

        }
        /// <summary>
        /// 数据接受回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void HandleDataReceived(IAsyncResult ar)
        {
            try
            {
                lock (_deviceLock)
                {
                    Device device = (Device)ar.AsyncState;
                    TcpClientState client = device.Client;
                    NetworkStream stream = client.NetworkStream;
                    try
                    {
                        int recv = stream.EndRead(ar);
                        if (recv == 0)
                        {
                            Close(device);
                            //触发客户端连接断开事件
                            RaiseClientDisconnected(device);
                            return;
                        }

                        // received byte and trigger event notification
                        // byte[] buff = new byte[recv];
                        // Buffer.BlockCopy(client.Buffer, 0, buff, 0, recv);
                        client.RecvedLength = recv;

                        // client.MemBufStream.Write(client.buff, 0, recv);
                        //触发数据收到事件
                        RaiseDataReceived(device);
                    }
                    catch (Exception exp)
                    {
                        ReportMsg(device, HDCommunicationManager.GetLogMsgString(device, exp.Message));
                        device.EndToSend();
                    }

                    // continue listening for tcp datagram packets
                    stream.BeginRead(client.Buffer, 0, client.Buffer.Length, HandleDataReceived, device);

                    device.LastSendRecvDataTime = System.Environment.TickCount;
                }
            }

            catch (Exception exp)
            {
               HDCommunicationManager.GetLogMsgString(exp.Message);
            }
        }


        /// <summary>
        /// 异步发送数据至指定的客户端
        /// </summary>
        /// <param name="device">接收数据的客户端会话</param>
        /// <param name="data">数据报文</param>
        internal void Send(Device device, byte[] data)
        {
            try
            {
                RaisePrepareSend(device);

                if (device.Client.TcpClient == null)
                    throw new ArgumentNullException("device.Client.TcpClient is null.");

                if (data == null)
                    throw new ArgumentNullException("data is null.");

                device.Client.TcpClient.GetStream().BeginWrite(data, 0, data.Length, SendDataEnd, device);
            }
            catch (Exception exp)
            {
                ReportMsg(device, HDCommunicationManager.GetLogMsgString(device, exp.Message));
                device.EndToSend();
            }

        }



        /// <summary>
        /// 发送数据完成处理函数
        /// </summary>
        /// <param name="ar">目标客户端Socket</param>
        private void SendDataEnd(IAsyncResult ar)
        {
            Device device = (Device)ar.AsyncState;
            try
            {
                device.Client.TcpClient.GetStream().EndWrite(ar);
                RaiseCompletedSend((Device)ar.AsyncState);
            }
            catch (Exception exp)
            {
                ReportMsg(device, HDCommunicationManager.GetLogMsgString(device, exp.Message));
                device.EndToSend();
            }
        }
        #endregion

        #region 事件

        /// <summary>
        /// 触发客户端连接事件
        /// </summary>
        /// <param name="state"></param>
        private void RaiseClientConnected(Device device)
        {
            // 发送版本协商协商命令
            device.SendEnsureProtocolVersionCmd();

            // 发送SDK版本协商命令
            device.SendEnsureSdkVersionCmd();

            if (ClientConnected != null)
            {
                ClientConnected(this, new AsyncEventArgs(device));
            }
        }
        /// <summary>
        /// 触发客户端连接断开事件
        /// </summary>
        /// <param name="client"></param>
        private void RaiseClientDisconnected(Device state)
        {
            state.EndToSend();

            //offline
            if (MsgReport != null)
            {
                MsgReport(state, "offline");
            }

            if (ClientDisconnected != null)
            {
                ClientDisconnected(this, new AsyncEventArgs("连接断开"));
            }
        }

        /// <summary>
        /// 接收到数据事件
        /// </summary>
        public event EventHandler<AsyncEventArgs> DataReceived;

        private void RaiseDataReceived(Device device)
        {

            if (DataReceived != null)
            {
                DataReceived(this, new AsyncEventArgs(device));
            }

            //    if (device.Client.RecvedLength > 0)
            //    {
            // byte[] data = new byte[device.Client.RecvedLength];
            // Array.Copy(device.Client.Buffer, 0, data, 0, data.Length);

            device.Client.MemBufStream.Write(device.Client.Buffer, 0, device.Client.RecvedLength);


            while (true)
            {
                if (device.Client.MemBufStream.Length < 4)
                {
                    break;
                }


                byte[] data = device.Client.MemBufStream.GetBuffer();

                int nCmdPackLen = DataProtocol.GetInt(data[0], data[1]);
                CmdType cmd = (CmdType)DataProtocol.GetInt(data[2], data[3]);

                // 如果当前接收的数据比命令包长度还要短，直接退出继续接收数据
                if (nCmdPackLen > device.Client.MemBufStream.Length)
                {
                    break;
                }

                device.Client.MemBufStream.Position = 0;
                byte[] commanddata = new byte[nCmdPackLen];
                device.Client.MemBufStream.Read(commanddata, 0, commanddata.Length);

                // 处理TCP粘包的情况，把后面的包放到MemBufStream前面
                if (nCmdPackLen <= device.Client.MemBufStream.Length)
                {
                    device.Client.MemBufStream.SetLength((int)device.Client.MemBufStream.Length - nCmdPackLen);
                    device.Client.MemBufStream.Position = 0;
                    device.Client.MemBufStream.Write(data, nCmdPackLen, (int)device.Client.MemBufStream.Length);
                }

                ResolveInfo ri;

                //  要处理返回来的xml总数据比最大单个数据包还大的情况
                if (cmd == CmdType.kSDKCmdAnswer)
                {
                    device.ResolveTcpReturnData(commanddata, out ri);
                    // xml数据还没有完全接收，退出不反馈
                    if (device.SDKCmdAnswerXmlData != null)
                    {
                        break;
                    }
                    
                }
                else
                {
                    device.ResolveTcpReturnData(commanddata, out ri);
                }

                
                

                if (ri.errorCode == ErrorCode.kSuccess)
                {
                    if (ri.method == SdkMethod.GetDeviceInfo.ToString())
                    {
                        //online
                        if (MsgReport != null)
                        {
                            MsgReport(device, "online");
                        }
                    }
                    //else if (ri.method == SdkMethod.GetIFVersion.ToString())
                    //{
                    //    //online
                    //    if (MsgReport != null)
                    //    {
                    //        MsgReport(device, "online");
                    //    }
                    //}
                        device.SendNext();
                }
                else
                {
                    device.EndToSend();
                }

                if (ResolvedInfoReport != null)
                {
                    ResolvedInfoReport(device, ri);
                }
            }

        }

        /// <summary>
        /// 发送数据前的事件
        /// </summary>
        public event EventHandler<AsyncEventArgs> PrepareSend;

        /// <summary>
        /// 触发发送数据前的事件
        /// </summary>
        /// <param name="state"></param>
        private void RaisePrepareSend(Device state)
        {
            if (PrepareSend != null)
            {
                PrepareSend(this, new AsyncEventArgs(state));
            }
        }

        /// <summary>
        /// 数据发送完毕事件
        /// </summary>
        public event EventHandler<AsyncEventArgs> CompletedSend;

        /// <summary>
        /// 触发数据发送完毕的事件
        /// </summary>
        /// <param name="state"></param>
        private void RaiseCompletedSend(Device device)
        {
            if (CompletedSend != null)
            {
                CompletedSend(this, new AsyncEventArgs(device));
            }

            device.CompletedSend();
        }

        /// <summary>
        /// 网络错误事件
        /// </summary>
        public event EventHandler<AsyncEventArgs> NetError;
        /// <summary>
        /// 触发网络错误事件
        /// </summary>
        /// <param name="state"></param>
        private void RaiseNetError(TcpClientState state)
        {
            if (NetError != null)
            {
                NetError(this, new AsyncEventArgs(state));
            }
        }

        /// <summary>
        /// 异常事件
        /// </summary>
        public event EventHandler<AsyncEventArgs> OtherException;
        /// <summary>
        /// 触发异常事件
        /// </summary>
        /// <param name="state"></param>
        private void RaiseOtherException(TcpClientState state, string descrip)
        {
            if (OtherException != null)
            {
                OtherException(this, new AsyncEventArgs(descrip, state));
            }
        }
        private void RaiseOtherException(TcpClientState state)
        {
            RaiseOtherException(state, "");
        }

        #endregion

        #region Close
        /// <summary>
        /// 关闭一个与客户端之间的会话
        /// </summary>
        /// <param name="state">需要关闭的客户端会话对象</param>
        public void Close(Device device)
        {
            if (device != null)
            {
                device.Close();
                lock (_deviceLock)
                {
                    Devices.Remove(device);
                }

            }
        }
        /// <summary>
        /// 关闭所有的客户端会话,与所有的客户端连接会断开
        /// </summary>
        public void CloseAllClient()
        {
            lock (_deviceLock)
            {
                for (int i = Devices.Count - 1; i >= 0; --i)
                {
                  Close(Devices[i]);
                }
                Devices.Clear();
            }
        }
        #endregion

        #region 释放
        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release 
        /// both managed and unmanaged resources; <c>false</c> 
        /// to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    try
                    {
                        Stop();
                        if (_listener != null)
                        {
                            _listener = null;
                        }
                    }
                    catch (SocketException)
                    {
                        //TODO
                        RaiseOtherException(null);
                    }
                }
                _disposed = true;
            }
        }
        #endregion
    }
}
