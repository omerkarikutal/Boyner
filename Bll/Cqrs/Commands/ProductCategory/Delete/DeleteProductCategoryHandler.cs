using Core.Consts;
using Core.Model;
using Core.Redis;
using Dal.Abstract;
using Dal.UnitOfWork;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProductCategory = Core.Entity.ProductCategory;
namespace Bll.Cqrs.Commands.ProductCategory.Delete
{
    public class DeleteProductCategoryHandler : IRequestHandler<DeleteProductCategoryRequest, BaseResponse<int>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IRedisService _redisService;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryAttributeRepository _categoryAttributeRepsoitory;
        private readonly IUnitOfWorkService _unitofWorkService;
        public DeleteProductCategoryHandler(IProductCategoryRepository productCategoryRepository,
            IAttributeRepository attributeRepository,
            IRedisService redisService,
            IProductRepository productRepository,
            ICategoryAttributeRepository categoryAttributeRepsoitory, IUnitOfWorkService unitofWorkService)
        {
            _productCategoryRepository = productCategoryRepository;
            _attributeRepository = attributeRepository;
            _redisService = redisService;
            _productRepository = productRepository;
            _categoryAttributeRepsoitory = categoryAttributeRepsoitory;
            _unitofWorkService = unitofWorkService;
        }

        public async Task<BaseResponse<int>> Handle(DeleteProductCategoryRequest request, CancellationToken cancellationToken)
        {
            var tr = await _unitofWorkService.BeginTransactionAsync();



            var result = await _productCategoryRepository.Delete(request.Id);

            if (!result.Status)
                return new BaseResponse<int>().Fail(result.ErrorMessage);



            var products = await _productRepository.GetAllAsync(s => s.ProductCategoryId == request.Id && s.IsActive);

            var categoryAttriubtes = await _categoryAttributeRepsoitory.GetAllAsync(s => s.ProductCategoryId == request.Id && s.IsActive);

            //bu kategoriye bağlı product lar ve attributeler soft delete yapılıyor
            //bulkupdate todo

            //transaction commitle
            var trResult = await _unitofWorkService.CommitTransactionAsync(tr.Data);



            if (!trResult.Status)
                return new BaseResponse<int>().Fail(trResult.ErrorMessage);

            #region rediste ki bu id li kayıt silindi.
            var getFromRedis = await _redisService.GetAsync(string.Format(DefaultCacheKey.ProductCategoryGetKey, request.Id));

            if (getFromRedis.Status && !string.IsNullOrEmpty(getFromRedis.Data))
                await _redisService.ClearAsync(string.Format(DefaultCacheKey.ProductCategoryGetKey, request.Id));

            #endregion


            #region product category rediste güncellendi

            var productCategoryList = await _productCategoryRepository.GetAllAsync(s => s.IsActive, x => x.Products, x => x.CategoryAtrributes);

            var setProdcutCategoryToRedis = await _redisService.GetAndClearAndSetData(
            DefaultCacheKey.ProductCategoryKey,
            productCategoryList.Data,
            TimeSpan.FromMinutes(5));

            #endregion

            return new BaseResponse<int>().Success(1);
        }
    }
}
