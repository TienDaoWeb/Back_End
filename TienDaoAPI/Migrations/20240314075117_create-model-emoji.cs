using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TienDaoAPI.Migrations
{
    /// <inheritdoc />
    public partial class createmodelemoji : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Emoji",
                table: "Chapters",
                newName: "EmojiId");

            migrationBuilder.CreateTable(
                name: "Emojis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Heart = table.Column<int>(type: "int", nullable: false),
                    Like = table.Column<int>(type: "int", nullable: false),
                    Fun = table.Column<int>(type: "int", nullable: false),
                    Sad = table.Column<int>(type: "int", nullable: false),
                    Angry = table.Column<int>(type: "int", nullable: false),
                    Attack = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emojis", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_EmojiId",
                table: "Chapters",
                column: "EmojiId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Emojis_EmojiId",
                table: "Chapters",
                column: "EmojiId",
                principalTable: "Emojis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Emojis_EmojiId",
                table: "Chapters");

            migrationBuilder.DropTable(
                name: "Emojis");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_EmojiId",
                table: "Chapters");

            migrationBuilder.RenameColumn(
                name: "EmojiId",
                table: "Chapters",
                newName: "Emoji");
        }
    }
}
