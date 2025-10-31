using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SDKLibrary
{
    public class HdScreen
    {
        private ScreenParam _screenParam;

        public List<HdProgram> Programs { get; set; }

        public HdScreen(ScreenParam screenParam)
        {
            _screenParam = screenParam;
            Programs = new List<HdProgram>();
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            XmlElement screenElem = _screenParam.GetXmlElement(doc);
            foreach (HdProgram program in Programs)
            {
                screenElem.AppendChild(program.GetXmlElement(doc));
            }
            return screenElem;
        }
    }
}
