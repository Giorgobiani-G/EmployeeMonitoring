﻿// <auto-generated />
using System;
using EmployeeMonitoring.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmployeeMonitoring.Migrations
{
    [DbContext(typeof(EmpContext))]
    [Migration("20220227135344_primarykey")]
    partial class primarykey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("EmployeeMonitoring.Model.EmpModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("GacceniliSaatebi")
                        .HasColumnType("int");

                    b.Property<string>("Saxeli")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ShesvlisDro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("WasvlisDro")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("MyProperty");
                });
#pragma warning restore 612, 618
        }
    }
}
