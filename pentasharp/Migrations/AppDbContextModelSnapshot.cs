// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using pentasharp.Data;

#nullable disable

namespace pentasharp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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

            modelBuilder.Entity("pentasharp.Models.Entities.BusRouteAssignments", b =>
            {
                b.Property<int>("AssignmentId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AssignmentId"));

                b.Property<int>("BusId")
                    .HasColumnType("int");

                b.Property<DateTime>("CreatedAt")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("GETUTCDATE()");

                b.Property<int>("RouteId")
                    .HasColumnType("int");

                b.Property<DateTime?>("UpdatedAt")
                    .HasColumnType("datetime2");

                b.HasKey("AssignmentId");

                b.HasIndex("BusId");

                b.HasIndex("RouteId");

                b.ToTable("BusRouteAssignments");
            });

            modelBuilder.Entity("pentasharp.Models.Entities.BusRoutes", b =>
            {
                b.Property<int>("RouteId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RouteId"));

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

                b.HasKey("BusId");

                b.ToTable("Buses");
            });

            modelBuilder.Entity("pentasharp.Models.Entities.Notifications", b =>
            {
                b.Property<int>("NotificationId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                b.Property<int>("BookingId")
                    .HasColumnType("int");

                b.Property<string>("Message")
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<DateTime>("SentAt")
                    .HasColumnType("datetime2");

                b.Property<int>("UserId")
                    .HasColumnType("int");

                b.HasKey("NotificationId");

                b.HasIndex("BookingId");

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
                    .HasColumnType("datetime2");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<string>("DropoffLocation")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<string>("Fare")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("PickupLocation")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<int>("Status")
                    .HasColumnType("int");

                b.Property<int>("TaxiId")
                    .HasColumnType("int");

                b.Property<DateTime>("TripEndTime")
                    .HasColumnType("datetime2");

                b.Property<DateTime>("TripStartTime")
                    .HasColumnType("datetime2");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("datetime2");

                b.Property<int>("UserId")
                    .HasColumnType("int");

                b.HasKey("BookingId");

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

                b.Property<decimal>("Fare")
                    .HasPrecision(18, 2)
                    .HasColumnType("decimal(18, 2)");

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

                b.Property<int>("TaxiId")
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

#pragma warning restore 612, 618
        }
    }
}
