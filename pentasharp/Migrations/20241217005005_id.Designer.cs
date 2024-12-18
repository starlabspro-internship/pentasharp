﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using pentasharp.Data;

#nullable disable

namespace pentasharp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241217005005_id")]
    partial class id
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("pentasharp.Models.Entities.BaseReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("ReviewType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("Id");

                    b.ToTable("Reviews");

                    b.HasDiscriminator<string>("ReviewType").HasValue("BaseReview");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("pentasharp.Models.Entities.BusCompany", b =>
                {
                    b.Property<int>("BusCompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BusCompanyId"));

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("BusCompanyId");

                    b.ToTable("BusCompanies");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.BusReservations", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservationId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("NumberOfSeats")
                        .HasColumnType("int");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReservationId");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("UserId");

                    b.ToTable("BusReservations");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.BusRoutes", b =>
                {
                    b.Property<int>("RouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RouteId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("EstimatedDuration")
                        .HasColumnType("time");

                    b.Property<string>("FromLocation")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ToLocation")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("RouteId");

                    b.ToTable("BusRoutes");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.BusSchedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScheduleId"));

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("AvailableSeats")
                        .HasColumnType("int");

                    b.Property<int>("BusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ScheduleId");

                    b.HasIndex("BusId");

                    b.HasIndex("RouteId");

                    b.ToTable("BusSchedules");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.Buses", b =>
                {
                    b.Property<int>("BusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BusId"));

                    b.Property<int>("BusCompanyId")
                        .HasColumnType("int");

                    b.Property<int>("BusNumber")
                        .HasColumnType("int");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("BusId");

                    b.HasIndex("BusCompanyId");

                    b.ToTable("Buses");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.Notifications", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                    b.Property<int?>("BookingId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("SentAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int?>("TaxiBookingsBookingId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.HasIndex("BookingId");

                    b.HasIndex("TaxiBookingsBookingId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.Taxi", b =>
                {
                    b.Property<int>("TaxiId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaxiId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("DriverName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TaxiCompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("TaxiId");

                    b.HasIndex("LicensePlate")
                        .IsUnique();

                    b.HasIndex("TaxiCompanyId");

                    b.ToTable("Taxis");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.TaxiBookings", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingId"));

                    b.Property<DateTime>("BookingTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("DropoffLocation")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal?>("Fare")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("PassengerCount")
                        .HasColumnType("int");

                    b.Property<string>("PickupLocation")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TaxiCompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("TaxiId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TripEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TripStartTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("BookingId");

                    b.HasIndex("TaxiCompanyId");

                    b.HasIndex("TaxiId");

                    b.HasIndex("UserId");

                    b.ToTable("TaxiBookings");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.TaxiCompany", b =>
                {
                    b.Property<int>("TaxiCompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaxiCompanyId"));

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ContactInfo")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("TaxiCompanyId");

                    b.HasIndex("CompanyName")
                        .IsUnique();

                    b.ToTable("TaxiCompanies");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.TaxiReservations", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservationId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("DropoffLocation")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal?>("Fare")
                        .IsRequired()
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("PassengerCount")
                        .HasColumnType("int");

                    b.Property<string>("PickupLocation")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("ReservationTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TaxiCompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("TaxiId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TripEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TripStartTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReservationId");

                    b.HasIndex("TaxiCompanyId");

                    b.HasIndex("TaxiId");

                    b.HasIndex("UserId");

                    b.ToTable("TaxiReservations");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.UserReview", b =>
                {
                    b.HasBaseType("pentasharp.Models.Entities.BaseReview");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("UserId");

                    b.HasDiscriminator().HasValue("UserReview");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.BusReservations", b =>
                {
                    b.HasOne("pentasharp.Models.Entities.BusSchedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("pentasharp.Models.Entities.User", "User")
                        .WithMany("BusReservations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Schedule");

                    b.Navigation("User");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.BusSchedule", b =>
                {
                    b.HasOne("pentasharp.Models.Entities.Buses", "Bus")
                        .WithMany("BusSchedules")
                        .HasForeignKey("BusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("pentasharp.Models.Entities.BusRoutes", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Bus");

                    b.Navigation("Route");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.Buses", b =>
                {
                    b.HasOne("pentasharp.Models.Entities.BusCompany", "BusCompany")
                        .WithMany("Buses")
                        .HasForeignKey("BusCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BusCompany");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.Notifications", b =>
                {
                    b.HasOne("pentasharp.Models.Entities.TaxiBookings", "TaxiBooking")
                        .WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("pentasharp.Models.Entities.TaxiBookings", null)
                        .WithMany("Notifications")
                        .HasForeignKey("TaxiBookingsBookingId");

                    b.HasOne("pentasharp.Models.Entities.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TaxiBooking");

                    b.Navigation("User");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.Taxi", b =>
                {
                    b.HasOne("pentasharp.Models.Entities.TaxiCompany", "TaxiCompany")
                        .WithMany("Taxis")
                        .HasForeignKey("TaxiCompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TaxiCompany");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.TaxiBookings", b =>
                {
                    b.HasOne("pentasharp.Models.Entities.TaxiCompany", "TaxiCompany")
                        .WithMany("TaxiBookings")
                        .HasForeignKey("TaxiCompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("pentasharp.Models.Entities.Taxi", "Taxi")
                        .WithMany("TaxiBookings")
                        .HasForeignKey("TaxiId");

                    b.HasOne("pentasharp.Models.Entities.User", "User")
                        .WithMany("TaxiBookings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Taxi");

                    b.Navigation("TaxiCompany");

                    b.Navigation("User");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.TaxiReservations", b =>
                {
                    b.HasOne("pentasharp.Models.Entities.TaxiCompany", "TaxiCompany")
                        .WithMany("TaxiReservations")
                        .HasForeignKey("TaxiCompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("pentasharp.Models.Entities.Taxi", "Taxi")
                        .WithMany("TaxiReservations")
                        .HasForeignKey("TaxiId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("pentasharp.Models.Entities.User", "User")
                        .WithMany("TaxiReservations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Taxi");

                    b.Navigation("TaxiCompany");

                    b.Navigation("User");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.UserReview", b =>
                {
                    b.HasOne("pentasharp.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.BusCompany", b =>
                {
                    b.Navigation("Buses");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.Buses", b =>
                {
                    b.Navigation("BusSchedules");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.Taxi", b =>
                {
                    b.Navigation("TaxiBookings");

                    b.Navigation("TaxiReservations");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.TaxiBookings", b =>
                {
                    b.Navigation("Notifications");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.TaxiCompany", b =>
                {
                    b.Navigation("TaxiBookings");

                    b.Navigation("TaxiReservations");

                    b.Navigation("Taxis");
                });

            modelBuilder.Entity("pentasharp.Models.Entities.User", b =>
                {
                    b.Navigation("BusReservations");

                    b.Navigation("Notifications");

                    b.Navigation("TaxiBookings");

                    b.Navigation("TaxiReservations");
                });
#pragma warning restore 612, 618
        }
    }
}
