using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RecreateFamilyCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_familycodes_users_ParentUserId",
                table: "familycodes");

            migrationBuilder.DropIndex(
                name: "IX_familycodes_ParentUserId",
                table: "familycodes");

            migrationBuilder.DropColumn(
                name: "ParentUserId",
                table: "familycodes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentUserId",
                table: "familycodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_familycodes_ParentUserId",
                table: "familycodes",
                column: "ParentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_familycodes_users_ParentUserId",
                table: "familycodes",
                column: "ParentUserId",
                principalTable: "users",
                principalColumn: "userid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
