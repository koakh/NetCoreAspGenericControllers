using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NetCoreAspGenericControllers.Data;
using NetCoreAspGenericControllers.Abstract;

namespace NetCoreAspGenericControllers.Migrations
{
    [DbContext(typeof(DomainContext))]
    [Migration("20170223110514_First")]
    partial class First
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DomainModel.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Avatar");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Profession");

                    b.HasKey("Id");

                    b.ToTable("DemoUser");
                });

            modelBuilder.Entity("NetCoreAspGenericControllers.Model.Attendee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ScheduleId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("UserId");

                    b.ToTable("DemoAttendee");
                });

            modelBuilder.Entity("NetCoreAspGenericControllers.Model.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatorId");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2017, 2, 23, 11, 5, 14, 429, DateTimeKind.Local));

                    b.Property<DateTime>("DateUpdated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2017, 2, 23, 11, 5, 14, 436, DateTimeKind.Local));

                    b.Property<string>("Description");

                    b.Property<string>("Location");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<DateTime>("TimeEnd");

                    b.Property<DateTime>("TimeStart");

                    b.Property<string>("Title");

                    b.Property<int>("Type")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("DemoSchedule");
                });

            modelBuilder.Entity("NetCoreAspGenericControllers.Model.Attendee", b =>
                {
                    b.HasOne("NetCoreAspGenericControllers.Model.Schedule", "Schedule")
                        .WithMany("Attendees")
                        .HasForeignKey("ScheduleId");

                    b.HasOne("DomainModel.Entities.User", "User")
                        .WithMany("SchedulesAttended")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NetCoreAspGenericControllers.Model.Schedule", b =>
                {
                    b.HasOne("DomainModel.Entities.User", "Creator")
                        .WithMany("SchedulesCreated")
                        .HasForeignKey("CreatorId");
                });
        }
    }
}
