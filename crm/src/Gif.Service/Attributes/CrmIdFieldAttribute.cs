using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gif.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CrmIdFieldAttribute : Attribute
    {
        public CrmIdFieldAttribute()
        {
        }
    }
}
