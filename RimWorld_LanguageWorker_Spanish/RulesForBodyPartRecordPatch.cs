// <code-header>
//   <author>b606</author>
//   <summary>
//		RulesForBodyPartRecordPatch: Prefix libHarmony patch for GrammarUtility.RulesForBodyPartRecord.
//		The patch replaces entirely RulesForBodyPartRecord for the target language.
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
	public static class RulesForBodyPartRecordPatch
	{
		[HarmonyTargetMethod]
		static MethodBase TargetMethod()
		{
			return AccessTools.Method(typeof(GrammarUtility), "RulesForBodyPartRecord");
		}

		[HarmonyPrefix]
		static bool RulesForBodyPartRecordPrefix(ref IEnumerable<Rule> __result, string prefix, BodyPartRecord part)
		{
			// if the current language is not the target, do nothing
			if (! LanguageWorkerPatcher.IsTargetLanguage(LanguageDatabase.activeLanguage.FriendlyNameEnglish))
				return true;

			// Rewrite the method entirely since it is short enough
			__result = LanguageWorker_Spanish.FixRulesForBodyPartRecord(prefix, part);

#if DEBUG
			LanguageWorker_Spanish.LogMessage("--RulesForBodyPartRecordPrefix called...");
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
