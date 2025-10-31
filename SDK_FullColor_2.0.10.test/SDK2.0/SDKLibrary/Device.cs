using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace SDKLibrary
{
    /// <summary>
    /// 设备类，代表具体设备，
    /// </summary>
    public partial class Device
    {
        /// <summary>
        /// 初始化设备，程序作为客户端时使用该构造函数
        /// </summary>
        /// <param name="client">TCP连接对象</param>
        internal Device(HDCommunicationManager comm, TcpClient client) : this(comm, new TcpClientState(client))
        {

        }
        /// <summary>
        /// 初始化设备，程序作为服务端时使用该构造函数
        /// </summary>
        /// <param name="server">服务端对象，Device作为客户端时该只为null</param>
        /// <param name="client">TCP连接对象</param>
        internal Device(HDCommunicationManager comm, TcpClientState client)
        {
            CommunicationManager = comm;
            Client = client;

            //IsSending = false;
            _sendQueue = new Queue<byte[]>();
            TransportProtocolVersion = DataProtocol._LOCAL_TCP_VERSION;
            SdkProtocolVersion = DataProtocol._SDK_VERSION;
            EnsureProtocolVersion = EnsureSdkProtocolVersion = false;
            UploadItems = new List<UploadFileInfo>();
            _deviceInfo = new DeviceInfo();
            UploadingItem = new UploadFileInfo();
        }



        /// <summary>
        /// 标志正在发送的数据
        /// </summary>
        private class SendingInfo
        {
            public byte[] sendingData;
       //     public int startSendTick;
       //     public int lastSendTime = System.Environment.TickCount;
        }

        /// <summary>
        ///  Provides client connections for TCP network services.
        /// </summary>
        public TcpClientState Client { get; private set; }

        /// <summary>
        ///  通讯管理对象
        /// </summary>
        internal HDCommunicationManager CommunicationManager { get; private set; }


        /// <summary>
        /// 传输协议版本
        /// </summary>
        internal int TransportProtocolVersion { get; private set; }

        /// <summary>
        /// SDK协议版本
        /// </summary>
        internal int SdkProtocolVersion { get; private set; }

        /// <summary>
        /// 是否已经确认传输协议版本
        /// </summary>
        internal bool EnsureProtocolVersion { get; private set; }

        /// <summary>
        /// 是否已经确认SDK协议版本
        /// </summary>
        internal bool EnsureSdkProtocolVersion { get; private set; }

        /// <summary>
        /// 标志当前连接的唯一标志
        /// </summary>
        internal string SdkGuid { get; set; }

        /// <summary>
        /// SDK 版本
        /// </summary>
        internal string SdkVersion { get; private set; }

        /// <summary>
        /// 接收到的xml数据
        /// </summary>
        internal byte [] SDKCmdAnswerXmlData { get; private set; }

        /// <summary>
        /// 当前是否正在发送数据
        /// </summary>
       // internal bool IsSending { get; private set; }

        /// <summary>
        /// 当前文件上传列表
        /// </summary>
        public List<UploadFileInfo> UploadItems { get; private set; }

        /// <summary>
        /// 当前数据发送队列
        /// </summary>
        private Queue<byte[]> _sendQueue;

        /// <summary>
        /// 用于同步的对象
        /// </summary>
        private Object _dataLock = new Object();

        private SendingInfo _sendingInfo = new SendingInfo();
        internal int LastSendRecvDataTime = System.Environment.TickCount;       // 上一次发送接收数据时间
        internal int LastSendTime = System.Environment.TickCount;               // 上一次发送时间
        
        private UploadFileInfo UploadingItem;
        private DeviceInfo _deviceInfo;

        /// <summary>
        /// 是否正在发送文件
        /// </summary>
        public bool SendingFile { get; set; }

        /// <summary>
        /// 是否已经开始发送文件内容 （发送文件内容下位机没有返回，根据发送状态直接发下一包数据）
        /// </summary>
        internal bool HasStartSendFileContext { get; set; }


        /// <summary>
        /// 是否正在下载文件
        /// </summary>
        internal bool DowningFile { get; set; }


        /// <summary>
        /// 初始化通讯版本和设备信息
        /// </summary>
        internal void InitVersionAndDeviceInfo()
        {
            // 发送版本协商协商命令
            SendEnsureProtocolVersionCmd();

            // 发送SDK版本协商命令
            SendEnsureSdkVersionCmd();
        }


        /// <summary>
        /// 结束发送
        /// </summary>
        /// <param name="strError"></param>
        internal void EndToSend(string strError = "")
        {
            _sendingInfo = new SendingInfo();
            //IsSending = false;

            if (SendingFile)
            {
                UploadingItem.Dispose();
                SendingFile = false;
                UploadingItem = new UploadFileInfo();
            }

            lock (_dataLock)
            {
                _sendQueue.Clear();
            }

            if (strError != null && strError.Length > 0)
            {
                CommunicationManager.ReportMsg(this, _deviceInfo.deviceID + " : " + strError);
            }
        }


        /// <summary>
        /// 发送下一包数据
        /// </summary>
        internal void SendNext()
        {
            //IsSending = false;
            _sendingInfo = new SendingInfo();
            TryToSend();
        }

        /// <summary>
        /// 尝试发送数据
        /// </summary>
        private void TryToSend()
        {
            // 连接已经结束当做下线处理
            if (!Client.TcpClient.Connected)
            {
                EndToSend();
                return;
            }

            if (_sendingInfo.sendingData == null)
            {
                lock (_dataLock)
                {
                   // _sendingInfo = new SendingInfo();

                    if (_sendQueue.Count > 0)
                    {
                        _sendingInfo.sendingData = _sendQueue.Dequeue();

                        try
                        {
                            CommunicationManager.Send(this, _sendingInfo.sendingData);
                        //    IsSending = true;
                        }
                        catch (Exception exp)
                        {
                            CommunicationManager.ReportMsg(this, HDCommunicationManager.GetLogMsgString(this, exp.Message));
                            EndToSend();
                        }
                    }

                }
            }
        }


        /// <summary>
        /// 关闭当前设备连接
        /// </summary>
        public void Close()
        {
            if (Client != null)
            {
                Client.Close();
                Client.RecvedLength = 0;
            }
        }

        /// <summary>
        /// 函数说明: 	检测超时并自动发送发送队列数据
        /// </summary>
        internal bool CheckTimeOutAndAutoSend()
        {
            bool hasOffline = false;
            AutoSend();

            // 超时或者连接已经断开当做掉线处理 offline
            if (System.Environment.TickCount - LastSendRecvDataTime > 90000 || !Client.TcpClient.Connected)
            {
                EndToSend();
                hasOffline = true;
            }

            return hasOffline;
        }

        /// <summary>
        /// 自动发送数据
        /// </summary>
        private void AutoSend()
        {
            try
            {
                // 如果当前有待发送包没有发送则发送
                TryToSend();

                // 空闲时每三十秒自动发送心跳包维持tcp连接
                if (_sendQueue.Count == 0 && _sendingInfo.sendingData == null)
                {
                    //  IsSending = false;
                    if (System.Environment.TickCount - LastSendTime > 30000)
                    {
                        LastSendTime = System.Environment.TickCount;
                        SendHeartMsn();
                    }
                }
            }
            catch (Exception exp)
            {
                CommunicationManager.ReportMsg(this, HDCommunicationManager.GetLogMsgString(this, exp.Message));
            }
        }

        /// <summary>
        /// 发送确认协议版本命令
        /// </summary>
        internal void SendEnsureProtocolVersionCmd()
        {
            if (!EnsureProtocolVersion)
            {
                byte[] tpv = DataProtocol.GetTransportProtocolVersionCmd(TransportProtocolVersion);
                lock (_dataLock)
                {
                    _sendQueue.Enqueue(tpv);
                }
                TryToSend();


            }

        }

        /// <summary>
        /// 发送确认SDK协议版本命令
        /// </summary>
        internal void SendEnsureSdkVersionCmd()
        {
            if (!EnsureSdkProtocolVersion)
            {
                byte[] sdkv = DataProtocol.GetSdkVersionCmd(SdkProtocolVersion);
                lock (_dataLock)
                {
                    _sendQueue.Enqueue(sdkv);
                }
                TryToSend();
            }

        }

        /// <summary>
        /// 从xml发送数据
        /// </summary>
        /// <param name="xml">xml 数据</param>
        /// <param name="useCurrentSdkGuid">是否使用当前Sdk GUID 默认为 true，因为每次建立连接Sdk GUID都不一样，
        /// 所以需要使用当前 SdkGUID替换 文档里面的sdkGUID </param>
        public void SendFromXml(string xml, bool useCurrentSdkGuid = true)
        {
            string newXml = xml;

            if (useCurrentSdkGuid)
            {
                SdkXmlDocument doc = new SdkXmlDocument();
                doc.LoadXml(xml);
                if (doc.IsSdkXmlData)
                {
                    foreach (XmlNode node in doc)
                    {
                        if (node.Name == "sdk")
                        {
                            node.Attributes["guid"].Value = SdkGuid;
                            break;
                        }
                    }

                    MemoryStream stream = new MemoryStream();
                    XmlWriterSettings setting = new XmlWriterSettings();
                    setting.Encoding = Encoding.UTF8;
                    setting.Indent = true;
                    XmlWriter writer = XmlWriter.Create(stream, setting);
                    doc.Save(writer);
                    newXml = Encoding.UTF8.GetString(stream.ToArray());
                }
            }

            SendXmlData(newXml);
        }

        /// <summary>
        /// 直接发送xml数据
        /// </summary>
        /// <param name="xml"></param>
        private void SendXmlData(string xml)
        {
            var list = DataProtocol.GetSDKCmdAsk(xml);
            foreach (var data in list)
            {
                lock (_dataLock)
                {
                    _sendQueue.Enqueue(data);
                }
            }

            TryToSend();
        }

        /// <summary>
        /// 完成发送
        /// </summary>
        internal void CompletedSend()
        {
            LastSendRecvDataTime = System.Environment.TickCount;
            LastSendTime = System.Environment.TickCount;
            _sendingInfo.sendingData = null;
            //IsSending = false;

            // 直接发送下一包文件
            if (HasStartSendFileContext)
            {
                SendNextFilePacket();
            }
        }

        /// <summary>
        /// 上传下一个文件
        /// </summary>
        private void ContinueUploadFile()
        {
            if (UploadItems.Count > 0 && !SendingFile)
            {
                var item = UploadItems[0];
                UploadingItem = item;
                UploadItems.Remove(item);

                SendingFile = true;
                UploadingItem.isSending = true;
                HasStartSendFileContext = false;

                byte[] startUpload = DataProtocol.GetUploadFileStartAsk(Path.GetFileName(item.path), item.md5, (int)item.fs.Length, (int)item.type);
                lock (_dataLock)
                {
                    _sendQueue.Enqueue(startUpload);
                }
                TryToSend();
            }
        }

        /// <summary>
        /// 根据文件路径后缀获取文件类型
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        static public HFileType GetHFileType(string filePath)
        {
            HFileType ftype = HFileType.kImageFile;
            string ext = Path.GetExtension(filePath).ToLower();
            List<string> imageExt = new List<string> { ".bmp", ".jpg", ".jpeg", ".png", ".ico", ".gif", ".tif", ".tif" };
            List<string> videoExt = new List<string> {".mp3", ".swf", ".f4v", ".trp", ".wmv", ".asf", ".mpeg", ".webm", ".asx", ".rm", ".rmvb", ".mp4", ".3gp", ".mov", ".m4v", ".avi", ".dat", ".mkv", ".flv", ".vob", ".ts" };
            List<string> fontExt = new List<string> { ".ttc", ".ttf", ".bdf"};
            List<string> firewareExt = new List<string> { ".bin" };
            List<string> programTemplateExt = new List<string> { ".xml" };
            if (imageExt.Find(s => { return s == ext; }) != null)
            {
                ftype = HFileType.kImageFile;
            }
            else if (videoExt.Find(s => { return s == ext; }) != null)
            {
                ftype = HFileType.kVideoFile;
            }
            else if (fontExt.Find(s => { return s == ext; }) != null)
            {
                ftype = HFileType.kFont;
            }
            else if (firewareExt.Find(s => { return s == ext; }) != null)
            {
                ftype = HFileType.kImageFile;
            }
            else if (programTemplateExt.Find(s => { return s == ext; }) != null)
            {
                if (Path.GetFileName(filePath).ToLower() == "fpga.xml")
                {
                    ftype = HFileType.kFPGAConfig;
                }
                else if (Path.GetFileName(filePath).ToLower() == "config.xml")
                {
                    ftype = HFileType.kSettingCofnig;
                }
                else
                {
                    ftype = HFileType.kProgramTemplate;
                }
                
            }

            return ftype;
        }

        /// <summary>
        /// 添加上传文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="type">文件类型</param>
        /// <returns>返回保存上传文件信息的对象</returns>
        public UploadFileInfo AddUploadFile(string filePath,  HFileType type = HFileType.kauto)
        {
            bool tempFile = false;
            if (type == HFileType.kTempImageFile || type == HFileType.kTempVideoFile)
            {
                tempFile = true;
            }
            return AddUploadFile(filePath, tempFile, type);
        }

        /// <summary>
        /// 添加上传文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="tempFile">是否是临时文件</param>
        /// <param name="type">文件类型</param>
        /// <returns>返回保存上传文件信息的对象</returns>
        public UploadFileInfo AddUploadFile(string filePath, bool tempFile = false, HFileType type = HFileType.kauto)
        {
            if (UploadingItem.path == filePath)
            {
                return UploadingItem;
            }

            foreach (var item in UploadItems)
            {
                if (item.path == filePath)
                {
                    return item;
                }
            }

            FileStream fs = File.Open(filePath, FileMode.Open);
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(fs);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            UploadFileInfo sendFileInfo = new UploadFileInfo();
            sendFileInfo.md5 = sBuilder.ToString().ToLower();
            sendFileInfo.path = filePath;
            sendFileInfo.fs = fs;
            HFileType t = GetHFileType(filePath);
            if (tempFile && (t == HFileType.kVideoFile || t == HFileType.kImageFile))
            {
                if (t == HFileType.kVideoFile)
                {
                    type = HFileType.kTempVideoFile;
                }
                else
                {
                    type = HFileType.kTempImageFile;
                }
            }
            
            sendFileInfo.type = type != HFileType.kauto ? type : t;
            fs.Position = 0;

            UploadItems.Add(sendFileInfo);

            return sendFileInfo;
        }

        /// <summary>
        /// 开始上传文件
        /// </summary>
        /// <param name="fileinfo"></param>
        public void StartUploadFile()
        {
            ContinueUploadFile();
        }

        /// <summary>
        /// 删除指定上传的文件
        /// </summary>
        /// <param name="fileinfo"></param>
        public void RemoveUploadFile(UploadFileInfo fileinfo)
        {
            if (UploadingItem == fileinfo)
            {
                SendingFile = false;
                UploadingItem = new UploadFileInfo();
            }

            if (fileinfo.fs != null)
            {
                fileinfo.fs.Close();
            }
            UploadItems.Remove(fileinfo);

            ContinueUploadFile();
        }


        /// <summary>
        /// 根据文件路径直接开始传输文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public UploadFileInfo StartUploadFile(string filePath, bool tempFile, HFileType type = HFileType.kauto)
        {
            foreach (UploadFileInfo obj in UploadItems)
            {
                if (filePath == obj.path)
                {
                    return obj;
                }
            }

            UploadFileInfo sendFileInfo = AddUploadFile(filePath, tempFile, type);

            StartUploadFile();

            return sendFileInfo;
        }

        /// <summary>
        /// 发送心跳包
        /// </summary>
        public void SendHeartMsn()
        {
            if (_sendQueue.Count == 0)
            {
                byte[] data = DataProtocol.GetkTcpHeartbeatAsk();
                lock (_dataLock)
                {
                    _sendQueue.Enqueue(data);
                }
                TryToSend();

                // 打印发送心跳包信息
                string strTip = DateTime.Now.ToString() + "  " + GetDeviceInfo().deviceID + " (Send kTcpHeartbeatAsk) ";
                CommunicationManager.ReportMsg(this, strTip);
            }
        }




        /// <summary>
        /// 刷新设备信息
        /// </summary>
        public void RefreshDeviceInfo()
        {
            // 获取设备信息
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.GetDeviceInfo.ToString());
            SendXmlData(xml);

            // 获取以太网信息
            GetEthernetInfo();

            // 获取3G 4G信息
            //  xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.GetPppoeInfo.ToString());
            //  SendXmlData(xml);

            // 获取Wifi 信息
            //GetWifiInfo();
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <returns></returns>
        public DeviceInfo GetDeviceInfo()
        {
            return _deviceInfo;
        }



        /// <summary>
        /// 获取以太网网络地址信息
        /// </summary>
        public void GetEthernetInfo()
        {
           string xml = new EthernetInfo().GetCmdToXml(SdkGuid);
           SendXmlData(xml);
        }

        /// <summary>
        /// 设置以太网网络地址信息
        /// </summary>
        public void SetEthernetInfo(EthernetInfo info)
        {
            string xml = info.SetCmdToXml(SdkGuid);
            SendXmlData(xml);
        }

        /// <summary>
        /// 获取Wifi信息
        /// </summary>
        public void GetWifiInfo()
        {
            string xml = new WifiInfo().GetCmdToXml(SdkGuid);
            SendXmlData(xml);
        }

        /// <summary>
        /// 设置Wifi信息
        /// </summary>
        public void SetWifiInfo(WifiInfo info)
        {
            string xml = info.SetCmdToXml(SdkGuid);
            SendXmlData(xml);
        }

        /// <summary>
        /// 获取亮度信息
        /// </summary>
        public void GetLuminanceInfo()
        {
            string xml = new LuminanceInfo().GetXml_GetLuminanceInfo(SdkGuid);
            SendXmlData(xml);
        }

        /// <summary>
        /// 设置亮度信息
        /// </summary>
        /// <param name="luminanceInfo">要设置的亮度信息数据类</param>
        public void SetLuminanceInfo(LuminanceInfo luminanceInfo)
        {
            string xml = luminanceInfo.SetLuminanceInfoToXml(SdkGuid);
            SendXmlData(xml);
        }


        /// <summary>
        /// 获取时间信息
        /// </summary>
        public  void GetTimeInfo()
        {
            string xml = new TimeInfo().GetXml_GetTimeInfo(SdkGuid);
            SendXmlData(xml);
        }


        /// <summary>
        /// 设置时间信息
        /// </summary>
        /// <param name="timeInfo"></param>
        public void SetTimeInfo(TimeInfo timeInfo)
        {
            string xml = timeInfo.SetTimeInfoToXml(SdkGuid);
            SendXmlData(xml);
        }

        /// <summary>
        /// 开屏
        /// </summary>
        public void OpenScreen()
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.OpenScreen.ToString());
            SendXmlData(xml);
        }

       /// <summary>
       /// 关屏
       /// </summary>
        public void CloseScreen()
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.CloseScreen.ToString());
            SendXmlData(xml);
        }


        /// <summary>
        /// 获取开关机信息
        /// </summary>
        public void GetSwitchTimeInfo()
        {
            string xml = new SwitchTimeInfo().GetXml_GetSwitchTimeInfo(SdkGuid);
            SendXmlData(xml);

        }

        /// <summary>
        /// 设置开关机信息
        /// </summary>
        /// <param name="switchTimeInfo"></param>
        public void SetSwitchTimeInfo(SwitchTimeInfo switchTimeInfo)
        {
            string xml = switchTimeInfo.SetSwitchTimeInfoToXml(SdkGuid);
            SendXmlData(xml);
        }

        /// <summary>
        /// 获取开机画面信息
        /// </summary>
        public void GetBootLogoInfo()
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.GetBootLogo.ToString());
            SendXmlData(xml);
        }

        /// <summary>
        /// 设置开机画面信息
        /// </summary>
        /// <param name="bootLogo"></param>
        public void SetBootLogoInfo(BootLogoInfo bootLogo)
        {
            var list = bootLogo.GetXmlElements(null);
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.SetBootLogoName.ToString(), list);
            SendXmlData(xml);
        }


        /// <summary>
        /// 发送屏幕 (重新刷新所有节目)
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        public string SendScreen(HdScreen screen)
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.AddProgram.ToString(), screen.GetXmlElement(new XmlDocument()));
            SendXmlData(xml);
            return xml;
        }

        /// <summary>
        /// 更新指定节目
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public string UpdateDeviceProgram(HdProgram program)
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.UpdateProgram.ToString(), program.GetXmlElement(new XmlDocument()));
            SendXmlData(xml);
            return xml;
        }

        /// <summary>
        /// 删除制定节目
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public string DeleteDeviceProgram(HdProgram program)
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.DeleteProgram.ToString(), program.GetXmlElement(new XmlDocument()));
            SendXmlData(xml);
            return xml;
        }

        /// <summary>
        /// 获取字体信息
        /// </summary>
        public  void GetDeviceFontInfo()
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.GetAllFontInfo.ToString());
            SendXmlData(xml);
        }

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        public void GetTcpServerInfo()
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.GetSDKTcpServer.ToString());
            SendXmlData(xml);
        }

        /// <summary>
        /// 获取多屏同步标志
        /// </summary>
        public void GetMulScreenSync()
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.GetMulScreenSync.ToString());
            SendXmlData(xml);
        }

        /// <summary>
        /// 设置多屏同步标志
        /// </summary>
        public void SetMulScreenSync()
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.SetMulScreenSync.ToString());
            SendXmlData(xml);
        }

        /// <summary>
        /// 设置服务器信息
        /// </summary>
        /// <param name="info"></param>
        public void SetTcpServerInfo(ServerInfo info)
        {
            var list = info.GetXmlElements(null);
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.SetSDKTcpServer.ToString(), list);
            SendXmlData(xml);
        }

        /// <summary>
        /// 删除已上传到设备的文件
        /// </summary>
        /// <param name="fileNames">文件列表</param>
        public void DeleteFile(List<string> fileNames)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement filesElem = doc.CreateElement("files");
            foreach (string name in fileNames)
            {
                XmlElement itemElem = doc.CreateElement("file");
                itemElem.SetAttribute("name", name);
                filesElem.AppendChild(itemElem);
            }
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.DeleteFiles.ToString(), filesElem);
            SendXmlData(xml);
        }


        /// <summary>
        /// 删除一个文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void DeleteFile(string fileName)
        {
            DeleteFile(new List<string> { fileName });
        }

        /// <summary>
        /// 回读文件
        /// </summary>
        public void ReadbackFile()
        {

        }

        /// <summary>
        /// 回读已经上传到设备的文件列表
        /// </summary>
        public void ReadbackFileList()
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.GetFiles.ToString());
            SendXmlData(xml);
        }

        /// <summary>
        /// 从设备下载一个文件
        /// </summary>
        /// <param name="srcName"></param>
        /// <param name="savePath"></param>
        public void DownloadFileFromDevice(string srcName, string savePath)
        {
            byte[] startUpload = DataProtocol.GetDownloadFileStartAsk(srcName);
            lock (_dataLock)
            {
                _sendQueue.Enqueue(startUpload);
            }
            TryToSend();
        }
    }
}
