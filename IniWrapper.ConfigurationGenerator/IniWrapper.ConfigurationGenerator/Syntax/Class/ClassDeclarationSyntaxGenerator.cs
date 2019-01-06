using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IniWrapper.ConfigurationGenerator.Syntax.Class
{
    public class ClassDeclarationSyntaxGenerator
    {
        public ClassDeclarationSyntax GetClassSyntax(string className)
        {
            return ClassDeclaration(Identifier(TriviaList(), className, 
                                                    TriviaList(new[] { Space, LineFeed })))
                                                           .WithKeyword(Token(TriviaList(),
                                                                              SyntaxKind.ClassKeyword,
                                                                              TriviaList(
                                                                                  Space)))
                                                           .WithOpenBraceToken(
                                                               Token(
                                                                   TriviaList(),
                                                                   SyntaxKind.OpenBraceToken,
                                                                   TriviaList(
                                                                       new[] { Space, LineFeed })));
        }
    }
}