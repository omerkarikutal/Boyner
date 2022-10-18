using Core.Consts;
using Core.Entity;
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
using MapsterMapper;
using Mapster;
using Dal.UnitOfWork;

namespace Bll.Cqrs.Commands.Product.Edit
{
    public class EditProductHandler : IRequestHandler<EditProductRequest, BaseResponse<Core.Entity.Product>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IRedisService _redisService;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IUnitOfWorkService _unitOfWorkService;

        public EditProductHandler(IProductRepository productRepository,
            IRedisService redisService,
            IProductAttributeRepository productAttributeRepository, IUnitOfWorkService unitOfWorkService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _productAttributeRepository = productAttributeRepository;
            _unitOfWorkService = unitOfWorkService;
        }
        public async Task<BaseResponse<Core.Entity.Product>> Handle(EditProductRequest request, CancellationToken cancellationToken)
        {
            var productEntity = request.Adapt<Core.Entity.Product>();

            //transaction başlar , commitlenir
            var tr = await _unitOfWorkService.BeginTransactionAsync();

            var result = await _productRepository.Edit(productEntity);

            if (!result.Status)
                return result;





            //attribute value listesi db aktif olan product attributes ler ile karşılaştırılır
            //deactive edildi.
            if (request.Attributes.Count > 0)
                await _productAttributeRepository.UpdateList(request.Id, request.Attributes);

            //transaction commitle
            var trResult = await _unitOfWorkService.CommitTransactionAsync(tr.Data);



            if (!trResult.Status)
                return new BaseResponse<Core.Entity.Product>().Fail(trResult.ErrorMessage);


            //rediste ki bu id ye ait kayıt bulunur
            //rediste var ise uçurulup , güncel data basılır

            #region rediste_id'li kayıt güncellenir
            var getDataFromRedis = await _redisService.GetAsync(string.Format(DefaultCacheKey.GetKey, request.Id));

            if (getDataFromRedis.Status && !string.IsNullOrEmpty(getDataFromRedis.Data))
                await _redisService.ClearAsync(string.Format(DefaultCacheKey.GetKey, request.Id));

            var setDataToRedis = await _redisService.SetAsync(
                string.Format(DefaultCacheKey.GetKey, request.Id),
                result.Data,
                TimeSpan.FromMinutes(10));
            #endregion


            #region rediste ki product listesi güncellenir
            var productList = await _productRepository.GetAllAsync(s => s.IsActive);
            //set redis
            var redisSet = await _redisService.GetAndClearAndSetData(
                DefaultCacheKey.ProductKey,
                productList.Data, TimeSpan.FromMinutes(10));
            #endregion

            return result;

        }
    }
}
