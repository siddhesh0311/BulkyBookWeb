using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess.Repo.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bulky.Models.ViewModels;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.AspNetCore.Authorization;
using Bulky.Utilities;

namespace AspNet8DemoMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _context;
        private readonly IWebHostEnvironment _webHost;
        public ProductController(IUnitOfWork UnitOfWork, IWebHostEnvironment webHost)
        {
            _context = UnitOfWork;
            _webHost = webHost;
        }

        public IActionResult Index()
        {
            List<Products> products = _context.ProductRepository.GetAll(IncludeProperties: "Category").ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ProductVM productVM = new()
            {
                CategoryList = _context.CategoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Products = new Products()
            };
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM product, IFormFile? formFile)//Last Update on 08/31/2024 by Siddhesh
        {
            string wwwRoot = _webHost.WebRootPath;
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                    string folderPath = Path.Combine(wwwRoot, @"images\Product");

                    if (!string.IsNullOrEmpty(product.Products.ImageURL))
                    {
                        //Delete the Old image
                        var OldImagePath =
                            Path.Combine(wwwRoot, product.Products.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(OldImagePath))
                        {
                            System.IO.File.Delete(OldImagePath);
                        }

                    }

                    using (var filestream = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create))
                    {
                        formFile.CopyTo(filestream);
                    }
                    product.Products.ImageURL = @"\images\Product\" + fileName;
                }

                if (product.Products.Id != 0)
                {
                    _context.ProductRepository.Update(product.Products);
                }
                else
                {
                    _context.ProductRepository.Add(product.Products);
                }

                _context.Save();
                TempData["Success"] = "Product Added Successfully!!!";
                return RedirectToAction("Index", "Product");
            }
            else
            {

                product.CategoryList = _context.CategoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(product);
            }
        }

        public IActionResult Upsert(int? Id) //Update-Insert
        {
            if (Id == null)
            {
                ProductVM productVM = new()
                {
                    CategoryList = _context.CategoryRepository.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),

                    Products = new Products()
                };
                return View(productVM);
            }
            else
            {
                Products? Product = _context.ProductRepository.Get(u => u.Id == Id);
                if (Product == null)
                {
                    return NotFound();
                }
                ProductVM productVM = new()
                {
                    CategoryList = _context.CategoryRepository.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                    Products = Product
                };
                return View(productVM);
            }
        }

        //[HttpPost]
        //public IActionResult Edit(ProductVM obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.ProductRepository.Update(obj);
        //        _context.Save();
        //        TempData["Success"] = "Product Updated Successfully!!!";
        //        return RedirectToAction("Index", "Product");
        //    }
        //    return View();
        //}

        //public IActionResult Delete(int? Id)
        //{
        //    if (Id == null || Id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Products? products = _context.ProductRepository.Get(u => u.Id == Id);
        //    if (products == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(products);
        //}

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(Products obj)
        {
            if (ModelState.IsValid)
            {
                Products? products = _context.ProductRepository.Get(u => u.Id == obj.Id);
                if (products == null)
                {
                    return NotFound();
                }
                _context.ProductRepository.Remove(products);
                _context.Save();
                TempData["Success"] = "Product Deleted Successfully!!!";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        #region APICALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Products> products = _context.ProductRepository.GetAll(IncludeProperties: "Category").ToList();
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Products? products = _context.ProductRepository.Get(u => u.Id == id);
            if (products == null)
            {
                return Json(new { success = false, message = "Product Not Found..." });
            }

            //Delete image
            var OldImagePath =
                Path.Combine(_webHost.WebRootPath, products.ImageURL.TrimStart('\\'));
            if (System.IO.File.Exists(OldImagePath))
            {
                System.IO.File.Delete(OldImagePath);
            }

            _context.ProductRepository.Remove(products);
            _context.Save();
            return Json(new { success = true, message = "Product Deleted Successfully..." });
        }
        #endregion


    }
}
