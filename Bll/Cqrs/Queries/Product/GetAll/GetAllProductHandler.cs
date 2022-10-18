using Core.Consts;
using Core.Model;
using Core.Redis;
using Dal.Abstract;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bll.Cqrs.Queries.Product.GetAll
{
    public class GetAllProductHandler : IRequestHandler<GetAllProductRequest, BaseResponse<List<Core.Entity.Product>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IRedisService _redisService;
        public GetAllProductHandler(IProductRepository productRepository, IRedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
        }
        public async Task<BaseResponse<List<Core.Entity.Product>>> Handle(GetAllProductRequest request, CancellationToken cancellationToken)
        {
            //ilk önce redise soracağız

            var productList = new List<Core.Entity.Product>();

            var result = await _redisService.GetAsync(DefaultCacheKey.ProductKey);

            if (result.Status && !string.IsNullOrEmpty(result.Data))
                productList = JsonConvert.DeserializeObject<List<Core.Entity.Product>>(result.Data);
            else
            {
                var getDb = await _productRepository.GetAllAsync(s => s.IsActive, x => x.ProductCategory, x => x.ProductAttributes);

                productList = getDb.Data;

                var setRedis = await _redisService.SetAsync(DefaultCacheKey.ProductKey, productList, TimeSpan.FromMinutes(10));
            }


            var data = productList.Where(s =>
            (request.Name == null || s.Name.ToLower().Contains(request.Name.ToLower())) &&
            (request.CategoryName == null || s.ProductCategory.Name.ToLower().Contains(request.CategoryName.ToLower())) &&
            (s.Price > request.MinPrice && s.Price < request.MaxPrice)).ToList();


            return new BaseResponse<List<Core.Entity.Product>>().Success(data);
        }
    }
}
