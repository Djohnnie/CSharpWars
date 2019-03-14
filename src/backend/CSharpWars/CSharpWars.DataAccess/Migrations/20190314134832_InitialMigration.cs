using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSharpWars.DataAccess.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PLAYERS",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SysId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    Hashed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLAYERS", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "BOTS",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SysId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    Orientation = table.Column<int>(nullable: false),
                    MaximumHealth = table.Column<int>(nullable: false),
                    CurrentHealth = table.Column<int>(nullable: false),
                    MaximumStamina = table.Column<int>(nullable: false),
                    CurrentStamina = table.Column<int>(nullable: false),
                    Memory = table.Column<string>(nullable: true),
                    Move = table.Column<int>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: true),
                    Script = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOTS", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_BOTS_PLAYERS_TeamId",
                        column: x => x.TeamId,
                        principalTable: "PLAYERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BOTS_SysId",
                table: "BOTS",
                column: "SysId")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_BOTS_TeamId",
                table: "BOTS",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PLAYERS_SysId",
                table: "PLAYERS",
                column: "SysId")
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOTS");

            migrationBuilder.DropTable(
                name: "PLAYERS");
        }
    }
}