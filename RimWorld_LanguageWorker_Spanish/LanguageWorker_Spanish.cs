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

		// in plural, replace "ail" with "aux"
		private static readonly HashSet<string> Exceptions_Plural_aux = new HashSet<string> {
			"bail",
			"corail",
			"émail",
			"gemmail",
			"soupirail",
			"travail",
			"vantail",
			"vitrail"
		};

		// lieu (fish) takes an "s", but does not exist in RimWorld
		private static readonly HashSet<string> Exceptions_Plural_s = new HashSet<string> {
			"bleu",
			"émeu",
			"landau",
			"pneu",
			"sarrau",
			"bal",
			"banal",
			"fatal",
			"final",
			"festival"
		};

		// lieu (area) takes an "x", it exists in RimWorld (ex. lieu d'assemblage-crafting spot)
		private static readonly HashSet<string> Exceptions_Plural_x = new HashSet<string> {
			"bijou",
			"caillou",
			"chou",
			"genou",
			"hibou",
			"joujou",
			"pou",
			"lieu"
		};

		// Words with aspirated h do not get elision (list only words in RimWorld)
		// Added no elision to "onze", "onzième" -- do not appear in RiWorld yet)
		private static readonly HashSet<string> Exceptions_No_Elision = new HashSet<string> {
			"hache",
			"hack",
			"haine",
			"hameau",
			"hampe",
			"hamster",
			"hanche",
			"hareng",
			"haricot",
			"harpe",
			"hasard",
			"hase", // "hases",
      "hât", // "hâte", "hâtif", "hâtive", "hâtivement",
      "haut", // "haute",
      "héron",
			"hérisson",
			"hêtre",
			"hibou",
			"holding",
			"homard",
			"honte",
			"horde",
			"hors", // "hors-la-loi",
      "houblon",
			"huit",
			"hunter",
			"hurl", // "hurler", "hurle", "hurlé", "hurlement",
      "husky",
			"hutte",
			"hyène",
			"onz" // , "onze", "onzième"
    };

		private static readonly HashSet<string> PawnKind_FemaleOnly = new HashSet<string> {
			"Boomalope",
			"Gazelle",
			"Megaspider",
			"Ostrich",
			"Tortoise"
		};

		private static readonly HashSet<string> PawnKind_MaleOnly = new HashSet<string>{
			"Alphabeaver",
			"Bear_Grizzly",
			"Boomrat",
			"Capybara",
			"Caribou",
			"Cassowary",
			"Chinchilla",
			"Cobra",
			"Cougar",
			"Dromedary",
			"Elk",
			"Emu",
			"Fox_Fennec",
			"GuineaPig",
			"Husky",
			"Iguana",
			"LabradorRetriever",
			"Lynx",
			"Megascarab",
			"Megasloth",
			"Muffalo",
			"Raccoon",
			"Rhinoceros",
			"Spelopede",
			"Squirrel",
			"Thrumbo",
			"Warg",
			"YorkshireTerrier"
		};

		// For ToTitleCase: No uppercase if in the middle of the string
		// For Pluralize: Contains most of the prepositions (stopping test for compound words)
		private static HashSet<string> NonUppercaseWords = new HashSet<string>
		{
			"à", // "à la",
			"au",
			"avec",
			"chez",
			"dans",
			"de", // "de la",
			"des",
			"dès",
			"devant",
			"du",
			"en",
			"entre",
			"et",
			"hors",
			"jusqu'à",
			"jusqu'au",
			"jusque",
			"la",
			"le",
			"les",
			"lez",
			"par",
			"pour",
			"post",
			"sans",
			"sur", // "sur le", "sur la",
			"vers",
			"van", // some foreign words
			"von"
		};

		// Some labels are usually in plural because of contexts
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
      if( name )
        return str;

      if( plural )
        return (gender == Gender.Female ? "unas " : "unos ") + str;	

      else	

        return (gender == Gender.Female ? "una " : "un ") + str;
		}

		public override string WithDefiniteArticle(string str, Gender gender, bool plural = false, bool name = false)
		{
			if (str.NullOrEmpty())
				return str;

		//Names don't get articles
		if( name )
			return str;

		if( plural )
			return (gender == Gender.Female ? "las " : "los ") + str;

		else

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
			// TODO: pas de capitalisation après "[uU]n", "[uU]ne"
			if (str.NullOrEmpty())
				return str;

			int num = str.FirstLetterBetweenTags();
			string str2 = (num == 0) ? str[num].ToString().ToUpper() : (str.Substring(0, num) + char.ToUpper(str[num]));
			string processed_str = str2 + str.Substring(num + 1);

			return processed_str;
		}

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

		private string PluralizeOneWord(string str, Gender gender, int count = -1)
    {
      // Exceptions to general rules for plural
      string item = str.ToLower();
      string str_pluralized = str;
      
      // TODO: define plural exceptions for Spanish
      if (Exceptions_Plural_aux.Contains(item))
      {
        str_pluralized = str.Substring(0, str.Length - 3) + "aux";
      }
      else if (Exceptions_Plural_s.Contains(item))
      {
        str_pluralized = str + "s";
      }
      else if (Exceptions_Plural_x.Contains(item))
      {
        str_pluralized = str + "x";
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
      else if (str.ToLower().Equals("yorkshire", StringComparison.CurrentCulture)
        || str.ToLower().Equals("molotov", StringComparison.CurrentCulture)
        || str.ToLower().Equals("pemmican", StringComparison.CurrentCulture)
        || str.ToLower().Equals("pekoe", StringComparison.CurrentCulture)
      )
      {
        // Foreign words with no plural
        str_pluralized = str;
      }
      else
      {
        // Normal rules
        char last = str[str.Length - 1];
        char oneBeforeLast = str.Length >= 2 ? str[str.Length - 2] : '\0';
        
        if( IsVowel(last) )
        {
          if( str == "sí" )
            str_pluralized = "síes";
          else if( last == 'í' || last == 'ú' || last == 'Í' || last == 'Ú' )
            str_pluralized = str + "es";
          else
            str_pluralized = str + 's';
        }
        else
        {
          if( (last == 'y' || last == 'Y') && IsVowel(oneBeforeLast) )
            str_pluralized = str + "es";
          else if( "lrndzjsxLRNDZJSX".IndexOf(last) >= 0 || (last == 'h' && oneBeforeLast == 'c') )
            str_pluralized = str + "es";
          else
            str_pluralized = str + 's';
        }
      }
      
      return str_pluralized;
    }
    
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
					|| array[i].Equals("lance")
					|| array[i].Equals("pare")
					|| array[i].Equals("anti")
					|| array[i].Equals("dompte")
					) // or more generally, any verb
				{
					break;
				}
				else if (
						 array[i].Equals("mini")
					|| array[i].Equals("mono")
					|| array[i].Equals("multi")
					|| array[i].Equals("mi")
					|| array[i].Equals("demi")
					|| array[i].Equals("semi")
					|| array[i].Equals("t")     // t-shirt
					|| array[i].Equals("hi")    // hi-tech
					|| array[i].Equals("carbu") // game specific
					|| array[i].Equals("gastro")
					|| array[i].Equals("go")
					|| array[i].Equals("wake")
					|| array[i].Equals("mech")
					|| array[i].Equals("avant")
					|| array[i].Equals("arrière")
					|| array[i].Equals("après")
					|| array[i].Equals("contre")
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

			return string.Join("-", array);
		}

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
				// stop pluralization after these words
				if (NonUppercaseWords.Contains(array[i])
					|| array[i].StartsWith("d'", StringComparison.CurrentCulture)
					|| array[i].StartsWith("(", StringComparison.CurrentCulture)
				)
				{
					break;
				}
				else if ((i > 0) && (i < array.Length - 1) && array[i].Equals("non") // in the middle ?
							|| array[i].ToLower().Equals("avant")   // adverbs
							|| array[i].ToLower().Equals("arrière")
							|| array[i].ToLower().Equals("hautement")
							|| array[i].ToLower().Equals("iem")			// game specific
							|| array[i].ToLower().Equals("ia")
							|| array[i].ToLower().Equals("luciferium")
							|| array[i].Equals("-") // a separator ' - '
						)
				{
					// invariants
					continue;
				}
				else
				{
					array[i] = PluralizeHyphenated(array[i], gender, count);
				}
			}

			// Common last adjectives in the game
			int n = array.Length;
			if (array[n - 1].StartsWith("(", StringComparison.CurrentCulture))
				n -= 1;

			if ((n > 0) &&
					( array[n - 1].StartsWith("inachevé", StringComparison.CurrentCulture)
				|| array[n - 1].StartsWith("lié", StringComparison.CurrentCulture)
				|| array[n - 1].StartsWith("improvisé", StringComparison.CurrentCulture)
				|| array[n - 1].StartsWith("éventré", StringComparison.CurrentCulture)
				))
			{
				array[n - 1] = PluralizeHyphenated(array[n - 1], gender, count);
			}

			string str_pluralized = string.Join(" ", array);

			return str_pluralized;
		}

		public override string PostProcessed(string str)
		{
			//StartStatsLogging(new StackTrace());
			string processed_str = PostProcessedLanguageGrammar(base.PostProcessed(str));
			//StopStatsLogging(str, processed_str);
			return processed_str;
		}

		public override string PostProcessedKeyedTranslation(string translation)
		{
			//StartStatsLogging(new StackTrace());
			string processed_str = PostProcessedLanguageGrammar(base.PostProcessedKeyedTranslation(translation));
			//StopStatsLogging(translation, processed_str);
			return processed_str;
		}

		public bool IsVowel(char ch)
		{
			//Do not include [hH]
			// return "aàâäæeéèêëiîïoôöœuùüûAÀÂÄÆEÉÈÊËIÎÏOÔÖŒUÙÜÛ".IndexOf(ch) >= 0; // french vowels
      return "aeiouáéíóúAEIOUÁÉÍÓÚ".IndexOf(ch) >= 0;
		}

		// TODO: french typography, add space before [:;?!]
		// The Regex ([<][^>]*[>]|) component takes any XML tag into account,
		// ex. the name color tag <color=#D09B61FF> or <Name>
		private static readonly Regex WordsWithoutElision = new Regex(@"\b(h[^ <>]+|onz[^ <>]+)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		// NOTE: exception "lorsque aucun", "lorsque aucune", "lorsque avec", "lorsque <prenom>"
		private static readonly Regex ElisionE = new Regex(@"\b([cdjlmnst]|qu|quoiqu|lorsqu)e ([<][^>]*[>]|)([aàâäæeéèêëiîïoôöœuùüûh])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static readonly Regex ElisionLa = new Regex(@"\b(l)a ([<][^>]*[>]|)([aàâäæeéèêëiîïoôöœuùüûh])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static readonly Regex ElisionSi = new Regex(@"\b(s)i (ils?)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		// possessive + vowel/h muet. ex. instead of "sa épée" -> "son épée", "son/sa oreille" -> "son oreille"
		private static readonly Regex PossessiveVowel = new Regex(@"\b([mst])(on/[mst]|)a ([<][^>]*[>]|)([aàâäæeéèêëiîïoôöœuùüûh])", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static readonly Regex DeLe = new Regex(@"\b(d)e ([<][^>]*[>]|)le ", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static readonly Regex DeLes = new Regex(@"\b(d)e ([<][^>]*[>]|)l(es) ", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static readonly Regex ALe = new Regex(@"\b(à) ([<][^>]*[>]|)le(s?) ", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		// dénomination incorrect du sexe des pawns
		private static readonly Regex sexMale = new Regex(@"\b(sexe m)âle\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		private static readonly Regex sexFemale = new Regex(@"\b(sexe fem)elle\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private static string PostProcessedLanguageGrammar(string str)
		{
      // French typography commented out
			// "[dD]e des" are generated by some rules of type "de [x_indefinite]" in plural
      // 			str = str.Replace(" de des ", " des ")
      // 				.Replace("De des ", "Des ");
      // 
      // 			str = WordsWithoutElision.Replace(str, new MatchEvaluator(ReplaceNoElision));
      // 			str = ElisionE.Replace(str, "$1'$2$3");
      // 			str = ElisionLa.Replace(str, "$1'$2$3");
      // 			str = ElisionSi.Replace(str, "$1'$2");
      // 			str = PossessiveVowel.Replace(str, "$1on $3$4");
      // 			str = DeLe.Replace(str, "$1u $2");
      // 			str = DeLes.Replace(str, "$1$3 $2");
      // 			str = sexMale.Replace(str, "$1asculin");
      // 			str = sexFemale.Replace(str, "$1inin");
      // 			str = ALe.Replace(str, new MatchEvaluator(ReplaceALe));

			// Clean out zero-width space
			return str.Replace("\u200B", "");
		}

		// The ALe Regex has three groups: "\b(à) ([<][^>]*[>]|)le(s?) " where
		//		Groups[1] is (à|À),
		//		Groups[2] is an eventual text coloring tag <tag>,
		//		Groups[3] is (s?) for plural.
		// For each case:
		//		"à <tag>le ": return "au <tag>";
		//  	"à <tag>les ": return "aux <tag>";
		//		"\u00c0 <tag>le ": return "Au <tag>";
		//		"\u00c0 <tag>les ": return "Aux <tag>";
		private static string ReplaceALe(Match match)
		{
			string str = "";
			switch (match.Groups[1].Value)
			{
				case "à":
					str = "au";
					break;
				case "\u00c0":
					str = "Au";
					break;
			}

			// Add 'x' for plural
			if (match.Groups[3].Value == "s")
			{
				str += "x";
			}

			// Add space before any Group 2
			str += " ";
			if (match.Groups[2].Value != "")
			{
				str += match.Groups[2].Value;
			}
			return str;
		}

		private static string ReplaceNoElision(Match match)
		{
			string item_raw = match.ToString();
			string item = item_raw.ToLower();
			foreach (var s in Exceptions_No_Elision)
			{
				if (item.StartsWith(s, StringComparison.CurrentCulture))
				{
					// Add zero-width space to foul the elision rules
					return ("\u200B" + item_raw);
				}
			}
			return item_raw;
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
							kind.labelMale = kind.label + " femelle";

							if (kind.labelFemale.NullOrEmpty())
							{
								// build one if the language does not provide kind.labelFemale
								kind.labelFemale = kind.label + " femelle";
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
							kind.labelFemale = kind.label + " mâle";

							if (kind.labelMale.NullOrEmpty())
							{
								// build one if the language does not provide kind.labelMale
								kind.labelMale = kind.label + " mâle";
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
			yield return new Rule_String(prefix + "possessive", plural ? "ses" : gender.GetPossessive());
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
			yield return new Rule_String(prefix + "possessive", plural ? "ses" : gender.GetPossessive());
		}

	}
}