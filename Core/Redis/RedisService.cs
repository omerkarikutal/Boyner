using Core.Model;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Redis
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redisCon;
        private readonly IDatabase _cache;
        public RedisService(IConnectionMultiplexer redisCon)
        {
            _redisCon = redisCon;
            _cache = redisCon.GetDatabase();
        }
        public async Task<BaseResponse<bool>> ClearAsync(string key)
        {
            try
            {
                var result = await _cache.KeyDeleteAsync(key);
                return new BaseResponse<bool>().Success(result);
            }
            catch (Exception e)
            {
                return new BaseResponse<bool>().Fail(e.Message);
            }
        }

        public async Task<BaseResponse<bool>> GetAndClearAndSetData<T>(string key, T value, TimeSpan timeSpan)
        {
            try
            {
                var get = await GetAsync(key);

                if (get.Status)
                    await ClearAsync(key);

                var set = await SetAsync(key, value, timeSpan);

                return new BaseResponse<bool>().Success(true);
            }
            catch (Exception e)
            {
                return new BaseResponse<bool>().Fail(e.Message);
            }

        }

        public async Task<BaseResponse<string>> GetAsync(string key)
        {
            try
            {
                var result = await _cache.StringGetAsync(key);
                return new BaseResponse<string>().Success(result);
            }
            catch (Exception e)
            {
                return new BaseResponse<string>().Fail(e.Message);
            }
        }

        public async Task<BaseResponse<bool>> SetAsync<T>(string key, T value, TimeSpan timeSpan)
        {
            try
            {
                var result = await _cache.StringSetAsync(key,
                    JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
                    , timeSpan);
                return new BaseResponse<bool>().Success(result);
            }
            catch (Exception e)
            {
                return new BaseResponse<bool>().Fail(e.Message);
            }
        }
    }
}
