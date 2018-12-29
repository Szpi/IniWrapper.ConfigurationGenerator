using System.Collections.Generic;

namespace IniWrapper.ConfigurationGenerator.Ini.Class
{
    public class ComplexTypeToGenerate
    {
        public string ClassName { get; }
        public IReadOnlyList<PropertyDescriptor> PropertyDescriptors { get; }
        public IReadOnlyList<string> UsingsToGenerate { get; }

        public ComplexTypeToGenerate(string className, IReadOnlyList<PropertyDescriptor> propertyDescriptors, IReadOnlyList<string> usingsToGenerate)
        {
            ClassName = className;
            PropertyDescriptors = propertyDescriptors;
            UsingsToGenerate = usingsToGenerate;
        }
    }
}