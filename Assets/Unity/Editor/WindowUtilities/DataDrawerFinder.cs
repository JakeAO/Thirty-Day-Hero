using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class DataDrawerFinder
{
    private static readonly IReadOnlyDictionary<Type, ConstructorInfo> Constructors;

    static DataDrawerFinder()
    {
        Constructors = Assembly
            .GetAssembly(typeof(IDataDrawer))
            .GetTypes()
            .Where(x =>
                !x.IsAbstract &&
                x.IsClass &&
                typeof(IDataDrawer).IsAssignableFrom(x) &&
                x.GetConstructors().Any(y => y.GetParameters().Length == 1))
            .SelectMany(x => x.GetConstructors().Where(y => y.GetParameters().Length == 1))
            .Where(x => x != null)
            .ToDictionary(
                x => x.GetParameters()[0].ParameterType,
                x => x);
    }

    public static IDataDrawer Find<T>(T value)
    {
        Type type = typeof(T);
        if (Constructors.TryGetValue(type, out ConstructorInfo constructor))
        {
            if (constructor.Invoke(new object[1] {value}) is IDataDrawer dataDrawer)
            {
                return dataDrawer;
            }
        }

        Console.WriteLine($"[ERROR] Unable to find IDataDrawer for type {type}");
        return null;
    }
}