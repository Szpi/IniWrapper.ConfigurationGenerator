using Microsoft.CodeAnalysis.CSharp;

namespace IniWrapper.ConfigurationGenerator.Ini.Class
{
    public class PropertyDescriptor
    {
        public string Name { get;}
        public SyntaxKind SyntaxKind { get;}

        public PropertyDescriptor(string name, SyntaxKind syntaxKind)
        {
            Name = name;
            SyntaxKind = syntaxKind;
        }
    }
}