using ExaminationSystem.Common.Abstractions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Common.Behaviors;


public sealed class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

    private readonly IMemoryCache _cache;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    public CachingBehavior(IMemoryCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICacheableQuery<TResponse> cacheable)
            return await next().ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(cacheable.CacheKey))
        {
            _logger.LogWarning(
                "Request type {RequestType} implements {Cacheable} but returned an empty cache key; skipping cache.",
                typeof(TRequest).Name,
                nameof(ICacheableQuery<TResponse>));
            return await next().ConfigureAwait(false);
        }

        if (_cache.TryGetValue(cacheable.CacheKey, out TResponse? cached) && cached is not null)
        {
            _logger.LogDebug("Cache hit for key {CacheKey}.", cacheable.CacheKey);
            return cached;
        }

        var response = await next().ConfigureAwait(false);

        if (response is null)
            return response!;

        if (!cacheable.ShouldCacheResponse(response))
            return response;

        var ttl = cacheable.Expiration ?? DefaultExpiration;
        _cache.Set(
            cacheable.CacheKey,
            response,
            new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl });

        _logger.LogDebug("Cache miss for key {CacheKey}; stored with TTL {Ttl}.", cacheable.CacheKey, ttl);

        return response;
    }
}
