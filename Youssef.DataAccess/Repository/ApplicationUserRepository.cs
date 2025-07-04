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
    public class ApplicationUserRepository : Repository<ApplicationUser> , IApplicationUserRepository
    {
        private readonly ApplicationDbContext _Db;
        public ApplicationUserRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }    
    }
}
