using Microsoft.CodeAnalysis.CSharp;

namespace IniWrapper.ConfigurationGenerator.Ini.Class
{
    public class PropertyDescriptor
    {
        public string Name { get;}
        public SyntaxKind SyntaxKind { get;}
        public SyntaxKind UnderlyingSyntaxKind { get; }

        public PropertyDescriptor(string name, SyntaxKind syntaxKind, SyntaxKind underlyingSyntaxKind)
        {
            Name = name;
            SyntaxKind = syntaxKind;
            UnderlyingSyntaxKind = underlyingSyntaxKind;
        }
    }
}