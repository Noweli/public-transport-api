﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PublicTransportAPI.Data;

#nullable disable

namespace PublicTransportAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("PublicTransportAPI.Data.Models.Auth.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PublicTransportAPI.Data.Models.Line", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("LineIdentifier")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("PublicTransportAPI.Data.Models.StopPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("StopPoints");
                });

            modelBuilder.Entity("PublicTransportAPI.Data.Models.StopPointLineEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Arrival")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Departure")
                        .HasColumnType("TEXT");

                    b.Property<int?>("LineId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("StopPointId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LineId");

                    b.HasIndex("StopPointId");

                    b.ToTable("StopPointLineEvents");
                });

            modelBuilder.Entity("PublicTransportAPI.Data.Models.StopPointLineEvent", b =>
                {
                    b.HasOne("PublicTransportAPI.Data.Models.Line", "Line")
                        .WithMany()
                        .HasForeignKey("LineId");

                    b.HasOne("PublicTransportAPI.Data.Models.StopPoint", "StopPoint")
                        .WithMany()
                        .HasForeignKey("StopPointId");

                    b.Navigation("Line");

                    b.Navigation("StopPoint");
                });
#pragma warning restore 612, 618
        }
    }
}
