using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Lemon.InvokeRoute
{
	internal class BaseDescription
	{
        //public OutputCacheAttribute OutputCache { get; protected set; }
        //public SessionModeAttribute SessionMode { get; protected set; }
        //public AuthorizeAttribute Authorize { get; protected set; }

		protected BaseDescription(MemberInfo m)
		{
            //this.OutputCache = m.GetMyAttribute<OutputCacheAttribute>();
            //this.SessionMode = m.GetMyAttribute<SessionModeAttribute>();
            //this.Authorize = m.GetMyAttribute<AuthorizeAttribute>(true /* inherit */);
		}
	}



}
