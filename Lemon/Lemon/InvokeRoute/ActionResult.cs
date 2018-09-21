using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Lemon.InvokeRoute
{
	/// <summary>
	/// 表示一个页面结果（页面将由框架执行）
	/// </summary>
	public sealed class ActionResult : IActionResult
	{
		public string VirtualPath { get; private set; }
		public object Model { get; private set; }

		public ActionResult(string virtualPath) : this(virtualPath, null)
		{
		}

		public ActionResult(string virtualPath, object model)
		{
			this.VirtualPath = virtualPath;
			this.Model = model;
		}

        void IActionResult.Ouput(TrafficContext context)
		{
			//TODO
            //context.Response
		}
	}


}
