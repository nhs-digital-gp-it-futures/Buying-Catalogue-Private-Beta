using System;

namespace Gif.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CrmEntityAttribute : Attribute
    {
        public virtual string Name
        {
            get
            {
                return name;
            }
        }

        private string name;

        public CrmEntityAttribute(string name)
        {
            this.name = name;
        }
    }
}
