using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InvestigacionClinica.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Investigacion",
                columns: table => new
                {
                    IdInvestigacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    TipoEstudio = table.Column<string>(type: "text", nullable: false),
                    Fase = table.Column<string>(type: "text", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investigacion", x => x.IdInvestigacion);
                });

            migrationBuilder.CreateTable(
                name: "Resultado",
                columns: table => new
                {
                    IdResultado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    CodigoOrdenLaboratorio = table.Column<string>(type: "text", nullable: false),
                    CodigoPaciente = table.Column<string>(type: "text", nullable: false),
                    TipoPrueba = table.Column<string>(type: "text", nullable: false),
                    ValorObtenido = table.Column<string>(type: "text", nullable: false),
                    FechaRecepcion = table.Column<DateOnly>(type: "date", nullable: false),
                    TieneValorAnormal = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultado", x => x.IdResultado);
                });

            migrationBuilder.CreateTable(
                name: "TipoSintoma",
                columns: table => new
                {
                    IdTipoSintoma = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Gravedad = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoSintoma", x => x.IdTipoSintoma);
                });

            migrationBuilder.CreateTable(
                name: "Recoleccion",
                columns: table => new
                {
                    IdRecoleccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdInvestigacion = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    CodigoProtocolo = table.Column<string>(type: "text", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    Fechafin = table.Column<DateOnly>(type: "date", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Total = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recoleccion", x => x.IdRecoleccion);
                    table.ForeignKey(
                        name: "FK_Recoleccion_Investigacion_IdInvestigacion",
                        column: x => x.IdInvestigacion,
                        principalTable: "Investigacion",
                        principalColumn: "IdInvestigacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resultado_Sintoma",
                columns: table => new
                {
                    IdResultadoSintoma = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdResultado = table.Column<int>(type: "integer", nullable: false),
                    IdTipoSintoma = table.Column<int>(type: "integer", nullable: false),
                    FechaRegistro = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultado_Sintoma", x => x.IdResultadoSintoma);
                    table.ForeignKey(
                        name: "FK_Resultado_Sintoma_Resultado_IdResultado",
                        column: x => x.IdResultado,
                        principalTable: "Resultado",
                        principalColumn: "IdResultado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resultado_Sintoma_TipoSintoma_IdTipoSintoma",
                        column: x => x.IdTipoSintoma,
                        principalTable: "TipoSintoma",
                        principalColumn: "IdTipoSintoma",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recoleccion_Resultado",
                columns: table => new
                {
                    IdRecoleccionDetalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRecoleccion = table.Column<int>(type: "integer", nullable: false),
                    IdResultado = table.Column<int>(type: "integer", nullable: false),
                    FechaAsignacion = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaRegistro = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recoleccion_Resultado", x => x.IdRecoleccionDetalle);
                    table.ForeignKey(
                        name: "FK_Recoleccion_Resultado_Recoleccion_IdRecoleccion",
                        column: x => x.IdRecoleccion,
                        principalTable: "Recoleccion",
                        principalColumn: "IdRecoleccion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recoleccion_Resultado_Resultado_IdResultado",
                        column: x => x.IdResultado,
                        principalTable: "Resultado",
                        principalColumn: "IdResultado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recoleccion_IdInvestigacion",
                table: "Recoleccion",
                column: "IdInvestigacion");

            migrationBuilder.CreateIndex(
                name: "IX_Recoleccion_Resultado_IdRecoleccion",
                table: "Recoleccion_Resultado",
                column: "IdRecoleccion");

            migrationBuilder.CreateIndex(
                name: "IX_Recoleccion_Resultado_IdResultado",
                table: "Recoleccion_Resultado",
                column: "IdResultado");

            migrationBuilder.CreateIndex(
                name: "IX_Resultado_Sintoma_IdResultado",
                table: "Resultado_Sintoma",
                column: "IdResultado");

            migrationBuilder.CreateIndex(
                name: "IX_Resultado_Sintoma_IdTipoSintoma",
                table: "Resultado_Sintoma",
                column: "IdTipoSintoma");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recoleccion_Resultado");

            migrationBuilder.DropTable(
                name: "Resultado_Sintoma");

            migrationBuilder.DropTable(
                name: "Recoleccion");

            migrationBuilder.DropTable(
                name: "Resultado");

            migrationBuilder.DropTable(
                name: "TipoSintoma");

            migrationBuilder.DropTable(
                name: "Investigacion");
        }
    }
}
