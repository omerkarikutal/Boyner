using Core.Model;
using DataAccess.Concrete.EF.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.UnitOfWork
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly BoynerContext _context;
        private IDbContextTransaction _currentTransaction;
        public UnitOfWorkService(BoynerContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<IDbContextTransaction>> BeginTransactionAsync()
        {
            try
            {
                var result = await _context.Database.BeginTransactionAsync();
                return new BaseResponse<IDbContextTransaction>().Success(result);
            }
            catch (Exception e)
            {
                return new BaseResponse<IDbContextTransaction>().Fail(e.Message);
            }
        }

        public async Task<BaseResponse<bool>> CommitTransactionAsync(IDbContextTransaction transaction)
        {
            bool isSuccess = false;
            try
            {
                var result = await SaveAsync();

                if (result.Status)
                {
                    isSuccess = true;
                    transaction.Commit();
                }
                else
                    throw new Exception("Model Error");
            }
            catch
            {
                isSuccess = false;
                RollbackTransaction();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }

            return new BaseResponse<bool> { Status = isSuccess, ErrorMessage = isSuccess ? "" : "Transaction Error", Data = isSuccess };

        }
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        public async Task<BaseResponse<int>> SaveAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync();
                return new BaseResponse<int>().Success(result);
            }
            catch (Exception e)
            {
                return new BaseResponse<int>().Fail(e.Message);
            }
        }
        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
