using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Secretaria.Infra.Migrations
{
    /// <inheritdoc />
    public partial class IndiceNomeTurma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Turmas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Turmas_Nome",
                table: "Turmas",
                column: "Nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Turmas_Nome",
                table: "Turmas");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Turmas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
