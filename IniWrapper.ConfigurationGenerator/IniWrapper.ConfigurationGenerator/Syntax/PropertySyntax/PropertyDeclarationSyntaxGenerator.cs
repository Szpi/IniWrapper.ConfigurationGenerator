using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax
{
    public class PropertyDeclarationSyntaxGenerator
    {
        public PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, string iniValue, SyntaxKind syntaxKind)
        {
            return SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(),
                                syntaxKind,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Space))),
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
                                                SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                        SyntaxFactory.AccessorDeclaration(
                                                SyntaxKind.SetAccessorDeclaration)
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