using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AddedIntermediateEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatCatOwner");

            migrationBuilder.CreateTable(
                name: "CatsAndOwners",
                columns: table => new
                {
                    CatOwnersId = table.Column<int>(type: "int", nullable: false),
                    CatsName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatsAndOwners", x => new { x.CatOwnersId, x.CatsName });
                    table.ForeignKey(
                        name: "FK_CatsAndOwners_CatOwners_CatOwnersId",
                        column: x => x.CatOwnersId,
                        principalTable: "CatOwners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatsAndOwners_Cats_CatsName",
                        column: x => x.CatsName,
                        principalTable: "Cats",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatsAndOwners_CatsName",
                table: "CatsAndOwners",
                column: "CatsName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatsAndOwners");

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
    }
}
