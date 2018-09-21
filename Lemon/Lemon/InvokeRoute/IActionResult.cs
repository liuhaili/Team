using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Lemon.InvokeRoute
{
	public interface IActionResult
	{
		void Ouput(TrafficContext context);
	}
}
