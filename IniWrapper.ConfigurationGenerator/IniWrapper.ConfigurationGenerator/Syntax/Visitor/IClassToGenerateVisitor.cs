using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.Visitor
{
    public interface IClassToGenerateVisitor
    {
        CompilationUnitSyntax Accept(CompilationUnitSyntax compilationUnitSyntax, ClassToGenerate classToGenerate);
    }
}