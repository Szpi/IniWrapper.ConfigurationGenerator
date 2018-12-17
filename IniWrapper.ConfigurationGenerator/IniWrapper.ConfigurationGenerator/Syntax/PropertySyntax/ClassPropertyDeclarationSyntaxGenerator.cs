using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
namespace IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax
{
    public class ClassPropertyDeclarationSyntaxGenerator
    {
        public PropertyDeclarationSyntax GetClassPropertyDeclarationSyntax(string iniLine)
        {
           return PropertyDeclaration(
                         IdentifierName(iniLine),
                         Identifier(
                             TriviaList(),
                             iniLine,
                             TriviaList(
                                 Space)))
                     .WithModifiers(
                         TokenList(
                             Token(
                                 TriviaList(),
                                 SyntaxKind.PublicKeyword,
                                 TriviaList(
                                     Space))))
                     .WithAccessorList(
                         AccessorList(
                                 List<AccessorDeclarationSyntax>(
                                     new AccessorDeclarationSyntax[]
                                     {
                                        AccessorDeclaration(
                                                SyntaxKind.GetAccessorDeclaration)
                                            .WithSemicolonToken(
                                                Token(SyntaxKind.SemicolonToken)),
                                        AccessorDeclaration(
                                                SyntaxKind.SetAccessorDeclaration)
                                            .WithSemicolonToken(
                                                Token(SyntaxKind.SemicolonToken))
                                     }))
                             .WithCloseBraceToken(
                                 Token(
                                     TriviaList(),
                                     SyntaxKind.CloseBraceToken,
                                     TriviaList(
                                         new[] { Space, LineFeed }))));
        }
    }
}