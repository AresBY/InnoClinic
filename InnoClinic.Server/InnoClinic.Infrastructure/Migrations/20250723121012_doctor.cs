using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoClinic.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class doctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkerStatus",
                table: "Users",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkerStatus",
                table: "Users");
        }
    }
}
