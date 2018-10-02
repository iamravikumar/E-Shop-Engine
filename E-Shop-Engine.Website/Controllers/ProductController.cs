﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Models;
using X.PagedList;

namespace E_Shop_Engine.Website.Controllers
{
    public class ProductController : PagingBaseController
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Product
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            IQueryable<Product> model = _productRepository.GetAll();
            IPagedList<ProductViewModel> viewModel = IQueryableToPagedList<Product, ProductViewModel, DateTime?>(model, x => x.Edited, page, 25);
            return View("_ProductsDeck", viewModel);
        }

        public PartialViewResult ProductsToPagedList(IEnumerable<ProductViewModel> model, int? page)
        {
            int pageNumber = page ?? 1;
            IPagedList<ProductViewModel> viewModel = IEnumerableToPagedList(model, pageNumber, 25);

            return PartialView("_ProductsDeck", viewModel);
        }

        [HttpGet]
        public ViewResult Details(int id)
        {

            Product model = _productRepository.GetById(id);
            ProductViewModel viewModel = Mapper.Map<ProductViewModel>(model);

            return View(viewModel);
        }

        [HttpGet]
        public FileContentResult GetImage(int id)
        {

            Product product = _productRepository.GetById(id);
            if (product?.ImageData != null && product.ImageData.Length != 0)
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