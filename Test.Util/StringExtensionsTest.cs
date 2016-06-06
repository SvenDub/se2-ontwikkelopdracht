using NUnit.Framework;
using Util;

namespace Test.Util
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void TestContainsIgnoreCase()
        {
            Assert.True("abc".ContainsIgnoreCase("a"));
            Assert.True("abc".ContainsIgnoreCase("b"));
            Assert.True("abc".ContainsIgnoreCase("c"));

            Assert.True("aBC".ContainsIgnoreCase("a"));
            Assert.True("aBC".ContainsIgnoreCase("B"));
            Assert.True("aBC".ContainsIgnoreCase("c"));

            Assert.False("abc".ContainsIgnoreCase("d"));
            Assert.False("aBC".ContainsIgnoreCase("e"));
            Assert.False("aBC".ContainsIgnoreCase("F"));

            Assert.True("abc".ContainsIgnoreCase(""));
            Assert.True("".ContainsIgnoreCase(""));

            Assert.True("ß".ContainsIgnoreCase("ss"));
            Assert.True("ß".ContainsIgnoreCase("Ss"));
            Assert.True("ß".ContainsIgnoreCase("sS"));
            Assert.True("ss".ContainsIgnoreCase("ß"));
        }

        [Test]
        public void TestCalculateMD5()
        {
            Assert.AreEqual("acbd18db4cc2f85cedef654fccc4a4d8", "foo".CalculateMD5());
            Assert.AreEqual("901890a8e9c8cf6d5a1a542b229febff", "FOO".CalculateMD5());
            Assert.AreEqual("37b51d194a7513e45b56f6524f2d51f2", "bar".CalculateMD5());
            Assert.AreEqual("01e312984527641b9fcb26de2aa07e0d", "BaR".CalculateMD5());
            Assert.AreEqual("3d75eec709b70a350e143492192a1736", "BAR".CalculateMD5());
            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", "".CalculateMD5());

            string longString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec eleifend, eros ut " +
                                "vestibulum egestas, mauris sem suscipit nunc, sed suscipit tortor ligula nec nisl. " +
                                "Nullam tincidunt odio et enim interdum interdum sed nec ante. Donec eget massa in " +
                                "ante imperdiet viverra. Vivamus libero risus, porta vel facilisis vitae, faucibus " +
                                "consectetur neque. In laoreet nisi augue, vel aliquet magna dictum eget. Integer " +
                                "vel ex nec enim pulvinar lacinia et sit amet purus. Nam quis euismod odio, id " +
                                "hendrerit quam. Curabitur nec urna libero. Vestibulum pharetra metus in tortor " +
                                "blandit, ac venenatis metus posuere. Sed ut consectetur tortor. Nam rutrum nibh ut " +
                                "urna auctor fermentum. In bibendum at ante sit amet eleifend. Nulla tempus rhoncus " +
                                "consequat. Ut molestie velit ex, efficitur rhoncus nibh tincidunt vel.";
            Assert.AreEqual("bb3551296736f3cba26d685cd2e32203", longString.CalculateMD5());
        }
    }
}