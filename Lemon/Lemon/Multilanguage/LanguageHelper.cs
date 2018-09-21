using Lemon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.Multilanguage
{
    public class LanguageHelper:SingletonBase<LanguageHelper>
    {
        public string GetError(string error,string path)
        {
            Language l = new Language(path);
            return l.GetItemLanguage(LanguageArea.Error, error);
        }
    }
}
