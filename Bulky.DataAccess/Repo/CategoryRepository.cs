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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public readonly CategoryDbContext _db;
        public CategoryRepository(CategoryDbContext Db) : base(Db)
        {
            _db = Db;
        }
        public void Update(Category category)
        {
            _db.Update(category);
        }
    }
}
