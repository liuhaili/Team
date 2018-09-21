using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.InvokeRoute
{
	/// <summary>
	/// 将一个方法标记为一个Action
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ActionAttribute : Attribute
	{
		/// <summary>
		/// 允许哪些访问动词，与web.config中的httpHanlder的配置意义一致。
		/// </summary>
		public string Verb { get; set; }


		internal bool AllowExecute(string httpMethod)
		{
            //if( string.IsNullOrEmpty(Verb) || Verb == "*" ) {
            //    return true;
            //}
            //else {
            //    string[] verbArray = Verb.SplitTrim(StringExtensions.CommaSeparatorArray);

            //    return verbArray.Contains(httpMethod, StringComparer.OrdinalIgnoreCase);
            //}
            return true;
		}
	}


	
}
