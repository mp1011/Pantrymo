namespace Pantrymo.Domain.Extensions
{
    public static class DictionaryExtensions
    {
        public static V TryGet<K, V>(this Dictionary<K, V> dictionary, K key)
        {
            V val;
            if (dictionary.TryGetValue(key, out val))
                return val;
            else
                return default;
        }
    }
}
