using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using FlowerWorld.Models;
using FlowerWorld.Models.AccountViewModels;
using FlowerWorld.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Net;
using Microsoft.AspNetCore.Http;
using FlowerWorld.Infrastructure;

namespace FlowerWorld.Controllers
{
    public class CartController : Controller
    {
        private readonly DBFlowerContext db;
        public CartController(DBFlowerContext _db)
        {
            db = _db;
        }
        //
        // GET: /Cart/
        public ActionResult Index()
        {

            if (Request.Query["retUrl"].ToString() != "")
            {
                ViewBag.continueBuy = Request.Query["retUrl"].ToString();
            }
            else
            {
                ViewBag.continueBuy = Request.Headers["Referer"].ToString();
            }
            ViewBag.contBuy = ViewBag.continueBuy;

            List<int[]> curCart = HttpContext.Session.GetJson<List<int[]>>("Cart");
            List<int[]> curFavi = HttpContext.Session.GetJson<List<int[]>>("Favi");
            if (curCart == null) curCart = new List<int[]>();
            if (curFavi == null) curFavi = new List<int[]>();
            List<CartItem> cart = new List<CartItem>();
            List<CartItem> favi = new List<CartItem>();
            foreach (int[] i in curCart)
            {
                int curId = i[0];
                int curQty = i[1];
                CartItem cartItem = (from p in db.Product
                               where p.ObjId == curId
                               select
                                   new CartItem
                                   {
                                       productName = p.ProductName,
                                       feature = p.Feature,
                                       price = (double)p.Price,
                                       realPrice = (double)
                                           (from pr in db.PriceList where pr.TheProduct == curId && pr.TheCustomerType == 1 select pr.RealPrice).FirstOrDefault<double?>(),
                                       qty = curQty,
                                       smallImg = p.SmallImg 
                                   }).FirstOrDefault<CartItem>();
                cart.Add(cartItem);
            }
            foreach (int[] i in curFavi)
            {
                int curId = i[0];
                int curQty = i[1];
                CartItem cartItem = (from p in db.Product
                                     where p.ObjId == curId
                                     select
                                         new CartItem
                                         {
                                             productName = p.ProductName,
                                             feature = p.Feature,
                                             price = (double)p.Price,
                                             realPrice = (double)
                                                 (from pr in db.PriceList where pr.TheProduct == curId && pr.TheCustomerType == 1 select pr.RealPrice).FirstOrDefault<double?>(),
                                             qty = curQty,
                                             smallImg = p.SmallImg
                                         }).FirstOrDefault<CartItem>();
                favi.Add(cartItem);
            }
            List<ProductCat> productCats = new List<ProductCat>();
            foreach (var pt in db.ProductType.Where<ProductType>(m => m.ObjId > 0).GroupBy<ProductType, string>(m => m.ClassifyType))
            {
                ProductCat pc = new ProductCat();
                pc.typeName = pt.Key;
                pc.types = new List<ProductType>();
                foreach (var p in pt)
                {
                    pc.types.Add(new ProductType { ObjId = p.ObjId, ClassifyType = p.ClassifyType, TypeName = p.TypeName });
                }
                productCats.Add(pc);
            }
            ViewBag.cart = cart;
            ViewBag.favi = favi;
            ViewBag.productCats = productCats;
            return View("Cart");
        }

        public ActionResult AddCart(int id)
        {
            List<int[]> curCart = HttpContext.Session.GetJson<List<int[]>>("Cart");
            if(curCart == null)
                HttpContext.Session.SetJson("Cart", new List<int[]> { new int[] { id, 1 } });
            else
            {
                bool found = false;
                foreach (var p in curCart)
                {
                    if (p[0] == id)
                    {
                        found = true;
                        p[1] += 1;
                        break;
                    }
                }
                if (!found)
                {
                    curCart.Add(new int[] { id, 1 });
                }
                HttpContext.Session.SetJson("Cart", curCart);
            }
            return Index();
        }

        public RedirectResult AddFavi(int id)
        {
            List<int[]> curFavi = HttpContext.Session.GetJson<List<int[]>>("Favi");
            if (curFavi == null)
                HttpContext.Session.SetJson("Favi", new List<int[]> { new int[] { id, 1 } });
            else
            {
                bool found = false;
                foreach (var p in curFavi)
                {
                    if (p[0] == id)
                    {
                        found = true;
                        p[1] += 1;
                        break;
                    }
                }
                if (!found)
                {
                    curFavi.Add(new int[] { id, 1 });
                }
                HttpContext.Session.SetJson("Favi", curFavi);
            }
            
            string continueBuy = Request.Headers["Referer"].ToString();
            if (Request.Query["retUrl"].ToString() != "")
            {
                continueBuy = Request.Query["retUrl"].ToString();
            }
            return Redirect(continueBuy);
        }

        public RedirectResult updateCartRow(int id)
        {
            int value = int.Parse(Request.Query["value"].ToString());
            List<int[]> curCart = HttpContext.Session.GetJson<List<int[]>>("Cart");
            curCart[id][1] = value;
            HttpContext.Session.SetJson("Cart", curCart);
            return Redirect("/Cart?retUrl=" + Request.Query["retUrl"].ToString());
        }
        public RedirectResult deleCartRow(int id)
        {
            List<int[]> curCart = HttpContext.Session.GetJson< List < int[] >>("Cart");
            curCart.RemoveAt(id);
            HttpContext.Session.SetJson("Cart", curCart);
            return Redirect("/Cart?retUrl=" + Request.Query["retUrl"].ToString());
        }
        public RedirectResult storeCartRow(int id)
        {
            List<int[]> curCart = HttpContext.Session.GetJson< List < int[] >>("Cart");
            List<int[]> curFavi = HttpContext.Session.GetJson< List < int[] >>("Favi");
            if (curFavi == null)
            {
                curFavi = new List<int[]>(); 
                HttpContext.Session.SetJson("Favi", curFavi);
            }
            curFavi.Add(curCart[id]);
            curCart.RemoveAt(id);
            HttpContext.Session.SetJson("Cart", curCart);
            HttpContext.Session.SetJson("Favi", curFavi);
            return Redirect("/Cart?retUrl=" + Request.Query["retUrl"].ToString());
        }
        public RedirectResult deleFaviRow(int id)
        {
            List<int[]> curFavi = HttpContext.Session.GetJson<List<int[]>>("Favi");
            curFavi.RemoveAt(id);
            HttpContext.Session.SetJson("Favi", curFavi);
            return Redirect("/Cart?retUrl=" + Request.Query["retUrl"].ToString());
        }
        public RedirectResult buyFaviRow(int id)
        {
            List<int[]> curCart = HttpContext.Session.GetJson<List<int[]>>("Cart");
            List<int[]> curFavi = HttpContext.Session.GetJson<List<int[]>>("Favi");
            curCart.Add(curFavi[id]);
            curFavi.RemoveAt(id);
            HttpContext.Session.SetJson("Cart", curCart);
            HttpContext.Session.SetJson("Favi", curFavi);
            return Redirect("/Cart?retUrl=" + Request.Query["retUrl"].ToString());
        }
    }
}
