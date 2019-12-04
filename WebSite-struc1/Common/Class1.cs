using System;

namespace Common
{
    public class CacheService
    {
        private readonly TimeSpan _expirationTime;
        private MemoryCache _memoryCache = Initialize();

        public CacheService(TimeSpan expirationTime)
        {
            _expirationTime = expirationTime;
        }

        public async Task<TValue> GetOrAdd<TValue>(string key, Func<Task<TValue>> getter)
        {
            var rawValue = (WrappedItem)_memoryCache.Get(key);
            if (rawValue == null)
            {
                rawValue = new WrappedItem(await getter().Caf());
                _memoryCache.Add(new CacheItem(key, rawValue), new CacheItemPolicy { SlidingExpiration = _expirationTime });
            }

            return (TValue)rawValue.Value;
        }

        public void Delete(string key) => _memoryCache.Remove(key);

        public void Clear()
        {
            _memoryCache.Dispose();
            _memoryCache = Initialize();
        }

        private static MemoryCache Initialize()
        {
            return new MemoryCache("MyCache");
        }

        private class WrappedItem
        {
            public object Value { get; }

            public WrappedItem(object value)
            {
                Value = value;
            }
        }
    }
}
