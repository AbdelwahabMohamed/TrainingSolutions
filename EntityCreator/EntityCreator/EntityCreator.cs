using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Spackle;

namespace Eurofins.BCAST.UnitTests.Data
{
  public static class EntityCreator
  {
    #region private

    private static bool IsComplexType(Type propertyType)
    {
      return !propertyType.IsPrimitive && propertyType != typeof(string) &&
             propertyType != typeof(decimal) && propertyType != typeof(DateTime) &&
             !propertyType.IsArray && !propertyType.IsGenericType;
    }

    private static void CreateNested<T>(T entity, PropertyInfo property) where T : new()
    {
      var nestedEntity = Activator.CreateInstance(property.PropertyType);

      var nestedProperties = property.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(_ => _.CanWrite).ToList();

      nestedProperties.ForEach(nestedProperty =>
      {
        var propertyType = nestedProperty.PropertyType;
        if (IsComplexType(propertyType))
        {
          if (propertyType.GetConstructor(Type.EmptyTypes) != null)
          {
            CreateNested(nestedEntity, nestedProperty);
          }
        }
        else
        {
          SetValueInEntity(nestedProperty, nestedEntity);
          //nestedProperty.SetValue(nestedEntity, typeof(RandomObjectGenerator).GetMethod("Generate", Type.EmptyTypes).MakeGenericMethod(nestedProperty.PropertyType).Invoke(Generator, null));
        }
      });
      property.SetValue(entity, nestedEntity);
    }

    private static void SetValueInEntity<T>(PropertyInfo property, T entity) where T : new()
    {
      property.SetValue(entity, typeof(RandomObjectGenerator).GetMethod("Generate", Type.EmptyTypes).MakeGenericMethod(property.PropertyType).Invoke(Generator, null));
    }
    #endregion

    #region members && constructor

    static EntityCreator()
    {
      Generator = new RandomObjectGenerator();
    }

    private static RandomObjectGenerator Generator { get; }

    #endregion

    #region public methods
    /// <summary>
    /// Creating a random simple property
    /// </summary>
    /// <typeparam name="T">values type or string, DateTime, decimal</typeparam>
    /// <returns>randomly generated value of type T</returns>
    public static T CreateSimple<T>()
    {
      if (IsComplexType(typeof(T))) throw new NotSupportedException($"{nameof(T)} is not a simple type");

      var generated = Generator.Generate<T>();
      return generated;
    }
    /// <summary>
    /// Creating a random complex object, this method will ignore collections , yet will randomize
    /// the values for all primitive properties and all primitive properties of the nested complex members
    /// </summary>
    /// <typeparam name="T">complex type to be randmoly generated</typeparam>
    /// <returns>T  instance</returns>
    public static T CreateComplex<T>() where T : new()
    {
      if (!IsComplexType(typeof(T))) throw new NotSupportedException($"{nameof(T)} is not a complex type");

      var entity = new T();
      var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(_ => _.CanWrite).ToList();

      properties.ForEach(property =>
      {
        var propertyType = property.PropertyType;

        if (IsComplexType(propertyType))
        {
          if (propertyType.GetConstructor(Type.EmptyTypes) != null)
          {
            CreateNested(entity, property);
          }
        }
        else
        {
          SetValueInEntity(property, entity);
        }
      });

      return entity;
    }

    public static List<T> CreateSimpleList<T>(int count = 10)
    {
      var returnValue = new List<T>();
      for (var i = 0; i < count; i++)
      {
        returnValue.Add(CreateSimple<T>());
      }

      return returnValue;
    }

    public static List<T> CreateComplexList<T>(int count = 10) where T : new()
    {
      var returnValue = new List<T>();

      for (var i = 0; i < count; i++)
      {
        returnValue.Add(CreateComplex<T>());
      }

      return returnValue;
    }

    #endregion
  }
}