using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax
{
    public interface ISyntaxGeneratorFacade
    {
        PropertyDeclarationSyntax AddIniOptionsAttributeToProperty(string section, string key, PropertyDeclarationSyntax property);
        UsingDirectiveSyntax GetCollectionsGenericUsingSyntax();
        PropertyDeclarationSyntax GetComplexListPropertyDeclarationSyntax(string propertyName);
        UsingDirectiveSyntax GetIniWrapperAttributeUsingSyntax();
        PropertyDeclarationSyntax GetListPropertyDeclarationSyntax(string propertyName, string iniValue);
        PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, string iniValue, SyntaxKind syntaxKind);
        PropertyDeclarationSyntax GetClassPropertyDeclarationSyntax(string iniLine);

        CompilationUnitSyntax GetClassSyntax(string sectionName,
                                             SyntaxList<MemberDeclarationSyntax> members,
                                             SyntaxList<UsingDirectiveSyntax> usingDirectiveSyntax,
                                             string nameSpace);
    }
}