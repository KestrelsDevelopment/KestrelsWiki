using System.Collections.Generic;

namespace kestrelswiki.util;

public class DictionaryBuilder<K, V> where K : notnull
{
    protected Dictionary<K, V> dictionary = new();

    public DictionaryBuilder<K, V> Add(K key, V value)
    {
        dictionary.Add(key, value);
        return this;
    }

    public Dictionary<K, V> Build()
    {
        return dictionary;
    }
}