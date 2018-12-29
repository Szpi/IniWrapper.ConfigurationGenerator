using IniWrapper.ConfigurationGenerator.IniParser;
using IniWrapper.ConfigurationGenerator.Section;
using System.Collections.Generic;
using IniWrapper.ConfigurationGenerator.Ini.Class;
using IniWrapper.ConfigurationGenerator.Ini.Using;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Kind;

namespace IniWrapper.ConfigurationGenerator.Ini
{
    public class IniFileAnalyzer : IIniFileAnalyzer
    {
        private readonly IIniParserWrapper _iniParserWrapper;
        private readonly ISectionsAnalyzer _sectionsAnalyzer;

        private readonly ISyntaxKindManager _syntaxKindManager;
        private readonly IIniFileUsingsAnalyzer _iniFileUsingsAnalyzer;

        public IniFileAnalyzer(IIniParserWrapper iniParserWrapper,
                               ISectionsAnalyzer sectionsAnalyzer,
                               ISyntaxKindManager syntaxKindManager, 
                               IIniFileUsingsAnalyzer iniFileUsingsAnalyzer)
        {
            _iniParserWrapper = iniParserWrapper;
            _sectionsAnalyzer = sectionsAnalyzer;
            _syntaxKindManager = syntaxKindManager;
            _iniFileUsingsAnalyzer = iniFileUsingsAnalyzer;
        }

        public IniFileContext AnalyzeIniFile()
        {
            var sectionNames = _iniParserWrapper.GetSectionsFromFile();
            var (separateSections, complexDataSections) = _sectionsAnalyzer.AnalyzeSections(sectionNames);

            var classesToGenerate = AnalyzeSeparateSections(separateSections);
            var complexClassesToGenerate = AnalyzeComplexDataSections(complexDataSections);
            var usingsToGenerate = _iniFileUsingsAnalyzer.AnalyzeIniFileNecessaryUsings(complexClassesToGenerate);


            return new IniFileContext(classesToGenerate, complexClassesToGenerate, usingsToGenerate);
        }

        private List<ComplexTypeToGenerate> AnalyzeComplexDataSections(List<string> complexDataSections)
        {
            var complexTypeToGenerates = new List<ComplexTypeToGenerate>();

            foreach (var complexSection in complexDataSections)
            {
                var iniValues = _iniParserWrapper.ReadAllFromSection(complexSection);
                var propertiesDescriptor = AnalyzeIniValues(iniValues);
                var necessaryUsings = _iniFileUsingsAnalyzer.AnalyzeIniFileNecessaryUsings(propertiesDescriptor);
                var classToGenerate = new ComplexTypeToGenerate(complexSection, propertiesDescriptor, necessaryUsings);
                complexTypeToGenerates.Add(classToGenerate);
            }

            return complexTypeToGenerates;
        }

        private List<PropertyDescriptor> AnalyzeIniValues(IDictionary<string, string> iniValues)
        {
            var propertiesDescriptor = new List<PropertyDescriptor>();
            foreach (var iniValue in iniValues)
            {
                var syntaxKind = _syntaxKindManager.GetSyntaxKind(iniValue.Value);
                var property = new PropertyDescriptor(iniValue.Key, syntaxKind);
                propertiesDescriptor.Add(property);
            }

            return propertiesDescriptor;
        }

        private List<ClassToGenerate> AnalyzeSeparateSections(List<string> separateSections)
        {
            var classesToGenerate = new List<ClassToGenerate>();

            foreach (var separateSection in separateSections)
            {
                var iniValues = _iniParserWrapper.ReadAllFromSection(separateSection);
                var propertiesDescriptor = AnalyzeIniValues(iniValues);
                var necessaryUsings = _iniFileUsingsAnalyzer.AnalyzeIniFileNecessaryUsings(propertiesDescriptor);
                var classToGenerate = new ClassToGenerate(separateSection, propertiesDescriptor, necessaryUsings);
                classesToGenerate.Add(classToGenerate);
            }

            return classesToGenerate;
        }
    }
}