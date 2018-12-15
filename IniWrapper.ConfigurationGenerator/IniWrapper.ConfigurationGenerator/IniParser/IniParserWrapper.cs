using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace IniWrapper.ConfigurationGenerator.IniParser
{
    internal class IniParserWrapper : IIniParserWrapper
    {
        private const char IniApiSeparator = '\0';

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSectionNames(byte[] buffer, int size, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

        private readonly string _iniFilePath;
        private readonly byte[] _buffer;
        private IReadSectionsParser _readSectionsParser;

        public IniParserWrapper(string iniFilePath, int bufferSize, IReadSectionsParser readSectionsParser)
        {
            _iniFilePath = iniFilePath;
            _readSectionsParser = readSectionsParser;
            _buffer = new byte[bufferSize];
        }

        public string[] GetSectionsFromFile()
        {
            var readCharacters = GetPrivateProfileSectionNames(_buffer, _buffer.Length, _iniFilePath);
            var allSections = Encoding.ASCII.GetString(_buffer, 0 , readCharacters);
            var sectionNames = allSections.Trim(IniApiSeparator).Split(new[] { IniApiSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return sectionNames;
        }

        public IDictionary<string, string> ReadAllFromSection(string section)
        {
            var readCharacters = GetPrivateProfileSection(section, _buffer, _buffer.Length, _iniFilePath);
            var readFromFile = Encoding.ASCII.GetString(_buffer, 0, readCharacters).Trim(IniApiSeparator);
            return _readSectionsParser.Parse(readFromFile);
        }
    }
}