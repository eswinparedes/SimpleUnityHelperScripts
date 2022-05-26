using SUHScripts.Functional;

namespace SUHScripts.SaveSystem
{
    public interface ISaveSystem
    {
        void Save<T>(T value, string key, string filePath = null);
        Option<T> Load<T>(string key, string filePath = null);
        SaveValue<T> Get<T>(T defaultValue, string key, string filePath = null);
        bool KeyExists(string key, string filePath = null);
        bool FileExists(string filePath);
    }
}