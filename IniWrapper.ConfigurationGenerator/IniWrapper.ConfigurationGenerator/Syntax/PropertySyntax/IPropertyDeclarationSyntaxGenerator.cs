using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax
{
    public interface IPropertyDeclarationSyntaxGenerator
    {
        PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, SyntaxKind syntaxKind);
    }
}