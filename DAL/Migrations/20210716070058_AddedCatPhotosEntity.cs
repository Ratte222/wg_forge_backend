using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AddedCatPhotosEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatPhotoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CatName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatPhotos_Cats_CatName",
                        column: x => x.CatName,
                        principalTable: "Cats",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatPhotos_CatName",
                table: "CatPhotos",
                column: "CatName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatPhotos");
        }
    }
}
