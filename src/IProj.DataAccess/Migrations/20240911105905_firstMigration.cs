using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IProj.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class firstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageContent = table.Column<string>(type: "text", nullable: false),
                    SendTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReadTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    Gmail = table.Column<string>(type: "text", nullable: false),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "FirstName", "Gmail", "IsOnline", "Position", "RoleName" },
                values: new object[,]
                {
                    { new Guid("17820355-1ee5-49c8-9cca-ea6cfb5312ca"), "Olim", "olim@gmail.com", false, "Project Manager", "Worker" },
                    { new Guid("53aa79f4-1650-4baa-b5c7-c7a1fd9d4128"), "Samandarbek", "samandarbekyr@gmail.com", false, "Backend Developer", "Worker" },
                    { new Guid("6effb728-a7cd-460c-91e5-9f048185fd11"), "Muhammadqodir", "muhammadqodir5050@gmail.com", false, "Desktop Developer", "Owner" },
                    { new Guid("a8f7f39e-3445-433c-93d3-e6755610a5e0"), "Samandar", "sharpistmaster@gmail.com", false, "Full-stack Developer", "Worker" },
                    { new Guid("aae35ced-e156-4e1d-beb0-0a5d035763e0"), "Able", "able.devops@gmail.com", false, "Devops", "Worker" },
                    { new Guid("dd13aea5-d3fd-4afc-8fb8-7f5940766935"), "Behruz", "uzgrandmaster@gmail.com", false, "Frontend Developer", "Worker" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
