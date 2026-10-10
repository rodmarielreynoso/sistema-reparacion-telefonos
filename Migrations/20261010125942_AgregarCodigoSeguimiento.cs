using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistema_reparacion_telefonos.Migrations
{
    public partial class AgregarCodigoSeguimiento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Agregar la columna temporalmente como opcional.
            migrationBuilder.AddColumn<string>(
                name: "CodigoSeguimiento",
                table: "Reparaciones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            // 2. Asignar un código único a cada reparación existente.
            migrationBuilder.Sql(@"
                UPDATE Reparaciones
                SET CodigoSeguimiento =
                    'REP-' + RIGHT('000000' + CAST(Id AS VARCHAR(6)), 6)
            ");

            // 3. Hacer que el código sea obligatorio.
            migrationBuilder.AlterColumn<string>(
                name: "CodigoSeguimiento",
                table: "Reparaciones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            // 4. Garantizar que no se repitan los códigos.
            migrationBuilder.CreateIndex(
                name: "IX_Reparaciones_CodigoSeguimiento",
                table: "Reparaciones",
                column: "CodigoSeguimiento",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Eliminar el índice único.
            migrationBuilder.DropIndex(
                name: "IX_Reparaciones_CodigoSeguimiento",
                table: "Reparaciones");

            // 2. Eliminar la columna agregada.
            migrationBuilder.DropColumn(
                name: "CodigoSeguimiento",
                table: "Reparaciones");
        }
    }
}