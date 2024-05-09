using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace main_service.Migrations
{
    /// <inheritdoc />
    public partial class ProductRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_ProductDescription_ProductDescriptionId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_ProductDescription_ProductDescriptionId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductDescription_ProductDescriptionId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDescription_Products_ProductId",
                table: "ProductDescription");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductDescriptionId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Images_ProductDescriptionId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ProductDescriptionId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "ProductDescription");

            migrationBuilder.DropColumn(
                name: "ProductDescriptionId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductDescriptionId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ProductDescriptionId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "ProductRemovedId",
                table: "ProductRemoved",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "ProductRemovedId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductDescription",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDescription_Products_ProductId",
                table: "ProductDescription",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDescription_Products_ProductId",
                table: "ProductDescription");

            migrationBuilder.DropColumn(
                name: "ProductRemovedId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductRemoved",
                newName: "ProductRemovedId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductDescription",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "ProductDescription",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductDescriptionId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductDescriptionId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductDescriptionId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductDescriptionId",
                table: "OrderItems",
                column: "ProductDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductDescriptionId",
                table: "Images",
                column: "ProductDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ProductDescriptionId",
                table: "Categories",
                column: "ProductDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_ProductDescription_ProductDescriptionId",
                table: "Categories",
                column: "ProductDescriptionId",
                principalTable: "ProductDescription",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ProductDescription_ProductDescriptionId",
                table: "Images",
                column: "ProductDescriptionId",
                principalTable: "ProductDescription",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductDescription_ProductDescriptionId",
                table: "OrderItems",
                column: "ProductDescriptionId",
                principalTable: "ProductDescription",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDescription_Products_ProductId",
                table: "ProductDescription",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
