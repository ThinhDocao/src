using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using S3Train.Contract;
using S3Train.Domain;
using S3Train.Models;
using S3Train.Web.Models;

namespace S3Train.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductAdvertisementService _productAdvertisementService;
        private readonly IBrandService _brandService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IFooterClientService _footerClientService;
        private readonly IMenuService _menuService;
        private readonly IContentCategoryService _contentCategoryService;
        private readonly IMenuTypeService _menuTypeService;

        public HomeController(IProductService productService, IProductAdvertisementService productAdvertisementService, IBrandService brandService, IProductCategoryService productCategoryService, IFooterClientService footerClientService, IMenuService menuService, IContentCategoryService contentCategoryService, IMenuTypeService menuTypeService)
        {
            _productService = productService;
            _productAdvertisementService = productAdvertisementService;
            _productCategoryService = productCategoryService;
            _brandService = brandService;
            _footerClientService = footerClientService;
            _menuService = menuService;
            _contentCategoryService = contentCategoryService;
            _menuTypeService = menuTypeService;
        }

        public ActionResult Index()
        {
            ViewBag.TopHotALL = _productService.ListTopHotALL();

            ViewBag.productCategory = _productCategoryService.ListAll();
            

            return View();
        }

        [ChildActionOnly]
        public ActionResult _Footer(FooterClientViewModels model)
        {
            var footer = _footerClientService.ListAll();
            model = new FooterClientViewModels(footer);
            return PartialView(model);
        }



        [ChildActionOnly]
        public ActionResult ProductCategoryMenu()
        {
            var productCategory = _productCategoryService.ListAllOrderByCreateDate();
            var model = productCategory.Select(item=>new ProductCategoryViewModels(item)).ToList();
            ViewBag.ContentCategory = _contentCategoryService.ListAllDisplayOrder();
            ViewBag.MenuType = _menuTypeService.ListAll();
            ViewBag.Menu = _menuService.ListAll();



            //Giỏ hàng
            var cart = ListCart.listCart;
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            ViewBag.Cart = list;


            return PartialView(model);
        }

        public ActionResult MenuPrice()
        {
            ViewBag.MenuType = _menuTypeService.ListAll();
            ViewBag.Menu = _menuService.ListAll();
            return PartialView();
        }

        public ActionResult Brand()
        {
            var view = _brandService.ListAll();
            var model = view.Select(item => new BrandVIewModels(item)).ToList();
            return PartialView(model);
        }

        





        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Detail(Guid id)
        {
            var product = _productService.GetById(id);



            //More Image
            var images = product.MoreImage;
            List<string> listImagesReturn = new List<string>();
            if (images == null)
            {
                return View();
            }
            XElement xImages = XElement.Parse(images);


            foreach (XElement element in xImages.Elements())
            {
                listImagesReturn.Add(element.Value);
            }
            ////////////
            ViewBag.MoreImage = listImagesReturn.ToList();


            for(int i=0;i< ViewBag.MoreImage.Count;i++)
            {
                var s = listImagesReturn[i];
            }



            var model = new ProductViewModel(product);
            ViewBag.relatedProduct = _productService.relatedProduct(id);
            return PartialView(model);
        }
    }
}