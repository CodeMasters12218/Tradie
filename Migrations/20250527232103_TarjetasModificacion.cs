using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tradie.Migrations
{
    /// <inheritdoc />
    public partial class TarjetasModificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCardProfileModel",
                columns: table => new
                {
                    CurrentCardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_UserCardProfileModel_UserCards_CurrentCardId",
                        column: x => x.CurrentCardId,
                        principalTable: "UserCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCardProfileModel_CurrentCardId",
                table: "UserCardProfileModel",
                column: "CurrentCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCardProfileModel");
        }
    }
}
