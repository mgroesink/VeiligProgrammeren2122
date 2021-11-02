using Microsoft.VisualStudio.TestTools.UnitTesting;
using VeiligProgrammeren2122;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeiligProgrammeren2122.Tests
{
    [TestClass()]
    public class StaticMethodsTests
    {
        [TestMethod()]
        public void ShiftCypherTest1()
        {
            string text = "HALLO";
            string encrypted = StaticMethods.ShiftCypher(text, 2);

            Assert.AreEqual("JCNNQ", encrypted);
        }

        [TestMethod()]
        public void UnShiftCypherTest1()
        {
            string text = "JCNNQ";
            string decrypted = StaticMethods.UnShiftCypher(text, 2);

            Assert.AreEqual("HALLO", decrypted);
        }

        [TestMethod()]
        public void ShiftCypherTest2()
        {
            string text = "Hallo123";
            string encrypted = StaticMethods.ShiftCypher(text, 2);

            Assert.AreEqual("Jcnnq123", encrypted);
        }

        [TestMethod()]
        public void UnShiftCypherTest2()
        {
            string text = "Jcnnq123";
            string decrypted = StaticMethods.UnShiftCypher(text, 2);

            Assert.AreEqual("Hallo123", decrypted);
        }

        [TestMethod()]
        public void ShiftCypherTest3()
        {
            string text = "Hallo123";
            string encrypted = StaticMethods.ShiftCypher(text, "ABC");

            Assert.AreEqual("Icomq123", encrypted);
        }
        [TestMethod()]
        public void UnShiftCypherTest3()
        {
            string text = "Icomq123";
            string decrypted = StaticMethods.UnShiftCypher(text, "ABC");

            Assert.AreEqual("Hallo123", decrypted);
        }
        [TestMethod()]
        public void ShiftCypherTest4()
        {
            string text = "Hallo123";
            string encrypted = StaticMethods.ShiftCypher(text, "AaZz");

            Assert.AreEqual("IBLlp123", encrypted);
        }
        [TestMethod()]
        public void UnShiftCypherTest4()
        {
            string text = "IBLlp123";
            string decrypted = StaticMethods.UnShiftCypher(text, "AaZz");

            Assert.AreEqual("Hallo123", decrypted);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException),"Key should only contain letters")]
        public void ShiftCypherTest5()
        {
            string text = "Hallo123";
            string encrypted = StaticMethods.ShiftCypher(text, "A12z");

            Assert.AreEqual("IBLlp123", encrypted);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Key should only contain letters")]
        public void UnShiftCypherTest5()
        {
            string text = "IBLlp123";
            string decrypted = StaticMethods.UnShiftCypher(text, "A12z");

            Assert.AreEqual("Hallo123", decrypted);
        }
    }
}