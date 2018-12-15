using System;
using Microsoft.CodeAnalysis.CSharp;

namespace IniWrapper.ConfigurationGenerator.PropertySyntax.Kind
{
    public class SyntaxKindManager : ISyntaxKindManager
    {
        private readonly char _listSeparator;

        public SyntaxKindManager(char listSeparator)
        {
            _listSeparator = listSeparator;
        }

        public SyntaxKind GetSyntaxKind(string value)
        {
            var valueContainsSeparator = value.AsSpan().Contains(new ReadOnlySpan<char>(new[] { _listSeparator }), StringComparison.InvariantCultureIgnoreCase);
            if (valueContainsSeparator)
            {
                return SyntaxKind.List;
            }

            if (Boolean.TryParse(value, out var boolResult))
            {
                return SyntaxKind.BoolKeyword;
            }

            if (int.TryParse(value, out var intResult))
            {
                return SyntaxKind.IntKeyword;
            }

            if (float.TryParse(value, out var floatResult))
            {
                return SyntaxKind.FloatKeyword;
            }

            if (double.TryParse(value, out var doubleResult))
            {
                return SyntaxKind.DoubleKeyword;
            }

            if (byte.TryParse(value, out var byteResult))
            {
                return SyntaxKind.ByteKeyword;
            }

            return SyntaxKind.StringKeyword;
        }
    }
}