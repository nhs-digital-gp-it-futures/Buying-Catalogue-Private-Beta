using System;

namespace Gif.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CrmEntityRelationAttribute : Attribute
    {
        public string Name
        {
            get
            {
                return name;
            }
        }

        private string name;

        public CrmEntityRelationAttribute(string name)
        {
            this.name = name;
        }
    }
}
