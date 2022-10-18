using Core.Consts;
using Core.Model;
using Core.Redis;
using Dal.Abstract;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bll.Cqrs.Queries.ProductCategory.GetAllProductCategory
{
    public class GetAllProductCategoryHandler : IRequestHandler<GetAllProductCategoryRequest, BaseResponse<List<Core.Entity.ProductCategory>>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IRedisService _redisService;
        public GetAllProductCategoryHandler(IProductCategoryRepository productCategoryRepository, IRedisService redisService)
        {
            _productCategoryRepository = productCategoryRepository;
            _redisService = redisService;
        }

        public async Task<BaseResponse<List<Core.Entity.ProductCategory>>> Handle(GetAllProductCategoryRequest request, CancellationToken cancellationToken)
        {
            //ilk önce redise soracağız

            var productList = new List<Core.Entity.ProductCategory>();

            var result = await _redisService.GetAsync(DefaultCacheKey.ProductCategoryKey);

            if (result.Status && !string.IsNullOrEmpty(result.Data))
                productList = JsonConvert.DeserializeObject<List<Core.Entity.ProductCategory>>(result.Data);
            else
            {
                var getDb = await _productCategoryRepository.GetAllAsync(s => s.IsActive, x => x.CategoryAtrributes, x => x.Products);

                productList = getDb.Data;

                var setRedis = await _redisService.SetAsync(DefaultCacheKey.ProductCategoryKey, productList, TimeSpan.FromMinutes(10));
            }
            var attributesList = request.Attributes.ToList();
            //todo düzenlenecek
            var data = productList.Where(s => (request.Name == null || s.Name.ToLower().Contains(request.Name.ToLower()))
            && (request.Attributes.Count == 0)).ToList();

            return new BaseResponse<List<Core.Entity.ProductCategory>>().Success(data);
        }
    }
}
