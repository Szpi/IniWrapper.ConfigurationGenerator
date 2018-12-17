using IniWrapper.ConfigurationGenerator.Syntax.Class;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax;
using IniWrapper.ConfigurationGenerator.Syntax.UsingSyntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax
{
    public class SyntaxGeneratorFacade : ISyntaxGeneratorFacade
    {
        private readonly PropertyDeclarationSyntaxGenerator _propertyDeclarationSyntaxGenerator;
        private readonly ListPropertyDeclarationSyntaxGenerator _listPropertyDeclarationSyntaxGenerator;
        private readonly ListComplexDataDeclarationSyntaxGenerator _listComplexDataDeclarationSyntaxGenerator;
        private readonly IniOptionsAttributeSyntaxGenerator _iniOptionsAttributeSyntaxGenerator;
        private readonly CollectionsGenericUsingSyntaxGenerator _collectionsGenericUsingSyntaxGenerator;
        private readonly IniWrapperAttributeUsingSyntaxGenerator _iniWrapperAttributeUsingSyntaxGenerator;
        private readonly ClassPropertyDeclarationSyntaxGenerator _classPropertyDeclarationSyntaxGenerator;
        private readonly ClassSyntaxGenerator _classSyntaxGenerator;

        public SyntaxGeneratorFacade(IniOptionsAttributeSyntaxGenerator iniOptionsAttributeSyntaxGenerator,
                                      ListComplexDataDeclarationSyntaxGenerator listComplexDataDeclarationSyntaxGenerator,
                                      ListPropertyDeclarationSyntaxGenerator listPropertyDeclarationSyntaxGenerator,
                                      PropertyDeclarationSyntaxGenerator propertyDeclarationSyntaxGenerator,
                                      IniWrapperAttributeUsingSyntaxGenerator iniWrapperAttributeUsingSyntaxGenerator,
                                      CollectionsGenericUsingSyntaxGenerator collectionsGenericUsingSyntaxGenerator, 
                                      ClassPropertyDeclarationSyntaxGenerator classPropertyDeclarationSyntaxGenerator, 
                                      ClassSyntaxGenerator classSyntaxGenerator)
        {
            _iniOptionsAttributeSyntaxGenerator = iniOptionsAttributeSyntaxGenerator;
            _listComplexDataDeclarationSyntaxGenerator = listComplexDataDeclarationSyntaxGenerator;
            _listPropertyDeclarationSyntaxGenerator = listPropertyDeclarationSyntaxGenerator;
            _propertyDeclarationSyntaxGenerator = propertyDeclarationSyntaxGenerator;
            _iniWrapperAttributeUsingSyntaxGenerator = iniWrapperAttributeUsingSyntaxGenerator;
            _collectionsGenericUsingSyntaxGenerator = collectionsGenericUsingSyntaxGenerator;
            _classPropertyDeclarationSyntaxGenerator = classPropertyDeclarationSyntaxGenerator;
            _classSyntaxGenerator = classSyntaxGenerator;
        }

        public UsingDirectiveSyntax GetIniWrapperAttributeUsingSyntax()
        {
            return _iniWrapperAttributeUsingSyntaxGenerator.GetIniWrapperAttributeUsingSyntax();
        }

        public UsingDirectiveSyntax GetCollectionsGenericUsingSyntax()
        {
            return _collectionsGenericUsingSyntaxGenerator.GetCollectionsGenericUsingSyntax();
        }

        public PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, string iniValue,
                                                                      SyntaxKind syntaxKind)
        {
            return _propertyDeclarationSyntaxGenerator.GetPropertyDeclarationSyntax(propertyName, iniValue, syntaxKind);
        }

        public PropertyDeclarationSyntax GetClassPropertyDeclarationSyntax(string iniLine)
        {
            return _classPropertyDeclarationSyntaxGenerator.GetClassPropertyDeclarationSyntax(iniLine);
        }

        public PropertyDeclarationSyntax GetListPropertyDeclarationSyntax(string propertyName, string iniValue)
        {
            return _listPropertyDeclarationSyntaxGenerator.GetPropertyDeclarationSyntax(propertyName, iniValue);
        }

        public PropertyDeclarationSyntax GetComplexListPropertyDeclarationSyntax(string propertyName)
        {
            return _listComplexDataDeclarationSyntaxGenerator.GetPropertyDeclarationSyntax(propertyName);
        }

        public PropertyDeclarationSyntax AddIniOptionsAttributeToProperty(string section, string key, PropertyDeclarationSyntax property)
        {
            return _iniOptionsAttributeSyntaxGenerator.AddIniOptionsAttributeToProperty(section, key, property);
        }

        public CompilationUnitSyntax GetClassSyntax(string sectionName,
                                                    SyntaxList<MemberDeclarationSyntax> members,
                                                    SyntaxList<UsingDirectiveSyntax> usingDirectiveSyntax,
                                                    string nameSpace)
        {
            return _classSyntaxGenerator.GetClassSyntax(sectionName, members, usingDirectiveSyntax, nameSpace);
        }
    }
}