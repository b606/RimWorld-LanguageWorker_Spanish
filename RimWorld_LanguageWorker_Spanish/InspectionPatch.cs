// <code-header>
//   <author>b606</author>
//   <summary>
//		Postfix libHarmony patch for logging the targeted function result.
//	 </summary>
// </code-header>

using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;
using Verse.Grammar;

namespace RimWorld_LanguageWorker_Spanish
{
	[HarmonyPatch]
	public static class InspectionPatch
	{
		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> TargetMethods()
		{
			yield return AccessTools.Method(typeof(BattleLogEntry_AbilityUsed), "GenerateGrammarRequest");
			yield return AccessTools.Method(typeof(BattleLogEntry_DamageTaken), "GenerateGrammarRequest");
			yield return AccessTools.Method(typeof(BattleLogEntry_Event), "GenerateGrammarRequest");
			yield return AccessTools.Method(typeof(BattleLogEntry_ExplosionImpact), "GenerateGrammarRequest");
			yield return AccessTools.Method(typeof(BattleLogEntry_MeleeCombat), "GenerateGrammarRequest");
			yield return AccessTools.Method(typeof(BattleLogEntry_RangedFire), "GenerateGrammarRequest");
			yield return AccessTools.Method(typeof(BattleLogEntry_RangedImpact), "GenerateGrammarRequest");
			yield return AccessTools.Method(typeof(BattleLogEntry_StateTransition), "GenerateGrammarRequest");
			yield return AccessTools.Method(typeof(LogEntry_DamageResult), "GenerateGrammarRequest");
		}

		[HarmonyPostfix]
		static void Postfix(GrammarRequest __result)
		{
#if DEBUG
			LanguageWorkerPatcher.LogMessage("--InspectionPatch called...");
			LanguageWorkerPatcher.LogMessage("result: " + __result);
			foreach (Rule r in __result.Rules)
			{
				LanguageWorkerPatcher.LogMessage(r.ToString());
			}
			LanguageWorkerPatcher.LogMessage("constants: " + __result.Constants);
			if (__result.Constants != null)
			{
				foreach (var c in __result.Constants)
					LanguageWorkerPatcher.LogMessage(c.Key + "=" + c.Value);
			}
#endif
		}
	}
}
