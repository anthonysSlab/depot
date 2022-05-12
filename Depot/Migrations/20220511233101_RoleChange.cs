using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Migrations
{
    public partial class RoleChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_GuildUsers_GuildUserUserId_GuildUserGuildId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_GuildUserUserId_GuildUserGuildId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "GuildUserGuildId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "GuildUserUserId",
                table: "Roles");

            migrationBuilder.CreateTable(
                name: "GuildUserRole",
                columns: table => new
                {
                    RolesId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    UsersUserId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    UsersGuildId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildUserRole", x => new { x.RolesId, x.UsersUserId, x.UsersGuildId });
                    table.ForeignKey(
                        name: "FK_GuildUserRole_GuildUsers_UsersUserId_UsersGuildId",
                        columns: x => new { x.UsersUserId, x.UsersGuildId },
                        principalTable: "GuildUsers",
                        principalColumns: new[] { "UserId", "GuildId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildUserRole_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildUserRole_UsersUserId_UsersGuildId",
                table: "GuildUserRole",
                columns: new[] { "UsersUserId", "UsersGuildId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildUserRole");

            migrationBuilder.AddColumn<ulong>(
                name: "GuildUserGuildId",
                table: "Roles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "GuildUserUserId",
                table: "Roles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_GuildUserUserId_GuildUserGuildId",
                table: "Roles",
                columns: new[] { "GuildUserUserId", "GuildUserGuildId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_GuildUsers_GuildUserUserId_GuildUserGuildId",
                table: "Roles",
                columns: new[] { "GuildUserUserId", "GuildUserGuildId" },
                principalTable: "GuildUsers",
                principalColumns: new[] { "UserId", "GuildId" });
        }
    }
}
