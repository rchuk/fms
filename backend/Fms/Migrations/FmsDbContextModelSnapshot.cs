﻿// <auto-generated />
using System;
using Fms.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fms.Migrations
{
    [DbContext(typeof(FmsDbContext))]
    partial class FmsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Fms.Entities.AccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("integer");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Accounts", t =>
                        {
                            t.HasCheckConstraint("one_of", "(\"UserId\" IS NULL) <> (\"OrganizationId\" IS NULL)");
                        });
                });

            modelBuilder.Entity("Fms.Entities.OrganizationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Fms.Entities.OrganizationRoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("character varying(127)");

                    b.HasKey("Id");

                    b.ToTable("OrganizationRoles");
                });

            modelBuilder.Entity("Fms.Entities.OrganizationToUserEntity", b =>
                {
                    b.Property<int>("OrganizationId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("OrganizationId", "UserId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("OrganizationToUser");
                });

            modelBuilder.Entity("Fms.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(344)
                        .HasColumnType("character varying(344)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Fms.Entities.WorkspaceEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("KindId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("KindId");

                    b.ToTable("Workspaces");
                });

            modelBuilder.Entity("Fms.Entities.WorkspaceKindEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("character varying(127)");

                    b.HasKey("Id");

                    b.ToTable("WorkspaceKinds");
                });

            modelBuilder.Entity("Fms.Entities.WorkspaceRoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("character varying(127)");

                    b.HasKey("Id");

                    b.ToTable("WorkspaceRoles");
                });

            modelBuilder.Entity("Fms.Entities.WorkspaceToAccountEntity", b =>
                {
                    b.Property<int>("WorkspaceId")
                        .HasColumnType("integer");

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("WorkspaceId", "AccountId");

                    b.HasIndex("AccountId");

                    b.HasIndex("RoleId");

                    b.ToTable("WorkspaceToAccount");
                });

            modelBuilder.Entity("Fms.Entities.AccountEntity", b =>
                {
                    b.HasOne("Fms.Entities.OrganizationEntity", "Organization")
                        .WithOne("Account")
                        .HasForeignKey("Fms.Entities.AccountEntity", "OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fms.Entities.UserEntity", "User")
                        .WithOne("Account")
                        .HasForeignKey("Fms.Entities.AccountEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Organization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Fms.Entities.OrganizationToUserEntity", b =>
                {
                    b.HasOne("Fms.Entities.OrganizationEntity", "Organization")
                        .WithMany("Users")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fms.Entities.OrganizationRoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fms.Entities.UserEntity", "User")
                        .WithMany("Organizations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Fms.Entities.WorkspaceEntity", b =>
                {
                    b.HasOne("Fms.Entities.WorkspaceKindEntity", "Kind")
                        .WithMany()
                        .HasForeignKey("KindId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kind");
                });

            modelBuilder.Entity("Fms.Entities.WorkspaceToAccountEntity", b =>
                {
                    b.HasOne("Fms.Entities.AccountEntity", "Account")
                        .WithMany("Workspaces")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fms.Entities.WorkspaceRoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fms.Entities.WorkspaceEntity", "Workspace")
                        .WithMany("Accounts")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Role");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("Fms.Entities.AccountEntity", b =>
                {
                    b.Navigation("Workspaces");
                });

            modelBuilder.Entity("Fms.Entities.OrganizationEntity", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Fms.Entities.UserEntity", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();

                    b.Navigation("Organizations");
                });

            modelBuilder.Entity("Fms.Entities.WorkspaceEntity", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
