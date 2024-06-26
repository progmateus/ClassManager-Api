﻿// <auto-generated />
using System;
using ClassManager.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClassManager.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240427194953_CreateRoles")]
    partial class CreateRoles
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClassManager.Domain.Contexts.Accounts.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Avatar")
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Avatar");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<byte>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TINYINT")
                        .HasDefaultValue((byte)1)
                        .HasColumnName("Status");

                    b.Property<byte>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TINYINT")
                        .HasDefaultValue((byte)1)
                        .HasColumnName("Type");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("ClassManager.Domain.Contexts.Plans.Entities.Plan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("ClassesLimit")
                        .HasColumnType("TINYINT")
                        .HasColumnName("ClassesLimit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Name");

                    b.Property<decimal>("Price")
                        .HasColumnType("DECIMAL")
                        .HasColumnName("Price");

                    b.Property<int>("StudentsLimit")
                        .HasColumnType("INT")
                        .HasColumnName("StudentsLimit");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Plans", (string)null);
                });

            modelBuilder.Entity("ClassManager.Domain.Contexts.Roles.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Name");

                    b.Property<DateTime>("Updatedat")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("ClassManager.Domain.Contexts.Tenants.Entities.Tenant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Avatar")
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Avatar");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("Name");

                    b.Property<byte>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TINYINT")
                        .HasDefaultValue((byte)1)
                        .HasColumnName("Status");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Tenants", (string)null);
                });

            modelBuilder.Entity("ClassManager.Domain.Contexts.Accounts.Entities.User", b =>
                {
                    b.OwnsOne("ClassManager.Domain.Contexts.Shared.ValueObjects.Document", "Document", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(80)
                                .HasColumnType("VARCHAR")
                                .HasColumnName("Document");

                            b1.Property<byte>("Type")
                                .HasColumnType("TINYINT")
                                .HasColumnName("DocumentType");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("ClassManager.Domain.Contexts.Shared.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("VARCHAR")
                                .HasColumnName("Email");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");

                            b1.OwnsOne("ClassManager.Domain.Contexts.Shared.ValueObjects.Verification", "Verification", b2 =>
                                {
                                    b2.Property<Guid>("EmailUserId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("Code")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)")
                                        .HasColumnName("EmailVerificationCode");

                                    b2.Property<DateTime?>("ExpiresAt")
                                        .HasColumnType("datetime2")
                                        .HasColumnName("EmailVerificationExpiresAt");

                                    b2.Property<DateTime?>("VerifiedAt")
                                        .HasColumnType("datetime2")
                                        .HasColumnName("EmailVerificationVerifiedAt");

                                    b2.HasKey("EmailUserId");

                                    b2.ToTable("Users");

                                    b2.WithOwner()
                                        .HasForeignKey("EmailUserId");
                                });

                            b1.Navigation("Verification")
                                .IsRequired();
                        });

                    b.OwnsOne("ClassManager.Domain.Contexts.Shared.ValueObjects.Name", "Name", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(80)
                                .HasColumnType("VARCHAR")
                                .HasColumnName("FirstName");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(80)
                                .HasColumnType("VARCHAR")
                                .HasColumnName("LastName");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("ClassManager.Domain.Contexts.Shared.ValueObjects.Password", "Password", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Hash")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("PasswordHash");

                            b1.Property<string>("ResetCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("PasswordResetCode");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Document")
                        .IsRequired();

                    b.Navigation("Email")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();

                    b.Navigation("Password")
                        .IsRequired();
                });

            modelBuilder.Entity("ClassManager.Domain.Contexts.Tenants.Entities.Tenant", b =>
                {
                    b.OwnsOne("ClassManager.Domain.Contexts.Shared.ValueObjects.Document", "Document", b1 =>
                        {
                            b1.Property<Guid>("TenantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(80)
                                .HasColumnType("VARCHAR")
                                .HasColumnName("Document");

                            b1.Property<byte>("Type")
                                .HasColumnType("TINYINT")
                                .HasColumnName("DocumentType");

                            b1.HasKey("TenantId");

                            b1.ToTable("Tenants");

                            b1.WithOwner()
                                .HasForeignKey("TenantId");
                        });

                    b.OwnsOne("ClassManager.Domain.Contexts.Shared.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("TenantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("VARCHAR")
                                .HasColumnName("Email");

                            b1.HasKey("TenantId");

                            b1.ToTable("Tenants");

                            b1.WithOwner()
                                .HasForeignKey("TenantId");
                        });

                    b.Navigation("Document")
                        .IsRequired();

                    b.Navigation("Email")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
