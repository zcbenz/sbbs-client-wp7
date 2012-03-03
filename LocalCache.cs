using System.IO.IsolatedStorage;

namespace sbbs_client_wp7
{
    public static class LocalCache
    {
        public static T Get<T>(string key, T defaultValue)
        {
            T value;
            if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue<T>(key, out value))
                IsolatedStorageSettings.ApplicationSettings[key] = value = defaultValue;

            return value;
        }

        public static T Get<T>(string key)
        {
            T value;
            if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue<T>(key, out value))
                value = default(T);

            return value;
        }

        public static void Set<T>(string key, T value)
        {
            IsolatedStorageSettings.ApplicationSettings[key] = value;
        }
    }
}
