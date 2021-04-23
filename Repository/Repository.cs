using CRUD.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CRUD.Controllers.HomeController;

namespace CRUD.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        IQueryable<T> Get(Expression<Func<T, bool>> filters);

        void Add(T entity);

        void Edit(T entity);

        void Delete(T entity);

        void Dispose();
    }
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ObjDatabaseContext _context;
        public DbSet<T> _set;

        public GenericRepository(ObjDatabaseContext Context)
        {
            _context = Context;
            _set = Context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _set;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> filters)
        {
            IQueryable<T> query = _set.Where(filters);
            return query;
        }

        public void Add(T entity)
        {
            _set.Add(entity);
        }

        public void Edit(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
    public class Repository
    {
        ObjDatabaseContext _context;
        public Repository()
        {
            _context = new ObjDatabaseContext();
        }
        public OperationResult Commit()
        {
            try
            {
                _context.SaveChanges();
                return new OperationResult();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao aplicar alterações no banco de dados " + ex.Message);
            }
        }

        //Obj
        private GenericRepository<Obj> _obj;
        public GenericRepository<Obj> Obj
        {
            get
            {
                if (_obj == null)
                {
                    _obj = new GenericRepository<Obj>(_context);
                }
                return _obj;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
