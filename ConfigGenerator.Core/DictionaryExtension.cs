namespace ConfigGenerator.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class DictionaryExtension
    {
        public static string GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value.ToString() : string.Empty;
        }
    }
}