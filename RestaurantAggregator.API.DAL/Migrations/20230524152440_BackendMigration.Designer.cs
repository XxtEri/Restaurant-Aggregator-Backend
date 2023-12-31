﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RestaurantAggregator.API.DAL;

#nullable disable

namespace RestaurantAggregator.API.DAL.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20230524152440_BackendMigration")]
    partial class BackendMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Cook", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("RestaurantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Cooks");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Courier", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Couriers");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsVegetarian")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Photo")
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<double>("Rating")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.DishInCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DishId");

                    b.ToTable("DishesInCart");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Manager", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("RestaurantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RestaurantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.MenuDish", b =>
                {
                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid");

                    b.HasKey("DishId", "MenuId");

                    b.HasIndex("MenuId");

                    b.ToTable("MenusDishes");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("CookId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CourierId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DeliveryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("NumberOrder")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("NumberOrder"));

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CookId");

                    b.HasIndex("CourierId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.OrderDish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrdersDishes");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Rating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Restaurant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CookId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CookId")
                        .IsUnique();

                    b.HasIndex("ManagerId")
                        .IsUnique();

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.DishInCart", b =>
                {
                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Customer", "Customer")
                        .WithMany("DishInCarts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Dish", "Dish")
                        .WithMany("DishInCarts")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Menu", b =>
                {
                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Restaurant", "Restaurant")
                        .WithMany("Menus")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.MenuDish", b =>
                {
                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Dish", "Dish")
                        .WithMany("MenusDishes")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Menu", "Menu")
                        .WithMany("MenusDishes")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Order", b =>
                {
                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Cook", "Cook")
                        .WithMany("Orders")
                        .HasForeignKey("CookId");

                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Courier", "Courier")
                        .WithMany("Orders")
                        .HasForeignKey("CourierId");

                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cook");

                    b.Navigation("Courier");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.OrderDish", b =>
                {
                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Dish", "Dish")
                        .WithMany("OrderDishes")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Order", "Order")
                        .WithMany("OrderDishes")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Restaurant", b =>
                {
                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Cook", "Cook")
                        .WithOne("Restaurant")
                        .HasForeignKey("RestaurantAggregator.API.DAL.Entities.Restaurant", "CookId");

                    b.HasOne("RestaurantAggregator.API.DAL.Entities.Manager", "Manager")
                        .WithOne("Restaurant")
                        .HasForeignKey("RestaurantAggregator.API.DAL.Entities.Restaurant", "ManagerId");

                    b.Navigation("Cook");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Cook", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Courier", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Customer", b =>
                {
                    b.Navigation("DishInCarts");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Dish", b =>
                {
                    b.Navigation("DishInCarts");

                    b.Navigation("MenusDishes");

                    b.Navigation("OrderDishes");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Manager", b =>
                {
                    b.Navigation("Restaurant")
                        .IsRequired();
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Menu", b =>
                {
                    b.Navigation("MenusDishes");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Order", b =>
                {
                    b.Navigation("OrderDishes");
                });

            modelBuilder.Entity("RestaurantAggregator.API.DAL.Entities.Restaurant", b =>
                {
                    b.Navigation("Menus");
                });
#pragma warning restore 612, 618
        }
    }
}
