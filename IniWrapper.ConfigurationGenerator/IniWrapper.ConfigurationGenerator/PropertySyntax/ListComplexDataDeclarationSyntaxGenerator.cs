using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IniWrapper.ConfigurationGenerator.PropertySyntax
{
    public class ListComplexDataDeclarationSyntaxGenerator
    {
        public PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName)
        {
            return PropertyDeclaration(
                       GenericName(
                               Identifier("List"))
                           .WithTypeArgumentList(
                               TypeArgumentList(
                                   SingletonSeparatedList<TypeSyntax>(
                                       IdentifierName(propertyName)))),
                       Identifier(propertyName))
                   .WithModifiers(
                       TokenList(
                           Token(SyntaxKind.PublicKeyword)))
                   .WithAccessorList(
                       AccessorList(
                           List<AccessorDeclarationSyntax>(
                               new AccessorDeclarationSyntax[]{
                                   AccessorDeclaration(
                                           SyntaxKind.GetAccessorDeclaration)
                                       .WithSemicolonToken(
                                           Token(SyntaxKind.SemicolonToken)),
                                   AccessorDeclaration(
                                           SyntaxKind.SetAccessorDeclaration)
                                       .WithSemicolonToken(
                                           Token(SyntaxKind.SemicolonToken))})));
        }
    }
}