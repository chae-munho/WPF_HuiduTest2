using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SDKLibrary
{

    /// <summary>
    /// 区域类，管理区域，一个区域可以拥有多个区域项
    /// </summary>
    public class HdArea
    {
        private AreaParam areaParam;

        public List<object> AreaItems { get; set; }

        public HdArea(AreaParam areaParam)
        {
            this.areaParam = areaParam;
            AreaItems = new List<object>();
        }

        /// <summary>
        /// 添加文本
        /// </summary>
        /// <param name="areaItemParam"></param>
        /// <returns></returns>
        public int AddText(TextAreaItemParam areaItemParam)
        {
            AreaItems.Add(areaItemParam);
            return 0;
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="imageAreaItemParam"></param>
        /// <returns></returns>
        public int AddImage(ImageAreaItemParam imageAreaItemParam)
        {
            AreaItems.Add(imageAreaItemParam);
            return 0;
        }

        /// <summary>
        /// 添加视频
        /// </summary>
        /// <param name="videoAreaItemParam"></param>
        /// <returns></returns>
        public int AddVedio(VideoAreaItemParam videoAreaItemParam)
        {
            AreaItems.Add(videoAreaItemParam);
            return 0;
        }


        /// <summary>
        /// 添加时钟
        /// </summary>
        /// <param name="clockAreaItemParam"></param>
        /// <returns></returns>
        public int AddClock(ClockAreaItemParam clockAreaItemParam)
        {
            AreaItems.Add(clockAreaItemParam);
            return 0;
        }

        /// <summary>
        /// 把Color转为#RRGGBB格式的字符串
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToHdColor(Color c)
        {
            string hdcolor = "#" + c.R.ToString("x2") + c.G.ToString("x2") + c.B.ToString("x2");
            return hdcolor;
        }

        /// <summary>
        /// 获得区域xml数据
        /// </summary>
        /// <returns></returns>
        public XmlElement GetXmlElement(XmlDocument doc)
        {
            XmlElement areaElem = areaParam.GetXmlElement(doc);
            XmlElement resourcesElem = doc.CreateElement("resources");
            areaElem.AppendChild(resourcesElem);

            foreach (object obj in AreaItems)
            {
                XmlElement item = null;

                Type type = obj.GetType();
                if (type == typeof(TextAreaItemParam))
                {
                   TextAreaItemParam itemText = (TextAreaItemParam)obj;
                    item = itemText.GetXmlElement(doc); 
                } 
                else if (type == typeof(ImageAreaItemParam))
                {
                    ImageAreaItemParam itemImage = (ImageAreaItemParam)obj;
                    item = itemImage.GetXmlElement(doc);
                }
                else if (type == typeof(VideoAreaItemParam))
                {
                    VideoAreaItemParam itemVideo = (VideoAreaItemParam)obj;
                    item = itemVideo.GetXmlElement(doc);
                }
                else if (type == typeof(ClockAreaItemParam))
                {
                    ClockAreaItemParam clockVideo = (ClockAreaItemParam)obj;
                    item = clockVideo.GetXmlElement(doc);
                }

               resourcesElem.AppendChild(item);             
            }

            return areaElem;
        }
    }
}
