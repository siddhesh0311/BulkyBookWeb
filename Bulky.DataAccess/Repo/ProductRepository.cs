using Bulky.DataAccess.Repo.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repo
{
    public class ProductRepository : Repository<Products>, IProductRepository
    {
        public readonly CategoryDbContext _db;
        public ProductRepository(CategoryDbContext Db) : base(Db)
        {
            _db = Db;
        }
        public void Update(Products Product)
        {
            //_db.Update(Product);
            var prod = _db.Products.FirstOrDefault<Products>(u => u.Id == Product.Id);
            if (prod != null)
            {
                prod.Title = Product.Title;
                prod.Description = Product.Description;
                prod.CategoryId = Product.CategoryId;
                prod.Price = Product.Price; 
                prod.ListPrice= Product.ListPrice;
                prod.Price100 = Product.Price100;
                prod.Price50 = Product.Price50;
                prod.ISBN = Product.ISBN;
                prod.Author = Product.Author;
                if (prod.ImageURL != null)
                {
                    prod.ImageURL = Product.ImageURL;
                }
            }
        }
    }
}
