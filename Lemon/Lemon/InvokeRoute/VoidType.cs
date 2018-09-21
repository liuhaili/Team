using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lemon.InvokeRoute
{
	/// <summary>
	/// 表示在Action的参数列表中，不需要赋值值的类型，用于区分重载方法
	/// </summary>
	public sealed class VoidType
	{
		private VoidType() { }

		public static readonly VoidType Value = new VoidType();

		public override string ToString()
		{
			return string.Empty;
		}
	}
}
