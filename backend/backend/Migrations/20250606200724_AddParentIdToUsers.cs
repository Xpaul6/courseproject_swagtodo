using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddParentIdToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "familycodes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<int>(type: "integer", nullable: false),
                    ParentUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_familycodes", x => x.id);
                    table.ForeignKey(
                        name: "FK_familycodes_users_ParentUserId",
                        column: x => x.ParentUserId,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_familycodes_users_parent_id",
                        column: x => x.parent_id,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_parent_id",
                table: "users",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_familycodes_code",
                table: "familycodes",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_familycodes_parent_id",
                table: "familycodes",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_familycodes_ParentUserId",
                table: "familycodes",
                column: "ParentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_users_parent_id",
                table: "users",
                column: "parent_id",
                principalTable: "users",
                principalColumn: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_users_parent_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "familycodes");

            migrationBuilder.DropIndex(
                name: "IX_users_parent_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "users");
        }
    }
}
