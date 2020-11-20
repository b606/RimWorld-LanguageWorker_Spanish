// <code-header>
//   <author>b606</author>
//   <summary>
//		libHarmony patches manager.
//	 </summary>
// </code-header>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.Grammar;

namespace RimWorld_LanguageWorker_Spanish
{
	public static class LanguageWorkerPatcher
	{
		private static readonly HashSet<string> targetLanguageLSet = new HashSet<string>
		{
				"Spanish",
				"Latin American Spanish"
		};

		public static bool IsTargetLanguage(string aLang)
		{
			return targetLanguageLSet.Contains(aLang);
		}

		public static void DoPatching()
		{
#if DEBUG
			Harmony.DEBUG = true;
#endif
			try
			{
				Harmony harmony = new Harmony(id: "com.b606.mods.languageworkerSpanish");
				Assembly assembly = Assembly.GetExecutingAssembly();

				LanguageWorker_Spanish.LogMessage("Installing com.b606.mods.languageworkerSpanish...");
				LanguageWorker_Spanish.LogMessage(string.Format("Active language: {0}",
					LanguageDatabase.activeLanguage.FriendlyNameEnglish));

				harmony.PatchAll(assembly);
				InspectPatches(harmony);

				LanguageWorker_Spanish.LogMessage("Done.");
			}
			catch (Exception e)
			{
				LanguageWorker_Spanish.LogMessage("Mod installation failed.");
				LanguageWorker_Spanish.LogMessage(string.Format("Exception: {0}", e));
			}
		}

		// Retrieve all patches
		[Conditional("DEBUG")]
		public static void InspectPatches(Harmony harmony)
		{
			try
			{
				LanguageWorker_Spanish.LogMessage("Existing patches:");

				IEnumerable<MethodBase> myOriginalMethods = harmony.GetPatchedMethods();
				foreach (MethodBase method in myOriginalMethods)
				{
					Patches patches = Harmony.GetPatchInfo(method);
					if (patches != null)
					{
						foreach (var patch in patches.Prefixes)
						{
							// already patched
							LanguageWorker_Spanish.LogMessage("index: " + patch.index);
							LanguageWorker_Spanish.LogMessage("owner: " + patch.owner);
							LanguageWorker_Spanish.LogMessage("patch method: " + patch.PatchMethod);
							LanguageWorker_Spanish.LogMessage("priority: " + patch.priority);
							LanguageWorker_Spanish.LogMessage("before: " + patch.before.Join());
							LanguageWorker_Spanish.LogMessage("after: " + patch.after.Join());
						}
					}
				}
			}
			catch (Exception e)
			{
				LanguageWorker_Spanish.LogMessage("Patches inspection failed.");
				LanguageWorker_Spanish.LogMessage(string.Format("Exception: {0}", e));
			}
		}
	}
}
