using System.Collections.Generic;
using IniWrapper.ConfigurationGenerator.Ini;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.Generators
{
    public interface ICompilationUnitGenerator
    {
        List<(CompilationUnitSyntax compilationUnitsSyntax, string className)> Accept(IniFileContext iniFileContext);
    }
}