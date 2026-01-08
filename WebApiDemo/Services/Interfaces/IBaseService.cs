using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WebApiDemo.DTOs;

namespace WebApiDemo.Services.Interfaces
{
    public interface IBaseService
    {
        public T Find<T>(Guid id) where T : class;

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class;

        public PageDTO<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderBy, bool isAsc = true) where T : class;

        public T Insert<T>(T model) where T : class;
        public List<T> InsertBatch<T>(List<T> models) where T : class;
        public int Delete<T>(T model) where T : class;
        public int DeleteBatch<T>(List<T> models) where T : class;
        public int Update<T>(Guid id, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls) where T : class;
    }
}
