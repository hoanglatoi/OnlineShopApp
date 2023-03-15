using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Model.Models;
using OnlineShop.Service.Services.DataService;
using OnlineShop.ViewModels;

namespace OnlineShop.ViewComponents
{
    [ViewComponent(Name = "Header")]
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ILogger<HeaderViewComponent> _logger;
        IProductCategoryService _productCategoryService;
        IProductService _productService;
        ICommonService _commonService;

        public HeaderViewComponent(ILogger<HeaderViewComponent> logger,
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
            return View();
        }

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    return View();
        //}
    }
}
