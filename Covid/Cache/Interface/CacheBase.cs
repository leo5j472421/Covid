using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Covid.Enums;

namespace Covid.Cache.Interface
{
    public abstract class CacheBase<T>
    {
        private readonly object _lockKey;
        private EnumCache CacheKey { get; set; }
        private readonly MemoryCache _cache = MemoryCache.Default;

        protected CacheBase(EnumCache cacheKey)
        {
            this.CacheKey = cacheKey;
            _lockKey = new object();
        }

        protected CacheBase()
        {
        }

        public bool Contains(string key)
        {
            return _cache.Contains($"{CacheKey}_{key}");
        }

        public T Get(string key)
        {
            if (Contains(key))
            {
                return (T)_cache[$"{CacheKey}_{key}"];
            }

            lock (_lockKey)
            {
                if (_cache.Contains($"{CacheKey}_{key}"))
                {
                    return (T)_cache[$"{CacheKey}_{key}"];
                }
                var result = ReloadFromDb(key);
                _cache.Set($"{CacheKey}_{key}", result, GetItemPolicy());
                return result;
            }
        }

        public void ClearAll()
        {
            lock (_lockKey)
            {
                if (_cache == null) { return; }
                var cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
                foreach (var cacheKey in cacheKeys.Where(x=>x.Contains($"{CacheKey}_")))
                {
                    MemoryCache.Default.Remove(cacheKey);
                }
            }
        }

        public void AddOrUpdate(string key, T item)
        {
            _cache.Set($"{CacheKey}_{key}", item, GetItemPolicy());
        }

        protected abstract T ReloadFromDb(string key);

        protected abstract CacheItemPolicy GetItemPolicy();

    }
}