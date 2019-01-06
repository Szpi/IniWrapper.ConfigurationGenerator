using IniWrapper.ConfigurationGenerator.Ini.Class;
using IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ClassDeclarationGenerators;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator
{
    public class ClassSyntaxGenerator : IClassToGenerateGenerator
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;
        private readonly IReadOnlyList<IClassDeclarationGenerator> _classDeclarationGenerators;
        private readonly string _namespace;

        public ClassSyntaxGenerator(ISyntaxGeneratorFacade syntaxGeneratorFacade, IReadOnlyList<IClassDeclarationGenerator> classDeclarationGenerators, string @namepsace)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
            _classDeclarationGenerators = classDeclarationGenerators;
            _namespace = @namepsace;
        }

        public CompilationUnitSyntax Generate(CompilationUnitSyntax compilationUnitSyntax, ClassToGenerate classToGenerate)
        {
            var classDeclarationSyntax = _syntaxGeneratorFacade.GetClassSyntax(classToGenerate.ClassName);
            classDeclarationSyntax = _classDeclarationGenerators
                                     .Aggregate(classDeclarationSyntax, (current, classDeclarationGenerator) => classDeclarationGenerator.Generate(current, classToGenerate));

            var @namespace = _syntaxGeneratorFacade.GetNamespace(_namespace);
            @namespace = @namespace.AddMembers(classDeclarationSyntax);
            return compilationUnitSyntax.AddMembers(@namespace);
        }
    }
}