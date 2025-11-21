using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabajoFinal.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToProjectsAndJoinRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxMembers",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "JoinRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Pending");

            // Note: Foreign key constraints will be added manually after data cleanup if needed
            // to prevent conflicts with existing invalid data
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxMembers",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "JoinRequests");
        }
    }
}
