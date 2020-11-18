using NUnit.Framework;
using RimWorld_LanguageWorker_Spanish;
using System;

namespace RimWorldLanguageWorkerSpanish_NUnitTest
{
	[TestFixture()]
	public class TestToTitleCase
	{
		[Test()]
		public void TestPawnNames()
		{
			LanguageWorker_Spanish _lw = new LanguageWorker_Spanish();

			// Simple name
			string template = "lloraga crica";
			Assert.AreEqual("Lloraga Crica", _lw.ToTitleCaseProperName(template));

			// Name triple
			template = "cambiar 'tortue' legua";
			Assert.AreEqual("Cambiar 'Tortue' Legua", _lw.ToTitleCaseProperName(template));

			template = "Cagoguaxo éléphante de mer";
			// TODO: Should be "Cagoguaxo Éléphante de mer"
			Assert.AreEqual("Cagoguaxo Éléphante de Mer", _lw.ToTitleCaseProperName(template));

			template = "charles de gaulle";
			Assert.AreEqual("Charles de Gaulle", _lw.ToTitleCaseProperName(template));

			template = "charles De gaulle";
			Assert.AreEqual("Charles de Gaulle", _lw.ToTitleCaseProperName(template));

			template = "de gaulle";
			Assert.AreEqual("De Gaulle", _lw.ToTitleCaseProperName(template));

			template = "d'Autriche";
			Assert.AreEqual("D'Autriche", _lw.ToTitleCaseProperName(template));

			template = "werner von braun";
			Assert.AreEqual("Werner von Braun", _lw.ToTitleCaseProperName(template));

			template = "gérard D'aboville";
			Assert.AreEqual("Gérard d'Aboville", _lw.ToTitleCaseProperName(template));

			template = "antoine-françois gérard";
			Assert.AreEqual("Antoine-François Gérard", _lw.ToTitleCaseProperName(template));

			template = "marie-Thérèse d'autriche";
			Assert.AreEqual("Marie-Thérèse d'Autriche", _lw.ToTitleCaseProperName(template));

			template = "l'appel de cthulhu";
			Assert.AreEqual("L'Appel de Cthulhu", _lw.ToTitleCaseProperName(template));
		}
	}
}
