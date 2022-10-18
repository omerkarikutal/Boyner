using Core.Entity;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class EfRepository<TEntity> : IEfRepository<TEntity>
    where TEntity : class, IBaseEntity, new()
    {
        private readonly DbContext context;
        public EfRepository(DbContext dbContext)
        {
            context = dbContext;
        }
        public async Task<BaseResponse<TEntity>> Add(TEntity entity)
        {
            try
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                //await context.SaveChangesAsync();
                return new BaseResponse<TEntity>().Success(addedEntity.Entity);

            }
            catch (Exception e)
            {
                return new BaseResponse<TEntity>().Fail(e.Message);
            }

        }

        public async Task<BaseResponse<int>> AddRangeAsync(List<TEntity> entities)
        {
            try
            {
                await context.Set<TEntity>().AddRangeAsync(entities);
                //await context.SaveChangesAsync();
                return new BaseResponse<int>().Success(1);

            }
            catch (Exception e)
            {
                return new BaseResponse<int>().Fail(e.Message);
            }
        }

        public async Task<BaseResponse<int>> Delete(int id)
        {
            try
            {
                var data = await context.Set<TEntity>().Where(s => s.Id == id).FirstOrDefaultAsync();

                if (data == null)
                    return new BaseResponse<int>().Fail("Kayıt Bulunamadı");

                data.IsActive = false;
                var updateData = await Edit(data);

                if (updateData.Status)
                    return new BaseResponse<int>().Success(1);
                else
                    return new BaseResponse<int>().Fail("Kayıt Güncellenirken Hata Oluştu");

            }
            catch (Exception e)
            {

                throw;
            }
        }
        public async Task<BaseResponse<TEntity>> Edit(TEntity entity)
        {
            try
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Modified;
                await context.SaveChangesAsync();
                return new BaseResponse<TEntity>().Success(addedEntity.Entity);
            }
            catch (Exception e)
            {
                return new BaseResponse<TEntity>().Fail(e.Message);
            }

        }

        public async Task<BaseResponse<List<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> Filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }

                if (Filter != null)
                {
                    query = query.Where(Filter);
                }

                var result = await query.AsNoTracking().ToListAsync();

                return new BaseResponse<List<TEntity>>().Success(result);
            }
            catch (Exception e)
            {
                return new BaseResponse<List<TEntity>>().Fail("Servise Bağlanırken Hata Oluştu!!");
            }
        }

        public async Task<BaseResponse<TEntity>> GetAsync(Expression<Func<TEntity, bool>> Filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                if (includes.Length > 0)
                {
                    foreach (var item in includes)
                    {
                        query = query.Include(item);
                    }
                }

                var result = await query.AsNoTracking().FirstOrDefaultAsync(Filter);

                if (result == null)
                {
                    return new BaseResponse<TEntity>().Fail("Kayıt bulunamadı");
                }
                return new BaseResponse<TEntity>().Success(result);
            }
            catch (Exception e)
            {
                return new BaseResponse<TEntity>().Fail(e.Message);
            }
        }
    }
}
