using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayHub_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class addAvaliacaoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avaliacao",
                table: "Quartos");

            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuartoId = table.Column<int>(type: "int", nullable: false),
                    HospedeId = table.Column<int>(type: "int", nullable: false),
                    Avaliacao = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Hospedes_HospedeId",
                        column: x => x.HospedeId,
                        principalTable: "Hospedes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Quartos_QuartoId",
                        column: x => x.QuartoId,
                        principalTable: "Quartos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_HospedeId",
                table: "Avaliacoes",
                column: "HospedeId");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_QuartoId",
                table: "Avaliacoes",
                column: "QuartoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avaliacoes");

            migrationBuilder.AddColumn<decimal>(
                name: "Avaliacao",
                table: "Quartos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
