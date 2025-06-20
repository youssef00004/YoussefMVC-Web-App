using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Youssef.DataAccess.Data;
using Youssef.DataAccess.Repository.IRepository;
using Youssef.Models;

namespace Youssef.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository
    {
        private readonly ApplicationDbContext _Db;
        public CategoryRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }
        public void save()
        {
            _Db.SaveChanges();
        }

        public void update(Category obj)
        {
            _Db.Categories.Update(obj);
        }
    }
}
