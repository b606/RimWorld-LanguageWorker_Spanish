// <code-header>
//   <author>b606</author>
//   <summary>
//		LoadedLanguagePatch: If the loaded language equals the target language,
//		prefix the getter of the property Worker to the target LanguageWorker class.
//	 </summary>
// </code-header>
//
using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace RimWorld_LanguageWorker_Spanish
{
	/// <summary>
	/// libHarmony prefix patch for LoadedLanguage.Worker ("get_Worker" method).
	/// </summary>
	[HarmonyPatch]
	class LoadedLanguagePatch
	{
		[HarmonyTargetMethod]
		static MethodBase TargetMethod()
		{
			return AccessTools.DeclaredPropertyGetter(typeof(LoadedLanguage), "Worker");
		}

		[HarmonyPrefix]
		static bool GetWorkerPrefix(LoadedLanguage __instance, ref LanguageWorker ___workerInt)
		{
			// if the current language is not the target, do nothing
			if (!__instance.FriendlyNameEnglish.Equals(LanguageWorkerPatcher.__targetLanguage))
				return true;

			Type myType = typeof(RimWorld_LanguageWorker_Spanish.LanguageWorker_Spanish);
			if (__instance.info.languageWorkerClass != myType)
			{
				// overwrite all target language worker class
				__instance.info.languageWorkerClass = myType;
				if (___workerInt != null && ___workerInt.GetType() != myType)
				{
					___workerInt = (LanguageWorker_Spanish)Activator.CreateInstance(myType);
				}
			}

			// continue to original method
			return true;
		}
	}
}
