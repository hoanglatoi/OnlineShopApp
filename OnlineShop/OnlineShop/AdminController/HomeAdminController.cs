using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Service.Services.DataService;
using OnlineShop.ViewModels;
using OnlineShop.Model.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using LoginService.Attributes;
using OnlineShop.Service.Services.Token;

namespace OnlineShop.Controllers
{
    public class HomeAdminController : Controller
    {
        private readonly ILogger<HomeAdminController> _logger;
        IProductCategoryService _productCategoryService;
        IProductService _productService;
        ICommonService _commonService;

        public HomeAdminController(ILogger<HomeAdminController> logger,
            IProductCategoryService productCategoryService,
            IProductService productService,
            ICommonService commonService)
        {
            _logger = logger;
            _productCategoryService = productCategoryService;
            _commonService = commonService;
            _productService = productService;
        }

        [Authorize]
        [ClaimRequirementAttribute(groupName: "Maintenancer", role: "Admin")]
        public IActionResult Index()
        {
            var identityUser = User;
            var identityClaim = User.Claims.FirstOrDefault(x => x.Type == "UserName");
            var isLogin = User?.Identity.IsAuthenticated;
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ResponseCache( Duration = 3600)]
        public ActionResult Footer()
        {
            var footerModel = _commonService.GetFooter();
            var footerViewModel = footerModel == null ? null : AutoMap.Instance!.Mapper.Map<Footer, FooterVM>(footerModel);
            return PartialView(footerViewModel);
        }

        public ActionResult Header()
        {
            return PartialView();
        }

        [ResponseCache(Duration = 3600)]
        public ActionResult Category()
        {
            var model = _productCategoryService.GetAll();
            var listProductCategoryViewModel = AutoMap.Instance!.Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryVM>>(model);
            return PartialView(listProductCategoryViewModel);
        }
    }
}