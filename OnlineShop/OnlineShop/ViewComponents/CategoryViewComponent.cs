using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Model.Models;
using OnlineShop.Service.Services.DataService;
using OnlineShop.ViewModels;

namespace OnlineShop.ViewComponents
{
    [ViewComponent(Name = "Category")]
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ILogger<CategoryViewComponent> _logger;
        IProductCategoryService _productCategoryService;
        IProductService _productService;
        ICommonService _commonService;

        public CategoryViewComponent(ILogger<CategoryViewComponent> logger,
            IProductCategoryService productCategoryService,
            IProductService productService,
            ICommonService commonService)
        {
            _logger = logger;
            _productCategoryService = productCategoryService;
            _commonService = commonService;
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {
            var model = _productCategoryService.GetAll();
            var listProductCategoryViewModel = AutoMap.Instance!.Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryVM>>(model);
            return View(listProductCategoryViewModel);
        }

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    var model = _productCategoryService.GetAll();
        //    var listProductCategoryViewModel = AutoMap.Instance!.Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryVM>>(model);
        //    return View(listProductCategoryViewModel);
        //}
    }
}
