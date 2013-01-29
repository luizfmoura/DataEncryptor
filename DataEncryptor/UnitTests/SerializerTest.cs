using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataEncryptor.Serialization;
using DataEncryptor.Model;
using System.IO;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class SerializerTest
    {
        [TestMethod]
        public void TestCryptoKey()
        {
            var key1 = new CryptoKey("1234567890123456789012345678901234567890");

            Assert.AreEqual(32, key1.KeyBytes.Length);
            Assert.AreEqual(16, key1.IVBytes.Length);
        }

        [TestMethod]
        public void TestSerializer()
        {
            var key = new CryptoKey("123mudar");
            var obj = new Entry("Megaupload", "luiz22", "vascao123");
            var fileName = "file.dat";

            Serializer.SerializeToFile<Entry>(fileName, obj, key);

            Assert.IsTrue(File.Exists(fileName));

            var key2 = new CryptoKey("123mudar");

            var obj2 = Serializer.DeserializeFromFile<Entry>(fileName, key2);

            Assert.IsNotNull(obj);
            Assert.AreEqual("Megaupload", obj.Description);
            Assert.AreEqual("luiz22", obj.User);
            Assert.AreEqual("vascao123", obj.Password);
        }

        [TestMethod]
        public void TestSerializerListOfObjects()
        {
            var key = new CryptoKey("123mudar");
            var listObj = new List<Entry>() { 
                new Entry("Megaupload", "luiz22", "vascao123"),
                new Entry("Yahoo", "luiz324", "dsafd"),
                new Entry("Apple", "432432", "9324j32"),
                new Entry("Espn", "42343", "65463")                        
            };


            var fileName = "file.dat";

            Serializer.SerializeToFile<List<Entry>>(fileName, listObj, key);

            Assert.IsTrue(File.Exists(fileName));


            var key2 = new CryptoKey("123mudar");

            var listObj2 = Serializer.DeserializeFromFile<List<Entry>>(fileName, key2);

            Assert.IsNotNull(listObj2);
            Assert.AreEqual(4, listObj2.Count);
            Assert.AreEqual("Megaupload", listObj2[0].Description);
        }
    }
}