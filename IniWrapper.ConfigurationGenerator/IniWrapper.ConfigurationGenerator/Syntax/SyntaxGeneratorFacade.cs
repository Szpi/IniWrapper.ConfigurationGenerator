using IniWrapper.ConfigurationGenerator.Syntax.Class;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax;
using IniWrapper.ConfigurationGenerator.Syntax.UsingSyntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax
{
    public class SyntaxGeneratorFacade : ISyntaxGeneratorFacade
    {
        private readonly UsingSyntaxGenerator _usingSyntaxGenerator;
        private readonly PropertyDeclarationSyntaxGenerator _propertyDeclarationSyntaxGenerator;
        private readonly ClassDeclarationSyntaxGenerator _classDeclarationSyntaxGenerator;
        private readonly ListPropertyDeclarationSyntaxGenerator _listPropertyDeclarationSyntaxGenerator;
        private readonly IniOptionsAttributeSyntaxGenerator _iniOptionsAttributeSyntaxGenerator;

        public SyntaxGeneratorFacade(IniOptionsAttributeSyntaxGenerator iniOptionsAttributeSyntaxGenerator,
                                     ListPropertyDeclarationSyntaxGenerator listPropertyDeclarationSyntaxGenerator,
                                     PropertyDeclarationSyntaxGenerator propertyDeclarationSyntaxGenerator,
                                     UsingSyntaxGenerator usingSyntaxGenerator,
                                     ClassDeclarationSyntaxGenerator classDeclarationSyntaxGenerator)
        {
            _iniOptionsAttributeSyntaxGenerator = iniOptionsAttributeSyntaxGenerator;
            _listPropertyDeclarationSyntaxGenerator = listPropertyDeclarationSyntaxGenerator;
            _propertyDeclarationSyntaxGenerator = propertyDeclarationSyntaxGenerator;
            _usingSyntaxGenerator = usingSyntaxGenerator;
            _classDeclarationSyntaxGenerator = classDeclarationSyntaxGenerator;
        }

        public UsingDirectiveSyntax GetUsingSyntax(string @using)
        {
            return _usingSyntaxGenerator.GetUsingSyntax(@using);
        }

        public PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, SyntaxKind syntaxKind)
        {
            return _propertyDeclarationSyntaxGenerator.GetPropertyDeclarationSyntax(propertyName, syntaxKind);
        }

        public PropertyDeclarationSyntax GetListPropertyDeclarationSyntax(string propertyName, SyntaxKind underlyingSyntaxKind)
        {
            return _listPropertyDeclarationSyntaxGenerator.GetPropertyDeclarationSyntax(propertyName, underlyingSyntaxKind);
        }

        public SyntaxList<AttributeListSyntax> GetAttributeSyntax(string section, string key)
        {
            return _iniOptionsAttributeSyntaxGenerator.AddIniOptionsAttributeToProperty(section, key);
        }

        public ClassDeclarationSyntax GetClassSyntax(string className)
        {
            return _classDeclarationSyntaxGenerator.GetClassSyntax(className);
        }

        public CompilationUnitSyntax GetCompilationUnitSyntax()
        {
            return SyntaxFactory.CompilationUnit();
        }

        public NamespaceDeclarationSyntax GetNamespace(string @namespace)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(@namespace));
        }
    }
}