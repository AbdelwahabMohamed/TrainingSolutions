using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityCreator.Tests
{
    [TestClass]
    public class EntityCreatorTests
    {
        public ComplexType CreatedComplex { get; set; }

        [TestInitialize]
        public void Init()
        {
            CreatedComplex = EntityCreator.Create<ComplexType>();
        }

        [TestMethod]
        public void Primitive_Initialized()
        {
            var created = EntityCreator.Create<int>();
            Assert.IsInstanceOfType(created, typeof(int));
        }

        [TestMethod]
        public void Complex_DateTime_Initialized()
        {
            Assert.IsInstanceOfType(CreatedComplex.DateTime, typeof(DateTime));
            Assert.IsNotNull(CreatedComplex.DateTime);
            Assert.IsTrue(CreatedComplex.DateTime != default(DateTime));
        }

        [TestMethod]
        public void Complex_Number_Initialized()
        {
            Assert.IsInstanceOfType(CreatedComplex.Number, typeof(int));
            Assert.IsTrue(CreatedComplex.Number != default(int));
        }

        [TestMethod]
        public void Complex_String_Initialized()
        {
            Assert.IsInstanceOfType(CreatedComplex.String, typeof(string));
            Assert.IsNotNull(CreatedComplex.String);
            Assert.IsTrue(CreatedComplex.String != string.Empty);
        }

        [TestMethod]
        public void Complex_Bool_Initialized()
        {
            Assert.IsInstanceOfType(CreatedComplex.Boolean, typeof(bool));
            Assert.IsNotNull(CreatedComplex.Boolean);
        }

        [TestMethod]
        public void Complex_Decimal_Initialized()
        {
            Assert.IsInstanceOfType(CreatedComplex.Decimal, typeof(decimal));
            Assert.IsNotNull(CreatedComplex.Decimal);
            Assert.IsTrue(CreatedComplex.Decimal != default(decimal));
        }
        public void Complex_Double_Initialized()
        {
            Assert.IsInstanceOfType(CreatedComplex.Double, typeof(double));
            Assert.IsNotNull(CreatedComplex.Double);
            Assert.IsTrue(Math.Abs(CreatedComplex.Double - default(double)) > 1);
        }
        public void Complex_Byte_Initialized()
        {
            Assert.IsInstanceOfType(CreatedComplex.Byte, typeof(byte));
            Assert.IsNotNull(CreatedComplex.Byte);
            Assert.IsTrue(CreatedComplex.Decimal != default(byte));
        }
    }

    public class ComplexType
    {
        public int Number { get; set; }
        public string String { get; set; }
        public DateTime DateTime { get; set; }
        public bool Boolean { get; set; }
        public decimal Decimal { get; set; }
        public double Double { get; set; }
        public byte Byte { get; set; }
    }
}