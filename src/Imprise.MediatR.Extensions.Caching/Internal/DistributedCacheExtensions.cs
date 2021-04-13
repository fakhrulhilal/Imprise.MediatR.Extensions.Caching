using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Threading.Tasks;

namespace Imprise.MediatR.Extensions.Caching.Internal
{

    /// <summary>
    /// The IDistributedCache interface is designed to work solely with byte arrays, unlike IMemoryCache which can
    /// take any arbitrary object.
    ///
    /// Microsoft have indicated that they do not intend adding these extension methods to support automatically
    /// serialising objects in several github issue e.g. https://github.com/aspnet/Caching/pull/94
    ///
    /// This implementation was gratefully taken from a Stack Overflow solution posted by dzed at
    /// https://stackoverflow.com/a/50222288/2316834
    ///
    /// Thank you https://www.goodreads.com/book/show/29437996-copying-and-pasting-from-stack-overflow
    /// </summary>
    internal static class DistributedCacheExtensions
    {
        public static Task SetAsync<T>(
            this IDistributedCache cache,
            string key,
            T value,
            DistributedCacheEntryOptions options)
        {
            string json = JsonSerializer.Serialize(value);
            return cache.SetStringAsync(key, json, options);
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key)
        {
            string json = await cache.GetStringAsync(key);
            return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json);
        }
    }
}