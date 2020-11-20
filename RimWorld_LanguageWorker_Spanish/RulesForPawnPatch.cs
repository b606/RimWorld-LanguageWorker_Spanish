// <code-header>
//   <author>b606</author>
//   <summary>
//		RulesForPawnPatch: Prefix and and postfix libHarmony patch for GrammarUtility.RulesForPawn.
//		Fix the genders and names of pawns in the different texts where
//		the kind.labels are incorrect.
//	 </summary>
// </code-header>

using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.Grammar;

namespace RimWorld_LanguageWorker_Spanish
{
	/// <summary>
	/// Prefix and and postfix libHarmony patch for GrammarUtility.RulesForPawn
	/// </summary>
	[HarmonyPatch]
	class RulesForPawnPatch
	{
		/// <summary>
		/// A private class to save the physical character of the pawn
		/// at the patch prefix and restore it at patch postfix.
		/// Why: the language may differentiate the physical and grammatical gender. 
		/// </summary>
		private class PhysicalCharacter
		{
			// References to the original for later use.
			private PawnKindDef kind;
			private Gender gender;

			// Save old values.
			private string oldlabel;
			private string oldlabelMale;
			private string oldlabelFemale;
			private Gender oldgender;

			public PhysicalCharacter(ref PawnKindDef kind, ref Gender gender)
			{
				this.kind = kind;
				this.gender = gender;
				this.oldlabel = kind.label;
				this.oldlabelMale = kind.labelMale;
				this.oldlabelFemale = kind.labelFemale;
				this.oldgender = gender;
			}


			public void RestoreCharacter()
			{
				this.kind.label = this.oldlabel;
				this.kind.labelMale = this.oldlabelMale;
				this.kind.labelFemale = this.oldlabelFemale;
				this.gender = this.oldgender;
			}

			public string KindLabel { get => kind.label; set => kind.label = value; }
			public string KindLabelMale { get => kind.labelMale; set => kind.labelMale = value; }
			public string KindLabelFemale { get => kind.labelFemale; set => kind.labelFemale = value; }
			public Gender Gender { get => gender; set => gender = value; }
			public PawnKindDef Kind { get => kind; set => kind = value; }
			public Gender OldGender { get => oldgender; set => oldgender = value; }
			public string OldLabel { get => oldlabel; set => oldlabel = value; }
			public string OldLabelMale { get => oldlabelMale; set => oldlabelMale = value; }
			public string OldLabelFemale { get => oldlabelFemale; set => oldlabelFemale = value; }
		}

		[HarmonyTargetMethod]
		static MethodBase TargetMethod()
		{
			MethodInfo original = typeof(GrammarUtility).GetMethod("RulesForPawn",
				new Type[] {
						typeof(string), typeof(Name),
						typeof(string), typeof(PawnKindDef), typeof(Gender), typeof(Faction),
						typeof(int), typeof(int), typeof(string),
						typeof(bool), typeof(bool),
						typeof(bool), typeof(List<RoyalTitle>),
						typeof(Dictionary<string, string>), typeof(bool)
				});
			return original;
		}

		[HarmonyPrefix]
		static bool RulesforPawnPrefix(string pawnSymbol, Name name,
			string title, ref PawnKindDef kind, ref Gender gender, Faction faction,
			int age, int chronologicalAge, string relationInfo,
			bool everBeenColonistOrTameAnimal, bool everBeenQuestLodger,
			bool isFactionLeader, List<RoyalTitle> royalTitles,
			Dictionary<string, string> constants, bool addTags,
			out PhysicalCharacter __state)
		{
			// Save the PhysicalCharacter because it might be overwritten in the patch
			__state = new PhysicalCharacter(ref kind, ref gender);

			// if the current language is not the target, do nothing
			if (! LanguageWorkerPatcher.IsTargetLanguage(LanguageDatabase.activeLanguage.FriendlyNameEnglish))
				return true;

			// change kind.label and gender according to the language grammar
			LanguageWorker_Spanish.FixPawnGender(ref kind, ref gender, relationInfo);

			// continue to the original GrammarUtility.RulesforPawn
			return true;
		}

		[HarmonyPostfix]
		static IEnumerable<Rule> RulesforPawnPostfix(IEnumerable<Rule> __result, PhysicalCharacter __state)
		{
			// return the rules list
			foreach (Rule r in __result)
			{
				yield return r;
			}

			// restore the physical character
			__state.RestoreCharacter();
		}
	}
}
