using System;

namespace Gif.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CrmFieldNameAttribute : Attribute
    {
        public string Name
        {
            get
            {
                return name;
            }
        }

        private string name;

        public CrmFieldNameAttribute(string name)
        {
            this.name = name;
        }

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CrmFieldNameDataBindAttribute : Attribute
    {
        public string Name
        {
            get
            {
                return name;
            }
        }

        private string name;


        public CrmFieldNameDataBindAttribute(string name)
        {
            this.name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CrmFieldEntityDataBindAttribute : Attribute
    {
        public string Name
        {
            get
            {
                return name;
            }
        }

        private string name;


        public CrmFieldEntityDataBindAttribute(string name)
        {
            this.name = name;
        }
    }
}
