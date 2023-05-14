using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pronia.Migrations
{
    public partial class AddNewColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComment_AspNetUsers_AppUserId",
                table: "BlogComment");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogComment_Blogs_BlogId",
                table: "BlogComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_AspNetUsers_AppUserId",
                table: "ProductComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComment_Products_ProductId",
                table: "ProductComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductComment",
                table: "ProductComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogComment",
                table: "BlogComment");

            migrationBuilder.RenameTable(
                name: "ProductComment",
                newName: "ProductComments");

            migrationBuilder.RenameTable(
                name: "BlogComment",
                newName: "BlogComments");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComment_ProductId",
                table: "ProductComments",
                newName: "IX_ProductComments_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComment_AppUserId",
                table: "ProductComments",
                newName: "IX_ProductComments_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogComment_BlogId",
                table: "BlogComments",
                newName: "IX_BlogComments_BlogId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogComment_AppUserId",
                table: "BlogComments",
                newName: "IX_BlogComments_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductComments",
                table: "ProductComments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogComments",
                table: "BlogComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId",
                table: "BlogComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComments_Blogs_BlogId",
                table: "BlogComments",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_AspNetUsers_AppUserId",
                table: "ProductComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComments_Products_ProductId",
                table: "ProductComments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_AspNetUsers_AppUserId",
                table: "BlogComments");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogComments_Blogs_BlogId",
                table: "BlogComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_AspNetUsers_AppUserId",
                table: "ProductComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductComments_Products_ProductId",
                table: "ProductComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductComments",
                table: "ProductComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogComments",
                table: "BlogComments");

            migrationBuilder.RenameTable(
                name: "ProductComments",
                newName: "ProductComment");

            migrationBuilder.RenameTable(
                name: "BlogComments",
                newName: "BlogComment");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComments_ProductId",
                table: "ProductComment",
                newName: "IX_ProductComment_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductComments_AppUserId",
                table: "ProductComment",
                newName: "IX_ProductComment_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogComments_BlogId",
                table: "BlogComment",
                newName: "IX_BlogComment_BlogId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogComments_AppUserId",
                table: "BlogComment",
                newName: "IX_BlogComment_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductComment",
                table: "ProductComment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogComment",
                table: "BlogComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComment_AspNetUsers_AppUserId",
                table: "BlogComment",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogComment_Blogs_BlogId",
                table: "BlogComment",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_AspNetUsers_AppUserId",
                table: "ProductComment",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductComment_Products_ProductId",
                table: "ProductComment",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
