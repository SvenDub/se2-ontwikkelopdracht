using NUnit.Framework;
using Util;

namespace Test.Util
{
    [TestFixture]
    public class PluralFormatProviderTest
    {
        [Test]
        public void TestSingular()
        {
            Assert.AreEqual("I have 1 apple", string.Format(new PluralFormatProvider(), "I have {0:apple;apples}", 1));
        }

        [Test]
        public void TestPlural()
        {
            Assert.AreEqual("2 oranges", string.Format(new PluralFormatProvider(), "{0:orange;oranges}", 2));
            Assert.AreEqual("3 bananas", string.Format(new PluralFormatProvider(), "{0:banana;bananas}", 3));
            Assert.AreEqual("It's over 9000 grapes!",
                string.Format(new PluralFormatProvider(), "It's over {0:grape;grapes}!", 9000));
        }

        [Test]
        public void TestZero()
        {
            Assert.AreEqual("0 bothers were given that day",
                string.Format(new PluralFormatProvider(), "{0:bother;bothers} were given that day", 0));
        }

        [Test]
        public void TestNegativeSingular()
        {
            Assert.AreEqual("I have -1 apples", string.Format(new PluralFormatProvider(), "I have {0:apple;apples}", -1));
        }

        [Test]
        public void TestNegativePlural()
        {
            Assert.AreEqual("-2 oranges", string.Format(new PluralFormatProvider(), "{0:orange;oranges}", -2));
            Assert.AreEqual("-3 bananas", string.Format(new PluralFormatProvider(), "{0:banana;bananas}", -3));
            Assert.AreEqual("It's under -9000 grapes!",
                string.Format(new PluralFormatProvider(), "It's under {0:grape;grapes}!", -9000));
        }

        [Test]
        public void TestNegativeZero()
        {
            Assert.AreEqual("0 bothers were given that day",
                string.Format(new PluralFormatProvider(), "{0:bother;bothers} were given that day", -0));
        }
    }
}