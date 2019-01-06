using IniWrapper.ConfigurationGenerator.IniParser;
using IniWrapper.ConfigurationGenerator.Section;
using System.Collections.Generic;
using IniWrapper.ConfigurationGenerator.Ini.Class;
using IniWrapper.ConfigurationGenerator.Ini.Using;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Kind;
using Microsoft.CodeAnalysis.CSharp;

namespace IniWrapper.ConfigurationGenerator.Ini
{
    public class IniFileAnalyzer : IIniFileAnalyzer
    {
        private readonly IIniParserWrapper _iniParserWrapper;
        private readonly ISectionsAnalyzer _sectionsAnalyzer;

        private readonly ISyntaxKindManager _syntaxKindManager;
        private readonly IIniFileUsingsAnalyzer _iniFileUsingsAnalyzer;
        private readonly string _mainClassName;

        public IniFileAnalyzer(IIniParserWrapper iniParserWrapper,
                               ISectionsAnalyzer sectionsAnalyzer,
                               ISyntaxKindManager syntaxKindManager, 
                               IIniFileUsingsAnalyzer iniFileUsingsAnalyzer, 
                               string mainClassName)
        {
            _iniParserWrapper = iniParserWrapper;
            _sectionsAnalyzer = sectionsAnalyzer;
            _syntaxKindManager = syntaxKindManager;
            _iniFileUsingsAnalyzer = iniFileUsingsAnalyzer;
            _mainClassName = mainClassName;
        }

        public IniFileContext AnalyzeIniFile()
        {
            var sectionNames = _iniParserWrapper.GetSectionsFromFile();
            var (separateSections, complexDataSections) = _sectionsAnalyzer.AnalyzeSections(sectionNames);

            var classesToGenerate = AnalyzeSeparateSections(separateSections);
            var complexClassesToGenerate = AnalyzeComplexDataSections(complexDataSections);
            var usingsToGenerate = _iniFileUsingsAnalyzer.AnalyzeIniFileNecessaryUsings(complexClassesToGenerate);

            var propertiesInMainClass = AnalyzePropertiesInMainClass(classesToGenerate, complexClassesToGenerate);
            var mainClassToGenerate = new ClassToGenerate(_mainClassName, propertiesInMainClass, usingsToGenerate);

            classesToGenerate.Add(mainClassToGenerate);
            return new IniFileContext(classesToGenerate, complexClassesToGenerate);
        }

        private IReadOnlyList<PropertyDescriptor> AnalyzePropertiesInMainClass(List<ClassToGenerate> classesToGenerate, List<ClassToGenerate> complexClassesToGenerate)
        {
            var propertiesDescriptor = new List<PropertyDescriptor>();
            foreach (var classToGenerate in classesToGenerate)
            {
                var propertyDescriptor = new PropertyDescriptor(classToGenerate.ClassName, SyntaxKind.ClassDeclaration, SyntaxKind.None);

                propertiesDescriptor.Add(propertyDescriptor);
            }

            foreach (var classToGenerate in complexClassesToGenerate)
            {
                var propertyDescriptor = new PropertyDescriptor(classToGenerate.ClassName, SyntaxKind.List, SyntaxKind.ClassDeclaration);

                propertiesDescriptor.Add(propertyDescriptor);
            }

            return propertiesDescriptor;
        }

        private List<ClassToGenerate> AnalyzeComplexDataSections(List<(string className, string firstSectionInIniFile)> complexDataSections)
        {
            var complexTypeToGenerates = new List<ClassToGenerate>();

            foreach (var (complexClassName, firstIniSection) in complexDataSections)
            {
                var iniValues = _iniParserWrapper.ReadAllFromSection(firstIniSection);
                var propertiesDescriptor = AnalyzeIniValues(iniValues);
                var necessaryUsings = _iniFileUsingsAnalyzer.AnalyzeIniFileNecessaryUsings(propertiesDescriptor);
                var classToGenerate = new ClassToGenerate(complexClassName, propertiesDescriptor, necessaryUsings);
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
                var property = new PropertyDescriptor(iniValue.Key, syntaxKind.syntaxKind, syntaxKind.underlyingSyntaxKind);
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