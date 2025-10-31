using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SDKLibrary
{



    /// <summary>
    /// Sdk xml 文档处理类
    /// </summary>
    class SdkXmlDocument : XmlDocument
    {
        /// <summary>
        /// 是否是Sdk xml 数据
        /// </summary>
        public bool IsSdkXmlData { get; set; }

        /// <summary>
        /// SDK guid
        /// </summary>

        public string SdkGuid { get; private set; }
        /// <summary>
        /// 加载xml
        /// </summary>
        /// <param name="xml"></param>
        public override void LoadXml(string xml)
        {
            base.LoadXml(xml);
            foreach (XmlNode node in ChildNodes)
            {
                if (node.Name == "sdk" && node.Attributes["guid"] != null)
                {
                    SdkGuid = node.Attributes["guid"].Value;
                    IsSdkXmlData = true;
                    break;
                }  
            }
        }


        /// <summary>
        /// 解析下位机返回数据
        /// </summary>
        /// <param name="method"></param>
        /// <param name="result"></param>
        /// <param name="obj"></param>
        public void ResolveXmlPacketData(out string method, out string result, out XmlNode outObj)
        {
            outObj = null;
            method = null;
            result = null;
            if (!IsSdkXmlData)
            {
                return;
            }

            foreach (XmlNode node in ChildNodes)
            {
                if (node.Name != "sdk")
                {
                    continue;
                }

                foreach (XmlNode node2 in node)
                {
                    if (node2.Name != "out")
                    {
                        continue;
                    }
                    outObj = node2;
                    foreach (XmlAttribute attr in node2.Attributes)
                    {
                        if (attr.Name == "method")
                        {
                            method = attr.Value;
                        }
                        else if (attr.Name == "result")
                        {
                            result = attr.Value;
                        }
                    }
                }


                
                if (method == null || result != "kSuccess")
                {
                    break;
                }
            }
        }


    }
}
