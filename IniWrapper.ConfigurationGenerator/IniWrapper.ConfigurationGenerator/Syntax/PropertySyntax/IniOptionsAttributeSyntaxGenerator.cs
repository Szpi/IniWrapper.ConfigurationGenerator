using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax
{
    public class IniOptionsAttributeSyntaxGenerator
    {
        private readonly bool _shouldAddAttribute;

        public IniOptionsAttributeSyntaxGenerator(bool shouldAddAttribute)
        {
            _shouldAddAttribute = shouldAddAttribute;
        }

        public PropertyDeclarationSyntax AddIniOptionsAttributeToProperty(string section, string key, PropertyDeclarationSyntax property)
        {
            if (!_shouldAddAttribute)
            {
                return property;
            }

            return property.WithAttributeLists(
                SyntaxFactory.SingletonList<AttributeListSyntax>(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(
                                    SyntaxFactory.IdentifierName("IniOptions"))
                                .WithArgumentList(
                                    SyntaxFactory.AttributeArgumentList(
                                        SyntaxFactory.SeparatedList<AttributeArgumentSyntax>(
                                            new SyntaxNodeOrToken[]
                                            {
                                                SyntaxFactory.AttributeArgument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            SyntaxFactory.Literal(section)))
                                                    .WithNameEquals(
                                                        SyntaxFactory.NameEquals(
                                                            SyntaxFactory.IdentifierName("Section"))),
                                                SyntaxFactory.Token(SyntaxKind.CommaToken),
                                                SyntaxFactory.AttributeArgument(
                                                        SyntaxFactory.LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            SyntaxFactory.Literal(key)))
                                                    .WithNameEquals(
                                                        SyntaxFactory.NameEquals(
                                                            SyntaxFactory.IdentifierName("Key")))
                                            })))))));
        }
    }
}