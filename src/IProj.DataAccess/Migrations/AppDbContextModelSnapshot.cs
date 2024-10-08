﻿// <auto-generated />
using System;
using IProj.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IProj.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("IProj.Domain.Entities.Messages.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<bool?>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ReadTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SendTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("messages");
                });

            modelBuilder.Entity("IProj.Domain.Entities.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Gmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("boolean");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("53aa79f4-1650-4baa-b5c7-c7a1fd9d4128"),
                            FirstName = "Samandarbek",
                            Gmail = "samandarbekyr@gmail.com",
                            IsOnline = false,
                            Position = "Backend Developer",
                            RoleName = "Worker"
                        },
                        new
                        {
                            Id = new Guid("6effb728-a7cd-460c-91e5-9f048185fd11"),
                            FirstName = "Muhammadqodir",
                            Gmail = "muhammadqodir5050@gmail.com",
                            IsOnline = false,
                            Position = "Desktop Developer",
                            RoleName = "Owner"
                        },
                        new
                        {
                            Id = new Guid("a8f7f39e-3445-433c-93d3-e6755610a5e0"),
                            FirstName = "Samandar",
                            Gmail = "sharpistmaster@gmail.com",
                            IsOnline = false,
                            Position = "Full-stack Developer",
                            RoleName = "Worker"
                        },
                        new
                        {
                            Id = new Guid("aae35ced-e156-4e1d-beb0-0a5d035763e0"),
                            FirstName = "Able",
                            Gmail = "able.devops@gmail.com",
                            IsOnline = false,
                            Position = "Devops",
                            RoleName = "Worker"
                        },
                        new
                        {
                            Id = new Guid("dd13aea5-d3fd-4afc-8fb8-7f5940766935"),
                            FirstName = "Behruz",
                            Gmail = "uzgrandmaster@gmail.com",
                            IsOnline = false,
                            Position = "Frontend Developer",
                            RoleName = "Worker"
                        },
                        new
                        {
                            Id = new Guid("17820355-1ee5-49c8-9cca-ea6cfb5312ca"),
                            FirstName = "Olim",
                            Gmail = "olim@gmail.com",
                            IsOnline = false,
                            Position = "Project Manager",
                            RoleName = "Worker"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
