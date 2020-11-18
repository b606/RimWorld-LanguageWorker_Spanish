// <code-header>
//   <author>b606</author>
//   <summary>The class LanguageWorker_Mod derived from Verse.Mod </summary>
// </code-header>
//
using Verse;

namespace RimWorld_LanguageWorker_Spanish
{
	public class LanguageWorker_ModSettings : ModSettings
	{
		public override void ExposeData()
		{
			base.ExposeData();
		}
	}

	public class LanguageWorker_Mod : Mod
	{
		public static LanguageWorker_Mod Instance { get; private set; }

		/// <summary>
		/// Reference to the LanguageWorker settings.
		/// </summary>
		LanguageWorker_ModSettings settings;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:RimWorld_LanguageWorker_Spanish.LanguageWorker_Mod"/> class.
		/// </summary>
		/// <param name="content">Content.</param>
		public LanguageWorker_Mod(ModContentPack content) : base(content)
		{
			Instance = this;
			this.settings = GetSettings<LanguageWorker_ModSettings>();

			LongEventHandler.ExecuteWhenFinished(CheckAll);
		}

		/// <summary>
		/// Do patching and checks various parameters of the game.
		/// </summary>
		public static void CheckAll()
		{
			LanguageWorkerPatcher.DoPatching();

			LoadedLanguage active = LanguageDatabase.activeLanguage;
			LanguageWorker_Spanish.LogMessage("Active: " + active.FriendlyNameEnglish
				+ " (" + active.folderName + ") " + active.Worker.GetType());

			foreach (LoadedLanguage l in LanguageDatabase.AllLoadedLanguages)
			{
				if (l.FriendlyNameEnglish.Equals(LanguageWorkerPatcher.__targetLanguage))
				{
					LanguageWorker_Spanish.LogMessage("Other: " + l.FriendlyNameEnglish +
						" (" + l.folderName + ") " + l.info.languageWorkerClass);
				}
			}
		}
	}
}
