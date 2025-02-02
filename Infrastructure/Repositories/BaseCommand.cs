using Helper.NLog;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BaseCommand<T> where T : class
    {
        private readonly AppDbContext _context;
        private DbSet<T> _model { get; set; }

        public BaseCommand(AppDbContext context)
        {
            _context = context;
            _model = context.Set<T>();
        }

        public IQueryable<T> FindAll()
        {
            try
            {
                return _model.AsNoTracking();
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _model.Where(expression).AsNoTracking();
        }

        public async Task<int> CountByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _model.CountAsync(expression);
        }

        public T FindOrFail(object id)
        {
            try
            {
                var entity = _model.Find(id);
                if (entity == null)
                    throw new Exception("Tìm kiếm thực thể thất bại");
                return entity;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public T Create(T newEntity)
        {
            try
            {
                var entity = _model.Add(newEntity);
                _context.SaveChanges();
                return entity.Entity;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public T UpdateByEntity(T entity)
        {
            try
            {
                _model.Update(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public bool UpdateByEntityList(IEnumerable<T> entityList)
        {
            try
            {
                foreach (var entity in entityList)
                {
                    _model.Update(entity);
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public bool UpdateRange(List<T> range)
        {
            try
            {
                _model.UpdateRange(range);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public bool DeleteByEntity(T entity)
        {
            try
            {
                _model.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
