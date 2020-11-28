using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SadPumpkin.Util.Signals;

namespace Core.JSON
{
    public class NewtonsoftContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            // Don't serialize fields that can't be written back to
            if (!property.Writable)
            {
                property.ShouldSerialize = obj => false;
                return property;
            }

            // Don't serialize/deserialize Signals, what would that even mean.
            if (typeof(ISignal).IsAssignableFrom(property.PropertyType))
            {
                property.ShouldSerialize = obj => false;
                property.ShouldDeserialize = obj => false;
                return property;
            }
            
            // Only serialize collections if they are not empty.
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            {
                property.ShouldSerialize = obj =>
                {
                    IEnumerable enumerable = null;
                    switch (member)
                    {
                        case PropertyInfo propertyInfo:
                            enumerable = (IEnumerable) propertyInfo.GetValue(obj);
                            break;
                        case FieldInfo fieldInfo:
                            enumerable = (IEnumerable) fieldInfo.GetValue(obj);
                            break;
                    }

                    bool anyElements = enumerable?.GetEnumerator().MoveNext() ?? false;

                    return anyElements;
                };
                return property;
            }

            return property;
        }
    }
}