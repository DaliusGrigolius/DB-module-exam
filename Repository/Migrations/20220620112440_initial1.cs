using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Restaurants_RestaurantId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientWaiter_Clients_ClientsId",
                table: "ClientWaiter");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientWaiter_Waiters_WaitersId",
                table: "ClientWaiter");

            migrationBuilder.DropForeignKey(
                name: "FK_Waiters_Restaurants_RestaurantId",
                table: "Waiters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Waiters",
                table: "Waiters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "Waiters",
                newName: "WaitersDb");

            migrationBuilder.RenameTable(
                name: "Restaurants",
                newName: "RestaurantsDb");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "ClientsDb");

            migrationBuilder.RenameIndex(
                name: "IX_Waiters_RestaurantId",
                table: "WaitersDb",
                newName: "IX_WaitersDb_RestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_RestaurantId",
                table: "ClientsDb",
                newName: "IX_ClientsDb_RestaurantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaitersDb",
                table: "WaitersDb",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestaurantsDb",
                table: "RestaurantsDb",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientsDb",
                table: "ClientsDb",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientsDb_RestaurantsDb_RestaurantId",
                table: "ClientsDb",
                column: "RestaurantId",
                principalTable: "RestaurantsDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientWaiter_ClientsDb_ClientsId",
                table: "ClientWaiter",
                column: "ClientsId",
                principalTable: "ClientsDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientWaiter_WaitersDb_WaitersId",
                table: "ClientWaiter",
                column: "WaitersId",
                principalTable: "WaitersDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaitersDb_RestaurantsDb_RestaurantId",
                table: "WaitersDb",
                column: "RestaurantId",
                principalTable: "RestaurantsDb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientsDb_RestaurantsDb_RestaurantId",
                table: "ClientsDb");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientWaiter_ClientsDb_ClientsId",
                table: "ClientWaiter");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientWaiter_WaitersDb_WaitersId",
                table: "ClientWaiter");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitersDb_RestaurantsDb_RestaurantId",
                table: "WaitersDb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaitersDb",
                table: "WaitersDb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RestaurantsDb",
                table: "RestaurantsDb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientsDb",
                table: "ClientsDb");

            migrationBuilder.RenameTable(
                name: "WaitersDb",
                newName: "Waiters");

            migrationBuilder.RenameTable(
                name: "RestaurantsDb",
                newName: "Restaurants");

            migrationBuilder.RenameTable(
                name: "ClientsDb",
                newName: "Clients");

            migrationBuilder.RenameIndex(
                name: "IX_WaitersDb_RestaurantId",
                table: "Waiters",
                newName: "IX_Waiters_RestaurantId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientsDb_RestaurantId",
                table: "Clients",
                newName: "IX_Clients_RestaurantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Waiters",
                table: "Waiters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Restaurants",
                table: "Restaurants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Restaurants_RestaurantId",
                table: "Clients",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientWaiter_Clients_ClientsId",
                table: "ClientWaiter",
                column: "ClientsId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientWaiter_Waiters_WaitersId",
                table: "ClientWaiter",
                column: "WaitersId",
                principalTable: "Waiters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Waiters_Restaurants_RestaurantId",
                table: "Waiters",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
