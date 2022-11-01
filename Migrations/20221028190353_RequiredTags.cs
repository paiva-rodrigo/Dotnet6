using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet6.Migrations
{
    public partial class RequiredTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Products_ProductId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_ProductId",
                table: "Tag");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "Tag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);



            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Products_ProductId1",
                table: "Tag",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Products_ProductId1",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_ProductId1",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Tag");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Tag",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ProductId",
                table: "Tag",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Products_ProductId",
                table: "Tag",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
