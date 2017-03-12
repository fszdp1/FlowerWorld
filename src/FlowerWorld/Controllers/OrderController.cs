using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FlowerWorld.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Http;
using FlowerWorld.Infrastructure;
using Newtonsoft.Json;

namespace FlowerWorld.Controllers
{
    public class OrderController : Controller
    {
        private readonly DBFlowerContext db;
        public OrderController(DBFlowerContext _db)
        {
            db = _db;
        }
        //
        // GET: /Order/
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Request = Request;
            string uid = User.Identity.Name;
            OrderViewModel ovm = new OrderViewModel();
            ovm.orders = new List<OrderInfo>();
            ovm.receivers = new List<Consignee>();
            ovm.words = new List<CustomerWords>();
            ovm.payment = new Payment();
            //获取信息以显示在页面
            ovm.curCustomer = db.Customer.Single(m => m.UserName == uid);
            ViewBag.payments = db.PaymentType.Where(m => m.ObjId > 0).ToArray<PaymentType>();
            List<int[]> curCart = HttpContext.Session.GetJson< List < int[] >>("Cart");
            ovm.orderQty = 0;
            ovm.payment.Amount = 0.0;
            foreach (var cartItem in curCart)
            {
                ovm.orderQty += cartItem[1];
                int pObjId = cartItem[0];
                for(int i=0;i<cartItem[1];i++)
                {
                    var product = db.Product.Single(m => m.ObjId == pObjId);
                    var price = db.PriceList.Single(m => m.TheProduct == pObjId && m.TheCustomerType == ovm.curCustomer.TheCustomerType);
                    ovm.orders.Add(new OrderInfo{theProduct=product.ObjId,price=(double)product.Price,realPrice=(double)price.RealPrice,productName=product.ProductName,productFeature=product.Feature,smallImg=product.SmallImg});
                    ovm.receivers.Add(new Consignee());
                    ovm.words.Add(new CustomerWords());
                    ovm.payment.Amount += price.RealPrice;
                }
            }
            return View("Order",ovm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(OrderViewModel ovm)
        {
            ViewBag.Request = Request;
            //更新客户联系信息
            Customer curCust = db.Customer.Single(m => m.ObjId == ovm.curCustomer.ObjId);
            //var manager = new UserManager<ApplicationUser, int>(new UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(new ApplicationDbContext()));
            //var c = manager.FindById<ApplicationUser, int>(ovm.curCustomer.objId);
            if (curCust.MobilePhone != ovm.curCustomer.MobilePhone && ovm.curCustomer.MobilePhone != "")
                curCust.MobilePhone = ovm.curCustomer.MobilePhone;
            if (curCust.HomePhone != ovm.curCustomer.HomePhone && ovm.curCustomer.HomePhone != "")
                curCust.HomePhone = ovm.curCustomer.HomePhone;
            if (curCust.OfficePhone != ovm.curCustomer.OfficePhone && ovm.curCustomer.OfficePhone != "")
                curCust.OfficePhone = ovm.curCustomer.OfficePhone;
            if (curCust.Email != ovm.curCustomer.Email && ovm.curCustomer.Email != "")
                curCust.Email = ovm.curCustomer.Email;
            if (curCust.QqNumber != ovm.curCustomer.QqNumber && ovm.curCustomer.QqNumber != "")
                curCust.QqNumber = ovm.curCustomer.QqNumber;
            //manager.Update(c);
            db.SaveChanges();
            //保存订单。需做事务处理！在.NET EF core中，一个SaveChange方法所提交的内容会自动实现事务处理。
            bool succeed = true;
            int payId = 0;
            int curZip;
            try
            {
                //using (TransactionScope ts = new TransactionScope())
                //{
                    EntityEntry<Payment> p = db.Payment.Add(new Payment());
                    p.Entity.Amount = double.Parse(Request.Form["paymentAmt"]);
                    p.Entity.ThePaymentType = int.Parse(Request.Form["paymentType"]);
                    p.Entity.PaymentState = 0;
                    
                    for (int i = 0; i < ovm.orderQty; i++)
                    {
                        EntityEntry<Consignee> cons = db.Consignee.Add(new Consignee());
                        cons.Entity.TheArea = int.Parse(Request.Form["selDist_" + i]);
                        cons.Entity.TheCustomer = curCust.ObjId;
                        cons.Entity.Name = Request.Form["name_" + i].ToString().Trim();
                        cons.Entity.RoadName = Request.Form["road_" + i].ToString().Trim();
                        cons.Entity.StreetName = Request.Form["street_" + i].ToString().Trim();
                        cons.Entity.DoorNumber = Request.Form["door_" + i].ToString().Trim();
                        if (int.TryParse(Request.Form["zip_" + i].ToString().Trim(), out curZip))
                        {
                            cons.Entity.ZipCode = curZip;
                        }
                        cons.Entity.MobilePhone = Request.Form["mobile_" + i].ToString().Trim();
                        cons.Entity.HomePhone = Request.Form["home_" + i].ToString().Trim();
                        cons.Entity.OfficePhone = Request.Form["office_" + i].ToString().Trim();
                        cons.Entity.Email = Request.Form["email_" + i].ToString().Trim();
                        cons.Entity.QqNumber = Request.Form["QQ_" + i].ToString().Trim();
                        EntityEntry<Order> o = db.Order.Add(new Order());
                        o.Entity.ThePayment = p.Entity.ObjId;
                        o.Entity.TheConsignee = cons.Entity.ObjId;
                        o.Entity.TheCustomer = curCust.ObjId;
                        o.Entity.TheProduct = int.Parse(Request.Form["productId_" + i].ToString().Trim());
                        o.Entity.OrderState = 0;
                        o.Entity.OrderTime = DateTime.Now;
                        o.Entity.Amt = double.Parse(Request.Form["realPrice_" + i].ToString().Trim());
                        if (Request.Form["sendWord_" + i].ToString().Trim() != "")
                        {
                            EntityEntry<CustomerWords> cw = db.CustomerWords.Add(new CustomerWords());
                            cw.Entity.TheOrder = o.Entity.ObjId;
                            cw.Entity.Words = Request.Form["sendWord_" + i].ToString().Trim();
                        }
                    //}
                    db.SaveChanges();
                    payId = p.Entity.ObjId;
                    //ts.Complete();
                }
            }
            catch
            {
                succeed = false;
                await Response.WriteAsync("<script>alert('数据未成功保存，请重新尝试！');</script>");
            }
            if (succeed)
            {//进入支付处理
                string paymentUrl = "", paymentMethod = "";
                foreach (PaymentType pt in db.PaymentType.Where(m => m.ObjId > 0).ToArray<PaymentType>())
                {
                    if (pt.ObjId == int.Parse(Request.Form["paymentType"]))
                    {
                        paymentUrl = pt.Url;
                        paymentMethod = pt.MethodName;
                        break;
                    }
                }
                string merchantId = "Flower001";
                string returnUrl = "http://" + Request.Host + Url.Action("Index", "Payment");
                string amtStr = Request.Form["paymentAmt"];
                string merTransId = payId.ToString();
                //根据paymentMethod产生提交付款的对象并提交。以下为暂时的写法，无扩展性。
                //理想的写法是定义付款接口，针对不同的付款机构，写一个实现了接口的对应的付款类
                //在这里根据方法名构建付款对象，然后再调用接口方法实现付款。
                //这里写法是固定的，暂时使用。
                await RemotePost.PaymentPost(HttpContext, paymentUrl, merchantId, returnUrl, Request.Form["paymentType"], amtStr, merTransId);
            }
           
                //如果未能成功保存数据则执行以下行。由于ovm中未能将原来的order等数据带回，这里要重新获取
                ovm.orders = new List<OrderInfo>();
                ovm.receivers = new List<Consignee>();
                ovm.words = new List<CustomerWords>();
                ovm.payment = new Payment();
                //获取信息以显示在页面
                ViewBag.payments = db.PaymentType.Where(m => m.ObjId > 0).ToArray<PaymentType>();
                List<int[]> curCart = HttpContext.Session.GetJson<List<int[]>>("Cart");
                ovm.orderQty = 0;
                ovm.payment.Amount = 0.0;
                foreach (var cartItem in curCart)
                {
                    ovm.orderQty += cartItem[1];
                    int pObjId = cartItem[0];
                    for (int i = 0; i < cartItem[1]; i++)
                    {
                        var product = db.Product.Single(m => m.ObjId == pObjId);
                        var price = db.PriceList.Single(m => m.TheProduct == pObjId && m.TheCustomerType == ovm.curCustomer.TheCustomerType);
                        ovm.orders.Add(new OrderInfo { theProduct = product.ObjId, price = (double)product.Price, realPrice = (double)price.RealPrice, productName = product.ProductName, productFeature = product.Feature, smallImg = product.SmallImg });
                        ovm.receivers.Add(new Consignee());
                        ovm.words.Add(new CustomerWords());
                        ovm.payment.Amount += price.RealPrice;
                    }
                }
                return View("Order", ovm);
            
        }

        //该方法由ajax调用
        public void getProvinces()
        {
            var provinces = from ct in db.Province where ct.ObjId!=0 select new { ct.ObjId, ct.Name };
            Response.ContentType = "text/plain";
            Response.WriteAsync(JsonConvert.SerializeObject(provinces));
        }
        //该方法由ajax调用
        public void getCities(int id)
        {
            var cities = from ct in db.City where ct.TheProvince == id select new { ct.ObjId, ct.Name }; 
            Response.ContentType = "text/plain";
            Response.WriteAsync(JsonConvert.SerializeObject(cities));
        }
        //该方法由ajax调用
        public void getDistricts(int id)
        {
            var dists = from ct in db.Area where ct.TheCity == id select new { ct.ObjId, ct.Name };
            Response.ContentType = "text/plain";
            Response.WriteAsync(JsonConvert.SerializeObject(dists));
        }
    }
}
