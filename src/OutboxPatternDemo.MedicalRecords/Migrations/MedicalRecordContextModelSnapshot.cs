﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OutboxPatternDemo.MedicalRecords.MedicalRecords.Data;

#nullable disable

namespace OutboxPatternDemo.Publisher.Migrations
{
    [DbContext(typeof(MedicalRecordContext))]
    partial class MedicalRecordContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("MedicalRecord")
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OutboxPatternDemo.Publisher.AppointmentNotesServices.Data.AppointmentNotesDto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AppointmentTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("AppointmentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatientName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RequiresFollowUpAppointment")
                        .HasColumnType("bit");

                    b.Property<string>("Summary")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AppointmentNotes", "MedicalRecord");
                });
#pragma warning restore 612, 618
        }
    }
}
