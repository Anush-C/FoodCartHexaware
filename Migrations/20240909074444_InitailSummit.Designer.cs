﻿// <auto-generated />
using System;
using FoodCart_Hexaware.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FoodCart_Hexaware.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240909074444_InitailSummit")]
    partial class InitailSummit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FoodCart_Hexaware.Models.CardPayment", b =>
                {
                    b.Property<int>("CardPaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CardPaymentId"));

                    b.Property<string>("CardHolderName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.HasKey("CardPaymentId");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.ToTable("CardPayment");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Cart", b =>
                {
                    b.Property<int>("CartID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeliveryAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("ItemID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("CartID");

                    b.HasIndex("ItemID");

                    b.HasIndex("UserID");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.DeliveryAgent", b =>
                {
                    b.Property<int>("DeliveryAgentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeliveryAgentID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("UsersUserID")
                        .HasColumnType("int");

                    b.HasKey("DeliveryAgentID");

                    b.HasIndex("UsersUserID");

                    b.ToTable("DeliveryAgents");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.MenuCategory", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryID"));

                    b.Property<string>("CategoryDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CategoryID");

                    b.ToTable("MenuCategories");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.MenuItems", b =>
                {
                    b.Property<int>("ItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemID"));

                    b.Property<string>("AvailabilityStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("CuisineType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DietaryInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ingredients")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("ItemPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("TasteInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ItemID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.NetBankingPayment", b =>
                {
                    b.Property<int>("NetBankingPaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NetBankingPaymentId"));

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.HasKey("NetBankingPaymentId");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.ToTable("NetBankingPayment");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Notification", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationID"));

                    b.Property<int>("ItemID")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NotificationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("NotificationType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RestID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("NotificationID");

                    b.HasIndex("ItemID");

                    b.HasIndex("RestID");

                    b.HasIndex("UserID");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.OrderItems", b =>
                {
                    b.Property<int>("OrderItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderItemID"));

                    b.Property<decimal?>("Discount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("ItemID")
                        .HasColumnType("int");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderItemID");

                    b.HasIndex("ItemID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Orders", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderID"));

                    b.Property<int?>("DeliveryAgentID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeliveryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("RestaurantID")
                        .HasColumnType("int");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("OrderID");

                    b.HasIndex("DeliveryAgentID");

                    b.HasIndex("RestaurantID");

                    b.HasIndex("UserID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"));

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<int?>("OrdersOrderID")
                        .HasColumnType("int");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("TransDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("PaymentId");

                    b.HasIndex("OrdersOrderID");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Restaurant", b =>
                {
                    b.Property<int>("RestaurantID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RestaurantID"));

                    b.Property<TimeSpan>("ClosingHours")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("OpeningHours")
                        .HasColumnType("time");

                    b.Property<string>("RestaurantAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("RestaurantDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RestaurantEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RestaurantName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RestaurantPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RestaurantID");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.UpiPayment", b =>
                {
                    b.Property<int>("UpiPaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UpiPaymentId"));

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<string>("UpiId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UpiPaymentId");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.ToTable("UpiPayment");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Users", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MenuItemsRestaurant", b =>
                {
                    b.Property<int>("MenuItemsItemID")
                        .HasColumnType("int");

                    b.Property<int>("RestaurantsRestaurantID")
                        .HasColumnType("int");

                    b.HasKey("MenuItemsItemID", "RestaurantsRestaurantID");

                    b.HasIndex("RestaurantsRestaurantID");

                    b.ToTable("MenuItemsRestaurant");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.CardPayment", b =>
                {
                    b.HasOne("FoodCart_Hexaware.Models.Payment", "Payment")
                        .WithOne("CardPayment")
                        .HasForeignKey("FoodCart_Hexaware.Models.CardPayment", "PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Cart", b =>
                {
                    b.HasOne("FoodCart_Hexaware.Models.MenuItems", "MenuItems")
                        .WithMany("Carts")
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodCart_Hexaware.Models.Users", "Users")
                        .WithMany("Carts")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenuItems");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.DeliveryAgent", b =>
                {
                    b.HasOne("FoodCart_Hexaware.Models.Users", null)
                        .WithMany("DeliveryAgents")
                        .HasForeignKey("UsersUserID");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.MenuItems", b =>
                {
                    b.HasOne("FoodCart_Hexaware.Models.MenuCategory", "MenuCategory")
                        .WithMany("MenuItems")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenuCategory");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.NetBankingPayment", b =>
                {
                    b.HasOne("FoodCart_Hexaware.Models.Payment", "Payment")
                        .WithOne("NetBankingPayment")
                        .HasForeignKey("FoodCart_Hexaware.Models.NetBankingPayment", "PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Notification", b =>
                {
                    b.HasOne("FoodCart_Hexaware.Models.MenuItems", "MenuItems")
                        .WithMany("Notifications")
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodCart_Hexaware.Models.Restaurant", "Restaurant")
                        .WithMany("Notifications")
                        .HasForeignKey("RestID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodCart_Hexaware.Models.Users", "Users")
                        .WithMany("Notifications")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenuItems");

                    b.Navigation("Restaurant");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.OrderItems", b =>
                {
                    b.HasOne("FoodCart_Hexaware.Models.MenuItems", "MenuItems")
                        .WithMany("OrderItems")
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodCart_Hexaware.Models.Orders", "Orders")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenuItems");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Orders", b =>
                {
                    b.HasOne("FoodCart_Hexaware.Models.DeliveryAgent", "DeliveryAgent")
                        .WithMany("Orders")
                        .HasForeignKey("DeliveryAgentID");

                    b.HasOne("FoodCart_Hexaware.Models.Restaurant", "Restaurant")
                        .WithMany("Orders")
                        .HasForeignKey("RestaurantID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodCart_Hexaware.Models.Users", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeliveryAgent");

                    b.Navigation("Restaurant");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Payment", b =>
                {
                    b.HasOne("FoodCart_Hexaware.Models.Orders", "Orders")
                        .WithMany()
                        .HasForeignKey("OrdersOrderID");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.UpiPayment", b =>
                {
                    b.HasOne("FoodCart_Hexaware.Models.Payment", "Payment")
                        .WithOne("UpiPayment")
                        .HasForeignKey("FoodCart_Hexaware.Models.UpiPayment", "PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("MenuItemsRestaurant", b =>
                {
                    b.HasOne("FoodCart_Hexaware.Models.MenuItems", null)
                        .WithMany()
                        .HasForeignKey("MenuItemsItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodCart_Hexaware.Models.Restaurant", null)
                        .WithMany()
                        .HasForeignKey("RestaurantsRestaurantID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.DeliveryAgent", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.MenuCategory", b =>
                {
                    b.Navigation("MenuItems");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.MenuItems", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Notifications");

                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Orders", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Payment", b =>
                {
                    b.Navigation("CardPayment");

                    b.Navigation("NetBankingPayment");

                    b.Navigation("UpiPayment");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Restaurant", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("FoodCart_Hexaware.Models.Users", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("DeliveryAgents");

                    b.Navigation("Notifications");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
