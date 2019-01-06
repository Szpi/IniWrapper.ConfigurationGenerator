using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Kind
{
    public class SyntaxKindManager : ISyntaxKindManager
    {
        private readonly char _listSeparator;

        public SyntaxKindManager(char listSeparator)
        {
            _listSeparator = listSeparator;
        }

        public (SyntaxKind syntaxKind, SyntaxKind underlyingSyntaxKind) GetSyntaxKind(string value)
        {
            var valueContainsSeparator = value.AsSpan().Contains(new ReadOnlySpan<char>(new[] { _listSeparator }), StringComparison.InvariantCultureIgnoreCase);
            if (valueContainsSeparator)
            {
                var splitedValues = value.Split(new[] { _listSeparator }, StringSplitOptions.RemoveEmptyEntries);
                var splitValuesSyntaxKind = splitedValues.Select(GetSyntaxKind).Select(x => x.syntaxKind).Distinct().ToList();

                var underlyingSyntaxKind =  splitValuesSyntaxKind.Count() > 1
                    ? SyntaxKind.StringKeyword
                    : splitValuesSyntaxKind.FirstOrDefault();

                return (SyntaxKind.List, underlyingSyntaxKind);
            }

            if (Boolean.TryParse(value, out var boolResult))
            {
                return (SyntaxKind.BoolKeyword, SyntaxKind.None);
            }

            if (int.TryParse(value, out var intResult))
            {
                return (SyntaxKind.IntKeyword, SyntaxKind.None);
            }

            if (float.TryParse(value, out var floatResult))
            {
                return (SyntaxKind.FloatKeyword, SyntaxKind.None);
            }

            if (double.TryParse(value, out var doubleResult))
            {
                return (SyntaxKind.DoubleKeyword, SyntaxKind.None);
            }

            if (byte.TryParse(value, out var byteResult))
            {
                return (SyntaxKind.ByteKeyword, SyntaxKind.None);
            }

            return (SyntaxKind.StringKeyword, SyntaxKind.None);
        }
    }
}