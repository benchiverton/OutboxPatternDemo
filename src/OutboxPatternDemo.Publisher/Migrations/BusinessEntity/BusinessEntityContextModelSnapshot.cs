﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OutboxPatternDemo.Publisher.BusinessEntityServices.Data;

namespace OutboxPatternDemo.Publisher.Migrations.BusinessEntity
{
    [DbContext(typeof(BusinessEntityContext))]
    partial class BusinessEntityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("BusinessEntity")
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("OutboxPatternDemo.Publisher.BusinessEntityServices.Data.StateDetailDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("BusinessEntityId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeStampUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("StateDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
