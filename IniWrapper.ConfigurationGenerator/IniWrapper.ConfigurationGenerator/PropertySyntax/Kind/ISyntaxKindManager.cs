using Microsoft.CodeAnalysis.CSharp;

namespace IniWrapper.ConfigurationGenerator.PropertySyntax.Kind
{
    public interface ISyntaxKindManager
    {
        SyntaxKind GetSyntaxKind(string value);
    }
}