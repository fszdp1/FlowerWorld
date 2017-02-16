using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlowerWorld.Models.AccountViewModels;

namespace FlowerWorld.Models
{
    public class HomeIndexViewModel
    {
        public List<ProductList> hotProducts { get; set; }
        public List<ProductList> recProducts { get; set; }
        public List<ProductCat> productCats { get; set; }
    }
    public class ProductList
    {
        public Product p { get; set; }
        public List<Prices> pList { get; set; }
    }
    public class ProductCat
    {
        public string typeName { get; set; }
        public List<ProductType> types { get; set; }
    }
    public class Prices
    {
        public string memberName { get; set; }
        public double realPrice { get; set; }
    }
    public class CartItem
    {
        public string productName { get; set; }
        public string feature { get; set; }
        public double price { get; set; }
        public double realPrice { get; set; }
        public int qty { get; set; }
        public string smallImg { get; set; }
    }

    public class OrderList
    {
        public DateTime orderTime { get; set; }
        public double amt { get; set; }
        public string orderState { get; set; }
        public string productName { get; set; }
        public string smallImg { get; set; }
        public DateTime transTime { get; set; }
        public string name { get; set; }
        public string words { get; set; }
        public string receiptFile { get; set; }
    }
    public class OrderInfo
    {
        public double price { get; set; }
        public double realPrice { get; set; }
        public int theProduct { get; set; }
        public string productName { get; set; }
        public string productFeature { get; set; }
        public string smallImg { get; set; }
    }

    public class MemberHomeModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
        //public LocalPasswordModel PassWordModel { get; set; }
        public RegisterModel CustomerInfo { get; set; }
        public List<OrderList> Orders { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

    public class OrderViewModel
    {
        public Customer curCustomer { get; set; }
        public Payment payment { get; set; }
        public List<OrderInfo> orders { get; set; }
        public List<Consignee> receivers { get; set; }
        public List<CustomerWords> words { get; set; }
        public int orderQty { get; set; }
    }
    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
    
}
