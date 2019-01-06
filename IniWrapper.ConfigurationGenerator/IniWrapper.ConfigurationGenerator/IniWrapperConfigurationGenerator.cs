using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.Ini;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System.Collections.Generic;
using System.IO.Abstractions;
using IniWrapper.ConfigurationGenerator.Syntax.Generators;

namespace IniWrapper.ConfigurationGenerator
{
    public class IniWrapperConfigurationGenerator : IIniWrapperConfigurationGenerator
    {
        private readonly IFileSystem _fileSystem;
        private readonly GeneratorConfiguration _generatorConfiguration;

        private readonly IReadOnlyList<ICompilationUnitGenerator> _syntaxVisitors;
        private readonly IIniFileAnalyzer _iniFileAnalyzer;

        public IniWrapperConfigurationGenerator(IReadOnlyList<ICompilationUnitGenerator> syntaxVisitors,
                                                IIniFileAnalyzer iniFileAnalyzer,
                                                IFileSystem fileSystem,
                                                GeneratorConfiguration generatorConfiguration)
        {
            _syntaxVisitors = syntaxVisitors;
            _iniFileAnalyzer = iniFileAnalyzer;
            _fileSystem = fileSystem;
            _generatorConfiguration = generatorConfiguration;
        }

        public void Generate()
        {
            var iniFileContext = _iniFileAnalyzer.AnalyzeIniFile();

            CreateOutputDictionary();

            foreach (var syntaxVisitor in _syntaxVisitors)
            {
                var syntaxesToGenerate = syntaxVisitor.Generate(iniFileContext);

                foreach (var (compilationUnitsSyntax, className) in syntaxesToGenerate)
                {
                    var generatedClass = FormatSyntax(compilationUnitsSyntax);
                    _fileSystem.File.WriteAllText(GenerateClassFilePath(className), generatedClass);
                }
            }
        }

        private void CreateOutputDictionary()
        {
            if (!_fileSystem.Directory.Exists(_generatorConfiguration.OutputFolder))
            {
                _fileSystem.Directory.CreateDirectory(_generatorConfiguration.OutputFolder);
            }
        }

        private static string FormatSyntax(CompilationUnitSyntax classSyntax)
        {
            var workspace = new AdhocWorkspace();
            var formattedSyntax = Formatter.Format(classSyntax, workspace);
            var generatedClass = formattedSyntax.ToFullString();

            return generatedClass;
        }

        private string GenerateClassFilePath(string sectionName)
        {
            return _fileSystem.Path.Combine(_generatorConfiguration.OutputFolder, $"{sectionName}.cs");
        }
    }
}