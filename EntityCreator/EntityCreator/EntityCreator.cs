using System;
using System.Linq;
using System.Reflection;
using Spackle;

namespace EntityCreator
{
    public static class EntityCreator
    {
        public static T Create<T>()
            where T : new()
        {
            var generator = new RandomObjectGenerator();
            var entity = new T();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(_ => _.CanWrite).ToList();
            properties.ForEach(_ =>
            {
                try
                {
                    _.SetValue(entity,
                        typeof(RandomObjectGenerator)
                            .GetMethod("Generate", Type.EmptyTypes)
                            .MakeGenericMethod(new[] { _.PropertyType })
                            .Invoke(generator, null));
                }
                catch (TargetInvocationException)
                {
                }
            });
            return entity;
        }

        public static T Create<T>(Action<T> modifier)
            where T : new()
        {
            var entity = Create<T>();
            modifier(entity);
            return entity;
        }
    }
}
