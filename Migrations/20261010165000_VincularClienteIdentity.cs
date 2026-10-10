using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistema_reparacion_telefonos.Migrations
{
    /// <inheritdoc />
    public partial class VincularClienteIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Clientes",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_IdentityUserId",
                table: "Clientes",
                column: "IdentityUserId",
                unique: true,
                filter: "[IdentityUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_AspNetUsers_IdentityUserId",
                table: "Clientes",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_AspNetUsers_IdentityUserId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_IdentityUserId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Clientes");
        }
    }
}
