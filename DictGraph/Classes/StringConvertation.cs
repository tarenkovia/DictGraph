using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGraph
{
	public static class StringConvertation
	{
		public static string SetFirstLetterToUpper(this string s)
		{
			if (String.IsNullOrEmpty(s))
			{
				throw new ArgumentException("String is mull or empty");
			}

			return s[0].ToString().ToUpper() + s.Substring(1);
		}
	}
}
