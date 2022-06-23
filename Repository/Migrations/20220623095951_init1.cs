using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Restaurants_RestaurantId",
                table: "Clients");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Restaurants_RestaurantId",
                table: "Clients",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Restaurants_RestaurantId",
                table: "Clients");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Restaurants_RestaurantId",
                table: "Clients",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
