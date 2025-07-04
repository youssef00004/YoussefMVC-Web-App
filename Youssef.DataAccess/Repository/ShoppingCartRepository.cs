﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youssef.DataAccess.Data;
using Youssef.DataAccess.Repository.IRepository;
using Youssef.Models;

namespace Youssef.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _Db;
        public ShoppingCartRepository(ApplicationDbContext Db) : base(Db)
        {
            _Db = Db;
        }

        public void update(ShoppingCart obj)
        {
            _Db.ShoppingCarts.Update(obj);
        }

    }
}
