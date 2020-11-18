using System;
using NUnit.Framework;
using RimWorld_LanguageWorker_Spanish;

namespace RimWorldLanguageWorkerSpanish_NUnitTest
{
	[TestFixture]
	public class TestPostProcessed
	{
		[Test]
		public void TestElision()
		{
			LanguageWorker_Spanish _lw = new LanguageWorker_Spanish();

			// h aspirated words are prefixed with zero-width space
			string template = "azerty\n\nazerty Viande de husky\n\nazerty";
			Assert.AreEqual("azerty\n\nazerty Viande de husky\n\nazerty", _lw.PostProcessed(template));

			template = "azerty\n\nazerty le haut-de-forme\n\nazerty";
			Assert.AreEqual("azerty\n\nazerty le haut-de-forme\n\nazerty", _lw.PostProcessed(template));

			template = "azerty\n\nazerty la harpe azerty\n\nazerty";
			Assert.AreEqual("azerty\n\nazerty la harpe azerty\n\nazerty", _lw.PostProcessed(template));

			// unaspirated h
			template = "azerty\n\nazerty Viande de humain\n\nazerty";
			Assert.AreEqual("azerty\n\nazerty Viande d'humain\n\nazerty", _lw.PostProcessed(template));

			template = "azerty\n\nazerty un lit de hôpital\n\nazerty";
			Assert.AreEqual("azerty\n\nazerty un lit d'hôpital\n\nazerty", _lw.PostProcessed(template));

			template = "azerty\n\nazerty après le onzième jour azerty\n\nazerty";
			Assert.AreEqual("azerty\n\nazerty après le onzième jour azerty\n\nazerty", _lw.PostProcessed(template));

			template = "azerty\n\nazerty la jambe de le onze \n\nazerty";
			Assert.AreEqual("azerty\n\nazerty la jambe du onze \n\nazerty", _lw.PostProcessed(template));

		}

		[Test]
		public void TestColorTag()
		{
			LanguageWorker_Spanish _lw = new LanguageWorker_Spanish();

			// Elision should work with the color tag
			// à le -> au, de <tag>le -> du
			string template = "une morsure à le corps a causé la mort de <color=#D09B61FF>le lièvre</color>.";
			Assert.AreEqual("une morsure au corps a causé la mort du <color=#D09B61FF>lièvre</color>.", _lw.PostProcessed(template));

			// à <tag>le ->au
			template = "Anya a chanté une vielle berceuse à <color=#D09B61FF>le grizzly femelle</color>.";
			Assert.AreEqual("Anya a chanté une vielle berceuse au <color=#D09B61FF>grizzly femelle</color>.", _lw.PostProcessed(template));

			// À <tag>les ->Aux
			template = "À <color=#D09B61FF>Les Invalides</color>, c'est bouché.";
			Assert.AreEqual("Aux <color=#D09B61FF>Invalides</color>, c'est bouché.", _lw.PostProcessed(template));

			// de <tag>le -> du, two color tags
			template = "la balle de revolver de <color=#D09B61FF>Willow</color> a esquinté la patte arrière droite de <color=#D09B61FF>le labrador</color>.";
			Assert.AreEqual("la balle de revolver de <color=#D09B61FF>Willow</color> a esquinté la patte arrière droite du <color=#D09B61FF>labrador</color>.", _lw.PostProcessed(template));

			// de les -> des
			template = "<color=#D09B61FF>Andy</color> a été étourdi par l'attaque de <color=#D09B61FF>les bisons</color>.";
			Assert.AreEqual("<color=#D09B61FF>Andy</color> a été étourdi par l'attaque des <color=#D09B61FF>bisons</color>.", _lw.PostProcessed(template));

			// aspirated h
			template = "une griffure a causé la mort de <color=#D09B61FF>le husky</color>.";
			Assert.AreEqual("une griffure a causé la mort du <color=#D09B61FF>husky</color>.", _lw.PostProcessed(template));

			// "l'" takes precedence, h unaspirated
			template = "azerty\n\nazerty un lit à <color=#D09B61FF>le hôpital</color>\n\nazerty";
			Assert.AreEqual("azerty\n\nazerty un lit à <color=#D09B61FF>l'hôpital</color>\n\nazerty", _lw.PostProcessed(template));

			// de [aàâäæeéèêëiîïoôöœuùüûh] -> d'[aàâäæeéèêëiîïoôöœuùüûh]
			template = "le corps de <color=#D09B61FF>la mégathérium</color> a été percé par la balle de <color=#D09B61FF>Annabel</color>.";
			Assert.AreEqual("le corps de <color=#D09B61FF>la mégathérium</color> a été percé par la balle d'<color=#D09B61FF>Annabel</color>.", _lw.PostProcessed(template));

			template = "la balle de <color=#D09B61FF>Odette</color> a annihilé le œil gauche de <color=#D09B61FF>Andi</color>.";
			Assert.AreEqual("la balle d'<color=#D09B61FF>Odette</color> a annihilé l'œil gauche d'<color=#D09B61FF>Andi</color>.", _lw.PostProcessed(template));
		}

		[Test]
		public void TestSonSa()
		{
			LanguageWorker_Spanish _lw = new LanguageWorker_Spanish();

			// h muet
			string template = "<color=#D09B61FF>Wallis</color> a questionné <color=#D09B61FF>Lawman</color> sur sa habitude alimentaire.";
			Assert.AreEqual("<color=#D09B61FF>Wallis</color> a questionné <color=#D09B61FF>Lawman</color> sur son habitude alimentaire.", _lw.PostProcessed(template));

			// son/sa <voyelle> -> son <voyelle>
			template = "<color=#D09B61FF>Wallis</color> a attrapé une maladie : cancer dans son/sa oreille droite.";
			Assert.AreEqual("<color=#D09B61FF>Wallis</color> a attrapé une maladie : cancer dans son oreille droite.", _lw.PostProcessed(template));

		}
	}
}
