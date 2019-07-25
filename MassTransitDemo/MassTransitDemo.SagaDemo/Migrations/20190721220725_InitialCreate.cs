using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MassTransitDemo.SagaDemo.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingCart",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(nullable: false),
                    CurrentState = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    ExpirationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCart", x => x.CorrelationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCart");
        }
    }
}
