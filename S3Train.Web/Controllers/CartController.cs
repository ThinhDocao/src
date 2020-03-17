using S3Train.Contract;
using S3Train.Domain;
using S3Train.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace S3Train.Web.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        private readonly IProductService _productService;
        private readonly IProductAdvertisementService _productAdvertisementService;
        private readonly IBrandService _brandService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IFooterClientService _footerClientService;
        private readonly IMenuService _menuService;
        private readonly IContentCategoryService _contentCategoryService;
        private readonly IMenuTypeService _menuTypeService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;

     

        public CartController(IProductService productService, IProductAdvertisementService productAdvertisementService, IBrandService brandService, IProductCategoryService productCategoryService, IFooterClientService footerClientService, IMenuService menuService, IContentCategoryService contentCategoryService, IMenuTypeService menuTypeService, IOrderService orderService, IOrderDetailService orderDetailService)
        {
            
            _productService = productService;
            _productAdvertisementService = productAdvertisementService;
            _productCategoryService = productCategoryService;
            _brandService = brandService;
            _footerClientService = footerClientService;
            _menuService = menuService;
            _contentCategoryService = contentCategoryService;
            _menuTypeService = menuTypeService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }
        public ActionResult Index()
        {
            
            var cart = ListCart.listCart;
            var list = new List<CartItem>();
            if (cart!=null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public JsonResult Delete(Guid id)
        {
            var cart = (List<CartItem>)ListCart.listCart;
            cart.RemoveAll(x => x.Product.Id == id);
            ListCart.listCart = cart;

            return Json(new
            {
                status = true
            });
        }

        public JsonResult DeleteAll()
        {
            ListCart.listCart = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);

            var cart = (List<CartItem>)ListCart.listCart;

            foreach (var item in cart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.Id == item.Product.Id);
                if(jsonItem!=null)
                {
                    item.Quatity = jsonItem.Quatity;
                }
            }
            ListCart.listCart = cart;

            return Json(new
            {
                status = true
            });

        }

        public ActionResult AddItem(Guid id,int quantity)
        {
            
            var product = _productService.GetById(id);
            var cart = ListCart.listCart;
            if(cart!=null)
            {
                var list = (List<CartItem>)cart;
                if(list.Exists(x=>x.Product==product))
                {
                    foreach (var item in list)
                    {
                        if (item.Product == product)
                        {
                            item.Quatity += quantity;
                        }
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.Product= product;
                    item.Quatity = quantity;
                    list.Add(item);
                }
                ListCart.listCart = list;


            }
            else
            {
                //Tạo mới CartItem
                var item = new CartItem();
                item.Product = product;
                item.Quatity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                ListCart.listCart = list;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Payment()
        {
            var cart = ListCart.listCart;
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string ShipName,string Mobile,string Address,string Email)
        {
            var order = new Order();
            order.Id = Guid.NewGuid();
            order.CreateDate = DateTime.Now;
            order.ShipName = ShipName;
            order.ShipMobile = Mobile;
            order.ShipAddress = Address;
            order.ShipEmail = Email;

            var id = _orderService.Insert(order);
            var cart = (List<CartItem>)ListCart.listCart;
            
            foreach (var item in cart)
            {
                var orderDetail =new OrderDetail();
                orderDetail.Id = Guid.NewGuid();
                orderDetail.ProductID = item.Product.Id;
                orderDetail.OrderID = id;
                orderDetail.Price = item.Product.Price;
                orderDetail.Quantity = item.Quatity;
                var detail = _orderDetailService.Insert(orderDetail);

            }
            ListCart.listCart = null;
            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}