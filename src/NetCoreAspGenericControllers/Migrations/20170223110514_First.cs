using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NetCoreAspGenericControllers.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemoUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Avatar = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Profession = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DemoSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatorId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2017, 2, 23, 11, 5, 14, 429, DateTimeKind.Local)),
                    DateUpdated = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2017, 2, 23, 11, 5, 14, 436, DateTimeKind.Local)),
                    Description = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    TimeEnd = table.Column<DateTime>(nullable: false),
                    TimeStart = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoSchedule_DemoUser_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "DemoUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DemoAttendee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScheduleId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoAttendee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoAttendee_DemoSchedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "DemoSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemoAttendee_DemoUser_UserId",
                        column: x => x.UserId,
                        principalTable: "DemoUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemoAttendee_ScheduleId",
                table: "DemoAttendee",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoAttendee_UserId",
                table: "DemoAttendee",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoSchedule_CreatorId",
                table: "DemoSchedule",
                column: "CreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemoAttendee");

            migrationBuilder.DropTable(
                name: "DemoSchedule");

            migrationBuilder.DropTable(
                name: "DemoUser");
        }
    }
}
