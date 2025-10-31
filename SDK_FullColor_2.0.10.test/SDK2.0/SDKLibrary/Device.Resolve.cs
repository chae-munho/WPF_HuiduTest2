using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SDKLibrary
{
    partial class Device
    {

       /// <summary>
       /// 发送下一包文件内容
       /// </summary>
        public void SendNextFilePacket()
        {
            if (UploadingItem.isSending)
            {
                byte[] buff = DataProtocol.GetUploadFileNextPacket(UploadingItem);
                if (buff != null)
                {
                    lock (_dataLock)
                    {
                        _sendQueue.Enqueue(buff);
                    }
                }
                else
                {
                    SendEndFileAsk();
                }

                SendNext();
            }
        }



        /// <summary>
        /// 结束发送文件
        /// </summary>
        private void SendEndFileAsk()
        {
            if (UploadingItem.isSending && !UploadingItem.hasFinished)
            {
                lock (_dataLock)
                {
                    _sendQueue.Enqueue(DataProtocol.GetFileEndAsk());
                }
            }
            HasStartSendFileContext = false;
        }

        /// <summary>
        /// 解析返下位机回来的tcp数据
        /// </summary>
        /// <param name="srcData"></param>
        public ErrorCode ResolveTcpReturnData(byte[] srcData,  out ResolveInfo ri)
        {

            ri = new ResolveInfo();
            ri.ownerDevice = this;

            ErrorCode error = ErrorCode.kUnknown;
            if (srcData.Length < 4)
            {
                return error;
            }
            int length = DataProtocol.GetInt(srcData[0], srcData[1]);
            CmdType cmd = (CmdType)DataProtocol.GetInt(srcData[2], srcData[3]);

            ri.cmdType = cmd;

            switch (cmd)
            {
                case CmdType.kTcpHeartbeatAnswer:
                    {
                        error = ErrorCode.kSuccess;
                    }
                    break;
                case CmdType.kSDKServiceAnswer:
                    {
                        if (srcData.Length >= 8)
                        {
                            int version = DataProtocol.GetInt(srcData[4], srcData[5], srcData[6], srcData[7]);
                            EnsureProtocolVersion = true;
                            error = ErrorCode.kSuccess;
                        }

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
                case CmdType.kSDKCmdAnswer:
                    {
                        if (length >= 12)
                        {
                            int nTotalXmlLen = DataProtocol.GetInt(srcData[4], srcData[5], srcData[6], srcData[7]);
                            int nIndex = DataProtocol.GetInt(srcData[8], srcData[9], srcData[10], srcData[11]);
                            if (SDKCmdAnswerXmlData == null || SDKCmdAnswerXmlData.Length != nTotalXmlLen)
                            {
                                SDKCmdAnswerXmlData = new byte[nTotalXmlLen];
                            }
                            Array.Copy(srcData, 12, SDKCmdAnswerXmlData, nIndex, length - 12);

                            if (nIndex + length - 12 == nTotalXmlLen)
                            {
                                string xml = Encoding.UTF8.GetString(SDKCmdAnswerXmlData);
                                SDKCmdAnswerXmlData = null;
                                SdkXmlDocument doc = new SdkXmlDocument();

                                doc.LoadXml(xml);
                                ri.srcXml = xml;

                                string method = null;
                                string result = null;
                                XmlNode outObj = null;
                                doc.ResolveXmlPacketData(out method, out result, out outObj);

                                ri.method = method;

                                if (result == "kSuccess")
                                {
                                    error = ErrorCode.kSuccess;
                                    SdkGuid = doc.SdkGuid;
                                    ResolveXmlPacketData(method, outObj, out ri.returnInfo);

                                    /*
                                    if (method == SdkMethod.GetEth0Info.ToString())
                                    {
                                        IPAddress ip = ((System.Net.IPEndPoint)Client.TcpClient.Client.RemoteEndPoint).Address;
                                        FileStream fs = new FileStream("d:\\" + ip.ToString() + ".xml", FileMode.Create);
                                        byte[] buff = Encoding.UTF8.GetBytes(xml);
                                        fs.Write(buff, 0, buff.Length);
                                        fs.Close();
                                    }
                                    */
                                }
                                else
                                {
                                    try
                                    {
                                        error = (ErrorCode)Enum.Parse(typeof(ErrorCode), result);
                                    }
                                    catch /*(System.Exception ex)*/
                                    {
                                        error = ErrorCode.kUnknown;
                                    }
                                }
                            }
                        }

                    }
                    break;

                case CmdType.kFileStartAnswer:  // 文件开始上传请求应答
                    {
                        if (srcData.Length >= 14)
                        {
                            ErrorCode state = (ErrorCode)DataProtocol.GetInt(srcData[4], srcData[5]);
                            if (state == (int)ErrorCode.kSuccess)
                            {
                                // 当前已经上传的内容大小
                                int sendedSize = DataProtocol.GetInt(srcData[6], srcData[7], srcData[8], srcData[9]);
                                error = ErrorCode.kSuccess;

                                if (UploadingItem.isSending)
                                {
                                    // 文件已经上传
                                    if (sendedSize == UploadingItem.fs.Length)
                                    {
                                        HasStartSendFileContext = false;
                                        lock (_dataLock)
                                        {
                                            _sendQueue.Enqueue(DataProtocol.GetFileEndAsk());
                                        }
                                    }
                                    else
                                    {
                                        HasStartSendFileContext = true;
                                        byte[] startUpload = DataProtocol.GetUploadFileNextPacket(UploadingItem, sendedSize);
                                        lock (_dataLock)
                                        {
                                            _sendQueue.Enqueue(startUpload);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                 error = state;
                            }
                        }
                    }
                    break;

                case CmdType.kFileEndAnswer:  // 结束文件开始上传请求应答
                    {
                        HasStartSendFileContext = false;
                        SendingFile = false;
                        if (srcData.Length >= 6)
                        {
                            ErrorCode state = (ErrorCode)DataProtocol.GetInt(srcData[4], srcData[5]);
                            if (state == (int)ErrorCode.kSuccess)
                            {
                                error = ErrorCode.kSuccess;
                                if (UploadingItem.isSending)
                                {
                                    ri.otherText = Path.GetFileName(UploadingItem.path) + " successfully uploaded.";
                                    UploadingItem.progress = "100%";
                                    UploadingItem.isSending = false;
                                    UploadingItem.hasFinished = true;
                                    UploadingItem.fs.Close();
                                    SendingFile = false;
                                    UploadingItem = new UploadFileInfo();
                                    ContinueUploadFile();
                                }
                            }
                        }
                    }
                    break;

                    
                case CmdType.kReadFileAnswer:   // 回读文件应答
                    {
                        DowningFile = false;
                        if (srcData.Length >= 49)
                        {
                            ErrorCode state = (ErrorCode)DataProtocol.GetInt(srcData[4], srcData[5]);
                            if (state == (int)ErrorCode.kSuccess)
                            {
                                string fileMd5 = Encoding.UTF8.GetString(srcData, 6, 33);
                                long fileSize = (UInt32)DataProtocol.GetInt(srcData[39], srcData[40], srcData[41], srcData[42]);
                                int filetype = DataProtocol.GetInt(srcData[47], srcData[48]);
                                error = ErrorCode.kSuccess;

                                byte[] startDownload = new byte[14]; 
                                startDownload[0] = (byte)startDownload.Length;
                                startDownload[1] = (byte)(startDownload.Length >> 8);
                                startDownload[2] = (byte)(Convert.ToInt32(CmdType.kFileStartAnswer));
                                startDownload[3] = (byte)(Convert.ToInt32(CmdType.kFileStartAnswer) >> 8);
                                startDownload[4] = (byte)filetype;
                                startDownload[5] = (byte)(filetype >> 8);

                                lock (_dataLock)
                                {
                                    _sendQueue.Enqueue(startDownload);
                                }

                            }
                        }
                    }
                    break;

     

                case CmdType.kFileContentAsk:
                    {
                        error = ErrorCode.kSuccess;
                    }
                    break;

                case CmdType.kFileEndAsk:   // 文件下载结束请求
                    {
                        error = ErrorCode.kSuccess;

                        byte[] startDownload = new byte[6];
                        startDownload[0] = (byte)startDownload.Length;
                        startDownload[1] = (byte)(startDownload.Length >> 8);
                        startDownload[2] = (byte)(Convert.ToInt32(CmdType.kFileEndAnswer));
                        startDownload[3] = (byte)(Convert.ToInt32(CmdType.kFileEndAnswer) >> 8);
                        startDownload[4] = (byte)(Convert.ToInt32(ErrorCode.kSuccess));
                        startDownload[5] = (byte)(Convert.ToInt32(ErrorCode.kSuccess) >> 8);
                        lock (_dataLock)
                        {
                            _sendQueue.Enqueue(startDownload);
                        }
                    }
                    break;

                default:
                    error = (int)ErrorCode.kSuccess;
                    break;
            }

            ri.errorCode = error;

            return error;
        }

        /// <summary>
        /// 解析具体返回来的sdk信息
        /// </summary>
        /// <param name="method"></param>
        /// <param name="outObj"></param>
        public void ResolveXmlPacketData(string method, XmlNode outObj, out object returnObj)
        {
            returnObj = null;

            #region GetIFVersion
            if (method == SdkMethod.GetIFVersion.ToString())        // SDK协议版本
            {
                foreach (XmlNode node in outObj.ChildNodes)
                {
                    if (node.Name == "version")
                    {
                        SdkVersion = node.Attributes["value"].Value;
                        returnObj = SdkVersion;
                        break;
                    }
                }
                // 刷新设备信息
                //_deviceInfo.deviceID = "NULL";
                RefreshDeviceInfo();
            }
            #endregion
            #region GetAllFontInfo
            else if (method == SdkMethod.GetAllFontInfo.ToString())     // 字体信息
            {
                List<FontInfo> fontInfo = new List<FontInfo>();
                returnObj = fontInfo;
                foreach (XmlNode node in outObj.ChildNodes)
                {
                    if (node.Name == "fonts")
                    {
                        foreach (XmlNode nodeFont in node.ChildNodes)
                        {
                            FontInfo info = new FontInfo();
                            info.fontName = nodeFont.Attributes["name"] != null ? nodeFont.Attributes["name"].Value : "";
                            info.fileName = nodeFont.Attributes["file"] != null ? nodeFont.Attributes["file"].Value : "";

                            if (nodeFont.Attributes["bold"] != null)
                            {
                                bool.TryParse(nodeFont.Attributes["bold"].Value, out info.bold);
                            }
                            if (nodeFont.Attributes["italic"] != null)
                            {
                                bool.TryParse(nodeFont.Attributes["italic"].Value, out info.bold);
                            }
                            if (nodeFont.Attributes["underline"] != null)
                            {
                                bool.TryParse(nodeFont.Attributes["underline"].Value, out info.bold);
                            }
                            fontInfo.Add(info);
                        }
                    }
                }
            }
            #endregion
            #region GetLuminancePloy
            else if (method == SdkMethod.GetLuminancePloy.ToString())    // 获取亮度设置
            {
                LuminanceInfo info = new LuminanceInfo();
                info.SetFromXmlNode(outObj);
                returnObj = info;
            }
            #endregion
            else if (method == SdkMethod.GetSwitchTime.ToString())
            {
                SwitchTimeInfo info = new SwitchTimeInfo();
                info.SetFromXmlNode(outObj);
                returnObj = info;
            }
            else if (method == SdkMethod.GetTimeInfo.ToString())
            {
                TimeInfo info = new TimeInfo();
                info.SetFromXmlNode(outObj);
                returnObj = info;
            }
            else if (method == SdkMethod.GetBootLogo.ToString())
            {
                BootLogoInfo info = new BootLogoInfo();
                info.SetFromXmlNode(outObj);
                returnObj = info;
            }
            else if (method == SdkMethod.GetSDKTcpServer.ToString())
            {
                ServerInfo info = new ServerInfo();
                info.SetFromXmlNode(outObj);
                returnObj = info;
            }
            else if (method == SdkMethod.GetDeviceInfo.ToString())
            {
                DeviceInfo info = new DeviceInfo();
                _deviceInfo.SetFromXmlNode(outObj);
                returnObj = _deviceInfo;
                _deviceInfo.ethernetInfo.ip = ((System.Net.IPEndPoint)Client.TcpClient.Client.RemoteEndPoint).Address.ToString();

                // 删除ID一样的设备 （有可能已经掉线还没来得及下线）
                List<Device> Devices = CommunicationManager.GetDevices();
                foreach (var device in Devices)
                {
                    if (device.GetDeviceInfo().deviceID == _deviceInfo.deviceID && this != device)
                    {
                        CommunicationManager.Close(device);
                        break;
                    }
                }
               // _deviceInfo = info;
            }
            else if (method == SdkMethod.GetPppoeInfo.ToString())
            {
                PppoeInfo info = new PppoeInfo();
                //info.SetFromXmlNode(outObj);
                returnObj = info;
                //_deviceInfo = info;
            }
            else if (method == SdkMethod.GetEth0Info.ToString())
            {
                EthernetInfo info = new EthernetInfo();
                info.SetFromXmlNode(outObj);
                returnObj = info;
                if (info.enable)
                {
                    _deviceInfo.ethernetInfo = info;
                }
            }
            else if (method == SdkMethod.GetWifiInfo.ToString())
            {
                WifiInfo info = new WifiInfo();
                info.SetFromXmlNode(outObj);
                returnObj = info;
                _deviceInfo.wifiInfo = info;
            }

            else if (method == SdkMethod.GetFiles.ToString())
            {
                ReadbackFileListInfo info = new ReadbackFileListInfo();
                info.SetFromXmlNode(outObj);
                returnObj = info;
                _deviceInfo.fileListInfo = info;
            }

            else if (method == SdkMethod.DeleteFiles.ToString())
            {

            }
        }

    }
}
