using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class DataDrawerFinder
{
    private static IReadOnlyDictionary<Type, ConstructorInfo> Constructors 
    {
        get
        {
            if (_constructors == null || _constructors.Count == 0)
            {
                var groupedTypes = Assembly
                    .GetAssembly(typeof(IDataDrawer<>))
                    .GetTypes()
                    .Where(x =>
                        !x.IsAbstract &&
                        x.IsClass &&
                        x.GetConstructor(Type.EmptyTypes) != null &&
                        x.GetInterfaces() is Type[] interfaces &&
                        interfaces.Length > 0 &&
                        interfaces.Any(y =>
                            y.Name == "IDataDrawer`1" &&
                            y.IsConstructedGenericType))
                    .GroupBy(
                        x => x,
                        x => x.GetInterfaces()
                            .Where(y => y.Name == "IDataDrawer`1")
                            .Select(y => y.GenericTypeArguments[0]));

                foreach (var grouping in groupedTypes)
                {
                    foreach (var types in grouping)
                    {
                        foreach (var type in types)
                        {
                            _constructors[type] = grouping.Key.GetConstructor(Type.EmptyTypes);
                        }
                    }
                }
            }

            return _constructors;
        }
    }
    private static Dictionary<Type, ConstructorInfo> _constructors = new Dictionary<Type, ConstructorInfo>();
    
    public static IDataDrawer<T> Find<T>()
    {
        Type type = typeof(T);
        if (Constructors.TryGetValue(type, out ConstructorInfo constructor))
        {
            if (constructor.Invoke(new object[0]) is IDataDrawer<T> dataDrawer)
            {
                return dataDrawer;
            }
        }

        Console.WriteLine($"[ERROR] Unable to find IDataDrawer for type {type}");
        return null;
    }
}