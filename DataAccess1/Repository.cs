using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess1
{
    public class Repository<T>
        where T:class
    {
        private Context _context;

        public Repository()
        {
            _context = new Context();
        }

        public T Persist(T entity)
        {
            return _context.Set<T>().Add(entity);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public T Update(T entity)
        {
            _context.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;

            return entity;
        }

        public IQueryable<T> Set()
        {
            return _context.Set<T>(); 
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
