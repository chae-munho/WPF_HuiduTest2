using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace SDKLibrary
{
    // 特效类型
    public enum EffectType
    {
        IMMEDIATE_SHOW = 0,    ///< 直接显示.
        LEFT_PARALLEL_MOVE = 1,    ///< 向左平移.
        RIGHT_PARALLEL_MOVE = 2,    ///< 向右平移.
        UP_PARALLEL_MOVE = 3,    ///< 向上平移.
        DOWN_PARALLEL_MOVE = 4,    ///< 向下平移.
        LEFT_COVER = 5,    ///< 向左覆盖.
        RIGHT_COVER = 6,    ///< 向右覆盖.
        UP_COVER = 7,    ///< 向上覆盖.
        DOWN_COVER = 8,    ///< 向下覆盖.
        LEFT_TOP_COVER = 9,    ///< 左上覆盖.
        LEFT_BOTTOM_COVER = 10,   ///< 左下覆盖.
        RIGHT_TOP_COVER = 11,   ///< 右上覆盖.
        RIGHT_BOTTOM_COVER = 12,   ///< 右下覆盖.
        HORIZONTAL_DIVIDE = 13,   ///< 水平对开.
        VERTICAL_DIVIDE = 14,   ///< 垂直对开.
        HORIZONTAL_CLOSE = 15,   ///< 水平闭合.
        VERTICAL_CLOSE = 16,   ///< 垂直闭合.
        FADE = 17,              ///< 淡入淡出.
        HORIZONTAL_SHUTTER = 18,   ///< 水平百叶窗.
        VERTICAL_SHUTTER = 19,   ///< 垂直百叶窗.
        NOT_CLEAR_AREA = 20,   ///< 不清屏.
        LEFT_SERIES_MOVE = 21,   ///< 连续左移.
        RIGHT_SERIES_MOVE = 22,   ///< 连续右移.
        UP_SERIES_MOVE = 23,   ///< 连续上移.
        DOWN_SERIES_MOVE = 24,   ///< 连续下移.
        RANDOM = 25,   ///< 随机特效.
        HT_LEFT_SERIES_MOVE = 26,   ///< 首尾相接连续左移.
        HT_RIGHT_SERIES_MOVE = 27,   ///< 首尾相接连续右移.
        HT_UP_SERIES_MOVE = 28,   ///< 首尾相接连续上移.
        HT_DOWN_SERIES_MOVE = 29,   ///< 首尾相接连续下移.
        EFFECT_COUNTS = 30,   ///< 特效总数.
    };

    public enum CmdType
    {
        kUnknown = -1,
        kTcpHeartbeatAsk = 0x005f,      ///< TCP心跳包请求
        kTcpHeartbeatAnswer = 0x0060,   ///< TCP心跳包反馈
        kSearchDeviceAsk = 0x1001,      ///< 搜索设备请求
        kSearchDeviceAnswer = 0x1002,   ///< 搜索设备应答
        kErrorAnswer = 0x2000,          ///< 出错反馈
        kSDKServiceAsk = 0x2001,        ///< 版本协商请求
        kSDKServiceAnswer = 0x2002,     ///< 版本协商应答
        kSDKCmdAsk = 0x2003,            ///< sdk命令请求
        kSDKCmdAnswer = 0x2004,         ///< sdk命令反馈
        kGPSInfoAnswer = 0x3007,        ///<gps信息应答
        kFileStartAsk = 0x8001,         ///< 文件开始传输请求
        kFileStartAnswer = 0x8002,      ///< 文件开始传输应答
        kFileContentAsk = 0x8003,       ///< 携带文件内容的请求
        kFileContentAnswer = 0x8004,    ///< 写文件内容的应答
        kFileEndAsk = 0x8005,           ///< 文件结束传输请求
        kFileEndAnswer = 0x8006,        ///< 文件结束传输应答
        kReadFileAsk = 0x8007,          ///< 回读文件请求
        kReadFileAnswer = 0x8008,       ///< 回读文件应答

    };

    public enum ErrorCode
    {
        kUnknown = -1,
        kSuccess = 0,
        kWriteFinish,           ///< 写文件完成
        kProcessError,          ///< 流程错误
        kVersionTooLow,         ///< 版本过低
        kDeviceOccupa,          ///< 设备被占用
        kFileOccupa,            ///< 文件被占用
        kReadFileExcessive,     ///< 回读文件用户过多
        kInvalidPacketLen,      ///< 数据包长度错误
        kInvalidParam,          ///< 无效的参数
        kNotSpaceToSave,        ///< 存储空间不够
        kCreateFileFailed,      ///< 创建文件失败
        kWriteFileFailed,       ///< 写文件失败
        kReadFileFailed,        ///< 读文件失败
        kInvalidFileData,       ///< 无效的文件数据
        kFileContentError,      ///< 文件内容出错
        kOpenFileFailed,        ///< 打开文件失败
        kSeekFileFailed,        ///< 定位文件失败
        kRenameFailed,          ///< 重命名失败
        kFileNotFound,          ///< 文件未找到
        kFileNotFinish,         ///< 文件未接收完成
        kXmlCmdTooLong,         ///< xml命令过长
        kInvalidXmlIndex,       ///< 无效的xml命令索引值
        kParseXmlFailed,        ///< 解析xml出错
        kInvalidMethod,         ///< 无效的方法名
        kMemoryFailed,          ///< 内存错误
        kSystemError,           ///< 系统错误
        kUnsupportVideo,        ///< 不支持的视频
        kNotMediaFile,          ///< 不是多媒体文件
        kParseVideoFailed,      ///< 解析视频文件失败
        kUnsupportFrameRate,    ///< 不支持的波特率
        kUnsupportResolution,   ///< 不支持的分辨率(视频)
        kUnsupportFormat,       ///< 不支持的格式(视频)
        kUnsupportDuration,     ///< 不支持的时间长度(视频)
        kDownloadFileFailed,    ///< 下载文件失败

        kScreenNodeIsNull,      ///< 显示屏节点为null
        kNodeExist,             ///< 节点存在
        kNodeNotExist,          ///< 节点不存在
        kPluginNotExist,        ///< 插件不存在
        kCheckLicenseFailed,    ///< 校验license失败
        kNotFoundWifiModule,    ///< 未找到wifi模块
        kTestWifiUnsuccessful,  ///< 测试wifi模块未
        kRunningError,          ///< 运行错误
        kUnsupportMethod,       ///< 不支持的方法
        kInvalidGUID,           ///< 非法的guid
        kFirmwareFormatError,   ///< 固件格式错误
        kTagNotFound,           ///< 标签不存在
        kAttrNotFound,          ///< 属性不存在
        kCreateTagFailed,       ///< 创建标签失败
        kUnsupportDeviceType,   ///< 不支持的设备型号
        kPermissionDenied,      ///< 权限不足
        kPasswdTooSimple,       ///< 密码太简单

        //非错误码
        kDelayRespond,          ///< 延迟反馈
        kShortlyReturn,         ///< 直接返回, 不进行xml转换

        kCount,
    }

/*
    enum HErrorCode
    {
        kSuccess = 0,
        kWriteFinish,           ///< 写文件完成
        kProcessError,          ///< 流程错误
        kVersionTooLow,         ///< 版本过低
        kDeviceOccupa,          ///< 设备被占用
        kFileOccupa,            ///< 文件被占用
        kReadFileExcessive,     ///< 回读文件用户过多
        kInvalidPacketLen,      ///< 数据包长度错误
        kInvalidParam,          ///< 无效的参数
        kNotSpaceToSave,        ///< 存储空间不够
        kCreateFileFailed,      ///< 创建文件失败
        kWriteFileFailed,       ///< 写文件失败
        kReadFileFailed,        ///< 读文件失败
        kInvalidFileData,       ///< 无效的文件数据
        kFileContentError,      ///< 文件内容出错
        kOpenFileFailed,        ///< 打开文件失败
        kSeekFileFailed,        ///< 定位文件失败
        kRenameFailed,          ///< 重命名失败
        kFileNotFound,          ///< 文件未找到
        kFileNotFinish,         ///< 文件未接收完成
        kXmlCmdTooLong,         ///< xml命令过长
        kInvalidXmlIndex,       ///< 无效的xml命令索引值
        kParseXmlFailed,        ///< 解析xml出错
        kInvalidMethod,         ///< 无效的方法名
        kMemoryFailed,          ///< 内存错误
        kSystemError,           ///< 系统错误
        kUnsupportVideo,        ///< 不支持的视频
        kNotMediaFile,          ///< 不是多媒体文件
        kParseVideoFailed,      ///< 解析视频文件失败
        kUnsupportFrameRate,    ///< 不支持的波特率
        kUnsupportResolution,   ///< 不支持的分辨率(视频)
        kUnsupportFormat,       ///< 不支持的格式(视频)
        kUnsupportDuration,     ///< 不支持的时间长度(视频)
        kDownloadFileFailed,    ///< 下载文件失败

        kScreenNodeIsNull,      ///< 显示屏节点为null
        kNodeExist,             ///< 节点存在
        kNodeNotExist,          ///< 节点不存在
        kPluginNotExist,        ///< 插件不存在
        kCheckLicenseFailed,    ///< 校验license失败
        kNotFoundWifiModule,    ///< 未找到wifi模块
        kTestWifiUnsuccessful,  ///< 测试wifi模块未
        kRunningError,          ///< 运行错误
        kUnsupportMethod,       ///< 不支持的方法
        kInvalidGUID,           ///< 非法的guid
        kFirmwareFormatError,   ///< 固件格式错误
        kTagNotFound,           ///< 标签不存在
        kAttrNotFound,          ///< 属性不存在
        kCreateTagFailed,       ///< 创建标签失败
        kUnsupportDeviceType,   ///< 不支持的设备型号
        kPermissionDenied,      ///< 权限不足
        kPasswdTooSimple,       ///< 密码太简单

        //非错误码
        kDelayRespond,          ///< 延迟反馈
        kShortlyReturn,         ///< 直接返回, 不进行xml转换
    };
    */

    /// <summary>
    /// 解析出来的信息
    /// </summary>
    public class ResolveInfo
    {
        public Device ownerDevice;      // 设备
        public CmdType cmdType;         // 命令类型
        public ErrorCode errorCode;     // 错误代码
        public string method;           // 方法名（非null时有效）
        public object returnInfo;       // 返回结果（非null时有效）

        public string otherText;
        public string time;
        public string srcXml;
        public ResolveInfo()
        {
            time = DateTime.Now.ToString() + " ";
            ownerDevice = null;
            cmdType = CmdType.kUnknown;
            errorCode = ErrorCode.kUnknown;
            method = null;
            returnInfo = null;
            otherText = null;
        }
    }

    /// <summary>
    /// Sdk 传输协议版本信息
    /// </summary>
    public class SdkServicVersionInfo
    {
        public int verson;
    }


    // 服务器信息
    public class ServerInfo
    {
        public string host; // tcp服务器ip或者域名
        public int port;    // 服务器监听端口

        public ServerInfo()
        {
            host = "192.168.1.250";
            port = 10001;
        }



        /// <summary>
        /// 获取服务器信息命令xml
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <returns></returns>
        public string GetServerInfoToXml(string sdkGUID)
        {
            string xml = DataProtocol.GetSdkCmdXml(sdkGUID, SdkMethod.GetSDKTcpServer.ToString());
            return xml;
        }

        /// <summary>
        /// 获取设置服务器信息命令xml
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <returns></returns>
        public string SetServerInfoToXml(string sdkGUID)
        {
            var list = GetXmlElements(null);
            string xml = DataProtocol.GetSdkCmdXml(sdkGUID, SdkMethod.SetSDKTcpServer.ToString(), list);
            return xml;
        }




        // 获得对象元素集
        public List<XmlElement> GetXmlElements(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            List<XmlElement> list = new List<XmlElement>();
            XmlElement serverElem = doc.CreateElement("server");
            serverElem.SetAttribute("host", host);
            serverElem.SetAttribute("port", port.ToString());

            list.Add(serverElem);
            return list;
        }

        // 解析元素 
        public void SetFromXmlNode(XmlNode element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name == "server")
                {
                    port = int.Parse(node.Attributes["port"].Value);
                    host = node.Attributes["host"].Value;
                }
            }
        }
    }

    // 以太网信息
    public class EthernetInfo
    {
        public bool enable;
        public bool isAutoDHCP;
        public string ip;
        public string mask;
        public string gateway;
        public string dns;


        public EthernetInfo()
        {
            enable = true;
            isAutoDHCP = true;
            ip = mask = gateway = dns = "";
        }

        /// <summary>
        /// 获取命令xml
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <returns></returns>
        public string GetCmdToXml(string sdkGUID)
        {
            string xml = DataProtocol.GetSdkCmdXml(sdkGUID, SdkMethod.GetEth0Info.ToString());
            return xml;
        }

        /// <summary>
        /// 设置命令xml
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <returns></returns>
        public string SetCmdToXml(string sdkGUID)
        {
            var list = GetXmlElements(null);
            string xml = DataProtocol.GetSdkCmdXml(sdkGUID, SdkMethod.SetEth0Info.ToString(), list);
            return xml;
        }


        // 获得对象元素集
        public List<XmlElement> GetXmlElements(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            List<XmlElement> list = new List<XmlElement>();
            XmlElement ethElem = doc.CreateElement("eth");
            ethElem.SetAttribute("valid", true.ToString().ToLower());

            XmlElement enableElem = doc.CreateElement("enable");
            enableElem.SetAttribute("value", true.ToString().ToLower());

            XmlElement dhcpElem = doc.CreateElement("dhcp");
            dhcpElem.SetAttribute("auto", isAutoDHCP.ToString().ToLower());

            XmlElement addressElem = doc.CreateElement("address");
            addressElem.SetAttribute("ip", ip);
            addressElem.SetAttribute("netmask", mask);
            addressElem.SetAttribute("gateway", gateway);
            addressElem.SetAttribute("dns", dns);

            ethElem.AppendChild(enableElem);
            ethElem.AppendChild(dhcpElem);
            ethElem.AppendChild(addressElem);
            list.Add(ethElem);
            return list;
        }

        // 解析元素 
        public void SetFromXmlNode(XmlNode element)
        {
            XmlNode ethNode = element.SelectSingleNode("eth");
            XmlNode enableNode = element.SelectNodes("eth/enable")[0];
            XmlNode dhcpNode = element.SelectNodes("eth/dhcp")[0];
            XmlNode addrNode = element.SelectNodes("eth/address")[0];

            // valid = ethNode.Attributes["valid"].InnerText;
            enable = Boolean.Parse(enableNode.Attributes["value"].Value);
            isAutoDHCP = Boolean.Parse(dhcpNode.Attributes["auto"].Value);
            ip = addrNode.Attributes["ip"].Value;
            mask = addrNode.Attributes["netmask"].Value;
            gateway = addrNode.Attributes["gateway"].Value;
            dns = addrNode.Attributes["dns"].Value;
        }

    }

    // Wifi信息
    public class WifiInfo
    {
        public class APInfo
        {
            public string ssid;
            public string password;
            public string mac;
            public bool autpdhcp;
            public string ip;
            public string mask;
            public string gate;
            public string dhcp;
            public string channel;    // 信道
            public string encryption; // 加密方式

            public APInfo()
            {
                autpdhcp = false;
                ssid = password = mac = ip = mask = gate = dhcp;
            }
        }

        public bool hasWifi;      // 是否有wifi模块
        public bool enable;       // 是否有wifi网络接入
        public int workMode;      // 0 ap 模式 1 station模式
        public APInfo apInfo;     // 设备ap模式信息

        public string stationSSID;
        public string stationPassword;
        public string stationMAC;
        public int stationCurrentIndex;  // 表示当前使用的ap节点在list中的索引值, -1时表示未选中ap节点
        public List<APInfo> allAp;       // 设备发现的ap信息 （仅 getwifiinfo时有效）
        public WifiInfo()
        {
            hasWifi = false;
            enable = false; 
            workMode = 0;
            apInfo = new APInfo();
            allAp = new List<APInfo>();
            stationCurrentIndex = -1;
        }

        /// <summary>
        /// 获取命令xml
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <returns></returns>
        public string GetCmdToXml(string sdkGUID)
        {
            string xml = DataProtocol.GetSdkCmdXml(sdkGUID, SdkMethod.GetWifiInfo.ToString());
            return xml;
        }

        /// <summary>
        /// 设置命令xml
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <returns></returns>
        public string SetCmdToXml(string sdkGUID)
        {
            var list = GetXmlElements(null);
            string xml = DataProtocol.GetSdkCmdXml(sdkGUID, SdkMethod.GetWifiInfo.ToString(), list);
            return xml;
        }


        // 获得对象元素集
        public List<XmlElement> GetXmlElements(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            List<XmlElement> list = new List<XmlElement>();
            XmlElement modeElem = doc.CreateElement("mode");
            modeElem.SetAttribute("value", workMode == 0 ? "ap" : "station");
            list.Add(modeElem);

            XmlElement apElem = doc.CreateElement("ap");

            XmlElement apssidElem = doc.CreateElement("ssid");
            XmlElement appasswdElem = doc.CreateElement("passwd");
            XmlElement apchannelElem = doc.CreateElement("channel");
            XmlElement apenctyptionElem = doc.CreateElement("encryption");
            XmlElement apdhcpElem = doc.CreateElement("dhcp");
            XmlElement apaddressElem = doc.CreateElement("address");
            apElem.AppendChild(apssidElem);
            apElem.AppendChild(appasswdElem);
            apElem.AppendChild(apchannelElem);
            apElem.AppendChild(apenctyptionElem);
            apElem.AppendChild(apdhcpElem);
            apElem.AppendChild(apaddressElem);

            apssidElem.SetAttribute("value", apInfo.ssid);
            appasswdElem.SetAttribute("value", apInfo.password);
            apchannelElem.SetAttribute("value", apInfo.channel);
            apenctyptionElem.SetAttribute("value", "WPA-PSK");
            apdhcpElem.SetAttribute("auto", true.ToString().ToLower());

            apaddressElem.SetAttribute("ip", "0.0.0.0");
            apaddressElem.SetAttribute("netmask", "0.0.0.0");
            apaddressElem.SetAttribute("gateway", "0.0.0.0");
            apaddressElem.SetAttribute("dns", "0.0.0.0");

            list.Add(modeElem);

            XmlElement stationElem = doc.CreateElement("station");
            XmlElement stationSsidElem = doc.CreateElement("ssid");
            XmlElement stationPasswdElem = doc.CreateElement("passwd");
            XmlElement stationMacElem = doc.CreateElement("mac");
            stationElem.AppendChild(stationSsidElem);
            stationElem.AppendChild(stationPasswdElem);
            stationElem.AppendChild(stationMacElem);

            stationSsidElem.SetAttribute("value", stationSSID);
            stationPasswdElem.SetAttribute("value", stationPassword);
            stationMacElem.SetAttribute("value", stationMAC);

            list.Add(stationElem);

            return list;
        }

        // 解析元素 
        public void SetFromXmlNode(XmlNode element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name == "wifi")
                {
                    hasWifi = Boolean.Parse(node.Attributes["valid"].Value);
                }
                else if (node.Name == "enable")
                {
                    enable = Boolean.Parse(node.Attributes["value"].Value);
                }
                else if (node.Name == "mode")
                {
                    workMode = node.Attributes["value"].Value == "ap" ? 0 : 1;
                }
                else if (node.Name == "ap")
                {
                    apInfo.ssid = node.SelectSingleNode("ssid").Attributes["value"].Value;
                    apInfo.password = node.SelectSingleNode("passwd").Attributes["value"].Value;
                    apInfo.channel = node.SelectSingleNode("channel").Attributes["value"].Value;
                    apInfo.encryption = node.SelectSingleNode("encryption").Attributes["value"].Value;
  
                }
                else if (node.Name == "station")
                {
                  
                }
            }
        }
    }

    // APN信息
    public class APNInfo
    {
        public string apn;
    }

    // 3G 4G信息
    public class PppoeInfo
    {
        public string apn;
    }

    // 亮度信息
    public class LuminanceInfo
    {
        public int mode;            // 0 default , 1 ploys,  2 sensor
        public int defualtValue;    // 1~100
        public List<customItemData> customItem;
        public int sensorMin;   // 1~100
        public int sensorMax;   // 1~100
        public int sensorTime;  //5 ～ 15

        public class customItemData
        {
            public bool enable;
            public string start;        // hh:mm:ss
            public int percent;         // 1~100

            public customItemData()
            {
                enable = true;
                start = "00:00:00";
                percent = 100;

            }
        }

        public LuminanceInfo()
        {
            mode = 0;
            customItem = new List<customItemData>();
            customItem.Add(new customItemData());
            defualtValue = 100;

            sensorMin = 1;
            sensorMax = 100;
            sensorTime = 10;
        }
        /// <summary>
        /// 获取亮度信息命令xml
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <returns></returns>
        public string GetXml_GetLuminanceInfo(string sdkGUID)
        {
            return DataProtocol.GetSdkCmdXml(sdkGUID, SdkMethod.GetLuminancePloy.ToString());
        }

        /// <summary>
        /// 获得设置亮度信息的xml数据
        /// </summary>
        /// <returns></returns>
        public string SetLuminanceInfoToXml(string sdkGUID)
        {
            var list = GetXmlElements(null);
            return DataProtocol.GetSdkCmdXml(sdkGUID, SdkMethod.SetLuminancePloy.ToString(), list);
        }

        // 获得对象元素集
        public List<XmlElement> GetXmlElements(XmlDocument doc)
        {
            customItem.Clear();

            if (doc == null)
            {
                doc = new XmlDocument();
            }
            List<XmlElement> list = new List<XmlElement>();

            XmlElement modeElem = doc.CreateElement("mode");
            XmlElement defaultElem = doc.CreateElement("default");
            XmlElement ploy = doc.CreateElement("ploy");
            XmlElement sensor = doc.CreateElement("sensor");
            string modeValue = null;
            if (mode == 0)
            {
                modeValue = "default";
            }
            else if (mode == 1)
            {
                modeValue = "ploys";

            }
            else if (mode == 2)
            {
                modeValue = "sensor";

            }
            modeElem.SetAttribute("value", modeValue);

            foreach (customItemData obj in customItem)
            {
                XmlElement item = doc.CreateElement("item");
                item.SetAttribute("enable", obj.enable.ToString().ToLower());
                item.SetAttribute("start", obj.start);
                item.SetAttribute("percent", obj.percent.ToString());
                ploy.AppendChild(item);
            }
            defaultElem.SetAttribute("value", defualtValue.ToString());

            sensor.SetAttribute("min", sensorMin.ToString());
            sensor.SetAttribute("max", sensorMax.ToString());
            sensor.SetAttribute("time", sensorTime.ToString());

            list.Add(modeElem);
            list.Add(defaultElem);
            list.Add(ploy);
            list.Add(sensor);

            return list;
        }

        // 解析元素 
        public void SetFromXmlNode(XmlNode element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name == "mode")
                {
                    string modeValue = node.Attributes["value"].Value;
                    if (modeValue == "default")
                    {
                        mode = 0;
                    }
                    else if (modeValue == "ploys")
                    {
                        mode = 1;
                    }
                    else if (modeValue == "sensor")
                    {
                        mode = 2;
                    }
                }
                else if (node.Name == "default")
                {
                    defualtValue = int.Parse(node.Attributes["value"].Value);
                }
                else if (node.Name == "ploy")
                {
                    foreach (XmlNode ployitem in node.ChildNodes)
                    {
                        LuminanceInfo.customItemData item = new LuminanceInfo.customItemData();
                        item.enable = bool.Parse(ployitem.Attributes["enable"].Value);
                        item.start = ployitem.Attributes["start"].Value;
                        item.percent = int.Parse(ployitem.Attributes["percent"].Value);
                        customItem.Add(item);
                    }
                }
                else if (node.Name == "sensor")
                {
                    sensorMin = int.Parse(node.Attributes["min"].Value);
                    sensorMax = int.Parse(node.Attributes["max"].Value);
                    sensorTime = int.Parse(node.Attributes["time"].Value);
                }
            }
        }
    }

    // 设备信息
    public class DeviceInfo
    {
        public string deviceCPU;        // 设备CPU {"Freescale.iMax6", "TI.335x", "ZTE.902c"}
        public string deviceModel;      // 卡型号
        public string deviceID;         // 设备ID
        public string deviceName;       // 设备名称
        public string versionFpga;      // fpga版本
        public string versionApp;       // 下位机固件版本
        public string versionKernel;    // 内核版本
        public int screenWidth;         // 屏高
        public int screenHeight;        // 屏宽
        public int screenRotation;      // 旋转标志, 取值范围{"0", "90", "180", "270"}
        public DateTime lastScanTime;   // 
        public PppoeInfo ppoeInfo;                 // 3G 4G信息
        public EthernetInfo ethernetInfo;          // 以太网信息
        public WifiInfo wifiInfo;                  // wifi信息
        public ReadbackFileListInfo fileListInfo;  // 文件列表信息

        public DeviceInfo()
        {
            deviceCPU = "";
            deviceModel = "";
            deviceID = "";
            deviceName = "unknow";
            versionFpga = "";
            versionApp = "";
            versionKernel = "";
            screenWidth = 32;
            screenHeight = 16;
            screenRotation = 0;
            lastScanTime = DateTime.UtcNow;

            ppoeInfo = new PppoeInfo();
            ethernetInfo = new EthernetInfo();
            wifiInfo = new WifiInfo();
            fileListInfo = new ReadbackFileListInfo();
        }

        // 获得对象元素集
        public List<XmlElement> GetXmlElements(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            List<XmlElement> list = new List<XmlElement>();
            XmlElement deviceElem = doc.CreateElement("device");
            deviceElem.SetAttribute("cpu", deviceCPU);
            deviceElem.SetAttribute("model", deviceModel);
            deviceElem.SetAttribute("id", deviceID);
            deviceElem.SetAttribute("name", deviceName);

            XmlElement versionElem = doc.CreateElement("version");
            versionElem.SetAttribute("pfga", versionFpga);
            versionElem.SetAttribute("app", versionApp);
            versionElem.SetAttribute("kernel", versionKernel);

            XmlElement screenElem = doc.CreateElement("screen");
            screenElem.SetAttribute("width", screenWidth.ToString());
            screenElem.SetAttribute("height", screenHeight.ToString());
            screenElem.SetAttribute("rotation", screenRotation.ToString());


            list.Add(deviceElem);
            list.Add(versionElem);
            list.Add(screenElem);
            return list;
        }

        // 解析元素 
        public void SetFromXmlNode(XmlNode element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name == "device")
                {
                    deviceCPU = node.Attributes["cpu"].Value;
                    deviceModel = node.Attributes["model"].Value;
                    deviceID = node.Attributes["id"].Value;
                    deviceName = node.Attributes["name"].Value;
                }
                else if (node.Name == "version")
                {
                    versionFpga = node.Attributes["fpga"].Value;
                    versionApp = node.Attributes["app"].Value;
                    versionKernel = node.Attributes["kernel"].Value;
                }
                else if (node.Name == "screen")
                {
                    screenWidth = int.Parse(node.Attributes["width"].Value);
                    screenHeight = int.Parse(node.Attributes["height"].Value);
                    screenRotation = int.Parse(node.Attributes["rotation"].Value);
                }
            }
        }
    }


    // 时间信息
    public class TimeInfo
    {
        public string timezone; // (UTC+hh:mm)****     (UTC+08:00)Beijing,Chongqing,HongKong,Urumchi
        public bool summer;     // 是否启用夏令时
        public string sync;     // 时间同步模式 none gps network auto 
        public string time;     // 将设置设备的时间, 该值在sync.value="none"时生效, 其他情况忽略 格式: "yyyy-mm-dd hh:MM:ss", 例如: "2017-01-01 00:00:00"

        public TimeInfo()
        {
            timezone = "(UTC+08:00)Beijing,Chongqing,HongKong,Urumchi";
            summer = false;
            sync = "none";
            time = "2017-01-01 00:00:00";
        }

        /// <summary>
        /// 获取回读时间信息xml
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <returns></returns>
        public string  GetXml_GetTimeInfo(string sdkGUID)
        {
            return DataProtocol.GetSdkCmdXml(sdkGUID, SdkMethod.GetTimeInfo.ToString());
        }

        /// <summary>
        /// 获取设置时间信息xml
        /// </summary>
        /// <param name="sdkGUID"></param>
        /// <returns></returns>
        public string SetTimeInfoToXml(string sdkGUID)
        {
            var list = GetXmlElements(null);
            return DataProtocol.GetSdkCmdXml(sdkGUID, SdkMethod.SetTimeInfo.ToString(), list);
        }

        // 获得对象元素集
        public List<XmlElement> GetXmlElements(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            List<XmlElement> list = new List<XmlElement>();

            XmlElement timezoneElem = doc.CreateElement("timezone");
            timezoneElem.SetAttribute("value", timezone);

            XmlElement summerElem = doc.CreateElement("summer");
            summerElem.SetAttribute("enable", summer.ToString().ToLower());

            XmlElement syncElem = doc.CreateElement("sync");
            syncElem.SetAttribute("value", sync);

            XmlElement timeElem = doc.CreateElement("time");
            timeElem.SetAttribute("value", time);


            list.Add(timezoneElem);
            list.Add(summerElem);
            list.Add(syncElem);
            list.Add(timeElem);
            return list;
        }

        // 解析元素 
        public void SetFromXmlNode(XmlNode element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name == "timezone")
                {
                    timezone = node.Attributes["value"].Value;
                }
                if (node.Name == "sync")
                {
                    sync = node.Attributes["value"].Value;
                }
                if (node.Name == "time")
                {
                    time = node.Attributes["value"].Value;
                }
                else if (node.Name == "summer")
                {
                    summer = bool.Parse(node.Attributes["enable"].Value);
                }
            }
        }
    }

    // 开关机信息
    public class SwitchTimeInfo
    {
        public List<PloyItem> items;
        public bool ploy_enable;
        public bool open_enable;
        public class PloyItem
        {
            public bool enable;  // true open screen , false close screen
            public string start; // hh:mm:ss
            public string end; // hh:mm:ss

            public PloyItem()
            {

                enable = true;
                start = "00:00:00";
                end = "23:59:59";
            }
        }
        public SwitchTimeInfo()
        {
            open_enable = true;
            ploy_enable = false;
            items = new List<PloyItem>();
        }

        /// <summary>
        /// 获取快关机信息xml
        /// </summary>
        /// <returns></returns>
        public string GetXml_GetSwitchTimeInfo(string SdkGuid)
        {
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.GetSwitchTime.ToString());
            return xml;
        }

        /// <summary>
        ///  获取设置开关机信息xml
        /// </summary>
        /// <param name="switchTimeInfo"></param>
        public string SetSwitchTimeInfoToXml(string SdkGuid)
        {
            var list = GetXmlElements(null);
            string xml = DataProtocol.GetSdkCmdXml(SdkGuid, SdkMethod.SetSwitchTime.ToString(), list);
            return xml;
        }

        // 获得对象元素集
        public List<XmlElement> GetXmlElements(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            List<XmlElement> list = new List<XmlElement>();

            XmlElement openElem = doc.CreateElement("open");
            openElem.SetAttribute("enable", open_enable.ToString().ToLower());


            XmlElement ploy = doc.CreateElement("ploy");
            ploy.SetAttribute("enable", ploy_enable.ToString().ToLower());


            foreach (PloyItem obj in items)
            {
                XmlElement item = doc.CreateElement("item");
                item.SetAttribute("enable", obj.enable.ToString().ToLower());
                item.SetAttribute("start", obj.start);
                item.SetAttribute("end", obj.end);
                ploy.AppendChild(item);
            }

            list.Add(openElem);
            list.Add(ploy);

            return list;
        }

        // 解析元素 
        public void SetFromXmlNode(XmlNode element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name == "open")
                {
                    open_enable = bool.Parse(node.Attributes["enable"].Value);
                }
                else if (node.Name == "ploy")
                {
                    ploy_enable = bool.Parse(node.Attributes["enable"].Value);
                    foreach (XmlNode ployitem in node.ChildNodes)
                    {
                        SwitchTimeInfo.PloyItem item = new SwitchTimeInfo.PloyItem();
                        item.enable = bool.Parse(ployitem.Attributes["enable"].Value);
                        item.start = ployitem.Attributes["start"].Value;
                        item.end = ployitem.Attributes["end"].Value;
                        items.Add(item);
                    }
                }
            }
        }
    }

    // 开机画面信息
    public class BootLogoInfo
    {
        public bool exist;     // 是否设置开机画面 
        public string name;   // 开机画面引用图片的名字 
        public string md5;    // 开机画面引用图片的md5值

        public BootLogoInfo()
        {
            exist = false;
            name = "";
            md5 = "";
        }

        // 获得对象元素集
        public List<XmlElement> GetXmlElements(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            List<XmlElement> list = new List<XmlElement>();

            XmlElement logoElem = doc.CreateElement("logo");
            logoElem.SetAttribute("exist", exist.ToString().ToLower());
            logoElem.SetAttribute("name", name);
            logoElem.SetAttribute("md5", md5);

            list.Add(logoElem);
            return list;
        }

        // 解析元素 
        public void SetFromXmlNode(XmlNode element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name == "logo")
                {
                    exist = bool.Parse(node.Attributes["exist"].Value);
                    name = node.Attributes["name"].Value;
                    md5 = node.Attributes["md5"].Value;
                }
            }
        }
    }

    // 文件类型
    public enum HFileType
    {
        kauto = -1,         // 自动判断
        kImageFile = 0,     // 图片
        kVideoFile = 1,     // 视频
        kFont = 2,          // 字体
        kFireware = 3,      // 固件
        kFPGAConfig = 4,    // 基本配置参数
        kSettingCofnig = 5, // FPG配置参数
        kProgramTemplate = 9, // 节目模板布局
        kTempImageFile = 128, //临时图片文件  (临时文件总大小 <= 10M)
        kTempVideoFile = 129, //临时视频文件  (临时文件总大小 <= 10M)
    }



    // 发送文件信息
    public class UploadFileInfo : IDisposable
    {
        public string path;
        public string md5;
        public string progress;
        public FileStream fs;
        public HFileType type;

        public bool isSending;
        public bool hasFinished;

        private bool disposed = false;
        public UploadFileInfo()
        {
            isSending = false;
            hasFinished = false;
            path = md5 = "";
            progress = "Not uploaded.";
            fs = null;
            type = HFileType.kauto;
        }

        ~UploadFileInfo()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = false;
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
    }


    // 回读文件信息
    public class ReadbackFileInfo
    {

    }

    public class FileSession
    {
        public string md5;
        public string name;
        public string type;
        public long size;
        public long existSize;
    }

    // 回读文件列表信息
    public class ReadbackFileListInfo
    {
        public List<FileSession> fileList;      // 文件列表

        // 解析元素 
        public void SetFromXmlNode(XmlNode element)
        {
            fileList = new List<FileSession>();

            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name == "files")
                {
                    foreach (XmlNode fileItem in node)
                    {
                        FileSession fs = new FileSession();
                        fs.name = fileItem.Attributes["name"].Value;
                        fs.size = long.Parse(fileItem.Attributes["size"].Value);
                        fs.existSize = long.Parse(fileItem.Attributes["existSize"].Value);
                        fs.md5 = fileItem.Attributes["md5"].Value;
                        fs.type = fileItem.Attributes["type"].Value;
                        fileList.Add(fs);
                    }
          
                }
            }
        }
    }

    // 字体信息
    public class FontInfo
    {
        public string fontName;
        public string fileName;
        public bool bold;
        public bool italic;
        public bool underline;
        public FontInfo()
        {
            fontName = fileName = "";
            bold = italic = underline = true;
        }
    }

    // 屏幕参数
    public class ScreenParam
    {
        public bool isNewScreen;
        //private int id;
        //private long timeStamps;
        //private string guid;
        public ScreenParam()
        {
            //id = 0;
            //timeStamps = 0;
            //guid = "";
            isNewScreen = false;
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            XmlElement screenElem = doc.CreateElement("screen");
            if (isNewScreen)
            {
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                string ret = string.Empty;
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();
                screenElem.SetAttribute("timeStamps", ret);
            }
            //screenElem.SetAttribute("id", id.ToString());
            //screenElem.SetAttribute("guid", guid);
            //screenElem.SetAttribute("timeStamps", timeStamps.ToString());
            return screenElem;
        }
    }

    // 边框参数
    public class BorderParam
    {
        public int imageIndex;  // 
        public int effect;      // 0 rotate 1 blink 2 null
        public int speed;       // 0 slow 1 middle 2 fast
        public bool useBorder;  // 是否使用边框
        public BorderParam()
        {
            useBorder = false;
            imageIndex = 0;
            effect = 0;
            speed = 1;
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }
            XmlElement borderElem = doc.CreateElement("border");

            if (useBorder)
            {
                borderElem.SetAttribute("index", imageIndex.ToString());

                string effectText = "rotate";
                if (effect == 0)
                {
                    effectText = "rotate";
                }
                else if (effect == 1)
                {
                    effectText = "blink";
                }
                else
                {
                    effectText = "null";
                }
                borderElem.SetAttribute("effect", effectText);

                string speedText = "middle";
                if (speed == 0)
                {
                    speedText = "slow";
                }
                else if (speed == 1)
                {
                    speedText = "middle";
                }
                else
                {
                    speedText = "fast";
                }
                borderElem.SetAttribute("speed", speedText);
            }


            return borderElem;
        }
    }



    public enum ProgramType
    {
        normal = 0,
        template = 1,
        html5 = 2,
        offline = 3,
    }

    // 节目参数
    public class ProgramParam
    {
        public ProgramType type;
        public int id;
        public string guid;
        public string name;
        public bool realProgramflag;

        // 可选参数 默认为null
        BorderParam borderParam;
        BackgroundMusicParam backgroundMusiccParam;
        PlayControlParam playControlParam;
        public ProgramParam()
        {
            type = ProgramType.normal;
            id = 0;
            guid = "";
            name = "";
            realProgramflag = false;
            borderParam = new BorderParam();
            backgroundMusiccParam = new BackgroundMusicParam();
            playControlParam = new PlayControlParam();
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }
            XmlElement programElem = doc.CreateElement("program");
            programElem.SetAttribute("type", type.ToString());
            programElem.SetAttribute("id", id.ToString());
            programElem.SetAttribute("guid", guid);
            programElem.SetAttribute("name", name);
            if (realProgramflag)
            {
                programElem.SetAttribute("flag", "realtime");
            }

            if (borderParam != null && borderParam.useBorder)
            {
                programElem.AppendChild(borderParam.GetXmlElement(doc));
            }
            if (backgroundMusiccParam != null)
            {
                programElem.AppendChild(backgroundMusiccParam.GetXmlElement(doc));
            }
            if (playControlParam != null)
            {
                programElem.AppendChild(playControlParam.GetXmlElement(doc));
            }
            return programElem;
        }
    }

    // 背景音乐
    public class BackgroundMusicParam
    {
        public List<string> files;  // 音乐文件


        public BackgroundMusicParam()
        {
            files = new List<string>();
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }
            XmlElement bgMusicElem = doc.CreateElement("backgroundMusic");
            foreach (var file in files)
            {
                XmlElement fileElem = doc.CreateElement("file");
                fileElem.SetAttribute("name", file);
                bgMusicElem.AppendChild(fileElem);
            }
            return bgMusicElem;
        }
    }

    // 播放控制参数
    public class PlayControlParam
    {
        public string duration;             // 节目播放时间 hh:mm:ss count = 0时有效
        public int count;                   // 0~999节目播放次数
        public bool disabled;               // 节目是否被禁用，如果节目被禁用，则该节目不播放
        List<Date> dates;                   // 播放日期控制
        List<Time> times;                   // 播放时间控制
        List<Week> weeks;                   // 播放星期控制 
        List<Location> locations;           // 播放地点控制


        public PlayControlParam()
        {
            duration = "00:05:00";
            count = 1;
            disabled = false;
            dates = new List<Date>();
            times = new List<Time>();
            weeks = new List<Week>();
            locations = new List<Location>();
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }
            XmlElement playControlElem = doc.CreateElement("playControl");

            if (count == 0)
            {
                playControlElem.SetAttribute("duration", duration);
            }
            else
            {
                playControlElem.SetAttribute("count", count.ToString());
            }
            
            playControlElem.SetAttribute("disabled", disabled.ToString().ToLower());

            foreach (var date in dates)
            {
                XmlElement dateElem = doc.CreateElement("date");
                dateElem.SetAttribute("start", date.dateStart);
                dateElem.SetAttribute("end", date.dateEnd);
                playControlElem.AppendChild(dateElem);
            }

            foreach (var time in times)
            {
                XmlElement timeElem = doc.CreateElement("date");
                timeElem.SetAttribute("start", time.timeStart);
                timeElem.SetAttribute("end", time.timeEnd);
                playControlElem.AppendChild(timeElem);
            }


            string[] weeksArrtext = new string[7] { "Mon", "Tue", "Wed", "Thur", "Fri", "Sat", "Sun" };
            foreach (var week in weeks)
            {
                string weekControl = "";
                XmlElement weekElem = doc.CreateElement("week");
                for (int i = 0; i < week.enables.Length; ++i)
                {
                    if (week.enables[i])
                    {
                        weekControl += weeksArrtext[i] + ",";
                    }
                }
                if (weekControl.Length > 1)
                {
                    weekControl = weekControl.Remove(weekControl.Length - 1, 1);
                }
                weekElem.SetAttribute("enable", weekControl);
                playControlElem.AppendChild(weekElem);
            }

            return playControlElem;
        }


        public class Date
        {
            public string dateStart;    // YYYY-MM-DD
            public string dateEnd;      // YYYY-MM-DD
            public Date()
            {
                dateStart = "2018:01:01";
                dateEnd = "2030:01:01";
            }
        }

        public class Week
        {
            public bool[] enables = new bool[7]; //  Mon, Tue, Wed, Thur, Fri, Sat, Sun
            public Week()
            {
                for (int i = 0; i < enables.Length; ++i)
                {
                    enables[i] = true;
                }
            }
        }
        public class Time
        {
            public string timeStart;    // hh:mm:ss
            public string timeEnd;      // hh:mm:ss
            public Time()
            {
                timeStart = "00:00:00";
                timeEnd = "23:59:59";
            }
        }
        public class Location
        {
            public double lng;          // 经度
            public double lat;          // 维度
            public double range;        // 范围，单位千米
            public bool preemptive;     // 抢占式播放，当进入指定区域时，如果当前播放节目不为定点播放或不在当前区域内，则马上将当前节目停止，切换到本节目
            public bool playOutside;    // 是否允许区域外播放

            public Location()
            {
                lng = lat = 0.0;
                range = 1.0f;
                preemptive = false;
                playOutside = true;
            }
        }

    }

    // 区域参数
    public class AreaParam
    {
        public string name;
        public string guid;
        public int alpha;     // 0～255
        public int x;
        public int y;
        public int width;
        public int height;

        BorderParam borderParam;
        public AreaParam()
        {
            name = "";
            guid = Guid.NewGuid().ToString();
            alpha = 255;
            x = 0;
            y = 0;
            width = 32;
            height = 16;
            borderParam = new BorderParam();
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            XmlElement areaElem = doc.CreateElement("area");
            areaElem.SetAttribute("guid", guid);
            areaElem.SetAttribute("name", name);
            areaElem.SetAttribute("alpha", alpha.ToString());


            XmlElement rectElem = doc.CreateElement("rectangle");
            rectElem.SetAttribute("x", x.ToString());
            rectElem.SetAttribute("y", y.ToString());
            rectElem.SetAttribute("width", width.ToString());
            rectElem.SetAttribute("height", height.ToString());
            areaElem.AppendChild(rectElem);

            if (borderParam != null && borderParam.useBorder)
            {
                areaElem.AppendChild(borderParam.GetXmlElement(doc));
            }
            return areaElem;
        }
    }

    public enum HorizontalAlignment
    {
        left,
        center,
        right,

    }

    public enum VerticalAlignment
    {
        top,
        middle,
        bottom,
    }

    // 特效
    public class AreaItemEffect
    {
        public EffectType inEffet;
        public EffectType outEffet;
        public int inSpeed;
        public int outSpeed;
        public int duration;
        public AreaItemEffect()
        {
            inEffet = EffectType.RANDOM;
            outEffet = EffectType.RANDOM;
            inSpeed = 4;
            outSpeed = 4;
            duration = 3;
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            XmlElement effectElem = doc.CreateElement("effect");
            effectElem.SetAttribute("in", ((int)inEffet).ToString());
            effectElem.SetAttribute("inSpeed", inSpeed.ToString());
            effectElem.SetAttribute("out", ((int)outEffet).ToString());
            effectElem.SetAttribute("outSpeed", outSpeed.ToString());
            effectElem.SetAttribute("duration", (duration * 10).ToString());

            return effectElem;
        }
    }


    // 文本区域参数
    public class TextAreaItemParam
    {
        public string name;
        public string guid;
        public bool isSingleLine;
        public Color backgroundColor;

        public HorizontalAlignment hAlignment;
        public VerticalAlignment vAlignment;

        public string text;

        public AreaItemEffect effect;

        public string fontName;
        public int fontSize;
        public Color color;
        public bool bold;
        public bool italic;
        public bool underline;
        public bool useBackgroundColor;
        public TextAreaItemParam()
        {
            name = "";
            guid = Guid.NewGuid().ToString();
            isSingleLine = false;
            backgroundColor = Color.Black;
            effect = new AreaItemEffect();
            hAlignment = HorizontalAlignment.center;
            vAlignment = VerticalAlignment.middle;

            text = "";

            fontName = "Arial";
            fontSize = 12;
            color = Color.Red;
            bold = false;
            italic = false;
            underline = false;
            useBackgroundColor = false;
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            if(effect.inEffet == EffectType.HT_LEFT_SERIES_MOVE || effect.inEffet == EffectType.LEFT_SERIES_MOVE ||
               effect.inEffet == EffectType.HT_RIGHT_SERIES_MOVE || effect.inEffet == EffectType.RIGHT_SERIES_MOVE)
            {
                isSingleLine = true;
            }
            if (effect.inEffet == EffectType.HT_UP_SERIES_MOVE || effect.inEffet == EffectType.UP_SERIES_MOVE ||
                effect.inEffet == EffectType.HT_DOWN_SERIES_MOVE || effect.inEffet == EffectType.DOWN_SERIES_MOVE)
            {
                isSingleLine = false;
            }

            XmlElement textElem = doc.CreateElement("text");
            textElem.SetAttribute("guid", guid);
            textElem.SetAttribute("name", name);
            textElem.SetAttribute("singleLine", isSingleLine.ToString().ToLower());

            if (useBackgroundColor)
            {
                textElem.SetAttribute("background", HdArea.ToHdColor(backgroundColor));
            }
            
            XmlElement styleElem = doc.CreateElement("style");
            styleElem.SetAttribute("align", hAlignment.ToString());
            styleElem.SetAttribute("valign", vAlignment.ToString());
            textElem.AppendChild(styleElem);

            XmlElement stringElem = doc.CreateElement("string");
            stringElem.InnerText = text;
            textElem.AppendChild(stringElem);

            XmlElement fontElem = doc.CreateElement("font");
            fontElem.SetAttribute("name", fontName);
            fontElem.SetAttribute("size", fontSize.ToString());
            fontElem.SetAttribute("color", HdArea.ToHdColor(color));
            fontElem.SetAttribute("bold", bold.ToString().ToLower());
            fontElem.SetAttribute("italic", italic.ToString().ToLower());
            fontElem.SetAttribute("underline", underline.ToString().ToLower());
            textElem.AppendChild(fontElem);

            var effectItems = effect.GetXmlElement(doc);
            textElem.AppendChild(effectItems);

            return textElem;
        }
    }

    public enum ImageFitMode
    {
        fill,
        center,
        stretch,
        tile,
    }

    // 图片区域参数
    public class ImageAreaItemParam
    {
        public string name;
        public string guid;
        public ImageFitMode fit;
        public AreaItemEffect effect;
        public string file;     // image file

        public ImageAreaItemParam()
        {
            name = "";
            guid = Guid.NewGuid().ToString();
            file = "";
            fit = ImageFitMode.stretch;
            effect = new AreaItemEffect();
        }


        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            XmlElement imageElem = doc.CreateElement("image");
            imageElem.SetAttribute("guid", guid);
            imageElem.SetAttribute("name", name);
            imageElem.SetAttribute("fit", fit.ToString());

            imageElem.AppendChild(effect.GetXmlElement(doc));

            XmlElement fileElem = doc.CreateElement("file");
            fileElem.SetAttribute("name", file);
            imageElem.AppendChild(fileElem);
            return imageElem;
        }

    }

    // 视频区域参数
    public class VideoAreaItemParam
    {
        public string name;
        public string guid;
        public bool aspectRatio;  // 是否保持宽高比
        public string file;

        public VideoAreaItemParam()
        {
            name = "";
            guid = Guid.NewGuid().ToString();
            file = "";
            aspectRatio = false;
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (doc == null)
            {
                doc = new XmlDocument();
            }

            XmlElement videoElem = doc.CreateElement("video");
            videoElem.SetAttribute("guid", guid);
            videoElem.SetAttribute("name", name);
            videoElem.SetAttribute("aspectRatio", aspectRatio.ToString().ToLower());

            XmlElement fileElem = doc.CreateElement("file");
            fileElem.SetAttribute("name", file);
            videoElem.AppendChild(fileElem);
            return videoElem;
        }
    }

    // 时钟类型
    public enum ClockType
    {
        digital,        // 数字时钟
        dial,           // 模拟时钟
    }

    // 时钟区域参数
    public class ClockAreaItemParam
    {
        public string name;
        public string guid;
        public ClockType clockType;
        public string timezone;     // 时区 to convert as like "+8:00" 
        public string adjust;       // 时间微调 to convert as like "+00:05:00" "-00:05:00"

        // 可选参数 
        public Title title;
        public Date date;
        public Week week;
        public Time time;
        public LunarCalendar lunarCalendar;

        public class Title
        {
            public string titleValue;
            public Color titleColor;
            public bool titleDisplay;
            public Title()
            {
                titleValue = "";
                titleColor = Color.Red;
                titleDisplay = false;
            }
        }

        public class Date
        {
            public int dateFormat;      // 1~7 1、YYYY/MM/DD 2、MM/DD/YYYY 3、DD/MM/YYYY 4、Jan DD, YYYY 5、DD Jan, YYYY 6、YYYY年MM月DD日 7、MM月DD日
            public Color dateColor;     // #RRGGBB
            public bool dateDisplay;
            public Date()
            {
                dateFormat = 1;
                dateColor = Color.Red;
                dateDisplay = true;
            }
        }

        public class Week
        {
            public int weekFormat;     // 1、星期一（翻译后的语言）2、Monday 3、Mon
            public Color weekColor;
            public bool weekDisplay;
            public Week()
            {
                weekFormat = 1;     // 1、
                weekColor = Color.Red;
                weekDisplay = false;
            }
        }

        public class Time
        {
            public int timeFormat;   // 1、hh:mm:ss2、hh:ss3、hh时mm分ss秒（翻译后的语言）4、hh时mm分（翻译后的语言）
            public Color timeColor;
            public bool timeDisplay;
            public Time()
            {
                timeFormat = 1;   // 1、hh
                timeColor = Color.Red;
                timeDisplay = true;
            }
        }


        public class LunarCalendar
        {
            public Color lunarCalendarColor;
            public bool lunarCalendarDisplay;   // 是否显示农历
            public LunarCalendar()
            {
                lunarCalendarColor = Color.Red;
                lunarCalendarDisplay = false;
            }
        }

        public ClockAreaItemParam()
        {
            name = "";
            guid = Guid.NewGuid().ToString();
            clockType = ClockType.dial;
            timezone = "";
            adjust = "00:00:00";
            title = new Title();
            date = new Date();
            week = new Week();
            time = new Time();
            lunarCalendar = new LunarCalendar();
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (doc == null )
            {
                doc = new XmlDocument();
            }

            XmlElement clockElem = doc.CreateElement("clock");
            clockElem.SetAttribute("guid", guid);
            clockElem.SetAttribute("name", name);
            clockElem.SetAttribute("type", clockType.ToString().ToLower());

            if (timezone.Length > 0)
            {
                clockElem.SetAttribute("timezone", timezone);
            }
            clockElem.SetAttribute("adjust", adjust);

            XmlElement titleElem = doc.CreateElement("title");
            titleElem.SetAttribute("value", title.titleValue);
            titleElem.SetAttribute("color", HdArea.ToHdColor(title.titleColor));
            titleElem.SetAttribute("display", title.titleDisplay.ToString().ToLower());
            clockElem.AppendChild(titleElem);

            XmlElement dateElem = doc.CreateElement("date");
            dateElem.SetAttribute("format", date.dateFormat.ToString());
            dateElem.SetAttribute("color", HdArea.ToHdColor(date.dateColor));
            dateElem.SetAttribute("display", date.dateDisplay.ToString().ToLower());
            clockElem.AppendChild(dateElem);

            XmlElement weekElem = doc.CreateElement("week");
            weekElem.SetAttribute("format", week.weekFormat.ToString());
            weekElem.SetAttribute("color", HdArea.ToHdColor(week.weekColor));
            weekElem.SetAttribute("display", week.weekDisplay.ToString().ToLower());
            clockElem.AppendChild(weekElem);

            XmlElement timeElem = doc.CreateElement("time");
            timeElem.SetAttribute("format", time.timeFormat.ToString());
            timeElem.SetAttribute("color", HdArea.ToHdColor(time.timeColor));
            timeElem.SetAttribute("display", time.timeDisplay.ToString().ToLower());
            clockElem.AppendChild(timeElem);

            XmlElement lunarCalendarElem = doc.CreateElement("lunarCalendar");
            lunarCalendarElem.SetAttribute("color", HdArea.ToHdColor(lunarCalendar.lunarCalendarColor));
            lunarCalendarElem.SetAttribute("display", lunarCalendar.lunarCalendarDisplay.ToString().ToLower());
            clockElem.AppendChild(lunarCalendarElem);

            return clockElem;
        }

    }

    public enum SdkMethod
    {
        GetIFVersion,           // 获得SDK协议版本
        AddProgram,             // 更新节目单
        UpdateProgram,          // 更新指定节目
        DeleteProgram,          // 删除指定节目
        GetAllFontInfo,         // 获得字体信息
        GetLuminancePloy,       // 获得亮度设置
        SetLuminancePloy,       // 设置亮度设置
        GetSwitchTime,          // 获取开关屏请求
        SetSwitchTime,          // 设置开关屏请求
        OpenScreen,             // 立即开屏
        CloseScreen,            // 立即关屏
        GetTimeInfo,            // 获取时间校正信息
        SetTimeInfo,            // 设置时间校准信息
        GetBootLogo,            // 获取开机画面
        SetBootLogoName,        // 设置开机画面
        ClearBootLogo,          // 清除开机画面
        GetSDKTcpServer,        // 获取TCP服务器
        SetSDKTcpServer,        // 设置TCP服务器
        GetDeviceInfo,          // 获取设备信息
        GetEth0Info,            // 获取以太网网络地址信息
        SetEth0Info,            // 设置以太网网络地址信息
        GetPppoeInfo,           // 获取3/4G信息
        SetApn,                 // 设置Apn
        GetWifiInfo,            // 获取wifi信息
        SetWifiInfo,            // 设置Wifi信息
        GetFiles,               // 获得文件列表
        DeleteFiles,            // 删除文件
        GetGpsRespondEnable,    // 获取GPS信息上报使能标志
        SetGpsRespondEnable,    // 设置GPS信息上报使能标志
        GetMulScreenSync,       // 获取多屏同步标志
        SetMulScreenSync,       // 设置多屏同步标志
        GetPlayProgramCountsEnable,     // 设置节目播放统计标志 
        SetPlayProgramCountsEnable,     // 设置节目播放统计标志
        GetPlayProgramCountsFileName,   // 获取节目播放统计文件名
        GetCurrentPlayProgramGUID,      // 获取当前播放节目GUID
        SetSocketTimeInfo,              // 设置socket超时时间
        SetPlayTypeToNormal,            // 恢复正常播放
        GetProgram,                     // 回读节目单
        GetEnableUDiskFunction,         // 读取U盘可配置功能
        SetEnableUDiskFunction,         // 设置U盘可配置功能
    }
}
