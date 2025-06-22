using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youssef.DataAccess.Data;
using Youssef.DataAccess.Repository.IRepository;
using Youssef.Models;

namespace Youssef.DataAccess.Repository
{
    public class ProductRepository : Repository<Product> , IProductRepository
    {
        private readonly ApplicationDbContext _Db;
        public ProductRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }

        public void update(Product obj)
        {
            _Db.Products.Update(obj);
        }
    }
}
