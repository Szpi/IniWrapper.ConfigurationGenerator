using System.Collections.Generic;

namespace IniWrapper.ConfigurationGenerator.Ini.Class
{
    public class ClassToGenerate
    {
        public string ClassName { get;}
        public IReadOnlyList<PropertyDescriptor> PropertyDescriptors { get;}
        public IReadOnlyList<string> UsingsToGenerate { get; }

        public ClassToGenerate(string className, IReadOnlyList<PropertyDescriptor> propertyDescriptors, IReadOnlyList<string> usingsToGenerate)
        {
            ClassName = className;
            PropertyDescriptors = propertyDescriptors;
            UsingsToGenerate = usingsToGenerate;
        }
    }
}