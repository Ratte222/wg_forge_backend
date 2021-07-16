using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class FixMtMRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            //----------my--------------
            migrationBuilder.DropPrimaryKey(
                name: "PK_CatPhotos",
                table: "CatPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_CatsAndOwners_CatOwners_CatOwnersId",
                table: "CatsAndOwners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatOwners",
                table: "CatOwners");
            //----------my--------------

            migrationBuilder.DropForeignKey(
                name: "FK_CatPhotos_Cats_CatName",
                table: "CatPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_CatsAndOwners_Cats_CatsName",
                table: "CatsAndOwners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatsAndOwners",
                table: "CatsAndOwners");

            migrationBuilder.DropIndex(
                name: "IX_CatsAndOwners_CatsName",
                table: "CatsAndOwners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cats",
                table: "Cats");

            migrationBuilder.DropIndex(
                name: "IX_CatPhotos_CatName",
                table: "CatPhotos");

            migrationBuilder.DropColumn(
                name: "CatsName",
                table: "CatsAndOwners");

            migrationBuilder.DropColumn(
                name: "CatName",
                table: "CatPhotos");

            migrationBuilder.AlterColumn<long>(
                name: "CatOwnersId",
                table: "CatsAndOwners",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "CatsId",
                table: "CatsAndOwners",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cats",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Cats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CatPhotos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "CatId",
                table: "CatPhotos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CatOwners",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatsAndOwners",
                table: "CatsAndOwners",
                columns: new[] { "CatOwnersId", "CatsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cats",
                table: "Cats",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CatsAndOwners_CatsId",
                table: "CatsAndOwners",
                column: "CatsId");

            migrationBuilder.CreateIndex(
                name: "IX_CatPhotos_CatId",
                table: "CatPhotos",
                column: "CatId");

            //----------my--------------
            migrationBuilder.AddPrimaryKey(
                name: "PK_CatPhotos",
                table: "CatPhotos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
               name: "PK_CatOwners",
               table: "CatOwners",
               column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CatsAndOwners_CatOwnersId",
                table: "CatsAndOwners",
                column: "CatOwnersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatCatOwner_CatOwners_CatOwnersId",
                table: "CatsAndOwners",
                column: "CatOwnersId",
                principalTable: "CatOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            //----------my--------------

            migrationBuilder.AddForeignKey(
                name: "FK_CatPhotos_Cats_CatId",
                table: "CatPhotos",
                column: "CatId",
                principalTable: "Cats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatsAndOwners_Cats_CatsId",
                table: "CatsAndOwners",
                column: "CatsId",
                principalTable: "Cats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //----------my--------------
            migrationBuilder.DropPrimaryKey(
                name: "PK_CatPhotos",
                table: "CatPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_CatsAndOwners_CatOwners_CatOwnersId",
                table: "CatsAndOwners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatOwners",
                table: "CatOwners");
            //----------my--------------

            migrationBuilder.DropForeignKey(
                name: "FK_CatPhotos_Cats_CatId",
                table: "CatPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_CatsAndOwners_Cats_CatsId",
                table: "CatsAndOwners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatsAndOwners",
                table: "CatsAndOwners");

            migrationBuilder.DropIndex(
                name: "IX_CatsAndOwners_CatsId",
                table: "CatsAndOwners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cats",
                table: "Cats");

            migrationBuilder.DropIndex(
                name: "IX_CatPhotos_CatId",
                table: "CatPhotos");

            migrationBuilder.DropColumn(
                name: "CatsId",
                table: "CatsAndOwners");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Cats");

            migrationBuilder.DropColumn(
                name: "CatId",
                table: "CatPhotos");

            migrationBuilder.AlterColumn<int>(
                name: "CatOwnersId",
                table: "CatsAndOwners",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "CatsName",
                table: "CatsAndOwners",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cats",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CatPhotos",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "CatName",
                table: "CatPhotos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CatOwners",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatsAndOwners",
                table: "CatsAndOwners",
                columns: new[] { "CatOwnersId", "CatsName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cats",
                table: "Cats",
                column: "Name");

            //----------my--------------
            migrationBuilder.AddPrimaryKey(
                name: "PK_CatPhotos",
                table: "CatPhotos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
               name: "PK_CatOwners",
               table: "CatOwners",
               column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CatsAndOwners_CatOwnersId",
                table: "CatsAndOwners",
                column: "CatOwnersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatCatOwner_CatOwners_CatOwnersId",
                table: "CatsAndOwners",
                column: "CatOwnersId",
                principalTable: "CatOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            //----------my--------------

            migrationBuilder.CreateIndex(
                name: "IX_CatsAndOwners_CatsName",
                table: "CatsAndOwners",
                column: "CatsName");

            migrationBuilder.CreateIndex(
                name: "IX_CatPhotos_CatName",
                table: "CatPhotos",
                column: "CatName");

            migrationBuilder.AddForeignKey(
                name: "FK_CatPhotos_Cats_CatName",
                table: "CatPhotos",
                column: "CatName",
                principalTable: "Cats",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CatsAndOwners_Cats_CatsName",
                table: "CatsAndOwners",
                column: "CatsName",
                principalTable: "Cats",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
