using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.UsingSyntax
{
    public class IniWrapperAttributeUsingSyntaxGenerator
    {
        public UsingDirectiveSyntax GetIniWrapperAttributeUsingSyntax()
        {
            return SyntaxFactory.UsingDirective(
                       SyntaxFactory.QualifiedName(
                           SyntaxFactory.IdentifierName("IniWrapper"),
                           SyntaxFactory.IdentifierName("Attribute")))
                   .WithUsingKeyword(
                       SyntaxFactory.Token(
                           SyntaxFactory.TriviaList(),
                           SyntaxKind.UsingKeyword,
                           SyntaxFactory.TriviaList(
                               SyntaxFactory.Space)))
                   .WithSemicolonToken(
                       SyntaxFactory.Token(
                           SyntaxFactory.TriviaList(),
                           SyntaxKind.SemicolonToken,
                           SyntaxFactory.TriviaList(
                               SyntaxFactory.LineFeed)));
        }
    }
}