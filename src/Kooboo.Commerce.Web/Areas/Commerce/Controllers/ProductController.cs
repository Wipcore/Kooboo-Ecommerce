﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Products;
using Kooboo.Commerce.Web.Mvc.Controllers;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.ImageSizes;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Settings.Services;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Categories.Services;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Categories;
using Kooboo.CMS.Common;
using Kooboo.Commerce.ImageSizes.Services;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{

    public class ProductController : CommerceControllerBase
    {

        private readonly IProductService _productService;
        private readonly IImageSizeService _imageSizeService;
        private readonly IProductTypeService _productTypeService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;

        public ProductController(
                IProductService productService,
                IImageSizeService imageSizeService,
                IProductTypeService productTypeService,
                IBrandService brandService,
                ICategoryService categoryService)
        {
            _productService = productService;
            _imageSizeService = imageSizeService;
            _productTypeService = productTypeService;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        public ActionResult Index(string search, int? page, int? pageSize)
        {
            var productTypes = _productTypeService.GetAllProductTypes();
            var query = _productService.Query();
            if(!string.IsNullOrEmpty(search))
                query = query.Where(o => o.Name.Contains(search));
            var model = query
                .OrderByDescending(x => x.Id)
                .ToPagedList(page, pageSize);
            ViewBag.ProductTypes = productTypes.ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Edit");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View("Edit");
        }

        [HttpGet]
        public ActionResult Get(int? id = null, int? productTypeId = null)
        {
            Product product = null;
            if (id.HasValue)
            {
                product = _productService.GetById(id.Value);
            }
            if (product == null)
            {
                product = new Product();
                product.ProductTypeId = productTypeId.Value;
                product.Name = "New Product";
            }
            return JsonNet(product);
        }

        [HttpPost]
        public ActionResult Save(Product obj)
        {
            try
            {
                _productService.Update(obj);
                return this.JsonNet(new { status = 0, message = "product succssfully saved." });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var product = _productService.GetById(id);
                if (product != null)
                    _productService.Delete(product);
                return this.JsonNet(new JsonResultData()
                {
                    ReloadPage = true
                });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult Delete(ProductRowModel[] model)
        {
            try
            {
                foreach (var item in model)
                {
                    var product = _productService.GetById(item.Id);
                    if (product != null)
                        _productService.Delete(product);
                }
                return this.JsonNet(new JsonResultData()
                {
                    ReloadPage = true
                });
            }
            catch (Exception ex)
            {
                return this.JsonNet(new { status = 1, message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetImageTypes()
        {
            var sizes = _imageSizeService.Query()
                                         .Where(x => x.IsEnabled)
                                         .ToList();
            return JsonNet(sizes);
        }

        [HttpGet]
        public ActionResult GetProductType(int id)
        {
            var obj = _productTypeService.GetById(id);
            return JsonNet(obj);
        }

        [HttpGet]
        public ActionResult GetBrands()
        {

            var objs = _brandService.Query();
            return JsonNet(objs);
        }

        [HttpGet]
        public ActionResult GetCategories(int? parentId = null)
        {

            var query = _categoryService.Query();
            if(parentId.HasValue)
                query = query.Where(o => o.Parent.Id == parentId.Value);
            else
                query = query.Where(o => o.Parent == null);
            var objs = query.ToArray();
            return JsonNet(objs);
        }
    }
}
