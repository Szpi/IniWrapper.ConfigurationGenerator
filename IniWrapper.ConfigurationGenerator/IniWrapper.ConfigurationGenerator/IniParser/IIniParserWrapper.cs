using System.Collections.Generic;

namespace IniWrapper.ConfigurationGenerator.IniParser
{
    public interface IIniParserWrapper
    {
        string[] GetSectionsFromFile();
        IDictionary<string, string> ReadAllFromSection(string section);
    }
}