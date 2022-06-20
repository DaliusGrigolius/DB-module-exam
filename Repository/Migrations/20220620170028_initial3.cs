using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"update RestaurantDB.dbo.ClientsDb set RestaurantId = 'C9C062E1-E803-4A2A-9248-EEB564253357' where RestaurantId is null");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientsDb_RestaurantsDb_RestaurantId",
                table: "ClientsDb");

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "ClientsDb",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsDb_RestaurantsDb_RestaurantId",
                table: "ClientsDb",
                column: "RestaurantId",
                principalTable: "RestaurantsDb",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientsDb_RestaurantsDb_RestaurantId",
                table: "ClientsDb");

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "ClientsDb",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsDb_RestaurantsDb_RestaurantId",
                table: "ClientsDb",
                column: "RestaurantId",
                principalTable: "RestaurantsDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
