﻿// <auto-generated />
using System;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataBase.Migrations
{
    [DbContext(typeof(CurrentDbContext))]
    partial class CurrentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataBase.Model.Hero", b =>
                {
                    b.Property<byte>("Id")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Heroes");
                });

            modelBuilder.Entity("DataBase.Model.Item", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("DataBase.Model.LastWeekHeroWinRate", b =>
                {
                    b.Property<byte>("HeroIdMain")
                        .HasColumnType("tinyint");

                    b.Property<byte>("HeroIdCompareWith")
                        .HasColumnType("tinyint");

                    b.Property<int>("LoseAlly")
                        .HasColumnType("int");

                    b.Property<int>("WeekPatchId")
                        .HasColumnType("int");

                    b.Property<int>("WinAlly")
                        .HasColumnType("int");

                    b.Property<int>("WinEnemy")
                        .HasColumnType("int");

                    b.Property<int>("WinLose")
                        .HasColumnType("int");

                    b.HasKey("HeroIdMain", "HeroIdCompareWith");

                    b.HasIndex("WeekPatchId");

                    b.ToTable("LastWeekHeroWinRates");
                });

            modelBuilder.Entity("DataBase.Model.User", b =>
                {
                    b.Property<long>("SteamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("MyProperty")
                        .HasColumnType("datetime2");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("SteamId");

                    b.HasIndex("Role");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataBase.Model.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("DataBase.Model.WeekPatch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Patch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WeekId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("WeekPatches");
                });

            modelBuilder.Entity("DataBase.Model.WeeklyHeroWinRate", b =>
                {
                    b.Property<int>("WeekPatchId")
                        .HasColumnType("int");

                    b.Property<byte>("HeroId")
                        .HasColumnType("tinyint");

                    b.Property<int>("AllGames")
                        .HasColumnType("int");

                    b.Property<int>("Wins")
                        .HasColumnType("int");

                    b.HasKey("WeekPatchId", "HeroId");

                    b.HasIndex("HeroId");

                    b.ToTable("WeeklyHeroWinRates");
                });

            modelBuilder.Entity("DataBase.Model.LastWeekHeroWinRate", b =>
                {
                    b.HasOne("DataBase.Model.Hero", "HeroMain")
                        .WithMany("LastWeekHeroWinRates")
                        .HasForeignKey("HeroIdMain")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataBase.Model.WeekPatch", "WeekPatch")
                        .WithMany("LastWeekHeroWinRates")
                        .HasForeignKey("WeekPatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HeroMain");

                    b.Navigation("WeekPatch");
                });

            modelBuilder.Entity("DataBase.Model.User", b =>
                {
                    b.HasOne("DataBase.Model.UserRole", "UserRole")
                        .WithMany("Users")
                        .HasForeignKey("Role")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("DataBase.Model.WeeklyHeroWinRate", b =>
                {
                    b.HasOne("DataBase.Model.Hero", "Hero")
                        .WithMany("WeeklyHeroWinRates")
                        .HasForeignKey("HeroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataBase.Model.WeekPatch", "WeekPatch")
                        .WithMany("WeeklyHeroWinRates")
                        .HasForeignKey("WeekPatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hero");

                    b.Navigation("WeekPatch");
                });

            modelBuilder.Entity("DataBase.Model.Hero", b =>
                {
                    b.Navigation("LastWeekHeroWinRates");

                    b.Navigation("WeeklyHeroWinRates");
                });

            modelBuilder.Entity("DataBase.Model.UserRole", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("DataBase.Model.WeekPatch", b =>
                {
                    b.Navigation("LastWeekHeroWinRates");

                    b.Navigation("WeeklyHeroWinRates");
                });
#pragma warning restore 612, 618
        }
    }
}
