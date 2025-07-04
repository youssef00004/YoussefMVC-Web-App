﻿using System;
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
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

        public UnitOfWork(ApplicationDbContext Db)
        {
            _Db = Db;
            Category = new CategoryRepository(_Db);
            Product = new ProductRepository(_Db);
            Company = new CompanyRepository(_Db);
            ShoppingCart = new ShoppingCartRepository(_Db);
            ApplicationUser = new ApplicationUserRepository(_Db);
        }

        public void save()
        {
            _Db.SaveChanges();
        }
    }
}
