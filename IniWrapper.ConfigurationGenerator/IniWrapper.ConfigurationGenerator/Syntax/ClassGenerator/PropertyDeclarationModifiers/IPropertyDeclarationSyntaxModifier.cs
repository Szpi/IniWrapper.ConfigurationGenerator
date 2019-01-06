using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.PropertyDeclarationModifiers
{
    public interface IPropertyDeclarationSyntaxModifier
    {
        PropertyDeclarationSyntax Modify(ClassDeclarationSyntax classDeclarationSyntax, PropertyDeclarationSyntax propertyDeclarationSyntax);
    }
}