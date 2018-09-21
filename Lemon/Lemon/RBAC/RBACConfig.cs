using System;
using System.Xml;
using System.Collections.Generic;
using Lemon.Multilanguage;
using Lemon.RBAC.RoleProviders;
using System.Reflection;
using System.IO;
using Lemon.Extensions;

namespace Lemon.RBAC
{
    public class RBACConfig
    {
        private static RBACContext RBAC = null;
        public static RBACContext Load()
        {            
            string xmlPath = (Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\rbac.cfg.xml").Replace("file:\\", "");
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            if (RBAC == null)
            {
                RBAC = new Lemon.RBAC.RBACContext();
                LoadRBAC(RBAC.Menu, doc.DocumentElement);
            }
            return RBAC;
        }

        private static void LoadRBAC(Menu menu, XmlNode parentNode)
        {
            if (parentNode==null || parentNode.ChildNodes == null || parentNode.ChildNodes.Count == 0)
                return;
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;
                string nodeName = node.Name.ToLower();
                switch (nodeName)
                {
                    case "menu":
                    case "action":
                        LoadMenu(node, menu);
                        break;
                    case "roles":
                        LoadXMLRole(node,RBAC);
                        break;
                    case "functions":
                        LoadFunctions(node, RBAC);
                        break;
                }
            }
        }

        private static void LoadMenu(XmlNode node, Menu pmenu)
        {
            if (node.Name.ToLower() == "menu")
            {
                Menu m = new Menu();
                m.Name = node.GetAttribute("name", "");
                m.DN = node.GetAttribute("dn", "");
                m.Icon = node.GetAttribute("icon", "");
                m.Index = node.GetAttribute("index", 0);
                m.Privilege = node.GetAttribute("privilege", "");
                m.Url = node.GetAttribute("url", "javascript:void(0)");
                pmenu.Children.Add(m);

                if (node.HasChildNodes)
                {
                    foreach (XmlNode n in node.ChildNodes)
                    {
                        LoadMenu(n, m);
                    }
                }                             
            }
            else if (node.Name.ToLower() == "action")
            {
                ActionEntity act = new ActionEntity();
                string url = node.GetAttribute("url", null).ToLower();
                act.RawUrl = url;
                if (url.IndexOf('?') != -1)
                    url = url.Substring(0, url.IndexOf('?'));
                string parameter = node.GetAttribute("constraints", null).ToLower();
                if (!String.IsNullOrEmpty(parameter))
                    parameter = "?" + parameter;
                //处理mvc默认
                //if (url.Split('/').Length>3&&url.IndexOf('.') == -1)
                //{
                //   string id= url.Substring(url.IndexOf('.') + 1);
                //   if (!String.IsNullOrEmpty(parameter))
                //       parameter += "$id=" + id;
                //   else
                //       parameter = "?" + parameter;
                //}
                //
                act.Url = url + parameter;
                act.Privilege = node.GetAttribute("privilege", null).ToLower();
                pmenu.ActionList.Add(act);
                RBAC.Menu.ActionList.Add(act);
            }
        }

        private static void LoadXMLRole(XmlNode node, RBACContext rbac)
        {
            rbac.RoleProvider = node.GetAttribute("provider", null);
            foreach (System.Xml.XmlNode nodeR in node.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;
                IRole role = new XMLRole()
                {
                    Id = nodeR.GetAttribute("ID", 0),
                    Name = nodeR.GetAttribute("name", null)
                };
                foreach (System.Xml.XmlNode nodeF in nodeR.ChildNodes)
                {
                    if (nodeF.NodeType != XmlNodeType.Element || nodeF.Name.ToLower() != "function")
                        continue;
                    role.FunctionIds.Add(nodeF.GetAttribute("ID", null));
                }
                rbac.Roles.Add(role);
            }
        }

        private static void LoadFunctions(XmlNode node, RBACContext rbac)
        {
            foreach (System.Xml.XmlNode groupNode in node.ChildNodes)
            {
                if (groupNode.NodeType != XmlNodeType.Element || groupNode.Name.ToLower() != "group")
                    continue;
                FunctionGroup group = new FunctionGroup()
                {
                    Name = groupNode.GetAttribute("name", null)
                };
                foreach (System.Xml.XmlNode funcNode in groupNode.ChildNodes)
                {
                    if (funcNode.NodeType != XmlNodeType.Element || funcNode.Name.ToLower() != "function")
                        continue;
                    Function func = new Function()
                    {
                        Id = funcNode.GetAttribute("ID", null),
                        Name = funcNode.GetAttribute("name", null)
                    };
                    group.Functions.Add(func);
                }
                rbac.Functions.Add(group);
            }
        }
    }
}
