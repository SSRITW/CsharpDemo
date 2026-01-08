using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WebApiDemo.Data;
using WebApiDemo.DTOs;
using WebApiDemo.Services.Interfaces;

namespace WebApiDemo.Services
{
    public class BaseService : IBaseService
    {

        private readonly AppDbContext _db;
        public BaseService(AppDbContext db)
        {
            _db = db;
            db.Database.EnsureCreated();
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return this._db.Set<T>();
        }


        public int Delete<T>(T model) where T : class
        {
            this._db.Remove(model);
            return this._db.SaveChanges();
        }

        public int DeleteBatch<T>(List<T> models) where T : class
        {
            this._db.RemoveRange(models);
            return this._db.SaveChanges();
        }

        public T Find<T>(Guid id) where T : class
        {
            return this._db.Set<T>().Find(id);
        }

        public T Insert<T>(T model) where T : class
        {
            this._db.Set<T>().Add(model);
            this._db.SaveChanges();
            return model;
        }

        public List<T> InsertBatch<T>(List<T> models) where T : class
        {
            this._db.Set<T>().AddRange(models);
            this._db.SaveChanges();
            return models;
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            return Set<T>().Where(funcWhere);
        }

        public PageDTO<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, 
            Expression<Func<T, S>> funcOrderBy, bool isAsc = true) where T : class
        {
            var list = Set<T>();
            if (funcWhere != null)
            {
                list = list.Where(funcWhere);
            }
            if (isAsc)
            {
                list = list.OrderBy(funcOrderBy);
            }
            else
            {
                list = list.OrderByDescending(funcOrderBy);
            }
            PageDTO<T> result = new PageDTO<T>()
            {
                Items = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                RecordCount = list.Count(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return result;
        }

        public int Update<T>(Guid id, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls) where T : class
        {
            return _db.Set<T>()
                .Where(e => EF.Property<Guid>(e, "Id") == id)
                .ExecuteUpdate(setPropertyCalls);
        }
    }
}
