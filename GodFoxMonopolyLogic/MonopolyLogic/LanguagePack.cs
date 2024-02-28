using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MonopolyLogic
{
    public static class LanguagePack
    {
        public enum Languages
        {
            Russian,
            English,
            Esperanto,
        }

        private static readonly Dictionary<string, int> languages;
        public static int AddedLanguagesCount { get; private set; }

        private static readonly Dictionary<string, string> languagePacksPaths;
        
        private static dynamic langData = null;
        
        static LanguagePack()
        {
            languagePacksPaths = new Dictionary<string, string>();

            languages = new Dictionary<string, int>();
            AddedLanguagesCount = languages.Count;

            AddLanguage(nameof(Languages.Russian), "locales\\ru.json");
            AddLanguage(nameof(Languages.English), "locales\\en.json");
            AddLanguage(nameof(Languages.Esperanto), "locales\\esper.json");
        }

        public static void AddLanguage(string name, string path)
        {
            languages.Add(name, AddedLanguagesCount);
            AddedLanguagesCount++;
            languagePacksPaths.Add(name, path);
        }

        public static void SetLanguage(string language) 
            => langData = JsonConvert.DeserializeObject(File.ReadAllText(languagePacksPaths[language]));

        public static string GetTranslation(string key) => langData[key];
        
        public static string GetTranslation(string key, params object[] args) 
            => string.Format(GetTranslation(key), args);
    }
}
