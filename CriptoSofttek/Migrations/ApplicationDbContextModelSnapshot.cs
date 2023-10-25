﻿// <auto-generated />
using System;
using CriptoSofttek.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CriptoSofttek.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CriptoSofttek.Entities.CryptoAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,6)");

                    b.Property<string>("UUID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CryptoAccounts");
                });

            modelBuilder.Entity("CriptoSofttek.Entities.FiatAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CBU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PesosBalance")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("USDBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FiatAccounts");
                });

            modelBuilder.Entity("CriptoSofttek.Entities.Movement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("TypeMovement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Movements");
                });

            modelBuilder.Entity("CriptoSofttek.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<int>("CryptoAccountId")
                        .HasColumnType("int");

                    b.Property<int?>("CryptoAccountId1")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FiatAccountId")
                        .HasColumnType("int");

                    b.Property<int?>("FiatAccountId1")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CryptoAccountId1");

                    b.HasIndex("FiatAccountId1");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Activo = false,
                            CryptoAccountId = 0,
                            Email = "maxi@gmail.com",
                            FiatAccountId = 0,
                            FirstName = "Maxi",
                            LastName = "Maiz",
                            Password = "7d61ee0ed586257cff94117dcd15a63b5c2216385cba22a8393d7376feb515c0"
                        });
                });

            modelBuilder.Entity("CriptoSofttek.Entities.User", b =>
                {
                    b.HasOne("CriptoSofttek.Entities.CryptoAccount", "CryptoAccount")
                        .WithMany()
                        .HasForeignKey("CryptoAccountId1");

                    b.HasOne("CriptoSofttek.Entities.FiatAccount", "FiatAccount")
                        .WithMany()
                        .HasForeignKey("FiatAccountId1");

                    b.Navigation("CryptoAccount");

                    b.Navigation("FiatAccount");
                });
#pragma warning restore 612, 618
        }
    }
}
