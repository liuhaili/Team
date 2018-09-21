using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Lemon.InvokeRoute
{
	internal sealed class ActionDescription : BaseDescription
	{
		public ControllerDescription PageController; //为PageAction保留
		public MethodInfo MethodInfo { get; private set; }
		public ActionAttribute Attr { get; private set; }
		public ParameterInfo[] Parameters { get; private set; }
		public bool HasReturn { get; private set; }

		public ActionDescription(MethodInfo m, ActionAttribute atrr)
			: base(m)
		{
			this.MethodInfo = m;
			this.Attr = atrr;
			this.Parameters = m.GetParameters();
			this.HasReturn = m.ReturnType != ReflectionHelper.VoidType;
		}
	}

}
