using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.Visitor.ClassDeclarationVisitors
{
    public interface IClassDeclarationVisitor
    {
        ClassDeclarationSyntax Accept(ClassDeclarationSyntax compilationUnitSyntax, ClassToGenerate classToGenerate);
    }
}