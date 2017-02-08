﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlowerWorld.Models
{
    public partial class DBFlowerContext : DbContext
    {
        public DBFlowerContext(DbContextOptions<DBFlowerContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__Area__19DFD96B");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.TheCity).HasColumnName("theCity");

                entity.HasOne(d => d.TheCityNavigation)
                    .WithMany(p => p.Area)
                    .HasForeignKey(d => d.TheCity)
                    .HasConstraintName("FK__Area__theCity__1AD3FDA4");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.ProviderKey).HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_AspNetUserRoles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetUserRoles_RoleId");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserRoles_UserId");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserTokens");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__City__5441852A");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("nchar(14)");

                entity.Property(e => e.TheProvince).HasColumnName("theProvince");

                entity.HasOne(d => d.TheProvinceNavigation)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.TheProvince)
                    .HasConstraintName("FK__City__theProvinc__5535A963");
            });

            modelBuilder.Entity<Consignee>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__Consignee__1CBC4616");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.DoorNumber)
                    .HasColumnName("doorNumber")
                    .HasColumnType("nchar(12)");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.HomePhone)
                    .HasColumnName("homePhone")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.MobilePhone)
                    .HasColumnName("mobilePhone")
                    .HasColumnType("nchar(18)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("nchar(16)");

                entity.Property(e => e.OfficePhone)
                    .HasColumnName("officePhone")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.QqNumber)
                    .HasColumnName("qqNumber")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.RoadName)
                    .HasColumnName("roadName")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.StreetName)
                    .HasColumnName("streetName")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.TheArea).HasColumnName("theArea");

                entity.Property(e => e.TheCustomer).HasColumnName("theCustomer");

                entity.Property(e => e.ZipCode).HasColumnName("zipCode");

                entity.HasOne(d => d.TheAreaNavigation)
                    .WithMany(p => p.Consignee)
                    .HasForeignKey(d => d.TheArea)
                    .HasConstraintName("FK__Consignee__theAr__1DB06A4F");

                entity.HasOne(d => d.TheCustomerNavigation)
                    .WithMany(p => p.Consignee)
                    .HasForeignKey(d => d.TheCustomer)
                    .HasConstraintName("FK__Consignee__theCu__1B29035F");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__tmp_ms_x__530A638C18D5C261");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(512);

                entity.Property(e => e.HomePhone)
                    .HasColumnName("homePhone")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.MobilePhone)
                    .HasColumnName("mobilePhone")
                    .HasColumnType("nchar(18)");

                entity.Property(e => e.OfficePhone)
                    .HasColumnName("officePhone")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.QqNumber)
                    .HasColumnName("qqNumber")
                    .HasColumnType("nchar(12)");

                entity.Property(e => e.RegistDate)
                    .HasColumnName("registDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.TheCustomerType).HasColumnName("theCustomerType");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userId")
                    .HasMaxLength(256);

                entity.Property(e => e.UserName)
                    .HasColumnName("userName")
                    .HasMaxLength(256);

                entity.HasOne(d => d.TheCustomerTypeNavigation)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.TheCustomerType)
                    .HasConstraintName("FK__Customer__theCus__5CD6CB2B");
            });

            modelBuilder.Entity<CustomerType>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__CustomerType__59FA5E80");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.MinSpending).HasColumnName("minSpending");

                entity.Property(e => e.TypeName)
                    .HasColumnName("typeName")
                    .HasColumnType("nchar(10)");
            });

            modelBuilder.Entity<CustomerWords>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__CustomerWords__489AC854");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.TheOrder).HasColumnName("theOrder");

                entity.Property(e => e.Words)
                    .HasColumnName("words")
                    .HasMaxLength(500);

                entity.HasOne(d => d.TheOrderNavigation)
                    .WithMany(p => p.CustomerWords)
                    .HasForeignKey(d => d.TheOrder)
                    .HasConstraintName("FK__CustomerW__theOr__498EEC8D");
            });

            modelBuilder.Entity<Division>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__Division__208CD6FA");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.DoorNumber)
                    .HasColumnName("doorNumber")
                    .HasColumnType("nchar(12)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.RoadName)
                    .HasColumnName("roadName")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.StreeName)
                    .HasColumnName("streeName")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.TheArea).HasColumnName("theArea");

                entity.Property(e => e.ZipCode).HasColumnName("zipCode");

                entity.HasOne(d => d.TheAreaNavigation)
                    .WithMany(p => p.Division)
                    .HasForeignKey(d => d.TheArea)
                    .HasConstraintName("FK__Division__theAre__2180FB33");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__News__3864608B");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasMaxLength(2000);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("nchar(40)");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__Order__40058253");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.Amt).HasColumnName("amt");

                entity.Property(e => e.OrderState).HasColumnName("orderState");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("orderTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TheClerk).HasColumnName("theClerk");

                entity.Property(e => e.TheConsignee).HasColumnName("theConsignee");

                entity.Property(e => e.TheCustomer).HasColumnName("theCustomer");

                entity.Property(e => e.TheDeliverer).HasColumnName("theDeliverer");

                entity.Property(e => e.ThePayment).HasColumnName("thePayment");

                entity.Property(e => e.TheProduct).HasColumnName("theProduct");

                entity.HasOne(d => d.TheClerkNavigation)
                    .WithMany(p => p.OrderTheClerkNavigation)
                    .HasForeignKey(d => d.TheClerk)
                    .HasConstraintName("FK__Order__theClerk__45BE5BA9");

                entity.HasOne(d => d.TheConsigneeNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.TheConsignee)
                    .HasConstraintName("FK__Order__theConsig__44CA3770");

                entity.HasOne(d => d.TheCustomerNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.TheCustomer)
                    .HasConstraintName("FK__Order__theCustom__1C1D2798");

                entity.HasOne(d => d.TheDelivererNavigation)
                    .WithMany(p => p.OrderTheDelivererNavigation)
                    .HasForeignKey(d => d.TheDeliverer)
                    .HasConstraintName("FK__Order__theDelive__46B27FE2");

                entity.HasOne(d => d.ThePaymentNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.ThePayment)
                    .HasConstraintName("FK__Order__thePaymen__42E1EEFE");

                entity.HasOne(d => d.TheProductNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.TheProduct)
                    .HasConstraintName("FK__Order__theProduc__41EDCAC5");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__Payment__787EE5A0");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.AccountName)
                    .HasColumnName("accountName")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.AccountNo)
                    .HasColumnName("accountNo")
                    .HasColumnType("nchar(24)");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.PaymentState).HasColumnName("paymentState");

                entity.Property(e => e.ThePaymentType).HasColumnName("thePaymentType");

                entity.Property(e => e.TransNo)
                    .HasColumnName("transNo")
                    .HasColumnType("nchar(16)");

                entity.Property(e => e.TransTime)
                    .HasColumnName("transTime")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.ThePaymentTypeNavigation)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.ThePaymentType)
                    .HasConstraintName("FK__Payment__thePaym__797309D9");
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__PaymentType__76969D2E");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.BigImg)
                    .HasColumnName("bigImg")
                    .HasMaxLength(80);

                entity.Property(e => e.MethodName)
                    .HasColumnName("methodName")
                    .HasColumnType("nchar(50)");

                entity.Property(e => e.SmallImg)
                    .HasColumnName("smallImg")
                    .HasMaxLength(80);

                entity.Property(e => e.TypeName)
                    .HasColumnName("typeName")
                    .HasColumnType("nchar(16)");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<PriceList>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__PriceList__6D0D32F4");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.RealPrice).HasColumnName("realPrice");

                entity.Property(e => e.TheCustomerType).HasColumnName("theCustomerType");

                entity.Property(e => e.TheProduct).HasColumnName("theProduct");

                entity.HasOne(d => d.TheCustomerTypeNavigation)
                    .WithMany(p => p.PriceList)
                    .HasForeignKey(d => d.TheCustomerType)
                    .HasConstraintName("FK__PriceList__theCu__6EF57B66");

                entity.HasOne(d => d.TheProductNavigation)
                    .WithMany(p => p.PriceList)
                    .HasForeignKey(d => d.TheProduct)
                    .HasConstraintName("FK__PriceList__thePr__6E01572D");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__Product__6B24EA82");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.BigImg)
                    .HasColumnName("bigImg")
                    .HasMaxLength(80);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.Feature)
                    .HasColumnName("feature")
                    .HasMaxLength(200);

                entity.Property(e => e.Meaning)
                    .HasColumnName("meaning")
                    .HasMaxLength(500);

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ProductId)
                    .HasColumnName("productId")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.ProductName)
                    .HasColumnName("productName")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.ProductState).HasColumnName("productState");

                entity.Property(e => e.SmallImg)
                    .HasColumnName("smallImg")
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<ProductClass>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__ProductClass__151B244E");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.TheProduct).HasColumnName("theProduct");

                entity.Property(e => e.TheProductType).HasColumnName("theProductType");

                entity.HasOne(d => d.TheProductNavigation)
                    .WithMany(p => p.ProductClass)
                    .HasForeignKey(d => d.TheProduct)
                    .HasConstraintName("FK__ProductCl__thePr__160F4887");

                entity.HasOne(d => d.TheProductTypeNavigation)
                    .WithMany(p => p.ProductClass)
                    .HasForeignKey(d => d.TheProductType)
                    .HasConstraintName("FK__ProductCl__thePr__17036CC0");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__ProductType__1332DBDC");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.ClassifyType)
                    .HasColumnName("classifyType")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.TypeName)
                    .HasColumnName("typeName")
                    .HasColumnType("nchar(20)");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__Province__52593CB8");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("nchar(10)");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__Receipt__4B7734FF");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.ReceiptFile)
                    .HasColumnName("receiptFile")
                    .HasMaxLength(500);

                entity.Property(e => e.TheOrder).HasColumnName("theOrder");

                entity.HasOne(d => d.TheOrderNavigation)
                    .WithMany(p => p.Receipt)
                    .HasForeignKey(d => d.TheOrder)
                    .HasConstraintName("FK__Receipt__theOrde__4C6B5938");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__Role__628FA481");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("nchar(10)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ObjId)
                    .HasName("PK__User__236943A5");

                entity.Property(e => e.ObjId).HasColumnName("objId");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.HomePhone)
                    .HasColumnName("homePhone")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.MobilePhone)
                    .HasColumnName("mobilePhone")
                    .HasColumnType("nchar(18)");

                entity.Property(e => e.OfficePhone)
                    .HasColumnName("officePhone")
                    .HasColumnType("nchar(20)");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("nchar(16)");

                entity.Property(e => e.QqNumber)
                    .HasColumnName("qqNumber")
                    .HasColumnType("nchar(12)");

                entity.Property(e => e.TheDivision).HasColumnName("theDivision");

                entity.Property(e => e.TheRole).HasColumnName("theRole");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("nchar(10)");

                entity.Property(e => e.UserName)
                    .HasColumnName("userName")
                    .HasColumnType("nchar(16)");

                entity.Property(e => e.UserState).HasColumnName("userState");

                entity.HasOne(d => d.TheDivisionNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.TheDivision)
                    .HasConstraintName("FK__User__theDivisio__245D67DE");

                entity.HasOne(d => d.TheRoleNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.TheRole)
                    .HasConstraintName("FK__User__theRole__25518C17");
            });
        }

        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Consignee> Consignee { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerType> CustomerType { get; set; }
        public virtual DbSet<CustomerWords> CustomerWords { get; set; }
        public virtual DbSet<Division> Division { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<PaymentType> PaymentType { get; set; }
        public virtual DbSet<PriceList> PriceList { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductClass> ProductClass { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Receipt> Receipt { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}