using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Immutable
{
    public class ImmutablePropertyDeclarationSyntaxGenerator : IPropertyDeclarationSyntaxGenerator
    {
        public PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, SyntaxKind syntaxKind)
        {
            var typeSyntax = ListPropertyDeclarationSyntaxGenerator.GetTypeSyntax(propertyName, syntaxKind);
            return SyntaxFactory.PropertyDeclaration(typeSyntax,
                        SyntaxFactory.Identifier(
                            SyntaxFactory.TriviaList(),
                            propertyName,
                            SyntaxFactory.TriviaList(
                                SyntaxFactory.Space)))
                    .WithModifiers(
                        SyntaxFactory.TokenList(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.PublicKeyword,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Space))))
                    .WithAccessorList(
                        SyntaxFactory.AccessorList(
                                SyntaxFactory.List<AccessorDeclarationSyntax>(
                                    new AccessorDeclarationSyntax[]
                                    {
                                        SyntaxFactory.AccessorDeclaration(
                                                SyntaxKind.GetAccessorDeclaration)
                                            .WithSemicolonToken(
                                                SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                                    }))
                            .WithCloseBraceToken(
                                SyntaxFactory.Token(
                                    SyntaxFactory.TriviaList(),
                                    SyntaxKind.CloseBraceToken,
                                    SyntaxFactory.TriviaList(
                                        new[] { SyntaxFactory.Space, SyntaxFactory.LineFeed }))));
        }
    }
}