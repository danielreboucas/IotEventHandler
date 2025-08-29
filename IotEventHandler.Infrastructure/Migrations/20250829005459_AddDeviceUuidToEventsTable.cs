using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IotEventHandler.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceUuidToEventsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeviceUuid",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceUuid",
                table: "Events");
        }
    }
}
