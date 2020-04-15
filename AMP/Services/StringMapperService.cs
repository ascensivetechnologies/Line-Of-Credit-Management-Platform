using AMP.Models;
using System.Linq;

namespace AMP.Services
{
    public class StringMapperService
    {
        public static string GetValue(string key)
        {
            AMPEntities aMPEntities = new AMPEntities();
            var value = aMPEntities.TBL_String_Mapper.Where(m => m.Key == key).Select(m => m.Value).FirstOrDefault();
            value = string.IsNullOrEmpty(value) ? "" : value;
            return value;
        }
    }
}