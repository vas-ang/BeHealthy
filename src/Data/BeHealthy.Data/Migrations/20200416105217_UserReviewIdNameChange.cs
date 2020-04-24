namespace BeHealthy.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UserReviewIdNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseReviews_AspNetUsers_UserId",
                table: "ExerciseRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseReviews",
                table: "ExerciseRatings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExerciseRatings");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExerciseRatings",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseReviews",
                table: "ExerciseRatings",
                columns: new[] { "UserId", "ExerciseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseReviews_AspNetUsers_AuthorId",
                table: "ExerciseRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseReviews_AspNetUsers_AuthorId",
                table: "ExerciseRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseReviews",
                table: "ExerciseRatings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExerciseRatings");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExerciseRatings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseReviews",
                table: "ExerciseRatings",
                columns: new[] { "UserId", "ExerciseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseReviews_AspNetUsers_UserId",
                table: "ExerciseRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
