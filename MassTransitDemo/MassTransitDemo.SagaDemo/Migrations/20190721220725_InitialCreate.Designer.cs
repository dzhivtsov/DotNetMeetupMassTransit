﻿// <auto-generated />
using System;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransitDemo.SagaDemo.Saga;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MassTransitDemo.SagaDemo.Migrations
{
    [DbContext(typeof(SagaDbContext<ShoppingCart, ShoppingCartMap>))]
    [Migration("20190721220725_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MassTransitDemo.SagaDemo.ShoppingCart", b =>
                {
                    b.Property<Guid>("CorrelationId");

                    b.Property<DateTime>("Created");

                    b.Property<int>("CurrentState");

                    b.Property<Guid?>("ExpirationId");

                    b.Property<DateTime>("Updated");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("CorrelationId");

                    b.ToTable("ShoppingCart");
                });
#pragma warning restore 612, 618
        }
    }
}
