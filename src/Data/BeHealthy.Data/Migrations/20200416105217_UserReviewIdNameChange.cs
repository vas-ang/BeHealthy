namespace BeHealthy.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UserReviewIdNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseReviews_AspNetUsers_UserId",
                table: "ExerciseReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseReviews",
                table: "ExerciseReviews");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExerciseReviews");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "ExerciseReviews",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseReviews",
                table: "ExerciseReviews",
                columns: new[] { "AuthorId", "ExerciseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseReviews_AspNetUsers_AuthorId",
                table: "ExerciseReviews",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseReviews_AspNetUsers_AuthorId",
                table: "ExerciseReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseReviews",
                table: "ExerciseReviews");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "ExerciseReviews");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExerciseReviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseReviews",
                table: "ExerciseReviews",
                columns: new[] { "UserId", "ExerciseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseReviews_AspNetUsers_UserId",
                table: "ExerciseReviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
