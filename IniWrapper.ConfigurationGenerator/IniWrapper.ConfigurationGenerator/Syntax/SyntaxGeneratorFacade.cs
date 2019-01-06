using System.Collections.Generic;
using IniWrapper.ConfigurationGenerator.Ini.Class;
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
        private readonly IPropertyDeclarationSyntaxGenerator _propertyDeclarationSyntaxGenerator;
        private readonly ClassDeclarationSyntaxGenerator _classDeclarationSyntaxGenerator;
        private readonly IListPropertyDeclarationSyntaxGenerator _listPropertyDeclarationSyntaxGenerator;
        private readonly IniOptionsAttributeSyntaxGenerator _iniOptionsAttributeSyntaxGenerator;
        private readonly ConstructorDeclarationSyntaxGenerator _constructorDeclarationSyntaxGenerator;

        public SyntaxGeneratorFacade(IniOptionsAttributeSyntaxGenerator iniOptionsAttributeSyntaxGenerator,
                                     IListPropertyDeclarationSyntaxGenerator listPropertyDeclarationSyntaxGenerator,
                                     IPropertyDeclarationSyntaxGenerator propertyDeclarationSyntaxGenerator,
                                     UsingSyntaxGenerator usingSyntaxGenerator,
                                     ClassDeclarationSyntaxGenerator classDeclarationSyntaxGenerator, 
                                     ConstructorDeclarationSyntaxGenerator constructorDeclarationSyntaxGenerator)
        {
            _iniOptionsAttributeSyntaxGenerator = iniOptionsAttributeSyntaxGenerator;
            _listPropertyDeclarationSyntaxGenerator = listPropertyDeclarationSyntaxGenerator;
            _propertyDeclarationSyntaxGenerator = propertyDeclarationSyntaxGenerator;
            _usingSyntaxGenerator = usingSyntaxGenerator;
            _classDeclarationSyntaxGenerator = classDeclarationSyntaxGenerator;
            _constructorDeclarationSyntaxGenerator = constructorDeclarationSyntaxGenerator;
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

        public ConstructorDeclarationSyntax GetConstructorDeclarationSyntax(string className, IReadOnlyList<PropertyDescriptor> propertyDescriptors)
        {
            return _constructorDeclarationSyntaxGenerator.GetConstructorDeclarationSyntax(className, propertyDescriptors);
        }
    }
}