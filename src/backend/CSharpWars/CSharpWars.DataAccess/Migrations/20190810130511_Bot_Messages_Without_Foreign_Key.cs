using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSharpWars.DataAccess.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Bot_Messages_Without_Foreign_Key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MESSAGES_BOTS_BotId",
                table: "MESSAGES");

            migrationBuilder.DropIndex(
                name: "IX_MESSAGES_BotId",
                table: "MESSAGES");

            migrationBuilder.DropColumn(
                name: "BotId",
                table: "MESSAGES");

            migrationBuilder.AddColumn<string>(
                name: "BotName",
                table: "MESSAGES",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BotName",
                table: "MESSAGES");

            migrationBuilder.AddColumn<Guid>(
                name: "BotId",
                table: "MESSAGES",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MESSAGES_BotId",
                table: "MESSAGES",
                column: "BotId");

            migrationBuilder.AddForeignKey(
                name: "FK_MESSAGES_BOTS_BotId",
                table: "MESSAGES",
                column: "BotId",
                principalTable: "BOTS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}