using Microsoft.EntityFrameworkCore;

namespace SalonWebApp.Models
{
    public class SalonContext : DbContext
    {
        public SalonContext(DbContextOptions<SalonContext> options) : base(options)
        {
        }

        // DbSet Properties
        public DbSet<User> Users { get; set; }
        public DbSet<Salon> Salons { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeService> EmployeeServices { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<WorkingDay> WorkingDays { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Time> Times { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.PhoneNumber).IsRequired();
            });

            modelBuilder.Entity<Salon>(entity =>
            {
                entity.HasKey(s => s.SalonId);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Address).IsRequired().HasMaxLength(200);
                entity.Property(s => s.Phone).IsRequired();
            });

            // Employee
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Salary).IsRequired();
                entity.Property(e => e.Phone).IsRequired();

                entity.HasOne(e => e.Salon)
                      .WithMany(s => s.Employees)
                      .HasForeignKey(e => e.SalonId);
            });

            // EmployeeService
            modelBuilder.Entity<EmployeeService>(entity =>
            {
                entity.HasKey(es => es.Id);
                entity.HasOne(es => es.Employee)
                      .WithMany(e => e.EmployeeServices)
                      .HasForeignKey(es => es.EmployeeId);

                entity.HasOne(es => es.Service)
                      .WithMany(s => s.EmployeeServices)
                      .HasForeignKey(es => es.ServiceId);
            });

            // Service
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(s => s.ServiceId);
                entity.Property(s => s.Name).IsRequired();
                entity.Property(s => s.Price).IsRequired();
                entity.Property(s => s.Duration).IsRequired();

                entity.HasOne(s => s.Salon)
                      .WithMany(salon => salon.Services)
                      .HasForeignKey(s => s.SalonId);
            });

            // Time
            modelBuilder.Entity<Time>(entity =>
            {
                entity.HasKey(t => t.TimeId);
                entity.Property(t => t.Date).IsRequired();
                entity.Property(t => t.StartTime).IsRequired();
                entity.Property(t => t.EndTime).IsRequired();
            });

            // WorkingDay
            modelBuilder.Entity<WorkingDay>(entity =>
            {
                entity.HasKey(wd => wd.WorkingDayId);
                entity.Property(wd => wd.Date).IsRequired();
                entity.Property(wd => wd.StartTime).IsRequired();
                entity.Property(wd => wd.EndTime).IsRequired();

                entity.HasOne(wd => wd.Employee)
                      .WithMany(e => e.WorkingDays)
                      .HasForeignKey(wd => wd.EmployeeId);
            });
        }
    }
}