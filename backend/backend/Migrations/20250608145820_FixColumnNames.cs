using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_familycodes_users_ParentUserId",
                table: "familycodes");

            migrationBuilder.DropForeignKey(
                name: "FK_familycodes_users_parent_id",
                table: "familycodes");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "familycodes",
                newName: "parent_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_familycodes_parent_id",
                table: "familycodes",
                newName: "IX_familycodes_parent_user_id");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "familycodes",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "ParentUserId",
                table: "familycodes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_familycodes_users_ParentUserId",
                table: "familycodes",
                column: "ParentUserId",
                principalTable: "users",
                principalColumn: "userid");

            migrationBuilder.AddForeignKey(
                name: "FK_familycodes_users_parent_user_id",
                table: "familycodes",
                column: "parent_user_id",
                principalTable: "users",
                principalColumn: "userid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_familycodes_users_ParentUserId",
                table: "familycodes");

            migrationBuilder.DropForeignKey(
                name: "FK_familycodes_users_parent_user_id",
                table: "familycodes");

            migrationBuilder.RenameColumn(
                name: "parent_user_id",
                table: "familycodes",
                newName: "parent_id");

            migrationBuilder.RenameIndex(
                name: "IX_familycodes_parent_user_id",
                table: "familycodes",
                newName: "IX_familycodes_parent_id");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "familycodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(8)",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<int>(
                name: "ParentUserId",
                table: "familycodes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_familycodes_users_ParentUserId",
                table: "familycodes",
                column: "ParentUserId",
                principalTable: "users",
                principalColumn: "userid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_familycodes_users_parent_id",
                table: "familycodes",
                column: "parent_id",
                principalTable: "users",
                principalColumn: "userid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
