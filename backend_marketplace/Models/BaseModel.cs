using System.Reflection;

namespace backend_marketplace.Models
{
    public class BaseModel
    {
        public Dictionary<string, object> ToDictionary()
        {
            return this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(this, null));
        }
    }
}
