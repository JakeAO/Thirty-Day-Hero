using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Abilities;

namespace Unity.Editor.WindowUtilities
{
    public static class ImplementationFinder
    {
        private static readonly Dictionary<Type, object> CachedImplementations = new Dictionary<Type, object>();

        public static IReadOnlyDictionary<Type, Func<T>> Find<T>(Type assemblySearchType = null)
        {
            Type type = typeof(T);
            if (CachedImplementations.TryGetValue(type, out object objValue) &&
                objValue is IReadOnlyDictionary<Type, Func<T>> value)
            {
                return value;
            }

            IEnumerable<Type> baseImplementingTypes = Assembly
                .GetAssembly(type)
                .GetTypes()
                .Where(x =>
                    !x.IsAbstract &&
                    type.IsAssignableFrom(x) &&
                    x.GetConstructor(Type.EmptyTypes) != null);

            Type searchType = assemblySearchType ?? typeof(IAbility);
            IEnumerable<Type> searchImplementingTypes = Assembly
                .GetAssembly(searchType)
                .GetTypes()
                .Where(x =>
                    !x.IsAbstract &&
                    type.IsAssignableFrom(x) &&
                    x.GetConstructor(Type.EmptyTypes) != null);

            Dictionary<Type, Func<T>> typeConstructors = new Dictionary<Type, Func<T>>();
            foreach (Type implementingType in baseImplementingTypes.Union(searchImplementingTypes))
            {
                if (!typeConstructors.ContainsKey(implementingType))
                {
                    ConstructorInfo constructor = implementingType.GetConstructor(Type.EmptyTypes);
                    typeConstructors[implementingType] = () => (T) constructor.Invoke(new object[0]);
                }
            }

            CachedImplementations[type] = typeConstructors;
            return typeConstructors;
        }
    }
}