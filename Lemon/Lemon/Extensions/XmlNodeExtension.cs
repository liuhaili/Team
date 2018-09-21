using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Lemon.Extensions
{
    public static class XmlNodeExtension
    {
        /// <summary>
        /// 获取元素指定属性
        /// </summary>
        /// <param name="element">元素引用</param>
        /// <param name="attrName">属性名称</param>
        /// <param name="defaultValue">没有该属性默认值</param>
        /// <returns></returns>
        public static string GetAttribute(this XmlNode element, string attrName, string defaultValue)
        {
            if (element.Attributes[attrName] != null)
                return element.Attributes[attrName].Value;
            return defaultValue;
        }

        public static int GetAttribute(this XmlNode element, string attrName, int defaultValue)
        {
            var attrVal = GetAttribute(element, attrName, defaultValue.ToString());
            int.TryParse(attrVal, out defaultValue);
            return defaultValue;
        }

        public static long GetAttribute(this XmlNode element, string attrName, long defaultValue)
        {
            var attrVal = GetAttribute(element, attrName, defaultValue.ToString());
            long.TryParse(attrVal, out defaultValue);
            return defaultValue;
        }

        public static bool GetAttribute(this XmlNode node, string attrName, bool defaultValue)
        {
            var attrVal = GetAttribute(node, attrName, defaultValue.ToString());
            bool.TryParse(attrVal, out defaultValue);
            return defaultValue;
        }

        public static XmlElement GetElementById(string xml, string elementId)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.GetElementById(elementId);
        }
    }
}
 