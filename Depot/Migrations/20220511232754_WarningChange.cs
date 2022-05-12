using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depot.Migrations
{
    public partial class WarningChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Guilds_GuildId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Guilds_GuildId1",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Warning_Guilds_GuildId",
                table: "Warning");

            migrationBuilder.DropForeignKey(
                name: "FK_Warning_GuildUsers_UserId_UserGuildId",
                table: "Warning");

            migrationBuilder.DropIndex(
                name: "IX_Warning_UserId_UserGuildId",
                table: "Warning");

            migrationBuilder.DropIndex(
                name: "IX_Roles_GuildId1",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserGuildId",
                table: "Warning");

            migrationBuilder.DropColumn(
                name: "GuildId1",
                table: "Roles");

            migrationBuilder.AlterColumn<ulong>(
                name: "GuildId",
                table: "Warning",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul,
                oldClrType: typeof(ulong),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<ulong>(
                name: "GuildId",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul,
                oldClrType: typeof(ulong),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "IgnoredRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    GuildId = table.Column<ulong>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgnoredRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IgnoredRole_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IgnoredRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warning_UserId",
                table: "Warning",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IgnoredRole_GuildId",
                table: "IgnoredRole",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_IgnoredRole_RoleId",
                table: "IgnoredRole",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Guilds_GuildId",
                table: "Roles",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warning_Guilds_GuildId",
                table: "Warning",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Warning_Users_UserId",
                table: "Warning",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Guilds_GuildId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Warning_Guilds_GuildId",
                table: "Warning");

            migrationBuilder.DropForeignKey(
                name: "FK_Warning_Users_UserId",
                table: "Warning");

            migrationBuilder.DropTable(
                name: "IgnoredRole");

            migrationBuilder.DropIndex(
                name: "IX_Warning_UserId",
                table: "Warning");

            migrationBuilder.AlterColumn<ulong>(
                name: "GuildId",
                table: "Warning",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(ulong),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<ulong>(
                name: "UserGuildId",
                table: "Warning",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AlterColumn<ulong>(
                name: "GuildId",
                table: "Roles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(ulong),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<ulong>(
                name: "GuildId1",
                table: "Roles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warning_UserId_UserGuildId",
                table: "Warning",
                columns: new[] { "UserId", "UserGuildId" });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_GuildId1",
                table: "Roles",
                column: "GuildId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Guilds_GuildId",
                table: "Roles",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Guilds_GuildId1",
                table: "Roles",
                column: "GuildId1",
                principalTable: "Guilds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warning_Guilds_GuildId",
                table: "Warning",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warning_GuildUsers_UserId_UserGuildId",
                table: "Warning",
                columns: new[] { "UserId", "UserGuildId" },
                principalTable: "GuildUsers",
                principalColumns: new[] { "UserId", "GuildId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
