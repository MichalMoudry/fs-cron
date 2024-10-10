using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FsCron.TestApp.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    FinishTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Added = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobResults");
        }
    }
}
