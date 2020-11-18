using NUnit.Framework;
using RimWorld_LanguageWorker_Spanish;
using System;

namespace RimWorldLanguageWorker_Spanish_NUnitTest
{
  [TestFixture]
  public class NumberCaseResolverTest
  {
    LanguageWorker_Spanish _lw;
    string _template;

    [SetUp]
    public void TestInit()
    {
      _lw = new LanguageWorker_Spanish();
      _template = "asd\n\nfasdf^Number( {0} | 'прошёл # день' | 'прошло # дня' | 'прошло # дней')^\n\nasdfasdf";
    }

    [Test]
    public void ProcessesInt()
    {
      Assert.AreEqual("asd\n\nfasdfпрошёл 1 день\n\nasdfasdf", _lw.TestResolver(string.Format(_template, 1)));
      Assert.AreEqual("asd\n\nfasdfпрошло 3 дня\n\nasdfasdf", _lw.TestResolver(string.Format(_template, 3)));
      Assert.AreEqual("asd\n\nfasdfпрошло 5 дней\n\nasdfasdf", _lw.TestResolver(string.Format(_template, 5)));
      Assert.AreEqual("asd\n\nfasdfпрошло 11 дней\n\nasdfasdf", _lw.TestResolver(string.Format(_template, 11)));
      Assert.AreEqual("asd\n\nfasdfпрошло 13 дней\n\nasdfasdf", _lw.TestResolver(string.Format(_template, 13)));
      Assert.AreEqual("asd\n\nfasdfпрошло 15 дней\n\nasdfasdf", _lw.TestResolver(string.Format(_template, 15)));
      Assert.AreEqual("asd\n\nfasdfпрошёл 21 день\n\nasdfasdf", _lw.TestResolver(string.Format(_template, 21)));
      Assert.AreEqual("asd\n\nfasdfпрошло 23 дня\n\nasdfasdf", _lw.TestResolver(string.Format(_template, 23)));
      Assert.AreEqual("asd\n\nfasdfпрошло 256 дней\n\nasdfasdf", _lw.TestResolver(string.Format(_template, 256)));
    }

    [Test]
    public void ProcessesFloatWithFracPart()
    {
      Assert.AreEqual("asd\n\nfasdfпрошло 3.0 дня\n\nasdfasdf", _lw.TestResolver(string.Format(_template, "3.0")));
      Assert.AreEqual("asd\n\nfasdfпрошло 3.1 дня\n\nasdfasdf", _lw.TestResolver(string.Format(_template, "3.1")));
      Assert.AreEqual("asd\n\nfasdfпрошло 3.9999 дня\n\nasdfasdf", _lw.TestResolver(string.Format(_template, "3.9999")));
    }

    [Test]
    public void ReturnsOrignalValueOnSyntaxError()
    {
      string template = "asd\n\nfasdf^Number( {0} | 'прошёл # день' 'прошло # дня' | 'прошло # дней')^\n\nasdfasdf";

      string incorrectValue = string.Format(template, "30");

      Assert.AreEqual(incorrectValue, _lw.TestResolver(incorrectValue));
    }
  }
}
