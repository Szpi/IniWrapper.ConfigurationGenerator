using System.Collections.Generic;
using IniWrapper.ConfigurationGenerator.Ini.Class;
using IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ClassDeclarationGenerators;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator
{
    public class ClassSyntaxGenerator : IClassToGenerateGenerator
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;
        private readonly IReadOnlyList<IClassDeclarationGenerator> _classDeclarationVisitors;

        public ClassSyntaxGenerator(ISyntaxGeneratorFacade syntaxGeneratorFacade, IReadOnlyList<IClassDeclarationGenerator> classDeclarationVisitors)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
            _classDeclarationVisitors = classDeclarationVisitors;
        }

        public CompilationUnitSyntax Accept(CompilationUnitSyntax compilationUnitSyntax, ClassToGenerate classToGenerate)
        {
            var classDeclarationSyntax = _syntaxGeneratorFacade.GetClassSyntax(classToGenerate.ClassName);
            foreach (var classDeclarationVisitor in _classDeclarationVisitors)
            {
                classDeclarationSyntax = classDeclarationVisitor.Accept(classDeclarationSyntax, classToGenerate);
            }

            return compilationUnitSyntax.AddMembers(classDeclarationSyntax);
        }
    }
}