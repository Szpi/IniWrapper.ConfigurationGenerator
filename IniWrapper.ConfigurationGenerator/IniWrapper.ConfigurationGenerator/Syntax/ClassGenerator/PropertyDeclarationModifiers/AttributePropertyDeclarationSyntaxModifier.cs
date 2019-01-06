using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.PropertyDeclarationModifiers
{
    public class AttributePropertyDeclarationSyntaxModifier : IPropertyDeclarationSyntaxModifier
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;

        public AttributePropertyDeclarationSyntaxModifier(ISyntaxGeneratorFacade syntaxGeneratorFacade)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
        }
        public PropertyDeclarationSyntax Modify(ClassDeclarationSyntax classDeclarationSyntax, PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            var attributeSyntax = _syntaxGeneratorFacade.GetAttributeSyntax(classDeclarationSyntax.Identifier.Text, propertyDeclarationSyntax.Identifier.Text);
            return propertyDeclarationSyntax.WithAttributeLists(attributeSyntax);
        }
    }
}