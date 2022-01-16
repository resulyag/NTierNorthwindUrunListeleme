using BLL.Tools;
using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        public static NorthwindEntities Instance = DbInstance.Instance;

        public void Delete(T t)
        {
            Set().Remove(t);
            SaveChanges();
        }

        public T GetByID(int id)
        {
            return Set().Find(id);
        }

        public List<T> GetListAll()
        {
            return Set().ToList();
        }

        public void Insert(T t)
        {
            Set().Add(t);
            SaveChanges();
        }

        public void Update(T t)
        {
            Instance.Entry(t).State = EntityState.Modified;
            SaveChanges();
        }

        public DbSet<T> Set()
        {
            return Instance.Set<T>();
        }

        public void SaveChanges()
        {
            try
            {
                Instance.SaveChanges();
            }
            catch (DbUpdateConcurrencyException dbUC) { /*throw new Exception(dbUC.Message);*/ }
            catch (DbUpdateException ex) { }
            catch (DbEntityValidationException ex) { }
            catch (NotSupportedException ex) { }
            catch (ObjectDisposedException ex) { }
            catch (InvalidOperationException ex) { }
            catch (Exception ex) { }

        }
    }
}
