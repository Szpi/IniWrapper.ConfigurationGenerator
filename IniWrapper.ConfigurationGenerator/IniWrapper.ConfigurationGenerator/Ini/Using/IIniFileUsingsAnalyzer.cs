using System.Collections.Generic;
using IniWrapper.ConfigurationGenerator.Ini.Class;

namespace IniWrapper.ConfigurationGenerator.Ini.Using
{
    public interface IIniFileUsingsAnalyzer
    {
        IReadOnlyList<string> AnalyzeIniFileNecessaryUsings(IReadOnlyList<PropertyDescriptor> propertyDescriptors);
        IReadOnlyList<string> AnalyzeIniFileNecessaryUsings(IReadOnlyList<ClassToGenerate> complexTypeToGenerates);
    }
}