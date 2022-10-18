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
namespace Bll.Cqrs.Commands.ProductCategory.Edit
{
    public class EditProductCategoryHandler : IRequestHandler<EditProductCategoryRequest, BaseResponse<int>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IRedisService _redisService;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly IUnitOfWorkService _unitofWorkService;
        public EditProductCategoryHandler(IProductCategoryRepository productCategoryRepository,
            IAttributeRepository attributeRepository,
            IRedisService redisService,
            ICategoryAttributeRepository categoryAttributeRepository, IUnitOfWorkService unitofWorkService)
        {
            _productCategoryRepository = productCategoryRepository;
            _attributeRepository = attributeRepository;
            _redisService = redisService;
            _categoryAttributeRepository = categoryAttributeRepository;
            _unitofWorkService = unitofWorkService;
        }

        public async Task<BaseResponse<int>> Handle(EditProductCategoryRequest request, CancellationToken cancellationToken)
        {
            var model = new Core.Entity.ProductCategory { Id = request.Id, Name = request.Name, IsActive = true };

            var tr = await _unitofWorkService.BeginTransactionAsync();

            var result = await _productCategoryRepository.Edit(model);

            if (!result.Status)
                return new BaseResponse<int>().Fail(result.ErrorMessage);

            if (request.Attributes.Count > 0)
            {
                var updateAttributes = await _categoryAttributeRepository.UpdateAttributes(request.Id, request.Attributes);
            }

            //transaction commitle
            var trResult = await _unitofWorkService.CommitTransactionAsync(tr.Data);
            if (!trResult.Status)
                return new BaseResponse<int>().Fail(trResult.ErrorMessage);

            #region rediste ki bu id li kayıt güncellendi.

            var getProductCategoryWithInclude = await _productCategoryRepository.GetAsync(s => s.Id == request.Id && s.IsActive, x => x.CategoryAtrributes, x => x.Products);

            if (getProductCategoryWithInclude.Data != null)
                await _redisService.GetAndClearAndSetData(
                   string.Format(DefaultCacheKey.ProductCategoryGetKey, result.Data.Id),
                   getProductCategoryWithInclude.Data,
                   TimeSpan.FromMinutes(5));
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
