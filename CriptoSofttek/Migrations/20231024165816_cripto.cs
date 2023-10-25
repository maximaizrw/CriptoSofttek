using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CriptoSofttek.Migrations
{
    public partial class cripto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CryptoAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FiatAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CBU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USDBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PesosBalance = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiatAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TypeMovement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FiatAccountId = table.Column<int>(type: "int", nullable: false),
                    FiatAccountId1 = table.Column<int>(type: "int", nullable: true),
                    CryptoAccountId = table.Column<int>(type: "int", nullable: false),
                    CryptoAccountId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_CryptoAccounts_CryptoAccountId1",
                        column: x => x.CryptoAccountId1,
                        principalTable: "CryptoAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_FiatAccounts_FiatAccountId1",
                        column: x => x.FiatAccountId1,
                        principalTable: "FiatAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Activo", "CryptoAccountId", "CryptoAccountId1", "Email", "FiatAccountId", "FiatAccountId1", "FirstName", "LastName", "Password" },
                values: new object[] { 1, false, 0, null, "maxi@gmail.com", 0, null, "Maxi", "Maiz", "7d61ee0ed586257cff94117dcd15a63b5c2216385cba22a8393d7376feb515c0" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CryptoAccountId1",
                table: "Users",
                column: "CryptoAccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FiatAccountId1",
                table: "Users",
                column: "FiatAccountId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CryptoAccounts");

            migrationBuilder.DropTable(
                name: "FiatAccounts");
        }
    }
}
