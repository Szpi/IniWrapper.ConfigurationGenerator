using System.Collections.Generic;
using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.Ini;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.Generators.MainClass
{
    public class MainClassCompilationUnitGenerator : ICompilationUnitGenerator
    {
        private readonly GeneratorConfiguration _generatorConfiguration;

        public MainClassCompilationUnitGenerator(GeneratorConfiguration generatorConfiguration)
        {
            _generatorConfiguration = generatorConfiguration;
        }

        public List<(CompilationUnitSyntax compilationUnitsSyntax, string className)> Accept(IniFileContext iniFileContext)
        {
            return null;
        }
    }
}