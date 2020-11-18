// <code-header>
//   <author>b606</author>
//   <summary>
//		Strings utility functions.
//	 </summary>
// </code-header>

using System;
using Verse;

namespace RimWorld_LanguageWorker_Spanish
{
	public static class StringExtension
	{
		public static string CapitalizeHyphenated(this string str)
		{
			if (str.NullOrEmpty())
				return str;
			if (str.IndexOf('-') < 0)
				return str.CapitalizeFirst();

			string[] array = str.Split('-');
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].CapitalizeFirst();
			}
			return string.Join("-", array);
		}
	}
}
