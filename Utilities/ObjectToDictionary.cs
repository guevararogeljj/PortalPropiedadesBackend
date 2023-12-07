using System.Collections;
using System.Reflection;

namespace Utilities
{
    public class ObjectToDictionary
    {

        public static Dictionary<string, object> Generate<T>(T obj)
        {
            if (typeof(IDictionary).IsAssignableFrom(obj.GetType()))
            {
                return obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
              .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null));

            }

            return new Dictionary<string, object>();
        }
    }
}
