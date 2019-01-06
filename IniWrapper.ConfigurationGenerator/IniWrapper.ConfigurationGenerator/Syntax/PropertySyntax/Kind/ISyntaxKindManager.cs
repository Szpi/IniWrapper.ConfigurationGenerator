using Microsoft.CodeAnalysis.CSharp;

namespace IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Kind
{
    public interface ISyntaxKindManager
    {
        (SyntaxKind syntaxKind, SyntaxKind underlyingSyntaxKind) GetSyntaxKind(string value);
    }
}