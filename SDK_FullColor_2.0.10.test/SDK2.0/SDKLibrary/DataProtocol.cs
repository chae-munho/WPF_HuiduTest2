using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SDKLibrary
{
    public class DataProtocol
    {
        public DataProtocol()
        {

        }

      public const int _maxContentLength = 8000;
      public const int _tcpHeaderLength = 4;
      public const int _udpHeaderLength = 6;
      public const int _tcpSdkCmdHeaderLength = 12;

      public const int _MAX_DEVICE_ID_LENGHT = 15;
      public const int _MD5_LENGHT = 32;
      public const int _LOCAL_TCP_VERSION = 0x1000005;
      public const int _LOCAL_UDP_VERSION = 0x1000005;
      public const int _SDK_VERSION = 0x1000000;
        /// <summary>
        /// 获得传输协议版本协商命令包
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static byte[]  GetTransportProtocolVersionCmd(int version = _LOCAL_TCP_VERSION)
        {
            byte[] buff = new byte[8];
            buff[0] = (byte)(buff.Length & 0xff);
            buff[1] = (byte)((buff.Length >> 8) & 0xff);
            buff[2] = (byte)(Convert.ToInt32(CmdType.kSDKServiceAsk) & 0xff);
            buff[3] = (byte)((Convert.ToInt32(CmdType.kSDKServiceAsk) >> 8) & 0xff); 
            buff[4] = (byte)(version & 0xff); ;
            buff[5] = (byte)((version >> 8) & 0xff);
            buff[6] = (byte)((version >> 16) & 0xff);
            buff[7] = (byte)((version >> 24) & 0xff);
            return buff;
        }

        /// <summary>
        /// 获得sdk版本命令
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static byte[] GetSdkVersionCmd(int version = _SDK_VERSION)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement versionElem = doc.CreateElement("version");
            versionElem.SetAttribute("value", Convert.ToString(version, 16));
            string xml = DataProtocol.GetSdkCmdXml("##GUID", SdkMethod.GetIFVersion.ToString(), versionElem);
           
            //string xml = "<?xml version = \"1.0\" encoding = \"utf-8\" ?>"
            //            + "<sdk guid = \"##GUID\"><in method = \"GetIFVersion\" ><version value = \"##sdkVersion\"/></in></sdk>";
            //string versionChars = System.Convert.ToString(version, 16);
            //xml = xml.Replace("##sdkVersion", versionChars);
            //byte[] buff = Encoding.UTF8.GetBytes(xml);
            var list = GetSDKCmdAsk(xml);
            return list.ElementAt(0);
        }


        /// <summary>
        /// 获得心跳包命令
        /// </summary>
        /// <returns></returns>
        public static byte[] GetkTcpHeartbeatAsk()
        {
            byte[] buff = new byte[4];
            buff[0] = (byte)(buff.Length & 0xff);
            buff[1] = (byte)((buff.Length >> 8) & 0xff);
            buff[2] = (byte)(Convert.ToInt32(CmdType.kTcpHeartbeatAsk) & 0xff);
            buff[3] = (byte)((Convert.ToInt32(CmdType.kTcpHeartbeatAsk) >> 8) & 0xff);
            return buff;
        }

        /// <summary>
        /// 获得上传文件开始请求命令包
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="md5"></param>
        /// <param name="fileSize"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static byte[] GetUploadFileStartAsk(string fileName, string md5, int fileSize, int type)
        {
            byte[] name = Encoding.UTF8.GetBytes(fileName);

            int headLen = 47;

            byte[] buff = new byte[headLen + name.Length+1];
            Array.Copy(name, 0, buff, headLen, name.Length);

            buff[0] = (byte)(buff.Length & 0xff);
            buff[1] = (byte)((buff.Length >> 8) & 0xff);

            buff[2] = (byte)(Convert.ToInt32(CmdType.kFileStartAsk) & 0xff);
            buff[3] = (byte)((Convert.ToInt32(CmdType.kFileStartAsk) >> 8) & 0xff);

            byte[] md5Bytes = Encoding.UTF8.GetBytes(md5);
            Array.Copy(md5Bytes, 0, buff, 4, md5Bytes.Length);
            buff[37] = (byte)(fileSize & 0xff); ;
            buff[38] = (byte)((fileSize >> 8) & 0xff);
            buff[39] = (byte)((fileSize >> 16) & 0xff);
            buff[40] = (byte)((fileSize >> 24) & 0xff);

            buff[45] = (byte)((type) & 0xff);
            buff[46] = (byte)((type >> 8) & 0xff);

            return buff;

        }


        /// <summary>
        /// 结束文件传输
        /// </summary>
        /// <returns></returns>
        public static byte[] GetFileEndAsk()
        {
            byte[] endBuf = new byte[4];

            endBuf[0] = (byte)(4 & 0xff);
            endBuf[1] = (byte)((4 >> 8) & 0xff);
            endBuf[2] = (byte)(Convert.ToInt32(CmdType.kFileEndAsk) & 0xff);
            endBuf[3] = (byte)((Convert.ToInt32(CmdType.kFileEndAsk) >> 8) & 0xff);
            return endBuf;
        }
        /// <summary>
        /// 结束文件传输应答
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static byte[] GetFileEndAnswer(ErrorCode code = ErrorCode.kSuccess)
        {
            byte[] endBuf = new byte[6];

            endBuf[0] = (byte)(endBuf.Length & 0xff);
            endBuf[1] = (byte)((endBuf.Length >> 8) & 0xff);
            endBuf[2] = (byte)(Convert.ToInt32(CmdType.kFileEndAnswer) & 0xff);
            endBuf[3] = (byte)((Convert.ToInt32(CmdType.kFileEndAnswer) >> 8) & 0xff);
            endBuf[4] = (byte)((int)code & 0xff);
            endBuf[5] = (byte)((int)code & 0xff);
            return endBuf;
        }


        /// <summary>
        /// 获取文件下一包
        /// </summary>
        /// <param name="fileinfo"></param>
        /// <returns></returns>
        public static byte[] GetUploadFileNextPacket(UploadFileInfo fileinfo, int nOffset = 0)
        {
            byte[] content = new byte[_tcpHeaderLength + _maxContentLength];

            fileinfo.fs.Position += nOffset;
            fileinfo.progress = ((double)fileinfo.fs.Position / (double)fileinfo.fs.Length * 100).ToString("f2") + "%";

            int len = fileinfo.fs.Read(content, _tcpHeaderLength, _maxContentLength);
            if (len == 0)
            {
                fileinfo.progress = "99.99%";
                return null;
            }
            else if (len != _maxContentLength)
            {
                // 不到_maxContentLength长度的内容时重新赋值
                byte[] contentTemp = new byte[_tcpHeaderLength+len];
                Array.Copy(content, 0, contentTemp, 0, contentTemp.Length);
                content = contentTemp;
            }

            content[0] = (byte)((content.Length) & 0xff);
            content[1] = (byte)(((content.Length) >> 8) & 0xff);

            content[2] = (byte)(Convert.ToInt32(CmdType.kFileContentAsk) & 0xff);
            content[3] = (byte)((Convert.ToInt32(CmdType.kFileContentAsk) >> 8) & 0xff);

            return content;
        }

        /// <summary>
        /// 获取开始下载文件请求命令
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] GetDownloadFileStartAsk(string fileName)
        {
            byte[] name = Encoding.UTF8.GetBytes(fileName);

            int headLen = 4;

            byte[] buff = new byte[headLen + name.Length + 1];
            Array.Copy(name, 0, buff, headLen, name.Length);

            buff[0] = (byte)(buff.Length & 0xff);
            buff[1] = (byte)((buff.Length >> 8) & 0xff);

            buff[2] = (byte)(Convert.ToInt32(CmdType.kReadFileAsk) & 0xff);
            buff[3] = (byte)((Convert.ToInt32(CmdType.kReadFileAsk) >> 8) & 0xff);

            return buff;
        }

        /// <summary>
        /// 获取sdk协议交互数据包
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static List<byte[]> GetSDKCmdAsk(string srcxmlData)
        {
            List<byte[]> packets = new List<byte[]>();

            byte[] xmlData = Encoding.UTF8.GetBytes(srcxmlData);

            for (int i = 0; i < xmlData.Length; i += _maxContentLength)
            {
                int buffsize = xmlData.Length - i >= _maxContentLength ? _maxContentLength : xmlData.Length - i;
                byte[] buff = new byte[_tcpSdkCmdHeaderLength + buffsize];
                Array.Copy(xmlData, i, buff, _tcpSdkCmdHeaderLength, buffsize);

                buff[0] = (byte)(buff.Length & 0xff);
                buff[1] = (byte)((buff.Length >> 8) & 0xff);

                buff[2] = (byte)(Convert.ToInt32(CmdType.kSDKCmdAsk) & 0xff);
                buff[3] = (byte)((Convert.ToInt32(CmdType.kSDKCmdAsk) >> 8) & 0xff);

                buff[4] = (byte)(xmlData.Length & 0xff); ;
                buff[5] = (byte)((xmlData.Length >> 8) & 0xff);
                buff[6] = (byte)((xmlData.Length >> 16) & 0xff);
                buff[7] = (byte)((xmlData.Length >> 24) & 0xff);

                int index =  i;
                buff[8] = (byte)(index & 0xff); ;
                buff[9] = (byte)((index >> 8) & 0xff);
                buff[10] = (byte)((index >> 16) & 0xff);
                buff[11] = (byte)((index >> 24) & 0xff);

                packets.Add(buff);
            }

            return packets;
        }


        /// <summary>
        /// 打包udp数据
        /// </summary>
        /// <param name="int version">版本号</param>
        /// <param name="CmdType cmd">命令</param>
        /// <param name="byte[] srcData">待打包数据</param>
        /// <param name="params byte[] deviceID">设备ID，该值为null时命令针对所有设备有效</param>
        /// <returns>List<byte[]></returns>	
        public static byte[] packetUdppData(int version, CmdType cmd, byte[] srcData, params byte[] deviceID)
        {
            int nSize = _udpHeaderLength;

            if (srcData != null)
            {
                nSize += srcData.Length;
            }
            if (deviceID != null && deviceID.Length > 0)
            {
                nSize += _MAX_DEVICE_ID_LENGHT;
            }


            byte[] buff = new byte[nSize];

            if (deviceID != null && deviceID.Length > 0)
            {
                Array.Copy(deviceID, 0, buff, _udpHeaderLength, deviceID.Length > _MAX_DEVICE_ID_LENGHT ? _MAX_DEVICE_ID_LENGHT : deviceID.Length);
            }

            if (srcData != null)
            {
                Array.Copy(srcData, 0, buff, _udpHeaderLength + _MAX_DEVICE_ID_LENGHT, srcData.Length);

            }
            buff[0] = (byte)(version & 0xff);
            buff[1] = (byte)((version >> 8) & 0xff);
            buff[2] = (byte)((version >> 16) & 0xff);
            buff[3] = (byte)((version >> 24) & 0xff);

            buff[4] = (byte)(Convert.ToInt32(cmd) & 0xff);
            buff[5] = (byte)((Convert.ToInt32(cmd) >> 8) & 0xff);

            return buff;
        }

        /// <summary>
        /// 获取指定设备的TCP服务器信息的数据包
        /// </summary>
        /// <param name="deviceIDString"></param>
        /// <returns></returns>
        public static byte[] GetUdpCmdGetTCPServerInfo(string deviceIDString)
        {
            string xml = DataProtocol.GetSdkCmdXml("##GUID", SdkMethod.GetSDKTcpServer.ToString());
            byte[] xmlData = Encoding.UTF8.GetBytes(xml);
            byte[] deviceID = Encoding.UTF8.GetBytes(deviceIDString);
            byte[] buff = packetUdppData(_LOCAL_UDP_VERSION, CmdType.kSDKCmdAsk, xmlData, deviceID);
            return buff;
        }

        /// <summary>
        /// 获取设置指定设备TCP服务器信息的数据包
        /// </summary>
        /// <param name="deviceIDString"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static byte[] GetUdpCmdSetTCPServerInfo(string deviceIDString, string host, int port = 10001)
        {
            /*
            byte[] buff = new byte[6];
            buff[0] = (byte)(DataProtocol._LOCAL_UDP_VERSION & 0xff);
            buff[1] = (byte)((DataProtocol._LOCAL_UDP_VERSION >> 8) & 0xff);
            buff[2] = (byte)((DataProtocol._LOCAL_UDP_VERSION >> 16) & 0xff);
            buff[3] = (byte)((DataProtocol._LOCAL_UDP_VERSION >> 24) & 0xff);
            buff[4] = (byte)(Convert.ToInt32(CmdType.kSearchDeviceAsk) & 0xff);
            buff[5] = (byte)((Convert.ToInt32(CmdType.kSearchDeviceAsk) >> 8) & 0xff);
            return buff;
            */

           return packetUdppData(_LOCAL_UDP_VERSION, CmdType.kSearchDeviceAsk, null);
        }

        /// <summary>
        /// 获得UDP扫描包
        /// </summary>
        /// <returns></returns>
        public static byte[] GetUdpCmdScan()
        {
            byte[] buff = new byte[6];
            buff[0] = (byte)(DataProtocol._LOCAL_UDP_VERSION & 0xff);
            buff[1] = (byte)((DataProtocol._LOCAL_UDP_VERSION >> 8) & 0xff);
            buff[2] = (byte)((DataProtocol._LOCAL_UDP_VERSION >> 16) & 0xff);
            buff[3] = (byte)((DataProtocol._LOCAL_UDP_VERSION >> 24) & 0xff);
            buff[4] = (byte)(Convert.ToInt32(CmdType.kSearchDeviceAsk) & 0xff);
            buff[5] = (byte)((Convert.ToInt32(CmdType.kSearchDeviceAsk) >> 8) & 0xff);
            return buff;
        }


        public static int GetInt(byte a1, byte a2)
        {
            return GetInt(a1, a2, 0, 0);
        }

        public static int GetInt(byte a1, byte a2, byte a3, byte a4)
        {
            return (int) ((a1 & 0xff) | ((a2 << 8) & 0xff00) | ((a3 << 16) & 0xff0000) | ((a4<<24)));
        }

        /// <summary>
        /// 获得SDK 命令 
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetSdkCmdXml(string sdkGUID, string method)
        {
            List<XmlElement> list = new List<XmlElement>();
            return GetSdkCmdXml(sdkGUID, method, list);
        }

        /// <summary>
        /// 获得SDK命令
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <param name="method"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetSdkCmdXml(string sdkGUID, string method, XmlElement element)
        {
            List<XmlElement> list = new List<XmlElement>();
            if (element != null)
            {
                list.Add(element);
            }
            return GetSdkCmdXml(sdkGUID, method, list);
        }
    
        /// <summary>
        /// 获得SDK命令
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <param name="method"></param>
        /// <param name="allElement"></param>
        /// <returns></returns>
        public static string GetSdkCmdXml(string sdkGUID, string method, List<XmlElement> allElement)
        {
            XmlDocument doc = null;
            if (allElement == null || allElement.Count == 0)
            {
                doc = new XmlDocument();
            }
            else
            {
                doc = allElement[0].OwnerDocument;
            }

            //XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            //XmlElement root = doc.DocumentElement;
            //doc.InsertBefore(xmldecl, root);

            XmlElement sdkElem = doc.CreateElement("sdk");
            sdkElem.SetAttribute("guid", sdkGUID);
            doc.AppendChild(sdkElem);

            XmlElement inElem = doc.CreateElement("in");
            inElem.SetAttribute("method", method);
            sdkElem.AppendChild(inElem);

            foreach (XmlElement node in allElement)
            {
                inElem.AppendChild(node);
            }

            MemoryStream stream = new MemoryStream();
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Encoding = new UTF8Encoding(false);
            setting.Indent = true;

            XmlWriter writer = XmlWriter.Create(stream, setting);
            doc.Save(writer);

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }


}
