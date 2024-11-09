﻿using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repo.IRepository
{
    public interface IProductRepository : IRepository<Products>
    {
        void Update(Products product);
        
    }
}
