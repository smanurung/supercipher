using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperCipher;

namespace SuperCipherTest
{
    /// <summary>
    /// Summary description for DekripsiTest
    /// </summary>
    [TestClass]
    public class DekripsiTest
    {
        public DekripsiTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TransposeOneCharTest()
        {
            //
            // TODO: Add test logic here
            //
            Dekripsi de = new Dekripsi();
            Enkripsi en = new Enkripsi();
            byte[] b = System.Text.Encoding.ASCII.GetBytes("a");
            //Assert.AreEqual(b[0],c[0]);
            CollectionAssert.AreEqual(b,de.transpose(en.transpose(b)));
        }

        [TestMethod]
        public void TransposeTwoCharTest()
        {
            Enkripsi en = new Enkripsi();
            Dekripsi de = new Dekripsi();
            byte[] b = System.Text.Encoding.ASCII.GetBytes("ab");
            CollectionAssert.AreEqual(b,de.transpose(en.transpose(b)));
        }

        [TestMethod]
        public void TransposeThreeCharTest()
        {
            Enkripsi en = new Enkripsi();
            Dekripsi de = new Dekripsi();
            byte[] b = System.Text.Encoding.ASCII.GetBytes("son");
            CollectionAssert.AreEqual(b, de.transpose(en.transpose(b)));
        }

        [TestMethod]
        public void TransposeSentenceTest()
        {
            Enkripsi en = new Enkripsi();
            Dekripsi de = new Dekripsi();
            byte[] b = System.Text.Encoding.ASCII.GetBytes("Mata Kuliah Kriptografi.");
            CollectionAssert.AreEqual(b, de.transpose(en.transpose(b)));
        }

        [TestMethod]
        public void OverallTest()
        {
            Enkripsi en = new Enkripsi();
            Dekripsi de = new Dekripsi();
        }

        [TestMethod]
        public void SubstitusiTest()
        {
            byte[] b = System.Text.Encoding.ASCII.GetBytes("Mata Kuliah Kriptografi.");
            String k = "12345678";
            CollectionAssert.AreEqual(b, Dekripsi.Substitusi(Enkripsi.Substitusi(b,k),k));
        }

        [TestMethod]
        public void FeistelTest()
        {
            Enkripsi en = new Enkripsi();
            Dekripsi de = new Dekripsi();

            byte[] b = System.Text.Encoding.ASCII.GetBytes("Mata Kuliah Kriptografi.");
            String k = "12345678";
            en.generateAllInternalKey(k);
            de.generateAllInternalKey(k);

            CollectionAssert.AreEqual(b, de.feistelDecipher(en.feistel(b,en.internalKey),de.internalKey));
        }
    }
}
