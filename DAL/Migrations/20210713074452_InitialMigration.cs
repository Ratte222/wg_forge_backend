using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_colors_info",
                columns: table => new
                {
                    color = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_colors_info", x => x.color);
                });

            migrationBuilder.CreateTable(
                name: "CatOwners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatOwners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cats",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TailLength = table.Column<int>(type: "int", nullable: false),
                    WhiskersLength = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cats", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "cats_stat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tail_length_mean = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tail_length_median = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tail_length_mode = table.Column<int>(type: "int", nullable: false),
                    whiskers_length_mean = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    whiskers_length_median = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    whiskers_length_mode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cats_stat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatCatOwner",
                columns: table => new
                {
                    CatOwnersId = table.Column<int>(type: "int", nullable: false),
                    CatsName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatCatOwner", x => new { x.CatOwnersId, x.CatsName });
                    table.ForeignKey(
                        name: "FK_CatCatOwner_CatOwners_CatOwnersId",
                        column: x => x.CatOwnersId,
                        principalTable: "CatOwners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatCatOwner_Cats_CatsName",
                        column: x => x.CatsName,
                        principalTable: "Cats",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatCatOwner_CatsName",
                table: "CatCatOwner",
                column: "CatsName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_colors_info");

            migrationBuilder.DropTable(
                name: "CatCatOwner");

            migrationBuilder.DropTable(
                name: "cats_stat");

            migrationBuilder.DropTable(
                name: "CatOwners");

            migrationBuilder.DropTable(
                name: "Cats");
        }
    }
}
