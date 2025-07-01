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
    public class CompanyRepository : Repository<Company> , ICompanyRepository
    {
        private readonly ApplicationDbContext _Db;
        public CompanyRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }

        public void update(Company comp)
        {
            _Db.Companies.Update(comp);
        }

    }
}
