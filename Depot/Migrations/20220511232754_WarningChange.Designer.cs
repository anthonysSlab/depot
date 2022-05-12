﻿// <auto-generated />
using System;
using Depot.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Depot.Migrations
{
    [DbContext(typeof(ModerationContext))]
    [Migration("20220511232754_WarningChange")]
    partial class WarningChange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("Depot.Enitities.Guild", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ActivityKicking")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("DurationKick")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("DurationWarn")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Depot.Enitities.GuildUser", b =>
                {
                    b.Property<ulong>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ActivityWarn")
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasActivityWarn")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastActivity")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "GuildId");

                    b.HasIndex("GuildId");

                    b.ToTable("GuildUsers");
                });

            modelBuilder.Entity("Depot.Enitities.IgnoredRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("RoleId");

                    b.ToTable("IgnoredRole");
                });

            modelBuilder.Entity("Depot.Enitities.Role", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildUserGuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildUserUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("GuildUserUserId", "GuildUserGuildId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Depot.Enitities.User", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Depot.Enitities.Warning", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("Warning");
                });

            modelBuilder.Entity("Depot.Enitities.GuildUser", b =>
                {
                    b.HasOne("Depot.Enitities.Guild", "Guild")
                        .WithMany("Users")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Depot.Enitities.User", "User")
                        .WithMany("Guilds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Depot.Enitities.IgnoredRole", b =>
                {
                    b.HasOne("Depot.Enitities.Guild", null)
                        .WithMany("IgnoredRoles")
                        .HasForeignKey("GuildId");

                    b.HasOne("Depot.Enitities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Depot.Enitities.Role", b =>
                {
                    b.HasOne("Depot.Enitities.Guild", "Guild")
                        .WithMany("Roles")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Depot.Enitities.GuildUser", null)
                        .WithMany("Roles")
                        .HasForeignKey("GuildUserUserId", "GuildUserGuildId");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Depot.Enitities.Warning", b =>
                {
                    b.HasOne("Depot.Enitities.Guild", "Guild")
                        .WithMany("Warnings")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Depot.Enitities.User", "User")
                        .WithMany("Warnings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Depot.Enitities.Guild", b =>
                {
                    b.Navigation("IgnoredRoles");

                    b.Navigation("Roles");

                    b.Navigation("Users");

                    b.Navigation("Warnings");
                });

            modelBuilder.Entity("Depot.Enitities.GuildUser", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Depot.Enitities.User", b =>
                {
                    b.Navigation("Guilds");

                    b.Navigation("Warnings");
                });
#pragma warning restore 612, 618
        }
    }
}
