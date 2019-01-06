using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ClassDeclarationGenerators
{
    public interface IClassDeclarationGenerator
    {
        ClassDeclarationSyntax Accept(ClassDeclarationSyntax compilationUnitSyntax, ClassToGenerate classToGenerate);
    }
}