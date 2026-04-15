using MediatR;

namespace ExaminationSystem.Common.Behaviors;

public interface ICacheableQuery<TResponse> : IRequest<TResponse>
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
    bool ShouldCacheResponse(TResponse response) => true;
}
