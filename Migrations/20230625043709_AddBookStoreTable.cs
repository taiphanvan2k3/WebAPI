using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnApiWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddBookStoreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaCuaHang",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookStores",
                columns: table => new
                {
                    MaCuaHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCuaHang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookStores", x => x.MaCuaHang);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_MaCuaHang",
                table: "Books",
                column: "MaCuaHang");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_BookStore",
                table: "Books",
                column: "MaCuaHang",
                principalTable: "BookStores",
                principalColumn: "MaCuaHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_BookStore",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BookStores");

            migrationBuilder.DropIndex(
                name: "IX_Books_MaCuaHang",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "MaCuaHang",
                table: "Books");
        }
    }
}
