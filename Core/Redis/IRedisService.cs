using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Redis
{
    public interface IRedisService
    {
        Task<BaseResponse<string>> GetAsync(string key);
        Task<BaseResponse<bool>> SetAsync<T>(string key, T value, TimeSpan timeSpan);
        Task<BaseResponse<bool>> ClearAsync(string key);
        Task<BaseResponse<bool>> GetAndClearAndSetData<T>(string key, T value, TimeSpan timeSpan);
    }
}
