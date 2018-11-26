using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventPropsClasses;
using NUnit.Framework;

namespace EventTestClasses
{
    [TestFixture]
    public class ProductPropsTests
    {
        ProductProps p;

        [SetUp]
        public void SetUpAllTests()
        {
            p = new ProductProps();
            p.ID = 1;
            p.code = "XXXX";
            p.description = "test product";
            p.price = 99.99m;
            p.quantity = 8;
            p.ConcurrencyID = 1;

        }

        [Test]
        public void TestClone()
        {
            ProductProps p2 = (ProductProps)p.Clone();
            Assert.AreEqual(p.ID, p2.ID);
            Assert.AreEqual(p.code, p2.code);
            Assert.AreEqual(p.description, p2.description);
            Assert.AreEqual(p.price, p2.price);
            Assert.AreEqual(p.quantity, p2.quantity);
            Assert.AreEqual(p.ConcurrencyID, p2.ConcurrencyID);

            p.ID = 4;
            Assert.AreNotEqual(p.ID, p2.ID);

        }

        [Test]
        public void TestGetState()
        {
            string pString = p.GetState();
            Console.WriteLine(pString);
        }

        [Test]
        public void TestSetState()
        {
            string pString = p.GetState();
            ProductProps p2 = new ProductProps();
            p2.SetState(pString);
            Assert.AreEqual(p.ID, p2.ID);
            Assert.AreEqual(p.code, p2.code);
            Assert.AreEqual(p.description, p2.description);
            Assert.AreEqual(p.price, p2.price);
            Assert.AreEqual(p.quantity, p2.quantity);
            Assert.AreEqual(p.ConcurrencyID, p2.ConcurrencyID);

        }

    }
}
