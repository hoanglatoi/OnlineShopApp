using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Model.Models;
using OnlineShop.Service.Services.DataService;
using OnlineShop.ViewModels;

namespace OnlineShop.ViewComponents
{
    [ViewComponent(Name = "Footer")]
    public class FooterViewComponent : ViewComponent
    {
        private readonly ILogger<FooterViewComponent> _logger;
        IProductCategoryService _productCategoryService;
        IProductService _productService;
        ICommonService _commonService;

        public FooterViewComponent(ILogger<FooterViewComponent> logger,
            IProductCategoryService productCategoryService,
            IProductService productService,
            ICommonService commonService)
        {
            _logger = logger;
            _productCategoryService = productCategoryService;
            _commonService = commonService;
            _productService = productService; 
        }

        //public IViewComponentResult Invoke()
        //{
        //    var footerModel = _commonService.GetFooter();
        //    var footerViewModel = footerModel == null ? null : AutoMap.Instance!.Mapper.Map<Footer, FooterVM>(footerModel);
        //    return View(footerViewModel);
        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = _commonService.GetFooter();
            var footerViewModel = footerModel == null ? null : AutoMap.Instance!.Mapper.Map<Footer, FooterVM>(footerModel);
            return View(footerViewModel);
        }
    }
}
