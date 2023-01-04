﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OutboxPatternDemo.Publisher.Outboxes.Custom;

#nullable disable

namespace OutboxPatternDemo.Publisher.Migrations.CustomOutbox
{
    [DbContext(typeof(CustomOutboxContext))]
    [Migration("20230104163037_CreateCustomOutboxContext")]
    partial class CreateCustomOutboxContext
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("CustomOutbox")
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OutboxPatternDemo.Publisher.CustomOutbox.CustomOutboxMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ProcessedTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RequestedTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Messages", "CustomOutbox");
                });
#pragma warning restore 612, 618
        }
    }
}
