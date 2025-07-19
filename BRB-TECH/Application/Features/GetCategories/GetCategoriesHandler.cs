using Application.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.Features.GetCategories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesRequest, IReadOnlyList<Category>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDistributedCache _cache;

        public GetCategoriesHandler(ICategoryRepository categoryRepository, IDistributedCache cache)
        {
            _categoryRepository = categoryRepository;
            _cache = cache;
        }

        public async Task<IReadOnlyList<Category>> Handle(GetCategoriesRequest request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Category> categories;
            var cachedCategories = await _cache.GetStringAsync($"categories:{request._getCategory.UserId}");

            if (!string.IsNullOrEmpty(cachedCategories))
            {
                categories = JsonConvert.DeserializeObject<IReadOnlyList<Category>>(cachedCategories);
                return categories;
            }

            categories = await _categoryRepository.GetAllAsync(request._getCategory);

            var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) };
            await _cache.SetStringAsync($"categories:{request._getCategory.UserId}", JsonConvert.SerializeObject(categories), cacheOptions);

            return categories;
        }
    }
}
