using Core.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.UnitOfWork
{
    public interface IUnitOfWorkService
    {
        Task<BaseResponse<int>> SaveAsync();
        Task<BaseResponse<IDbContextTransaction>> BeginTransactionAsync();
        Task<BaseResponse<bool>> CommitTransactionAsync(IDbContextTransaction transaction);

    }
}
