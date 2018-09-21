using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Lemon.RBAC
{
    /// <summary>
    /// 动作实体
    /// </summary>
    public class ActionEntity
    {
        public string Privilege
        {
            get;
            set;
        }
        public string RawUrl { get; set; }
        private string _Url;
        public string Url
        {
            get { return _Url; }
            set
            {
                _Url = value.Replace('&', '$');
                string urlPart = _Url;
                string parameterPart = "";
                int doubt = _Url.IndexOf('?');
                if (doubt > -1)
                {
                    urlPart = _Url.Substring(0, doubt);
                    parameterPart = _Url.Substring(doubt + 1);

                    string[] paramlist = parameterPart.Split('$');
                    for (int i = 0; i < paramlist.Length; i++)
                    {
                        int equal = paramlist[i].IndexOf('=');
                        if (equal == -1)
                            continue;
                        bool ignore = false;
                        string parameterName = paramlist[i].Substring(0, equal);
                        if (parameterName[0] == '|')
                        {
                            ignore = true;
                            parameterName = parameterName.Remove(0, 1);
                        }
                        string parameterValue = paramlist[i].Substring(equal + 1);
                        ActionParameter param = new ActionParameter()
                        {
                            Name = parameterName,
                            Value = parameterValue,
                            Ignore = ignore
                        };
                        Constraints.Add(param.Name, param);
                    }
                }
                UrlNotParameter = urlPart;
            }
        }
        public string UrlNotParameter
        {
            get;
            set;
        }
        public IDictionary<string, ActionParameter> Constraints
        {
            get;
            set;
        }
        public ActionEntity()
        {
            Constraints = new Dictionary<string, ActionParameter>();
        }
        public bool IsMatch(ActionEntity input)
        {
            if (input == null || !UrlNotParameter.Equals(input.UrlNotParameter))
                return false;
            int compare = 0;
            foreach (ActionParameter param in Constraints.Values)
            {
                if (param.Ignore)
                {
                    if (!input.Constraints.ContainsKey(param.Name))
                        continue;
                    if (!param.IsMatch(input.Constraints[param.Name]))
                        return false;
                    else
                    {
                        compare++;
                        continue;
                    }
                }

                if (!input.Constraints.ContainsKey(param.Name) || !param.IsMatch(input.Constraints[param.Name]))
                    return false;
                compare++;
            }
            return compare == input.Constraints.Count;
        }
    }
}
