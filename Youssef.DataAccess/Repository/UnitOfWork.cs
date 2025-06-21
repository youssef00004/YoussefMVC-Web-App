using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youssef.DataAccess.Data;
using Youssef.DataAccess.Repository.IRepository;

namespace Youssef.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _Db;
        public ICategoryRepository Category {  get; private set; } 
        public UnitOfWork(ApplicationDbContext Db)
        {
            _Db = Db;
            Category = new CategoryRepository(_Db);
        }

        public void save()
        {
            _Db.SaveChanges();
        }
    }
}
