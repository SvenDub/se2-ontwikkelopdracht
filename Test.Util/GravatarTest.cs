﻿using NUnit.Framework;
using Util;

namespace Test.Util
{
    public class GravatarTest
    {
        [Test]
        public void TestGetHashNormalFormatting()
        {
            Assert.AreEqual("87373a6b9021f2cd33c7a4e8515968c9", Gravatar.GetHash("sven.dubbeld1@gmail.com"));
            Assert.AreEqual("3134db8e86cd79b19fa0de1788a916c3", Gravatar.GetHash("sven@svendubbeld.nl"));
            Assert.AreEqual("34cda77ec30fb5a22d2e6e65d32932e1", Gravatar.GetHash("s.dubbeld@student.fontys.nl"));
        }

        [Test]
        public void TestGetHashCaps()
        {
            Assert.AreEqual("87373a6b9021f2cd33c7a4e8515968c9", Gravatar.GetHash("Sven.dubbeld1@gmail.com"));
            Assert.AreEqual("87373a6b9021f2cd33c7a4e8515968c9", Gravatar.GetHash("sven.Dubbeld1@gmail.com"));
            Assert.AreEqual("87373a6b9021f2cd33c7a4e8515968c9", Gravatar.GetHash("SVEN.DUBBELD1@GMAIL.com"));
        }

        [Test]
        public void TestGetHashWhitespace()
        {
            Assert.AreEqual("87373a6b9021f2cd33c7a4e8515968c9", Gravatar.GetHash(" sven.dubbeld1@gmail.com"));
            Assert.AreEqual("87373a6b9021f2cd33c7a4e8515968c9", Gravatar.GetHash("sven.dubbeld1@gmail.com "));
            Assert.AreEqual("87373a6b9021f2cd33c7a4e8515968c9", Gravatar.GetHash("     sven.dubbeld1@gmail.com     "));
        }

        [Test]
        public void TestGetHashEmpty()
        {
            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", Gravatar.GetHash(string.Empty));
        }

        [Test]
        public void TestGetUrlNormalFormatting()
        {
            Assert.AreEqual("https://www.gravatar.com/avatar/87373a6b9021f2cd33c7a4e8515968c9",
                Gravatar.GetUrl("sven.dubbeld1@gmail.com"));
            Assert.AreEqual("https://www.gravatar.com/avatar/3134db8e86cd79b19fa0de1788a916c3",
                Gravatar.GetUrl("sven@svendubbeld.nl"));
            Assert.AreEqual("https://www.gravatar.com/avatar/34cda77ec30fb5a22d2e6e65d32932e1",
                Gravatar.GetUrl("s.dubbeld@student.fontys.nl"));
        }

        [Test]
        public void TestGetUrlCaps()
        {
            Assert.AreEqual("https://www.gravatar.com/avatar/87373a6b9021f2cd33c7a4e8515968c9",
                Gravatar.GetUrl("Sven.dubbeld1@gmail.com"));
            Assert.AreEqual("https://www.gravatar.com/avatar/87373a6b9021f2cd33c7a4e8515968c9",
                Gravatar.GetUrl("sven.Dubbeld1@gmail.com"));
            Assert.AreEqual("https://www.gravatar.com/avatar/87373a6b9021f2cd33c7a4e8515968c9",
                Gravatar.GetUrl("SVEN.DUBBELD1@GMAIL.com"));
        }

        [Test]
        public void TestGetUrlNormalFormattingWhitespace()
        {
            Assert.AreEqual("https://www.gravatar.com/avatar/87373a6b9021f2cd33c7a4e8515968c9",
                Gravatar.GetUrl(" sven.dubbeld1@gmail.com"));
            Assert.AreEqual("https://www.gravatar.com/avatar/87373a6b9021f2cd33c7a4e8515968c9",
                Gravatar.GetUrl("sven.dubbeld1@gmail.com "));
            Assert.AreEqual("https://www.gravatar.com/avatar/87373a6b9021f2cd33c7a4e8515968c9",
                Gravatar.GetUrl("    sven.dubbeld1@gmail.com    "));
        }

        [Test]
        public void TestGetUrlDefaultAction()
        {
            Assert.AreEqual("https://www.gravatar.com/avatar/87373a6b9021f2cd33c7a4e8515968c9?d=404",
                Gravatar.GetUrl("sven.dubbeld1@gmail.com", "404"));
            Assert.AreEqual("https://www.gravatar.com/avatar/3134db8e86cd79b19fa0de1788a916c3?d=404",
                Gravatar.GetUrl("sven@svendubbeld.nl", "404"));
            Assert.AreEqual("https://www.gravatar.com/avatar/34cda77ec30fb5a22d2e6e65d32932e1?d=404",
                Gravatar.GetUrl("s.dubbeld@student.fontys.nl", "404"));
        }

        [Test]
        public void TestGetUrlEmptyDefaultAction()
        {
            Assert.AreEqual("https://www.gravatar.com/avatar/87373a6b9021f2cd33c7a4e8515968c9",
                Gravatar.GetUrl("sven.dubbeld1@gmail.com", ""));
            Assert.AreEqual("https://www.gravatar.com/avatar/3134db8e86cd79b19fa0de1788a916c3",
                Gravatar.GetUrl("sven@svendubbeld.nl", ""));
            Assert.AreEqual("https://www.gravatar.com/avatar/34cda77ec30fb5a22d2e6e65d32932e1",
                Gravatar.GetUrl("s.dubbeld@student.fontys.nl", ""));
        }
    }
}