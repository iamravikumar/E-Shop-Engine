﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Areas.Admin.Models;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        IProductRepository _productRepository;
        IRepository<Category> _categoryRepository;
        IRepository<Subcategory> _subcategoryRepository;

        public ProductController(IProductRepository productRepository, IRepository<Category> categoryRepository, IRepository<Subcategory> subcategoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subcategoryRepository = subcategoryRepository;
        }

        // GET: Admin/Product
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Product> model = _productRepository.GetAll();
            IEnumerable<ProductAdminViewModel> viewModel = model.Select(p => (ProductAdminViewModel)p).ToList();
            return View(viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id, string returnUrl)
        {
            ProductAdminViewModel model = _productRepository.GetById(id);
            model.Categories = _categoryRepository.GetAll();
            model.Subcategories = _subcategoryRepository.GetAll();
            model.ReturnUrl = returnUrl;

            return View(model);
        }
        //TODO add default img
        //TODO get byte array from db instead of storing in view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", model);
            }

            //model.ImageBytes = _productRepository.GetById(model.Id).ImageData ?? null;

            _productRepository.Update(model);

            return Redirect(model.ReturnUrl);
        }

        [HttpGet]
        public ViewResult Create()
        {
            ProductAdminViewModel model = new ProductAdminViewModel();
            model.Categories = _categoryRepository.GetAll();
            model.Subcategories = _subcategoryRepository.GetAll();

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ImageMimeType")] ProductAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            model.ImageMimeType = model.ImageData?.ContentType;

            _productRepository.Create(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id, string returnUrl)
        {
            Product product = _productRepository.GetById(id);
            ProductAdminViewModel model = product;
            model.ReturnUrl = returnUrl;
            model.Created = product.Created.ToLocalTime();
            model.Edited = product.Edited.GetValueOrDefault().ToLocalTime();

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            _productRepository.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public FileContentResult GetImage(int id)
        {
            Product product = _productRepository.GetById(id);
            if (product?.ImageData != null)
            {
                return new FileContentResult(product.ImageData, product.ImageMimeType);
            }
            else
            {
                byte[] img = System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/default-img.jpg"));

                return new FileContentResult(img, "image/jpg");
            }
        }
    }
}