using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoClinic.Services.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceSpecializationRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Services",
                newName: "IsActive");

            migrationBuilder.AddColumn<Guid>(
                name: "SpecializationId",
                table: "Services",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_SpecializationId",
                table: "Services",
                column: "SpecializationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Specializations_SpecializationId",
                table: "Services",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Specializations_SpecializationId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_Services_SpecializationId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "SpecializationId",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Services",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
