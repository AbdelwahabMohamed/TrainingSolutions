using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityCreator;
using System;
using System.Diagnostics;

namespace EntityCreator.Tests
{
    [TestClass]
    public class EntityCreatorTests
    {

        [TestMethod]
        public void Primitive_Initialized()
        {
            var created = EntityCreator.Create<int>();
            Assert.IsInstanceOfType(created, typeof(int));
        }

        [TestMethod]
        public void Complex_Initialized()
        {
            var createdComplex = EntityCreator.Create<ComplexType>();
            Assert.IsInstanceOfType(createdComplex.DateTime, typeof(DateTime));
            Assert.IsNotNull(createdComplex.DateTime);
            Assert.IsTrue(createdComplex.Number != 0);            
        }
    }

    public class ComplexType
    {
        public int Number { get; set; }
        public string String { get; set; }
        public DateTime DateTime { get; set; }
    }
}
