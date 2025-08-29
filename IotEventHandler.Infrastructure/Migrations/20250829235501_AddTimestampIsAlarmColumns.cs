using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IotEventHandler.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTimestampIsAlarmColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAlarm",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAlarm",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Events");
        }
    }
}
