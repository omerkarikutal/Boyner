using Core.Consts;
using Core.Entity;
using Core.Model;
using Core.Redis;
using Dal.Abstract;
using Dal.UnitOfWork;
using Mapster;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bll.Cqrs.Commands.Product.Add
{
    public class AddProductHandler : IRequestHandler<AddProductRequest, BaseResponse<Core.Entity.Product>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IRedisService _redisService;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IUnitOfWorkService _unitofWorkService;
        public AddProductHandler(IProductRepository productRepository,
            IRedisService redisService,
            IProductAttributeRepository productAttributeRepository,
            IUnitOfWorkService unitofWorkService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _productAttributeRepository = productAttributeRepository;
            _unitofWorkService = unitofWorkService;
        }
        public async Task<BaseResponse<Core.Entity.Product>> Handle(AddProductRequest request, CancellationToken cancellationToken)
        {
            var productEntity = request.Adapt<Core.Entity.Product>();

            var result = await _productRepository.Add(productEntity);

            //transaction başlar , commitlenir
            var tr = await _unitofWorkService.BeginTransactionAsync();

            var productCcategoryResult = await _unitofWorkService.SaveAsync();

            if (!productCcategoryResult.Status)
                return new BaseResponse<Core.Entity.Product>().Fail(productCcategoryResult.ErrorMessage);


            //alınan attributevalue id ler liste haline eklenip db ye insert ediliyor.
            if (request.Attributes.Count > 0)
            {
                var attributeList = request.Attributes.Select(s => new ProductAttribute
                {
                    ProductId = result.Data.Id,
                    AttributeValueId = Convert.ToInt32(s)
                }).ToList();

                var attributeResult = await _productAttributeRepository.AddRangeAsync(attributeList);
            }


            //transaction commitle
            var trResult = await _unitofWorkService.CommitTransactionAsync(tr.Data);



            if (!trResult.Status)
                return new BaseResponse<Core.Entity.Product>().Fail(trResult.ErrorMessage);


            #region

            var getDataWithInclude = await _productRepository.GetAsync(s => s.Id == result.Data.Id, x => x.ProductCategory, x => x.ProductAttributes);

            var setDataToRedis = await _redisService.SetAsync(
                string.Format(DefaultCacheKey.GetKey, result.Data.Id),
                getDataWithInclude.Data,
                TimeSpan.FromMinutes(10));
            #endregion


            #region product_list rediste güncellenir

            var productList = await _productRepository.GetAllAsync(s => s.IsActive, x => x.ProductCategory, x => x.ProductAttributes);


            var updateRedis = await _redisService.GetAndClearAndSetData(
                DefaultCacheKey.ProductKey,
                productList.Data,
                TimeSpan.FromMinutes(10));

            #endregion

            return result;

        }
    }
}
