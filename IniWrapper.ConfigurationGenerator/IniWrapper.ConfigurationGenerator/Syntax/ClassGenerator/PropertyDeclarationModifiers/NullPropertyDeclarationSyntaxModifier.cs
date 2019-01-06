using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.PropertyDeclarationModifiers
{
    public class NullPropertyDeclarationSyntaxModifier : IPropertyDeclarationSyntaxModifier
    {
        public PropertyDeclarationSyntax Modify(ClassDeclarationSyntax classDeclarationSyntax,
                                                PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            return propertyDeclarationSyntax;
        }
    }
}