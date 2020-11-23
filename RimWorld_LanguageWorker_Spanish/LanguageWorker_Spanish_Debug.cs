// <code-header>
//   <summary>
//		LanguageWorker_Spanish_Debug.cs contains functions (debugging and diagnostics)
//		not for inclusion in the RimWorld LanguageWorker_Spanish class.
//	 </summary>
// </code-header>

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Verse;

namespace RimWorld_LanguageWorker_Spanish
{
	public partial class LanguageWorker_Spanish
	{
		// General purpose logger
		private static Logger LogLanguageWorker = new Logger("LanguageWorker_Spanish.log");

		// Light weight loggers, for diffs
		private static Logger logLanguageWorkerIn = new Logger("LanguageWorkerIn_Spanish.log");
		private static Logger logLanguageWorkerOut = new Logger("LanguageWorkerOut_Spanish.log");

		// Heavy logger, for stats and CPU usage
		private static StatsLogger logStats = new StatsLogger();
		private static StatsLogger LogStats { get => logStats; set => logStats = value; }

		// Save call stack frames information
		private static Dictionary<string, int> nameCategoryFrames = new Dictionary<string, int>();
		private const int __nameCategoryCount = 8;

		public static void LogMessage(string a_str)
		{
			LogLanguageWorker.Message(a_str);
		}

		[Conditional("DEBUG")]
		public static void RecordInString(string a_str)
		{
			logLanguageWorkerIn.Message(a_str);
		}

		[Conditional("DEBUG")]
		public static void RecordOutString(string a_str)
		{
			logLanguageWorkerOut.Message(a_str);
		}

		[Conditional("DEBUG")]
		public static void StartStatsLogging(StackTrace callStack, string acontext = null)
		{
			LogStats.StartLogging(callStack, acontext);
		}

		[Conditional("DEBUG")]
		public static void StopStatsLogging(string original, string processed_str)
		{
			LogStats.StopLogging(original, processed_str);
		}

		[Conditional("DEBUG")]
		public static void IntermediateLogging(string original, string processed_str)
		{
			LogStats.IntermediateLogging(original, processed_str);
		}

		/// <summary>
		/// Temporary hacks:
		/// Ensure that the correct frame indices are used to to detect the string categories.
		/// Scan the callStack of ToTitleCase once during Debug and assert that
		/// the released functions bool Is*Name() use these indices.
		/// If not, the game code has changed.
		/// </summary>
		/// <returns>List of frame indices of the detected methods</returns>
		/// <param name="callStack">Call stack.</param>
		[Conditional("DEBUG")]
		public void Debug_NameCategory_StackFrame(StackTrace callStack)
		{
			bool detectedPawnName = false;
			bool detectedGenerateName = false;

			for (int i = 0; i < callStack.FrameCount; i++)
			{
				StackFrame frame = callStack.GetFrame(i);
				MethodBase method = frame.GetMethod();

				// Detect if called from PawnBioAndNameGenerator.GeneratePawnName
				// bool IsPawnName() uses callStack.GetFrame(4) or callStack.GetFrame(5)
				if ((method.Name == "GeneratePawnName")
					&& method.DeclaringType.Equals(typeof(RimWorld.PawnBioAndNameGenerator)))
				{
					Debug.Assert((!detectedPawnName && i == 4)
							|| (detectedPawnName && i == 5));

					if (i == 4) { detectedPawnName = true; }

					if (!nameCategoryFrames.ContainsKey("IsPawnName" + i))
					{
						// Two possible keys: "IsPawnName4" and "IsPawnName5"
						nameCategoryFrames.Add("IsPawnName" + i, i);
						LogMessage("IsPawnName:Frame[" + i + "]: "
						 + method.DeclaringType.ToString() + method.ToString());
					}
				}

				// Detect if called from  RimWorld.Planet.SettlementNameGenerator GenerateSettlementName
				// bool IsSettlementName() uses callStack.GetFrame(5)
				if (method.Name == "GenerateSettlementName")
				{
					Debug.Assert(i == 5);
					if (!nameCategoryFrames.ContainsKey("IsSettlementName"))
					{
						nameCategoryFrames.Add("IsSettlementName", i);
						LogMessage("IsSettlementName:Frame[" + i + "]: "
					 + method.DeclaringType.ToString() + method.ToString());
					}
				}

				// Detect if called from RimWorld.FeatureWorker AddFeature (WorldFeature names)
				// bool IsWorldFeatureName() uses callStack.GetFrame(5)
				if (method.Name == "AddFeature")
				{
					Debug.Assert(i == 5);
					if (!nameCategoryFrames.ContainsKey("IsWorldFeatureName"))
					{
						nameCategoryFrames.Add("IsWorldFeatureName", i);
						LogMessage("IsWorldFeatureName:Frame[" + i + "]: "
					 + method.DeclaringType.ToString() + method.ToString());
					}
				}

				// Detect if called from RimWorld.FactionGenerator RimWorld.Faction NewGeneratedFaction
				// bool IsFactionName() uses callStack.GetFrame(5)
				if (method.Name == "NewGeneratedFaction")
				{
					Debug.Assert(i == 5);
					if (!nameCategoryFrames.ContainsKey("IsFactionName"))
					{
						nameCategoryFrames.Add("IsFactionName", i);
						LogMessage("IsFactionName:Frame[" + i + "]: "
					 + method.DeclaringType.ToString() + method.ToString());
					}
				}

				// Detect if called from RimWorld.QuestGen.QuestNode_ResolveQuestName GenerateName
				// Need to test the DeclaringType
				// bool IsQuestName() uses callStack.GetFrame(3)
				if ((method.Name == "GenerateName")
					&& method.DeclaringType.Equals(typeof(RimWorld.QuestGen.QuestNode_ResolveQuestName)))
				{
					Debug.Assert(i == 3);
					if (!nameCategoryFrames.ContainsKey("IsQuestName"))
					{
						nameCategoryFrames.Add("IsQuestName", i);
						LogMessage("IsQuestName:Frame[" + i + "]: "
					 + method.DeclaringType.ToString() + method.ToString());
					}
				}

				// Detect if called from RimWorld.CompArt GenerateTitle
				// bool IsArtName() uses callStack.GetFrame(2)
				if ((method.Name == "GenerateTitle")
					&& method.DeclaringType.Equals(typeof(RimWorld.CompArt)))
				{
					Debug.Assert(i == 2);
					if (!nameCategoryFrames.ContainsKey("IsArtName"))
					{
						nameCategoryFrames.Add("IsArtName", i);
						LogMessage("IsArtName:Frame[" + i + "]: "
					 + method.DeclaringType.ToString() + method.ToString());
					}
				}

				// Detect if called from RimWorld.NameGenerator GenerateName(Verse.Grammar.GrammarRequest,...)
				// Misleading: detect other types of names first.
				// Many other methods have the same name, and  RimWorld.NameGenerator can also be called elsewhere.
				// bool IsName() uses callStack.GetFrame(2)
				if (method.Name == "GenerateName")
				{
					Debug.Assert(!detectedGenerateName && i == 2);
					if (i == 2)
					{
						if (!nameCategoryFrames.ContainsKey("IsName"))
						{
							nameCategoryFrames.Add("IsName", i);
							detectedGenerateName = true;
							LogMessage("IsName:Frame[" + i + "]: "
							 + method.DeclaringType.ToString() + method.ToString());
						}
					}
				}
			}
		}

		public string Debug_ToTitleCase(string str, StackTrace callStack)
		{
			if (str.NullOrEmpty())
				return str;

			// Reduce log messages by memorizing the already debugged call stacks.
			if (nameCategoryFrames.Count < __nameCategoryCount)
			{
				Debug_NameCategory_StackFrame(callStack);
			}

			string processed_str;

			// Split name categories for debugging purpose
			// NOTE: Tests order matters
			if (IsQuestName(callStack))
			{
				// The fastest to detect: callStack.GetFrame(3)
				// Capitalize only first letter (+ '\'')
				processed_str = ToTitleCaseOther(str);
			}
			else
			if (IsPawnName(callStack))
			{
				// callStack.GetFrame(4) or callStack.GetFrame(5)
				processed_str = ToTitleCaseProperName(str);
			}
			else
			if (IsSettlementName(callStack))
			{
				// callStack.GetFrame(5)
				processed_str = ToTitleCaseProperName(str);
			}
			else
			if (IsWorldFeatureName(callStack))
			{
				// callStack.GetFrame(5)
				processed_str = ToTitleCaseProperName(str);
			}
			else
			if (IsFactionName(callStack))
			{
				// callStack.GetFrame(5)
				processed_str = ToTitleCaseOtherName(str);
			}
			else
			if (IsArtName(callStack))
			{
				// callStack.GetFrame(2)
				processed_str = ToTitleCaseOther(str);
			}
			else
			if (IsName(callStack))
			{
				// Any other names generated by RimWorld.NameGenerator: callStack.GetFrame(2)
				// RimWorld.TradeShip
				processed_str = ToTitleCaseOtherName(str);
			}
			else
			{
				// Normal title : capitalize first letter.
				processed_str = ToTitleCaseOther(str);
			}

			return processed_str;
		}
	}
}
