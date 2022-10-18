using Core.Consts;
using Core.Entity;
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
namespace Bll.Cqrs.Commands.ProductCategory.Add
{
    public class AddProductCategoryHandler : IRequestHandler<AddProductCategoryRequest, BaseResponse<int>>
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly IRedisService _redisService;
        private readonly IUnitOfWorkService _unitofWorkService;
        public AddProductCategoryHandler(IProductCategoryRepository productCategoryRepository,
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

        public async Task<BaseResponse<int>> Handle(AddProductCategoryRequest request, CancellationToken cancellationToken)
        {
            var model = new Core.Entity.ProductCategory { Name = request.Name };

            var result = await _productCategoryRepository.Add(model);

            var tr = await _unitofWorkService.BeginTransactionAsync();

            var productCcategoryResult = await _unitofWorkService.SaveAsync();

            if (!productCcategoryResult.Status)
                return new BaseResponse<int>().Fail(productCcategoryResult.ErrorMessage);

            //transaction başlar , commitlenir

            //categoriye ait attr ler cat attribute tablosuna ekleniyor
            if (request.Attributes.Count > 0)
            {
                var atts = request.Attributes.Select(s =>
                new CategoryAttribute
                {
                    AttributeValueId = Convert.ToInt32(s),
                    ProductCategoryId = result.Data.Id
                }).ToList();

                var addAttrs = await _categoryAttributeRepository.AddRangeAsync(atts);
            }

            //transaction commitle
            var trResult = await _unitofWorkService.CommitTransactionAsync(tr.Data);



            if (!trResult.Status)
                return new BaseResponse<int>().Fail(trResult.ErrorMessage);


            #region rediste ki bu id li kayıt set edildi.

            var getproductCategoryWithInclude = await _productCategoryRepository.GetAsync(s => s.Id == result.Data.Id, x => x.CategoryAtrributes, x => x.Products);


            var setToRedis = await _redisService.SetAsync(
                string.Format(DefaultCacheKey.ProductCategoryGetKey, result.Data.Id),
               getproductCategoryWithInclude.Data,
                TimeSpan.FromMinutes(5));
            #endregion


            #region product category rediste güncellendi

            var productCategoryList = await _productCategoryRepository.GetAllAsync(s => s.IsActive, x => x.CategoryAtrributes, x => x.Products);

            var setProdcutCategoryToRedis = await _redisService.SetAsync(
            DefaultCacheKey.ProductCategoryKey,
            productCategoryList.Data,
            TimeSpan.FromMinutes(5));

            #endregion

            return new BaseResponse<int>().Success(1);
        }
    }
}
