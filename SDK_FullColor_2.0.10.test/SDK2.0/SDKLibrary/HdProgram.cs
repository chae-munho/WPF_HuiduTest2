using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SDKLibrary
{
    /// <summary>
    /// 节目类，配置节目参数，获取更新节目单、更新节目xml数据
    /// </summary>
    public class HdProgram
    {
        private ProgramParam programParam;

        public List<HdArea> Areas { get; set; }


        /// <summary>
        /// 创建节目
        /// </summary>
        /// <param name="programParam"></param>
        /// <returns></returns>
        public static HdProgram CreateProgram(ProgramParam programParam)
        {
            return new HdProgram(programParam);
        }

        public HdProgram(ProgramParam programParam)
        {
            this.programParam = programParam;
            Areas = new List<HdArea>();
        }

        // 添加区域
        public HdArea AddArea(AreaParam areaParam)
        {
            HdArea newArea = new HdArea(areaParam);
            Areas.Add(newArea);
            return newArea;
        }

        /// <summary>
        /// 获得节目xml数据
        /// </summary>
        /// <returns></returns>
        public XmlElement GetXmlElement(XmlDocument doc)
        {
            XmlElement programElem = programParam.GetXmlElement(doc);
            foreach(HdArea area in Areas)
            {
                programElem.AppendChild(area.GetXmlElement(doc));
            }
            return programElem;
        }
    }
}
