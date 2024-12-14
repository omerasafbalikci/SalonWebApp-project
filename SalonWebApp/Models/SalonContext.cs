using Microsoft.EntityFrameworkCore;

namespace SalonWebApp.Models
{
    public class SalonContext : DbContext
    {
        public SalonContext()
        {

        }

        public SalonContext(DbContextOptions<SalonContext> options) : base(options)
        {
        }

        // DbSet Properties
        public DbSet<Salon> Salons { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<WorkingDay> WorkingDays { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<Hours> Hours { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Salon Configuration
            modelBuilder.Entity<Salon>(entity =>
            {
                entity.ToTable("Salon");
                entity.HasKey(s => s.SalonId);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Type).IsRequired();
                entity.Property(s => s.Address).IsRequired().HasMaxLength(200);
                entity.Property(s => s.Phone).IsRequired().HasMaxLength(15);

                entity.HasMany(s => s.Employees)
                      .WithOne(e => e.Salon)
                      .HasForeignKey(e => e.SalonId);

                entity.HasMany(s => s.Services)
                      .WithOne(svc => svc.Salon)
                      .HasForeignKey(svc => svc.SalonId);

                entity.HasMany(s => s.Appointments)
                      .WithOne(a => a.Salon)
                      .HasForeignKey(a => a.SalonId);
            });

            // Employee Configuration
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");

                entity.HasMany(e => e.WorkingDays)
                      .WithOne(wd => wd.Employee)
                      .HasForeignKey(wd => wd.EmployeeId);

                entity.HasMany(e => e.Appointments)
                      .WithOne(a => a.Employee)
                      .HasForeignKey(a => a.EmployeeId);
            });

            // Appointment Configuration
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.AppointmentId);
                entity.Property(a => a.Description).HasMaxLength(500);

                entity.HasOne(a => a.Service)
                      .WithMany()
                      .HasForeignKey(a => a.ServiceId);

                entity.HasOne(a => a.WorkingTimes)
                      .WithMany(wd => wd.Appointments)
                      .HasForeignKey(a => a.WorkingTimesId);

                entity.HasOne(a => a.Hours)
                      .WithMany(h => h.Appointments)
                      .HasForeignKey(a => a.HoursId);
            });

            // WorkingDay Configuration
            modelBuilder.Entity<WorkingDay>(entity =>
            {
                entity.HasKey(wd => wd.WorkingDayId);
                entity.Property(wd => wd.Date).IsRequired();
                entity.Property(wd => wd.StartTime).IsRequired();
                entity.Property(wd => wd.EndTime).IsRequired();

                entity.HasMany(wd => wd.Hours)
                      .WithOne(h => h.WorkingDay)
                      .HasForeignKey(h => h.WorkingDayId);
            });

            // Hours Configuration
            modelBuilder.Entity<Hours>(entity =>
            {
                entity.HasKey(h => h.HoursId);
                entity.Property(h => h.Time).IsRequired();
                entity.Property(h => h.Selectable).IsRequired();
            });

            // Service Configuration
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(svc => svc.ServiceId);
                entity.Property(svc => svc.ServiceType).IsRequired().HasMaxLength(100);
                entity.Property(svc => svc.Price).HasColumnType("decimal(18,2)");
                entity.Property(svc => svc.Duration).IsRequired();
            });
        }
    }
}