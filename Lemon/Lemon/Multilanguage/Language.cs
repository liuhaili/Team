using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Lemon.Multilanguage
{
    //语言包格式，节点名字对应标签的名字
    //<?xml version="1.0" encoding="utf-8"?>
    //<language>
    //  <index></index>
    //  <!--样式-->
    //  <style>    
    //  </style>
    //  <site>    
    //  </site>
    //  <menu>
    //  </menu>
    //  <images>
    //  </images>
    //  <error></error>
    //</language>
    /// <summary>
    /// 语言
    /// </summary>
    public class Language
    {
        /// <summary>
        /// 语言名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 语言包源xml
        /// </summary>
        private XmlDocument _PackSource = new XmlDocument();
        /// <summary>
        /// 语言包根据名称加载，从语言包当前运行dll的目录里查找language文件夹下的xml文件
        /// </summary>
        /// <param name="name"></param>
        public Language(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            LoadLanguage(name, path);
        }

        private void LoadLanguage(string name,string path)
        {
            //首先从缓存中获取语言包
            var pack = LanguageCache.Get(name);
            if (pack == null)
            {
                var file = path;
                //判断语言包是否存在，不存在加载默认语言包
                if (!File.Exists(file))
                {
                    throw new Exception("语言包不存在!");
                }
                else
                {
                    this.Name = name;
                    this._PackSource.Load(file);
                    LanguageCache.Add(this);
                }
            }
            else
            {
                this.Name = pack.Name;
                this._PackSource = pack._PackSource;
            }
        }
        /// <summary>
        /// 获取单个项的语言信息
        /// </summary>
        /// <param name="area">区域路径</param>
        /// <param name="tagName">标签名称</param>
        /// <returns></returns>
        public string GetItemLanguage(string area, string tagName)
        {
            area = area.ToLower();
            tagName = tagName.ToLower().TrimStart('[').TrimStart('#').TrimEnd(']');
            try
            {
                var node = _PackSource.DocumentElement.SelectSingleNode(area + "/" + tagName);
                if (node.FirstChild is XmlCDataSection)
                    return node.FirstChild.InnerXml;
                return node.InnerXml;
            }
            catch
            {
                return tagName;
            }
        }
        /// <summary>
        /// 替换制定区域的语言信息
        /// </summary>
        /// <param name="area">区域路径</param>
        /// <param name="source">区域的源html</param>
        /// <returns></returns>
        public string GetAreaLanguage(string area, string source)
        {
            area = area.ToLower();
            try
            {
                var areanode = _PackSource.DocumentElement.SelectSingleNode(area);
                if (areanode == null || !areanode.HasChildNodes)
                    return source;
                foreach (XmlNode n in areanode.ChildNodes)
                {
                    if (n is XmlComment)
                    {
                        continue;
                    }
                    else if (n.FirstChild is XmlText)
                    {
                        source = source.Replace("[#" + n.Name + "]", n.InnerXml);
                    }
                    else if (n.FirstChild is XmlCDataSection)
                    {
                        source = source.Replace("[#" + n.Name + "]", n.FirstChild.InnerText);
                    }
                    else
                    {
                        source = GetAreaLanguage(area + "/" + n.Name, source);
                    }
                }
            }
            catch
            { }
            return source;
        }
    }
}
