using Microsoft.EntityFrameworkCore.Migrations;

namespace AirlineWeb.Migrations
{
    public partial class WebhookInitalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebhookSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WebhookURL = table.Column<string>(type: "TEXT", nullable: false),
                    Secret = table.Column<string>(type: "TEXT", nullable: false),
                    WebhookType = table.Column<string>(type: "TEXT", nullable: false),
                    WebhookPublisher = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookSubscriptions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebhookSubscriptions");
        }
    }
}
