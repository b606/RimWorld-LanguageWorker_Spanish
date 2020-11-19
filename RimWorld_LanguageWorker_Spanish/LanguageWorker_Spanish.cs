// <code-header>
//   <summary>
//		LanguageWorker_Spanish.cs contains the main functions destined
//		to inclusion in the RimWorld LanguageWorker_Spanish class
//		(after hacky code cleaned out).
//	 </summary>
//   <revisions>
//     <revision>2020-11-18: b606 adapted from LanguageWorkerFrench_Mod.</revision>
//   </revisions>
// </code-header>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Verse;
using Verse.Grammar;

namespace RimWorld_LanguageWorker_Spanish
{
	public partial class LanguageWorker_Spanish : LanguageWorker
	{
		public LanguageWorker_Spanish()
		{
			//StartStatsLogging(new StackTrace());
			LogMessage("--LanguageWorker_Spanish ctor called.");
			//StopStatsLogging("LanguageWorker_Spanish", "LanguageWorker_Spanish");
		}

		// TODO: in plural, these words get -es
		private static readonly HashSet<string> Exceptions_Plural_es = new HashSet<string> {
			"sí",
			"no"
		};

		// TODO: in plural, these words get -s
		private static readonly HashSet<string> Exceptions_Plural_s = new HashSet<string>
		{
			//	"menú",
			//	"popurrí"
		};

		// TODO: in plural, these words are invariant
		private static readonly HashSet<string> Exceptions_Plural_invariant = new HashSet<string>
		{
				"yorkshire",
				"molotov",
				"pemmican",
				"pekoe",
				"tórax",
				"protórax",
				"bum",			// for ratas-bum
				"ciempiés",
				"análisis", // not currently in the game
				"jueves"		// and others (not in the game) 
		};

		// TODO: Placeholder for words that do not get elision
		private static readonly HashSet<string> Exceptions_No_Elision = new HashSet<string> {
			//"hack",
			//"holding",
			//"hunter",
      //"husky"
    };

		// List of female epicen animals.
		// Commented items have xxx_labelMale
		private static readonly HashSet<string> PawnKind_FemaleOnly = new HashSet<string> {
			"Alpaca",
			"Boomrat",
			"Chinchilla",
			"Cobra",
			//"Cow",
			"Gazelle",
			"Goat",
			"Hare",
			"Iguana",
			"Megaspider",
			"Panther",
			"Rat",
			//"Sheep",
			"Snowhare",
			"Squirrel",
			"Tortoise" //
		};

		// List of male epicen animals.
		// Commented items have xxx_labelFemale
		private static readonly HashSet<string> PawnKind_MaleOnly = new HashSet<string>{
			"Alphabeaver",
			//"Bear_Grizzly",
			//"Bear_Polar",
			"Bison",
			"Boomalope",
			"Cassowary",
			//"Cat",
			"Capybara",
			"Caribou",
			//"Chicken",
			"Cougar",
			//"Deer",
			"Donkey",
			"Dromedary",
			"Duck",
			"Elephant",
			"Elk",
			"Emu",
			//"Fox_Arctic",
			//"Fox_Fennec",
			"Fox_Red",
			"Goose",
			"GuineaPig",
			//"Horse",
			"Husky",
			"Ibex",
			"LabradorRetriever",
			"Lynx",
			"Megascarab",
			"Megasloth",
			//"Monkey",
			"Muffalo",
			"Ostrich",
			//"Pig",
			"Raccoon",
			"Rhinoceros",
			"Spelopede",
			"Thrumbo",
			//"Turkey",
			"Warg",
			"WildBoar",
			"Warg",
			//"Wolf_Arctic",
			//"Wolf_Timber",
			"Yak",
			"YorkshireTerrier"
		};

		// For ToTitleCase: No uppercase if in the middle of the string
		// For Pluralize: Contains most of the prepositions (these stop the pluralization for compound words)
		// TODO: check if spanish typography is similar to french one.
		private static HashSet<string> NonUppercaseWords = new HashSet<string>
		{
			"a",		// "a la",
			"de",		// "de la",
			"del",
			"la",
			"el",
			"por",
			"post",
			"van",	// some foreign words
			"von"
		};

		// Some labels are usually in plural because of contexts,
		// however the in-game grammar engine treats them as singular (since it is an XXX_label not an XXX_labelPlural).
		// The list below is used to patch RulesForDef (in FixRulesForDef) and FixRulesForBodyPartRecord (in FixRulesForBodyPartRecord)
		private static readonly HashSet<string> DefLabel_InPlural = new HashSet<string> {
		// ThingDef/Races_* tools..label
			"griffes",
			"dents",
			"défenses",
			"mandibules",
			"mignonnes petites dents",
			"crocs acérés",
		// BodyPart group
			"yeux",
		// HediffDef
			"engelures",
			"taillades",
			"artères bouchées",
			"mécanites fibreuses",
			"parasites musculaires",
			"mécanites sensorielles",
			"vers intestinaux",
		//ThingDef
			"rideaux",
			"décombres",
			"sacs de sable éventrés",
			"débris",
			"croquettes",
			"hautes herbes"
		};

		public override string WithIndefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			//Names don't get articles
			if (name)
				return str;

			if (plural)
				return (gender == Gender.Female ? "unas " : "unos ") + str;

			return (gender == Gender.Female ? "una " : "un ") + str;
		}

		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
				return str;

			//Names don't get articles
			if (name)
				return str;

			if (plural)
				return (gender == Gender.Female ? "las " : "los ") + str;

			return (gender == Gender.Female ? "la " : "el ") + str;
		}

		public override string OrdinalNumber(int number, Gender gender = Gender.None)
		{
			return number + ".º";
		}

		#region StackTrace helpers
		// Detect if called from PawnBioAndNameGenerator.GeneratePawnName
		bool IsPawnName(StackTrace callStack)
		{
			MethodBase method = callStack.GetFrame(4).GetMethod();
			if ((method.Name == "GeneratePawnName")
					&& method.DeclaringType.Equals(typeof(RimWorld.PawnBioAndNameGenerator)))
				return true;
			method = callStack.GetFrame(5).GetMethod();
			if ((method.Name == "GeneratePawnName")
					&& method.DeclaringType.Equals(typeof(RimWorld.PawnBioAndNameGenerator)))
				return true;

			return false;
		}

		// Detect if called from  RimWorld.Planet.SettlementNameGenerator GenerateSettlementName
		bool IsSettlementName(StackTrace callStack)
		{
			MethodBase method = callStack.GetFrame(5).GetMethod();
			if (method.Name == "GenerateSettlementName")
				return true;

			return false;
		}

		// Detect if called from RimWorld.FeatureWorker AddFeature
		bool IsWorldFeatureName(StackTrace callStack)
		{
			MethodBase method = callStack.GetFrame(5).GetMethod();
			if (method.Name == "AddFeature")
				return true;

			return false;
		}

		// Detect if called from RimWorld.FactionGenerator RimWorld.Faction NewGeneratedFaction
		bool IsFactionName(StackTrace callStack)
		{
			MethodBase method = callStack.GetFrame(5).GetMethod();
			if (method.Name == "NewGeneratedFaction")
				return true;

			return false;
		}

		// Detect if called from RimWorld.QuestGen.QuestNode_ResolveQuestName GenerateName
		bool IsQuestName(StackTrace callStack)
		{
			MethodBase method = callStack.GetFrame(3).GetMethod();
			if ((method.Name == "GenerateName")
				&& method.DeclaringType.Equals(typeof(RimWorld.QuestGen.QuestNode_ResolveQuestName)))
				return true;

			return false;
		}

		// Detect if called from RimWorld.CompArt GenerateTitle
		bool IsArtName(StackTrace callStack)
		{
			MethodBase method = callStack.GetFrame(2).GetMethod();
			if ((method.Name == "GenerateTitle")
				&& method.DeclaringType.Equals(typeof(RimWorld.CompArt)))
				return true;

			return false;
		}

		// Detect if called from RimWorld.NameGenerator GenerateName
		bool IsName(StackTrace callStack)
		{
			MethodBase method = callStack.GetFrame(2).GetMethod();
			if (method.Name == "GenerateName")
				return true;

			return false;
		}
		#endregion // StackTrace helpers

		/// <summary>
		/// ToTitleCase for pawns, world features and settlements names.
		/// Pawn names may contain 'Nickname'.
		/// The first word is always capitalized and all words are capitalized
		/// except those listed in NonUppercaseWords and "d'".
		/// N0TE: last name in NameTriple is not capitalized by the default
		/// LanguageWorker if generated from tribal words.
		/// </summary>
		/// <returns>The title case string for a proper name.</returns>
		/// <param name="str">String.</param>
		public string ToTitleCaseProperName(string str)
		{
			if (str.NullOrEmpty())
				return str;

			string[] array = str.Split(' ');
			for (int i = 0; i < array.Length; i++)
			{
				string str2 = array[i];
				// Check if uppercase is needed, the first word is always capitalized.
				if ((i > 0) && NonUppercaseWords.Contains(str2.ToLower()))
				{
					array[i] = str2.ToLower();
					continue;
				}

				// Capitalize word: skip "'", "d'" and "l'"
				char firstChar = str2[0];
				switch (firstChar)
				{
					case '\'':
						array[i] = "'" + str2.Substring(1).CapitalizeHyphenated();
						break;
					default:
						if (str2.Length == 2)
						{
							array[i] = str2.CapitalizeHyphenated();
							break;
						}
						if (str2.StartsWith("d'", StringComparison.CurrentCulture)
							|| str2.StartsWith("D'", StringComparison.CurrentCulture)
							|| str2.StartsWith("l'", StringComparison.CurrentCulture)
							|| str2.StartsWith("L'", StringComparison.CurrentCulture))
						{
							// First word always capitalized
							array[i] = ((i == 0) ? str2[0].ToString().ToUpper() : str2[0].ToString().ToLower()) +
												"'" + str2.Substring(2).CapitalizeHyphenated();
						}
						else
						{
							// default rule
							array[i] = str2.CapitalizeHyphenated();
						}
						break;
				}
			}
			string processed_str = string.Join(" ", array);

			return processed_str;
		}

		/// <summary>
		/// ToTitleCase for other names, mostly starting with determinant (le, la, les),
		/// ex TradeShip and faction names.
		/// Business types and political unions do not follow the same Cap rules
		/// and have more complicated logic.
		/// Do not capitalize the first determinant for inclusion in generated sentences
		/// in the rulepack defs.
		/// </summary>
		/// <returns>The title case for other names.</returns>
		/// <param name="str">String.</param>
		public string ToTitleCaseOtherName(string str)
		{
			if (str.NullOrEmpty())
				return str;

			string[] array = str.Split(' ');
			for (int i = 0; i < array.Length; i++)
			{
				string str2 = array[i];

				// if the first word is le/la/les/l'.
				if (i == 0)
				{
					char deter = str2[0];
					if ((deter == 'l') || (deter == 'L'))
					{
						string tmp = str2.ToLower();
						if (tmp.Equals("le") || tmp.Equals("la") || tmp.Equals("les"))
						{
							array[i] = tmp;
							continue;
						}

						if (str2.StartsWith("l'", StringComparison.CurrentCulture)
							|| str2.StartsWith("L'", StringComparison.CurrentCulture))
						{
							// First word always capitalized
							array[i] = "l'" + str2.Substring(2).CapitalizeHyphenated();
							continue;
						}
					}
				}

				// Continue processing
				if (NonUppercaseWords.Contains(str2.ToLower()))
				{
					array[i] = str2.ToLower();
					continue;
				}

				// Same as ToTitleCaseProperName
				// Capitalize word: skip "'", "d'" and "l'"
				char first = str2[0];
				switch (first)
				{
					case '\'':
						array[i] = "'" + str2.Substring(1).CapitalizeHyphenated();
						break;
					default:
						if (str2.Length == 2)
						{
							array[i] = str2.CapitalizeHyphenated();
							break;
						}

						if (str2.StartsWith("d'", StringComparison.CurrentCulture)
							|| str2.StartsWith("D'", StringComparison.CurrentCulture)
							|| str2.StartsWith("l'", StringComparison.CurrentCulture)
							|| str2.StartsWith("L'", StringComparison.CurrentCulture))
						{
							// First word always capitalized
							array[i] = ((i == 0) ? str2[0].ToString().ToUpper() : str2[0].ToString().ToLower()) +
												"'" + str2.Substring(2).CapitalizeHyphenated();
						}
						else
						{
							// default rule
							array[i] = str2.CapitalizeHyphenated();
						}
						break;
				}
			}
			string processed_str = string.Join(" ", array);

			return processed_str;
		}

		/// <summary>
		/// ToTitleCase for other categories: mostly quest titles.
		/// </summary>
		/// <returns>The title case other.</returns>
		/// <param name="str">String.</param>
		public string ToTitleCaseOther(string str)
		{
			// TODO: verify: no capitalisation after "[uU]n", "[uU]na"
			if (str.NullOrEmpty())
				return str;

			int num = str.FirstLetterBetweenTags();
			string str2 = (num == 0) ? str[num].ToString().ToUpper() : (str.Substring(0, num) + char.ToUpper(str[num]));
			string processed_str = str2 + str.Substring(num + 1);

			return processed_str;
		}

		/// <summary>
		/// Main ToTitleCase function, calls other specialized functions based on the context 
		/// identified from the callstack.
		/// Each other function implements a different title case algo depending on the lang/culture
		/// (ex. names, quest title etc.)
		/// The callstack crawling was adopted because the function to patch in the Rimworld code
		/// was not clearly identified at the time. Other method is welcome.
		/// </summary>
		/// <returns>The title cased string.</returns>
		/// <param name="str">String.</param>
		public override string ToTitleCase(string str)
		{
			if (str.NullOrEmpty())
				return str;

			StackTrace callStack = new StackTrace();
			string processed_str;

#if DEBUG
			processed_str = Debug_ToTitleCase(str, callStack);
#else
			// Split name categories
			// NOTE: Tests order matters
			if (IsQuestName(callStack)
					|| IsArtName(callStack)
			)
			{
				// Capitalize only first letter (+ '\'')
				processed_str = ToTitleCaseOther(str);
			}
			else
			if (IsPawnName(callStack)
					|| IsSettlementName(callStack)
					|| IsWorldFeatureName(callStack)
				)
			{
				processed_str = ToTitleCaseProperName(str);
			}
			else
			if (IsName(callStack))
			{
				// Any other names ex. FactionName, RimWorld.TradeShip
				processed_str = ToTitleCaseOtherName(str);
			}
			else
			{
				// Normal title : capitalize first letter.
				processed_str = ToTitleCaseOther(str);
			}
#endif
			return processed_str;
		}

		/// <summary>
		/// PluralizeOneWord implements teh basic grammar rules for one word.
		/// Do not intend to be complete, but to be sufficient so that the words
		/// in the game are correct.
		/// Called on each appropriate part of a compound word () separated with ' ' or '-'
		/// in PluralizeHyphenated and in the main function Pluralize.
		/// Uses the word lists (HashSets):
		///  	Exceptions_Plural_es, Exceptions_Plural_s, Exceptions_Plural_invariant.
		/// </summary>
		/// <returns>The plural of the given str according to gender and count.</returns>
		/// <param name="str">String.</param>
		/// <param name="gender">Verse.Gender.</param>
		/// <param name="count">int.</param>
		private string PluralizeOneWord(string str, Gender gender, int count = -1)
		{
			// Exceptions to general rules for plural
			string item = str.ToLower();
			string str_pluralized = str;

			if (Exceptions_Plural_es.Contains(item))
			{
				str_pluralized = str.Substring(0, str.Length - 3) + "es";
			}
			else if (Exceptions_Plural_s.Contains(item))
			{
				str_pluralized = str + "s";
			}
			else if (str.Equals("box", StringComparison.CurrentCulture))
			{
				// Foreign words with plural in the original language
				str_pluralized = "boxes";
			}
			else if (str.Equals("œil", StringComparison.CurrentCulture))
			{
				// Irregular forms: plural exceptions should be in a lookup dictionary
				str_pluralized = "yeux";
			}
			else if (str.Equals("pilum", StringComparison.CurrentCulture))
			{
				// latin exception
				str_pluralized = "pila";
			}
			else if (Exceptions_Plural_invariant.Contains(str.ToLower()))
			{
				// Words with invariant plural, ex. foreign words (Molotov, Yorkshire)
				str_pluralized = str;
			}
			else
			{
				// Normal rules
				char last = str[str.Length - 1];
				char oneBeforeLast = str.Length >= 2 ? str[str.Length - 2] : '\0';

				if (IsVowel(last))
				{
					if (str == "sí")  // TODO: remove since this is already handled in Exceptions_Plural_es
					{
						str_pluralized = "síes";
					}
					else
					{
						if (last == 'í' || last == 'ú' || last == 'Í' || last == 'Ú')
						{
							str_pluralized = str + "es";
						}
						else
						{
							str_pluralized = str + 's';
						}
					}
				}
				else
				{
					// TODO: check if needed in the game
					// UNDONE: Ending with -ión -> -iones
					if ((last == 'y' || last == 'Y') && IsVowel(oneBeforeLast))
					{
						str_pluralized = str + "es";
					}
					else if ((last == 'z' || last == 'Z') && (str.Length >= 2))
					{
						// Replace 'z' with 'c', then add "es"
						str_pluralized = str.Substring(0, str.Length - 1) + "ces";
					}
					else
					{
						//  - ("lrndzjsxLRNDZJSX".IndexOf(last) >= 0 && IsVowel(oneBeforeLast)) gets -es
						//  - Ending with "ch" gets -es
						//  - Ending with a consonant cluster, or a vowel + consonant!="lrndzjsxLRNDZJSX" gets -s
						//  - Ending with st or zt are invariant (ex. test)
						//  - Ending with s and unstressed last syllable are invariant
						if (("lrndzjsxLRNDZJSX".IndexOf(last) >= 0 && IsVowel(oneBeforeLast)) gets - es) || (last == 'h' && oneBeforeLast == 'c'))
						{
							str_pluralized = str + "es";
						}
						else if (("zsZS".IndexOf(oneBeforeLast) >= 0 && (last == 't'))
							|| ("aeiouAEIOU".IndexOf(oneBeforeLast) >= 0 && (last == 's'))
							)
						{
							// Invariant, ex. el test/los test, el lunes/los lunes
							str_pluralized = str;
						}
						else
						{
							str_pluralized = str + 's';
						}
					}
				}
			}

			return str_pluralized;
		}

		/// <summary>
		/// PluralizeHyphenated splits the given str at '-' and calls PluralizeOneWord on each part.
		/// The word list in NonUppercaseWords (HashSet) stops further recursion (ex. de, à etc).
		/// (This might need different lists in other languages).
		/// Another list is for words that are invariant: ex. multi-, nano-, auto- etc.
		///  	Exceptions_Plural_es, Exceptions_Plural_s, Exceptions_Plural_invariant.
		/// </summary>
		private string PluralizeHyphenated(string str, Gender gender, int count = -1)
		{
			string[] array = str.Split('-');
			if (array.NullOrEmpty())
				return str;

			if (array.Length == 1)
			{
				return PluralizeOneWord(str, gender, count);
			}

			// compound words
			for (int i = 0; i < array.Length; i++)
			{
				// stop pluralization after these words
				if (NonUppercaseWords.Contains(array[i])
					// || array[i].Equals("para")
					//|| array[i].Equals("anti")
					) // or more generally, any verb
				{
					break;
				}
				else
				{
					if (array[i].Equals("mini")
						|| array[i].Equals("mono")
						|| array[i].Equals("multi")
						|| array[i].Equals("mi")
						|| array[i].Equals("semi")
						|| array[i].Equals("neuro")
						|| array[i].Equals("t")     // t-shirt
						|| array[i].Equals("hi")    // hi-tech
						|| array[i].Equals("carbu") // game specific
						|| array[i].Equals("gastro")
						|| array[i].Equals("go")
						|| array[i].Equals("wake")
						|| array[i].Equals("mech")
						|| array[i].Equals("meca")
						|| array[i].Equals("auto")
						|| array[i].Equals("nano")
						|| array[i].Equals("psico")
						|| array[i].Equals("electro")
						)
					{
						// exception to the pluralize rules
						continue;
					}
					else
					{
						array[i] = PluralizeOneWord(array[i], gender, count);
					}
				}
			}

			return string.Join("-", array);
		}

		/// <summary>
		/// main Pluralize function. It splits the given str at ' ' and calls PluralizeHyphenated on each part.
		/// </summary>
		public override string Pluralize(string str, Gender gender, int count = -1)
		{
			if (str.NullOrEmpty())
			{
				return str;
			}

			// Do not pluralize
			if (count == 1)
			{
				return str;
			}

			string[] array = str.Split(' ');

			for (int i = 0; i < array.Length; i++)
			{
				// Stop pluralization after these words
				if (NonUppercaseWords.Contains(array[i])
						|| array[i].StartsWith("d'", StringComparison.CurrentCulture)  // French elision of "de (vowel)" follows the rule for "de"
						|| array[i].StartsWith("(", StringComparison.CurrentCulture)	 // Start of words in parentheses
					)
				{
					break;
				}
				else
				{
					if ((i > 0) && (i < array.Length - 1) &&
								 																				// French has invariant adverbs listed here, such as "avant", "arrière", "hautement" 
								 array[i].Equals("non") 								// in the middle ?
							|| array[i].ToLower().Equals("pem")     	// game specific: IEM (PEM)
							|| array[i].ToLower().Equals("ia")     		// game specific: AI (IA)
							|| array[i].ToLower().Equals("luciferium")
							|| array[i].Equals("-")										// a separator ' - '
						)
					{
						// invariants: do not change this split.
						continue;
					}
					else
					{
						array[i] = PluralizeHyphenated(array[i], gender, count);
					}
				}
			}

			// Adjectives are impossible to detect at the level of the languageworker but
			// common last adjectives in the game may give a hint.
			int n = array.Length;
			if (array[n - 1].StartsWith("(", StringComparison.CurrentCulture))
				n -= 1;

			if ((n > 0) &&
					 (array[n - 1].StartsWith("inacabad", StringComparison.CurrentCulture)
						// || array[n - 1].StartsWith("lié", StringComparison.CurrentCulture)
						// || array[n - 1].StartsWith("improvisé", StringComparison.CurrentCulture)
						// || array[n - 1].StartsWith("éventré", StringComparison.CurrentCulture)
						)
				)
			{
				// UNDONE:
				array[n - 1] = PluralizeHyphenated(array[n - 1], gender, count);
			}

			string str_pluralized = string.Join(" ", array);

			return str_pluralized;
		}

		/// <summary>
		/// main PostProcessed function.
		/// </summary>
		public override string PostProcessed(string str)
		{
			//StartStatsLogging(new StackTrace());
			string processed_str = PostProcessedLanguageGrammar(base.PostProcessed(str));
			//StopStatsLogging(str, processed_str);
			return processed_str;
		}

		/// <summary>
		/// main PostProcessedKeyedTranslation function.
		/// </summary>
		public override string PostProcessedKeyedTranslation(string translation)
		{
			//StartStatsLogging(new StackTrace());
			string processed_str = PostProcessedLanguageGrammar(base.PostProcessedKeyedTranslation(translation));
			//StopStatsLogging(translation, processed_str);
			return processed_str;
		}

		public bool IsVowel(char ch)
		{
			// return "aàâäæeéèêëiîïoôöœuùüûAÀÂÄÆEÉÈÊËIÎÏOÔÖŒUÙÜÛ".IndexOf(ch) >= 0; // french vowels
			return "aeiouáéíóúAEIOUÁÉÍÓÚ".IndexOf(ch) >= 0;
		}

		// TODO: adapt to spanish typography.
		// The Regex ([<][^>]*[>]|) component takes any XML tag into account,
		// ex. the name color tag <color=#D09B61FF> or <Name>

		private static readonly Regex DeEl = new Regex(@"\b(d)e ([<][^>]*[>]|)el ", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		// Incorrect denomination of psychic drone affected gender: should be "de sexo masculino/femenino"
		private static readonly Regex sexMale = new Regex(@"\b(sexo )varón\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static readonly Regex sexFemale = new Regex(@"\b(sexo )mujer\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/// <summary>
		/// The PostProcessing core.
		/// </summary>
		private static string PostProcessedLanguageGrammar(string str)
		{

			str = DeEl.Replace(str, "$1el $2");
			str = sexMale.Replace(str, "$1masculino");
			str = sexFemale.Replace(str, "$1femenino");

			// Clean out zero-width space
			return str.Replace("\u200B", "");
		}

		/// <summary>
		/// Change the kind.label and the gender of the pawn so that
		/// GrammarUtility.RulesForPawn generates grammatically correct rules.
		/// </summary>
		/// <param name="kind">Kind.</param>
		/// <param name="gender">Gender.</param>
		/// <param name="relationInfo">Relation info.</param>
		public static void FixPawnGender(ref PawnKindDef kind, ref Gender gender, string relationInfo)
		{
			if (kind != null)
			{
				/**
				 * Changing the kind.label has a global side-effect.
				 * The postfix method in the libHarmony patch ensures that
				 * the kind.label will be restore to the original value.
				 * This solution leaves GrammarUtility.RulesForPawn untouched.
				 * 
				 * Other solutions:
				 * 1. Rewrite GrammarUtility.RulesForPawn by replacing kind.label
				 * 		with RimWorld.GenLabel.BestKindLabel(kind, gender), ~100 lines.
				 * 2. Detect calls with WithIndefiniteArticle(kind.label, gender)
				 * 		and WithDefiniteArticle(kind.label, gender) from
				 * 		GrammarUtility.RulesForPawn (tricky but no patch needed).
				 */

				string oldlabel = kind.label;
				Gender oldgender = gender;

				switch (gender)
				{
					case Gender.Female:
						if (PawnKind_MaleOnly.Contains(kind.defName))
						{
							// the grammar uses male only as gender
							gender = Gender.Male;
							// RW will use kind.labelMale since the grammatical gender is Male !
							// TODO: verify female (ex. la hembra de avestruz)
							kind.labelMale = kind.label + " hembra";

							if (kind.labelFemale.NullOrEmpty())
							{
								// build one if the language does not provide kind.labelFemale
								kind.labelFemale = kind.label + " hembra";
							}
						}

						// for previous RW version, overwrite kind.label
						if (!kind.labelFemale.NullOrEmpty())
						{
							kind.label = kind.labelFemale;
						}
						break;
					case Gender.Male:
						if (PawnKind_FemaleOnly.Contains(kind.defName))
						{
							// the grammar uses female only as gender
							gender = Gender.Female;
							// RW will use kind.labelFemale since the grammatical gender is Female !
							// TODO: verify macho, macha, masculino, masculina
							kind.labelFemale = kind.label + " macho";

							if (kind.labelMale.NullOrEmpty())
							{
								// build one if the language does not provide kind.labelMale
								kind.labelMale = kind.label + " macho";
							}
						}

						// for previous RW version, overwrite kind.label
						if (!kind.labelMale.NullOrEmpty())
						{
							kind.label = kind.labelMale;
						}
						break;
					case Gender.None:
						// the grammar uses male as default neuter gender
						gender = Gender.Male;
						break;
				}
			}
			else
				LogMessage("--kind == null");
		}

		/// <summary>
		/// Code shamelessly copied from the base game and hopefully included there.
		/// Fixs the rules for def in GrammarUtility.RulesForDef.
		///
		/// The main change is the use of gender.GetPossessive() instead of "Proits".Translate()
		/// in the last rule (This fix shoud apply to all languages).
		///
		/// Specific to Spanish: detect whether some def.label are in plural only
		/// Other language might add some specific symbols here if needed.
		/// </summary>
		/// <returns>The grammar rules for def.</returns>
		/// <param name="prefix">rules prefix.</param>
		/// <param name="def">Def.</param>
		public static IEnumerable<Rule> FixRulesForDef(string prefix, Def def)
		{
			if (def == null)
			{
				Log.ErrorOnce($"Tried to insert rule {prefix} for null def", 79641686);
				yield break;
			}

			LanguageWorker languageWorker = Find.ActiveLanguageWorker;
			Gender gender = LanguageDatabase.activeLanguage.ResolveGender(def.label);
			bool plural = DefLabel_InPlural.Contains(def.label);

			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", def.label);
			if (def is PawnKindDef)
			{
				yield return new Rule_String(prefix + "labelPlural", ((PawnKindDef)def).GetLabelPlural());
			}
			else
			{
				// Log to inspect the difference with the original rules
				yield return new Rule_String(prefix + "labelPlural", plural ? def.label : languageWorker.Pluralize(def.label));
			}
			yield return new Rule_String(prefix + "description", def.description);
			yield return new Rule_String(prefix + "definite", languageWorker.WithDefiniteArticle(def.label, gender, plural));
			yield return new Rule_String(prefix + "indefinite", languageWorker.WithIndefiniteArticle(def.label, gender, plural));
			// TODO: verify correctness of "sus" (de él, de ella, de ellos, de ellas)
			yield return new Rule_String(prefix + "possessive", plural ? "sus" : gender.GetPossessive());
		}

		public static IEnumerable<Rule> FixRulesForBodyPartRecord(string prefix, BodyPartRecord part)
		{
			if (part == null)
			{
				Log.ErrorOnce($"Tried to insert rule {prefix} for null body part", 394876778);
				yield break;
			}

			LanguageWorker languageWorker = Find.ActiveLanguageWorker;
			Gender gender = LanguageDatabase.activeLanguage.ResolveGender(part.Label);
			bool plural = DefLabel_InPlural.Contains(part.Label);

			if (!prefix.NullOrEmpty())
			{
				prefix += "_";
			}
			yield return new Rule_String(prefix + "label", part.Label);
			yield return new Rule_String(prefix + "definite", languageWorker.WithDefiniteArticle(part.Label, gender, plural));
			yield return new Rule_String(prefix + "indefinite", languageWorker.WithIndefiniteArticle(part.Label, gender, plural));
			// TODO: verify correctness of "sus" (de él, de ella, de ellos, de ellas)
			yield return new Rule_String(prefix + "possessive", plural ? "sus" : gender.GetPossessive());
		}

	}
}