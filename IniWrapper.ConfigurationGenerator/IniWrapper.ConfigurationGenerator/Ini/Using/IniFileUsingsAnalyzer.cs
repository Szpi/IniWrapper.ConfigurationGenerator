using System.Collections.Generic;
using System.Linq;
using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp;

namespace IniWrapper.ConfigurationGenerator.Ini.Using
{
    public class IniFileUsingsAnalyzer : IIniFileUsingsAnalyzer
    {
        private const string CollectionsGenericUsing = "System.Collections.Generic";
        private const string IniWrapperUsing = "IniWrapper.Attribute";

        private readonly GeneratorConfiguration _generatorConfiguration;

        public IniFileUsingsAnalyzer(GeneratorConfiguration generatorConfiguration)
        {
            _generatorConfiguration = generatorConfiguration;
        }

        public IReadOnlyList<string> AnalyzeIniFileNecessaryUsings(IReadOnlyList<PropertyDescriptor> propertyDescriptors)
        {
            var necessaryUsings = new List<string>();
            AnalyzeIniWrapperUsing(necessaryUsings);

            if (propertyDescriptors.Any(x => x.SyntaxKind == SyntaxKind.List))
            {
                necessaryUsings.Add(CollectionsGenericUsing);
            }

            return necessaryUsings;
        }

        public IReadOnlyList<string> AnalyzeIniFileNecessaryUsings(IReadOnlyList<ClassToGenerate> complexTypeToGenerates)
        {
            var necessaryUsings = new List<string>();
            AnalyzeIniWrapperUsing(necessaryUsings);

            if (complexTypeToGenerates.Any())
            {
                necessaryUsings.Add(CollectionsGenericUsing);
            }

            return necessaryUsings;
        }

        private void AnalyzeIniWrapperUsing(List<string> necessaryUsings)
        {
            if (_generatorConfiguration.GenerateIniOptionAttribute || _generatorConfiguration.ImmutableConfiguration)
            {
                necessaryUsings.Add(IniWrapperUsing);
            }
        }
    }
}