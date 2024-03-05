using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TienDaoAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnAndAddNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Stories_story_id",
                table: "Chapters");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Stories_story_id",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_user_id",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Stories_story_id",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_user_id",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Genres_genre_id",
                table: "Stories");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Users_user_id",
                table: "Stories");

            migrationBuilder.RenameColumn(
                name: "views",
                table: "Stories",
                newName: "Views");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Stories",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Stories",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Stories",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Stories",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "author",
                table: "Stories",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Stories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Stories",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "update_date",
                table: "Stories",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "genre_id",
                table: "Stories",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "create_date",
                table: "Stories",
                newName: "CreateDate");

            migrationBuilder.RenameIndex(
                name: "IX_Stories_user_id",
                table: "Stories",
                newName: "IX_Stories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Stories_genre_id",
                table: "Stories",
                newName: "IX_Stories_GenreId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Reviews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Reviews",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "time",
                table: "Reviews",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "story_id",
                table: "Reviews",
                newName: "StoryId");

            migrationBuilder.RenameColumn(
                name: "review_content",
                table: "Reviews",
                newName: "ReviewContent");

            migrationBuilder.RenameColumn(
                name: "rating_world",
                table: "Reviews",
                newName: "RatingWorld");

            migrationBuilder.RenameColumn(
                name: "rating_translation",
                table: "Reviews",
                newName: "RatingTranslation");

            migrationBuilder.RenameColumn(
                name: "rating_plot",
                table: "Reviews",
                newName: "RatingPlot");

            migrationBuilder.RenameColumn(
                name: "rating_character",
                table: "Reviews",
                newName: "RatingCharacter");

            migrationBuilder.RenameColumn(
                name: "chapter_number",
                table: "Reviews",
                newName: "ChapterNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_user_id",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_story_id",
                table: "Reviews",
                newName: "IX_Reviews_StoryId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Genres",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Genres",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Genres",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "time",
                table: "Comments",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Comments",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Comments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "story_id",
                table: "Comments",
                newName: "StoryId");

            migrationBuilder.RenameColumn(
                name: "chapter_number",
                table: "Comments",
                newName: "ChapterNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_user_id",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_story_id",
                table: "Comments",
                newName: "IX_Comments_StoryId");

            migrationBuilder.RenameColumn(
                name: "order",
                table: "Chapters",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Chapters",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "emoji",
                table: "Chapters",
                newName: "Emoji");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "Chapters",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Chapters",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "story_id",
                table: "Chapters",
                newName: "StoryId");

            migrationBuilder.RenameColumn(
                name: "published_date",
                table: "Chapters",
                newName: "PublishedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Chapters_story_id",
                table: "Chapters",
                newName: "IX_Chapters_StoryId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Stories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ReadChapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ChapterId = table.Column<int>(type: "int", nullable: false),
                    ReadTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadChapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadChapters_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadChapters_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadChapters_ChapterId",
                table: "ReadChapters",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadChapters_UserId",
                table: "ReadChapters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Stories_StoryId",
                table: "Chapters",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Stories_StoryId",
                table: "Comments",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Stories_StoryId",
                table: "Reviews",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Genres_GenreId",
                table: "Stories",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Users_UserId",
                table: "Stories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Stories_StoryId",
                table: "Chapters");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Stories_StoryId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Stories_StoryId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Genres_GenreId",
                table: "Stories");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Users_UserId",
                table: "Stories");

            migrationBuilder.DropTable(
                name: "ReadChapters");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "Views",
                table: "Stories",
                newName: "views");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Stories",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Stories",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Stories",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Stories",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Stories",
                newName: "author");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Stories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Stories",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Stories",
                newName: "update_date");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "Stories",
                newName: "genre_id");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Stories",
                newName: "create_date");

            migrationBuilder.RenameIndex(
                name: "IX_Stories_UserId",
                table: "Stories",
                newName: "IX_Stories_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Stories_GenreId",
                table: "Stories",
                newName: "IX_Stories_genre_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reviews",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reviews",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "StoryId",
                table: "Reviews",
                newName: "story_id");

            migrationBuilder.RenameColumn(
                name: "ReviewContent",
                table: "Reviews",
                newName: "review_content");

            migrationBuilder.RenameColumn(
                name: "RatingWorld",
                table: "Reviews",
                newName: "rating_world");

            migrationBuilder.RenameColumn(
                name: "RatingTranslation",
                table: "Reviews",
                newName: "rating_translation");

            migrationBuilder.RenameColumn(
                name: "RatingPlot",
                table: "Reviews",
                newName: "rating_plot");

            migrationBuilder.RenameColumn(
                name: "RatingCharacter",
                table: "Reviews",
                newName: "rating_character");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reviews",
                newName: "time");

            migrationBuilder.RenameColumn(
                name: "ChapterNumber",
                table: "Reviews",
                newName: "chapter_number");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                newName: "IX_Reviews_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_StoryId",
                table: "Reviews",
                newName: "IX_Reviews_story_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Genres",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Genres",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Genres",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Comments",
                newName: "time");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Comments",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Comments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "StoryId",
                table: "Comments",
                newName: "story_id");

            migrationBuilder.RenameColumn(
                name: "ChapterNumber",
                table: "Comments",
                newName: "chapter_number");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                newName: "IX_Comments_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_StoryId",
                table: "Comments",
                newName: "IX_Comments_story_id");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Chapters",
                newName: "order");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Chapters",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Emoji",
                table: "Chapters",
                newName: "emoji");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Chapters",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Chapters",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StoryId",
                table: "Chapters",
                newName: "story_id");

            migrationBuilder.RenameColumn(
                name: "PublishedDate",
                table: "Chapters",
                newName: "published_date");

            migrationBuilder.RenameIndex(
                name: "IX_Chapters_StoryId",
                table: "Chapters",
                newName: "IX_Chapters_story_id");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "Stories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Stories_story_id",
                table: "Chapters",
                column: "story_id",
                principalTable: "Stories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Stories_story_id",
                table: "Comments",
                column: "story_id",
                principalTable: "Stories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_user_id",
                table: "Comments",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Stories_story_id",
                table: "Reviews",
                column: "story_id",
                principalTable: "Stories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_user_id",
                table: "Reviews",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Genres_genre_id",
                table: "Stories",
                column: "genre_id",
                principalTable: "Genres",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Users_user_id",
                table: "Stories",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
