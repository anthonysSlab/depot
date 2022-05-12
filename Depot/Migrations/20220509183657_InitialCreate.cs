using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivityKicking = table.Column<bool>(type: "INTEGER", nullable: false),
                    DurationWarn = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    DurationKick = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildUsers",
                columns: table => new
                {
                    UserId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    GuildId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HasActivityWarn = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActivityWarn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildUsers", x => new { x.UserId, x.GuildId });
                    table.ForeignKey(
                        name: "FK_GuildUsers_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GuildId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    GuildId1 = table.Column<ulong>(type: "INTEGER", nullable: true),
                    GuildUserGuildId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    GuildUserUserId = table.Column<ulong>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Roles_Guilds_GuildId1",
                        column: x => x.GuildId1,
                        principalTable: "Guilds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Roles_GuildUsers_GuildUserUserId_GuildUserGuildId",
                        columns: x => new { x.GuildUserUserId, x.GuildUserGuildId },
                        principalTable: "GuildUsers",
                        principalColumns: new[] { "UserId", "GuildId" });
                });

            migrationBuilder.CreateTable(
                name: "Warning",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    UserGuildId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuildId = table.Column<ulong>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warning", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warning_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Warning_GuildUsers_UserId_UserGuildId",
                        columns: x => new { x.UserId, x.UserGuildId },
                        principalTable: "GuildUsers",
                        principalColumns: new[] { "UserId", "GuildId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildUsers_GuildId",
                table: "GuildUsers",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_GuildId",
                table: "Roles",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_GuildId1",
                table: "Roles",
                column: "GuildId1");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_GuildUserUserId_GuildUserGuildId",
                table: "Roles",
                columns: new[] { "GuildUserUserId", "GuildUserGuildId" });

            migrationBuilder.CreateIndex(
                name: "IX_Warning_GuildId",
                table: "Warning",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Warning_UserId_UserGuildId",
                table: "Warning",
                columns: new[] { "UserId", "UserGuildId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Warning");

            migrationBuilder.DropTable(
                name: "GuildUsers");

            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
