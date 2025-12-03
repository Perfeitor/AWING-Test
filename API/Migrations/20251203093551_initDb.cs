using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class initDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TreasureRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    N = table.Column<int>(type: "INTEGER", nullable: false),
                    M = table.Column<int>(type: "INTEGER", nullable: false),
                    P = table.Column<int>(type: "INTEGER", nullable: false),
                    MatrixJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasureRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreasureResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SessionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResultFuel = table.Column<double>(type: "REAL", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreasureResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreasureResult_TreasureRequest_SessionId",
                        column: x => x.SessionId,
                        principalTable: "TreasureRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreasureResult_SessionId",
                table: "TreasureResult",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreasureResult");

            migrationBuilder.DropTable(
                name: "TreasureRequest");
        }
    }
}
