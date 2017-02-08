using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlowerWorld.Models;
using System.Net;

namespace FlowerWorld.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBFlowerContext db;

        public HomeController(DBFlowerContext flowerdb)
        {
            db = flowerdb;
        }

        public IActionResult Index()
        {
            //新闻仍然采用固定的，因为表中没有数据。
            ViewBag.news = new[] { "新年送花留言", "情人节的由来", "古今鲜花浪漫故事", "如何让鲜花保存更长时间", "如何给鲜花浇水", "元宵花灯节", "七夕的传说", "不同场合送花提示" };
            ViewBag.Title = "主页";
            ViewBag.contBuy = "/";

            //FlowerDbContext db = new FlowerDbContext();
            HomeIndexViewModel ivm = new HomeIndexViewModel();
            ivm.hotProducts = new List<ProductList>();
            ivm.recProducts = new List<ProductList>();
            ivm.productCats = new List<ProductCat>();
            //获取鲜花分类信息(按照分类名分组)
            foreach (var pt in db.ProductType.Where<ProductType>(m => m.ObjId > 0).GroupBy<ProductType, string>(m => m.ClassifyType))
            {
                ProductCat pc = new ProductCat();
                pc.typeName = pt.Key;
                pc.types = new List<ProductType>();
                foreach (var p in pt)
                {
                    pc.types.Add(new ProductType { ObjId = p.ObjId, ClassifyType = p.ClassifyType, TypeName = p.TypeName });
                }
                ivm.productCats.Add(pc);
            }
            //ViewBag.flowerClassify = from pt in db.ProductType where pt.objId>0 group pt by (pt.classifyType);   

            //获取热销和推荐商品（各6种）
            var hotProducts = db.Product.Where<Product>(m => m.ObjId > 0).OrderBy<Product, float>(m => (float)m.Price).Take<Product>(6);
            foreach (var p in hotProducts)
            {
                ProductList pl = new ProductList();
                pl.p = new Product { ObjId = p.ObjId, ProductName = p.ProductName, BigImg = p.BigImg, Price = p.Price };
                pl.pList = new List<Prices>();
                var priceList = db.PriceList.Where<PriceList>(m => m.TheProduct == p.ObjId);
                foreach (var pclst in priceList)
                {
                    pl.pList.Add(new Prices
                    {
                        memberName = db.CustomerType.Where<CustomerType>(m => m.ObjId == pclst.TheCustomerType).First<CustomerType>().TypeName,
                        realPrice = (double)pclst.RealPrice
                    });
                }
                ivm.hotProducts.Add(pl);
            }
            var recProducts = db.Product.Where<Product>(m => m.ObjId > 0).OrderByDescending<Product, float>(m => (float)m.Price).Take<Product>(6);
            foreach (var p in recProducts)
            {
                ProductList pl = new ProductList();
                pl.p = new Product { ObjId = p.ObjId, ProductName = p.ProductName, BigImg = p.BigImg, Price = p.Price };
                pl.pList = new List<Prices>();
                var priceList = db.PriceList.Where<PriceList>(m => m.TheProduct == p.ObjId);
                foreach (var pclst in priceList)
                {
                    pl.pList.Add(new Prices
                    {
                        memberName = db.CustomerType.Where<CustomerType>(m => m.ObjId == pclst.TheCustomerType).First<CustomerType>().TypeName,
                        realPrice = (double)pclst.RealPrice
                    });
                }
                ivm.recProducts.Add(pl);
            }
            /*
            ViewBag.hotProducts = from p in hotProducts
                                    select new
                                    {
                                        objId = p.objId, productName = p.productName, bigImg = p.bigImg, price = p.price,
                                        prices = from price in db.PriceList
                                                where price.theProduct == p.objId
                                                join tp in db.CustomerType on price.theCustomerType equals tp.objId
                                                select new
                                                {
                                                    typeName = tp.typeName,
                                                    realPrice = price.realPrice
                                                }
                                    };
            ViewBag.recProducts = from p in recProducts
                                    select new
                                    {
                                        objId = p.objId,
                                        productName = p.productName,
                                        bigImg = p.bigImg,
                                        price = p.price,
                                        prices = from price in db.PriceList
                                                where price.theProduct == p.objId
                                                join tp in db.CustomerType on price.theCustomerType equals tp.objId
                                                select new
                                                {
                                                    typeName = tp.typeName,
                                                    realPrice = price.realPrice
                                                }
                                    };
             */
            return View(ivm);
        }

        public IActionResult Catalog(int typeId, string typeName)
        {
            ViewBag.catalogName = typeName;
            //FlowerDbContext db = new FlowerDbContext();
            List<ProductCat> productCats = new List<ProductCat>();
            List<ProductList> hotProducts = new List<ProductList>();
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
            var products = from p in db.Product where p.ProductState == 1 && (from t in db.ProductClass where t.TheProductType == typeId select t.TheProduct).Contains(p.ObjId) select p;
            foreach (var p in products)
            {
                ProductList pl = new ProductList();
                pl.p = new Product { ObjId = p.ObjId, ProductName = p.ProductName, BigImg = p.BigImg, Price = p.Price };
                pl.pList = new List<Prices>();
                var priceList = db.PriceList.Where<PriceList>(m => m.TheProduct == p.ObjId);
                foreach (var pclst in priceList)
                {
                    pl.pList.Add(new Prices
                    {
                        memberName = db.CustomerType.Where<CustomerType>(m => m.ObjId == pclst.TheCustomerType).First<CustomerType>().TypeName,
                        realPrice = (double)pclst.RealPrice
                    });
                }
                hotProducts.Add(pl);
            }
            ViewBag.productCats = productCats;
            ViewBag.catProducts = hotProducts;
            ViewBag.contBuy = Request.Path+Request.QueryString;
            return View();
        }

        public IActionResult Detail(int? id)
        {
            ViewBag.contBuy = Request.Headers["Referer"].ToString();
            //FlowerDbContext db = new FlowerDbContext();
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
            ViewBag.productCats = productCats;

            ProductList pl = new ProductList();
            pl.p = db.Product.Single<Product>(m => m.ObjId == id);
            pl.pList = new List<Prices>();
            var priceList = db.PriceList.Where<PriceList>(m => m.TheProduct == pl.p.ObjId);
            foreach (var pclst in priceList)
            {
                pl.pList.Add(new Prices
                {
                    memberName = db.CustomerType.Where<CustomerType>(m => m.ObjId == pclst.TheCustomerType).First<CustomerType>().TypeName,
                    realPrice = (double)pclst.RealPrice
                });
            }
            return View(pl);
        }

        public IActionResult About()
        {
            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "你的联系方式页。";

            return View();
        }
        /****************
        public IActionResult Index()
        {
            ViewBag.contBuy = "/";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        ***************/
    }
}
