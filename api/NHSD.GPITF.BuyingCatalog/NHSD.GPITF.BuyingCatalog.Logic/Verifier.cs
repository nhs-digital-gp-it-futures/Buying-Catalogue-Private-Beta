using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public static class Verifier
  {
    public static void Verify(object obj)
    {
      if (obj == null)
      {
        return;
      }

      var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
      foreach (var prop in props)
      {
        if (prop.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)) &&
          prop.PropertyType != typeof(string))
        {
          foreach (var item in (IEnumerable)prop.GetValue(obj, null))
          {
            Verify(item);
          }
        }

        if (prop.PropertyType != typeof(string))
        {
          continue;
        }

        var required = prop.GetCustomAttribute<RequiredAttribute>();
        if (required != null && !required.AllowEmptyStrings)
        {
          var value = prop.GetValue(obj) as string;
          if (string.IsNullOrWhiteSpace(value))
          {
            throw new InvalidOperationException($"Value is null or whitespace:  {prop.ReflectedType}.{prop.Name}");
          }
        }
      }
    }
  }
}
