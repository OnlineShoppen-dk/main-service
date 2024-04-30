using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace main_service.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingModelsAndData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "UserDetails");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "UserDetails",
                newName: "IX_UserDetails_Email");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Orders",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderNumber",
                table: "Orders",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "UserDetailsId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserDetailsId",
                table: "Orders",
                column: "UserDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserDetails_UserDetailsId",
                table: "Orders",
                column: "UserDetailsId",
                principalTable: "UserDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserDetails_UserDetailsId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserDetailsId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserDetailsId",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "UserDetails",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_UserDetails_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "TransactionId",
                keyValue: null,
                column: "TransactionId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Orders",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNumber",
                table: "Orders",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");
        }
    }
}
