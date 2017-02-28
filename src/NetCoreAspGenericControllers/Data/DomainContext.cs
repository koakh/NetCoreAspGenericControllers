using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NetCoreAspGenericControllers.Model;
using NetCoreAspGenericControllers.Abstract;

namespace NetCoreAspGenericControllers.Data
{
    public class DomainContext : DbContext
    {
        public virtual DbSet<Attendee> Attendee { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<User> User { get; set; }

        public DomainContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Schedule>()
                .ToTable("DemoSchedule");

            modelBuilder.Entity<Schedule>()
                .Property(s => s.CreatorId)
                .IsRequired();

            modelBuilder.Entity<Schedule>()
                .Property(s => s.DateCreated)
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.DateUpdated)
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Type)
                .HasDefaultValue(ScheduleType.Work);

            modelBuilder.Entity<Schedule>()
                .Property(s => s.Status)
                .HasDefaultValue(ScheduleStatus.Valid);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Creator)
                .WithMany(c => c.SchedulesCreated);

            modelBuilder.Entity<User>()
              .ToTable("DemoUser");

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Attendee>()
              .ToTable("DemoAttendee");

            modelBuilder.Entity<Attendee>()
                .HasOne(a => a.User)
                .WithMany(u => u.SchedulesAttended)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Attendee>()
                .HasOne(a => a.Schedule)
                .WithMany(s => s.Attendees)
                .HasForeignKey(a => a.ScheduleId);
        }
    }
}
