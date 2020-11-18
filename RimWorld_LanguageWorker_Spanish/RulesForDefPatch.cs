// <code-header>
//   <author>b606</author>
//   <summary>
//		RulesForDefPatch: Prefix libHarmony patch for GrammarUtility.RulesForDef.
//		The patch replaces entirely RulesForDef for the target language.
//	</summary>
// </code-header>
//
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;
using Verse.Grammar;

namespace RimWorld_LanguageWorker_Spanish
{
	[HarmonyPatch]
	public static class RulesForDefPatch
	{
		[HarmonyTargetMethod]
		static MethodBase TargetMethod()
		{
			return AccessTools.Method(typeof(GrammarUtility), "RulesForDef");
		}

		[HarmonyPrefix]
		static bool RulesForDefPrefix(ref IEnumerable<Rule> __result, string prefix, Def def)
		{
			// if the current language is not the target, do nothing
			if (!LanguageDatabase.activeLanguage.FriendlyNameEnglish.Equals(LanguageWorkerPatcher.__targetLanguage))
				return true;

			// Rewrite the method entirely since it is short enough
			__result = LanguageWorker_Spanish.FixRulesForDef(prefix, def);

#if DEBUG
			LanguageWorker_Spanish.LogMessage("--RulesForDefPrefix called...");
			LanguageWorker_Spanish.LogMessage("result: " + __result);
			foreach (Rule r in __result)
			{
				LanguageWorker_Spanish.LogMessage(r.ToString());
			}
#endif
			// DO NOT CONTINUE to the original GrammarUtility.RulesforPawn
			return false;
		}
	}
}
