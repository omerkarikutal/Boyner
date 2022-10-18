using Core.Entity;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IEfRepository<T> where T : class, IBaseEntity
    {
        Task<BaseResponse<T>> GetAsync(Expression<Func<T, bool>> Filter = null, params Expression<Func<T, object>>[] includes);
        Task<BaseResponse<List<T>>> GetAllAsync(Expression<Func<T, bool>> Filter = null, params Expression<Func<T, object>>[] includes);
        Task<BaseResponse<T>> Add(T Entity);
        Task<BaseResponse<T>> Edit(T Entity);
        Task<BaseResponse<int>> Delete(int id);
        Task<BaseResponse<int>> AddRangeAsync(List<T> entities);
    }
}
