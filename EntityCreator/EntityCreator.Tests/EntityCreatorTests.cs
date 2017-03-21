using System;
using System.Collections.Generic;
using System.Linq;
using Eurofins.BCAST.UnitTests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eurofins.BCAST.Service.UnitTests.EntityCreatorModule
{
  [TestClass]
  public class EntityCreatorTests
  {
    #region Complex Types
    private ComplexType CreatedComplex { get; set; }

    [TestInitialize]
    [TestCategory("complex")]
    public void InitComplex()
    {
      CreatedComplex = EntityCreator.CreateComplex<ComplexType>();
    }

    [TestCategory("complex")]
    [TestMethod]
    public void Complex_DateTime_When_Initialized_Has_Value()
    {
      Assert.IsInstanceOfType(CreatedComplex.DateTime, typeof(DateTime));
      Assert.IsNotNull(CreatedComplex.DateTime);
      Assert.IsTrue(CreatedComplex.DateTime != default(DateTime));
    }

    [TestCategory("complex")]
    [TestMethod]
    public void Complex_Number_When_Initialized_Has_Value()
    {
      Assert.IsInstanceOfType(CreatedComplex.Number, typeof(int));
      Assert.IsTrue(CreatedComplex.Number != default(int));
    }

    [TestCategory("complex")]
    [TestMethod]
    public void Complex_String_When_Initialized_Has_Value()
    {
      Assert.IsInstanceOfType(CreatedComplex.String, typeof(string));
      Assert.IsNotNull(CreatedComplex.String);
      Assert.IsTrue(CreatedComplex.String != string.Empty);
    }

    [TestCategory("complex")]
    [TestMethod]
    public void Complex_Bool_When_Initialized_Has_Value()
    {
      Assert.IsInstanceOfType(CreatedComplex.Boolean, typeof(bool));
      Assert.IsNotNull(CreatedComplex.Boolean);
    }

    [TestCategory("complex")]
    [TestMethod]
    public void Complex_Decimal_When_Initialized_Has_Value()
    {
      Assert.IsInstanceOfType(CreatedComplex.Decimal, typeof(decimal));
      Assert.IsNotNull(CreatedComplex.Decimal);
      Assert.IsTrue(CreatedComplex.Decimal != default(decimal));
    }

    [TestCategory("complex")]
    [TestMethod]
    public void Complex_Double_When_Initialized_Has_Value()
    {
      Assert.IsInstanceOfType(CreatedComplex.Double, typeof(double));
      Assert.IsNotNull(CreatedComplex.Double);
      Assert.IsTrue(Math.Abs(Math.Abs(CreatedComplex.Double - default(double)) - default(double)) > 0);
    }

    [TestCategory("complex")]
    [TestMethod]
    public void Complex_Byte_When_Initialized_Has_Value()
    {
      Assert.IsInstanceOfType(CreatedComplex.Byte, typeof(byte));
      Assert.IsNotNull(CreatedComplex.Byte);
      Assert.IsTrue(CreatedComplex.Decimal != default(byte));
    }

    [TestCategory("complex")]
    [TestMethod]
    public void Complex_NestedComplex_When_Initialized_Has_Value()
    {
      Assert.IsInstanceOfType(CreatedComplex.FirstComplex, typeof(FirstComplex));
      Assert.IsNotNull(CreatedComplex.FirstComplex.Byte);
      Assert.IsTrue(CreatedComplex.FirstComplex.Byte != default(byte));
    }

    [TestCategory("complex")]
    [TestMethod]
    public void Complex_SecondNestedComplex_When_Initialized_Has_Value()
    {
      Assert.IsInstanceOfType(CreatedComplex.FirstComplex.SecondComplex, typeof(SecondComplex));
      Assert.IsNotNull(CreatedComplex.FirstComplex.SecondComplex.Byte);
      Assert.IsTrue(CreatedComplex.FirstComplex.SecondComplex.Byte != default(byte));
    }

    [TestCategory("complex")]
    [TestMethod]
    public void Complex_ThirdNestedComplex_When_Initialized_Has_Value()
    {
      Assert.IsInstanceOfType(CreatedComplex.FirstComplex.SecondComplex.ThirdComplex, typeof(ThirdComplex));
      Assert.IsNotNull(CreatedComplex.FirstComplex.SecondComplex.ThirdComplex.Byte);
      Assert.IsTrue(CreatedComplex.FirstComplex.SecondComplex.ThirdComplex.Byte != default(byte));
    }

    [TestCategory("complex")]
    [TestMethod]
    public void ComplexList_When_Initialized_Has_Value()
    {
      var complexList = EntityCreator.CreateComplexList<ComplexType>();
      Assert.IsInstanceOfType(complexList, typeof(List<ComplexType>));
      Assert.IsTrue(complexList.Count > 0);
      Assert.IsNotNull(complexList.First().DateTime != default(DateTime));
    }

    [TestCategory("complex")]
    [TestMethod]
    public void ComplexList_When_InitializedWithCount_Has_CorrectCount()
    {
      var complexList = EntityCreator.CreateComplexList<ComplexType>(20);
      Assert.IsTrue(complexList.Count ==20);
    }

    [TestCategory("complex")]
    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void Complex_When_Simple_Through_Exception()
    {
      var created = EntityCreator.CreateComplex<int>();
    }

    #endregion

    #region Simple types
    [TestCategory("simple")]
    [TestMethod]
    public void SimpleInt_When_Initialized_Has_Value()
    {
      var created = EntityCreator.CreateSimple<int>();
      Assert.IsInstanceOfType(created, typeof(int));
    }

    [TestCategory("simple")]
    [TestMethod]
    public void SimpleString_When_Initialized_Has_Value()
    {
      var created = EntityCreator.CreateSimple<string>();
      Assert.IsInstanceOfType(created, typeof(string));
    }

    [TestCategory("simple")]
    [TestMethod]
    public void SimpleIntList_When_Initialized_Has_Value()
    {
      var created = EntityCreator.CreateSimpleList<int>();
      Assert.IsInstanceOfType(created, typeof(List<int>));
    }


    [TestCategory("simple")]
    [TestMethod]
    public void SimpleDateTimeList_When_Initialized_Has_Value()
    {
      var created = EntityCreator.CreateSimpleList<DateTime>();
      Assert.IsInstanceOfType(created, typeof(List<DateTime>));
    }

    [TestCategory("simple")]
    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void Simple_When_Complex_Through_Exception()
    {
      var created = EntityCreator.CreateSimple<ComplexType>();
    }

    [TestCategory("simple")]
    [TestMethod]
    [ExpectedException(typeof(NotSupportedException))]
    public void Simple_When_List_Through_Exception()
    {
      var created = EntityCreator.CreateSimple<List<int>>();
    }

    #endregion
  }

  #region Complex Model

  public class ComplexType
  {
    public int Number { get; set; }
    public string String { get; set; }
    public DateTime DateTime { get; set; }
    public bool Boolean { get; set; }
    public decimal Decimal { get; set; }
    public double Double { get; set; }
    public byte Byte { get; set; }
    public FirstComplex FirstComplex { get; set; }
  }

  public class FirstComplex
  {
    public int Number { get; set; }
    public string String { get; set; }
    public DateTime DateTime { get; set; }
    public bool Boolean { get; set; }
    public decimal Decimal { get; set; }
    public double Double { get; set; }
    public byte Byte { get; set; }
    public SecondComplex SecondComplex { get; set; }
  }

  public class SecondComplex
  {
    public int Number { get; set; }
    public string String { get; set; }
    public DateTime DateTime { get; set; }
    public bool Boolean { get; set; }
    public decimal Decimal { get; set; }
    public double Double { get; set; }
    public byte Byte { get; set; }
    public ThirdComplex ThirdComplex { get; set; }
  }

  public class ThirdComplex
  {
    public int Number { get; set; }
    public string String { get; set; }
    public DateTime DateTime { get; set; }
    public bool Boolean { get; set; }
    public decimal Decimal { get; set; }
    public double Double { get; set; }
    public byte Byte { get; set; }
  }

  #endregion
}