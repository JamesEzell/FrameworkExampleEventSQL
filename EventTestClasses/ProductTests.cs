using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using EventClasses;
using EventPropsClasses;
using EventDBClasses;
using ToolsCSharp;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
    [TestFixture]
    public class ProductTests
    {
        private string dataSource = "Data Source=SABER\\SQLEXPRESS;Initial Catalog=EventCalendar;Integrated Security=True";

        [SetUp]
        public void SetUp()
        {

        }
        [Test]
        public void TestRetrieveExistingProduct()
        {
            // retrieves from Data Store
            Product p = new Product(1, dataSource);

            Assert.AreEqual(p.ID, 1);
            Assert.AreEqual(p.Code, "A4CS");
            Assert.AreEqual(p.Description, "Murach's ASP.NET 4 Web Programming with C# 2010");
            Assert.AreEqual(p.Price, 56.50m);
            Assert.AreEqual(p.Quantity, 4637);
            Console.WriteLine(p.ToString());
        }

        [Test]
        public void TestCreateNewProduct()
        {
            Product p = new Product(dataSource);

            p.Code = "XXXX";
            p.Description = "This is A test product";
            p.Quantity = 10;
            p.Price = 10.99m;
            p.Save();

            Product p2 = new Product(p.ID, dataSource);

            Assert.AreEqual(p.ID , p2.ID);
            Assert.AreEqual(p.Code, p2.Code);
            Assert.AreEqual(p.Description, p2.Description);
            Assert.AreEqual(p.Price, p2.Price);
            Assert.AreEqual(p.Quantity, p2.Quantity);
            Console.WriteLine(p.ToString());

        }

    }
}
