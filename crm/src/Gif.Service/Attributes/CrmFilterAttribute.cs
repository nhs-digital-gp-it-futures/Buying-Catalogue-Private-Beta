using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gif.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CrmFilterAttribute : Attribute
    {
        public virtual string Name
        {
            get { return name; }
        }

        public virtual string FilterName
        {
            get { return filterName; }
            set { filterName = value; }
        }

        public virtual string FilterValue
        {
            get { return filterValue; }
            set { filterValue = value; }
        }

        public virtual bool? QuotesRequired
        {
            get { return quotesRequired; }
            set { quotesRequired = value; }
        }

        public virtual bool? MultiConditional
        {
            get { return quotesRequired; }
            set { quotesRequired = value; }
        }

        private string name;
        private string filterName;
        private string filterValue;
        private bool? quotesRequired;
        private bool? multiConditionl;

        public CrmFilterAttribute(string name)
        {
            this.name = name;
        }
    }
    
}
