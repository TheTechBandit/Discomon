using System;
using System.Collections.Generic;

namespace DiscomonProject.Storage.Implementations
{
    public class InMemoryStorage : IDataStorage
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public void StoreAllObjects(Dictionary<string, object> dict)
        {
            foreach(KeyValuePair<string, object> entry in dict)
            {
                StoreObject(entry.Value, entry.Key);
            }
        }

        public void StoreObject(object obj, string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = obj;
                return;
            }

            _dictionary.Add(key, obj);
        }

        public T RestoreObject<T>(string key)
        {
            if(!_dictionary.ContainsKey(key))
                throw new ArgumentException($"The provided key '{key}' wasn't found.");
            return (T)(_dictionary[key]);
        }

        public int StorageLength()
        {
            return _dictionary.Count;
        }

        public Dictionary<string, object> GetDict()
        {
            return _dictionary;
        }
    }
}
