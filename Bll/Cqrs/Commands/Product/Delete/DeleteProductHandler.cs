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

namespace Bll.Cqrs.Commands.Product.Delete
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, BaseResponse<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IRedisService _redisService;
        private readonly IProductAttributeRepository _productAttributeRepository;
        private readonly IUnitOfWorkService _unitOfWorkService;
        public DeleteProductHandler(IProductRepository productRepository,
    IRedisService redisService,
    IProductAttributeRepository productAttributeRepository,
    IUnitOfWorkService unitOfWorkService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _productAttributeRepository = productAttributeRepository;
            _unitOfWorkService = unitOfWorkService;
        }

        public async Task<BaseResponse<int>> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {

            //transaction başlar , commitlenir
            var tr = await _unitOfWorkService.BeginTransactionAsync();


            var deleteResult = await _productRepository.Delete(request.Id);

            var getAttributes = await _productAttributeRepository.GetAllAsync(s => s.ProductId == request.Id && s.IsActive);



            //attributes değerleri pasif hale getirildi.
            //if (getAttributes.Status && getAttributes.Data.Count > 0)
            //    await _productAttributeRepository.BulkUpdate(request.Id);
            //transaction commitle
            var trResult = await _unitOfWorkService.CommitTransactionAsync(tr.Data);


            if (!trResult.Status)
                return new BaseResponse<int>().Fail(trResult.ErrorMessage);

            #region redisten bu id li kayıt silinecek

            var getDataFromRedis = await _redisService.GetAsync(string.Format(DefaultCacheKey.GetKey, request.Id));
            if (getDataFromRedis.Status && !string.IsNullOrEmpty(getDataFromRedis.Data))
                await _redisService.ClearAsync(string.Format(DefaultCacheKey.GetKey, request.Id));

            #endregion

            #region product_list rediste güncellenir
            var productList = await _productRepository.GetAllAsync(s => s.IsActive);
            //set redis
            var updateRedis = await _redisService.GetAndClearAndSetData(DefaultCacheKey.ProductKey, productList.Data, TimeSpan.FromMinutes(10));

            #endregion


            return new BaseResponse<int>().Success(1);
        }
    }
}
