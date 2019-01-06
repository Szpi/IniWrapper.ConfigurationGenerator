using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax
{
    public interface ISyntaxGeneratorFacade
    {
        UsingDirectiveSyntax GetUsingSyntax(string @using);
        PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, SyntaxKind syntaxKind);
        ClassDeclarationSyntax GetClassSyntax(string className);
        PropertyDeclarationSyntax GetListPropertyDeclarationSyntax(string propertyName, SyntaxKind syntaxKind);
        SyntaxList<AttributeListSyntax> GetAttributeSyntax(string section, string key);
        CompilationUnitSyntax GetCompilationUnitSyntax();
    }
}