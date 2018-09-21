using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Lemon.InvokeRoute
{
	internal static class ReflectionExtensions2
	{
		internal static T GetMyAttribute<T>(this MemberInfo m, bool inherit) where T : Attribute
		{
			T[] array = m.GetCustomAttributes(typeof(T), inherit) as T[];

			if( array.Length == 1 )
				return array[0];

			if( array.Length > 1 )
				throw new InvalidProgramException(string.Format("方法 {0} 不能同时指定多次 [{1}]。", m.Name, typeof(T)));

			return default(T);
		}

		internal static T GetMyAttribute<T>(this MemberInfo m) where T : Attribute
		{
			return GetMyAttribute<T>(m, false);
		}


		internal static T[] GetMyAttributes<T>(this MemberInfo m, bool inherit) where T : Attribute
		{
			return m.GetCustomAttributes(typeof(T), inherit) as T[];
		}

		internal static T[] GetMyAttributes<T>(this MemberInfo m) where T : Attribute
		{
			return m.GetCustomAttributes(typeof(T), false) as T[];
		}


		/// <summary>
		/// 判断是否是一个可支持的参数类型。仅包括：基元类型，string ，decimal，DateTime，Guid, string[], 枚举
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsSupportableType(this Type type)
		{
			return type.IsPrimitive
				|| type == typeof(string)
				|| type == typeof(DateTime)
				|| type == typeof(decimal)
				|| type == typeof(Guid)
				|| type.IsEnum
				|| type == typeof(string[]);
		}

		/// <summary>
		/// 得到一个实际的类型（排除Nullable类型的影响）。比如：int? 最后将得到int
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Type GetRealType(this Type type)
		{
			if( type.IsGenericType )
				return Nullable.GetUnderlyingType(type) ?? type;
			else
				return type;
		}


		public static bool IsNullableType(this Type nullableType)
		{
			if( nullableType.IsGenericType 
				&& nullableType.IsGenericTypeDefinition == false 
				&& nullableType.GetGenericTypeDefinition() == typeof(Nullable<>) )
				return true;

			return false;
		}

	}
}
