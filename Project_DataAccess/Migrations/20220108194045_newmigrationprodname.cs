using Microsoft.EntityFrameworkCore.Migrations;

namespace Project_DataAccess.Migrations
{
    public partial class newmigrationprodname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InquiryDetail_Product_ProductId",
                table: "InquiryDetail");

            migrationBuilder.DropColumn(
                name: "PropductId",
                table: "InquiryDetail");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "InquiryDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InquiryDetail_Product_ProductId",
                table: "InquiryDetail",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InquiryDetail_Product_ProductId",
                table: "InquiryDetail");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "InquiryDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PropductId",
                table: "InquiryDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_InquiryDetail_Product_ProductId",
                table: "InquiryDetail",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
