namespace BeHealthy.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ExerciseStepOptimizeAndUserDataRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseSteps_ExerciseSteps_NextId",
                table: "ExerciseSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseSteps_ExerciseSteps_PreviousId",
                table: "ExerciseSteps");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseSteps_NextId",
                table: "ExerciseSteps");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseSteps_PreviousId",
                table: "ExerciseSteps");

            migrationBuilder.DropColumn(
                name: "NextId",
                table: "ExerciseSteps");

            migrationBuilder.DropColumn(
                name: "PreviousId",
                table: "ExerciseSteps");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "ExerciseSteps",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "ExerciseSteps");

            migrationBuilder.AddColumn<int>(
                name: "NextId",
                table: "ExerciseSteps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousId",
                table: "ExerciseSteps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSteps_NextId",
                table: "ExerciseSteps",
                column: "NextId",
                unique: true,
                filter: "[NextId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSteps_PreviousId",
                table: "ExerciseSteps",
                column: "PreviousId",
                unique: true,
                filter: "[PreviousId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseSteps_ExerciseSteps_NextId",
                table: "ExerciseSteps",
                column: "NextId",
                principalTable: "ExerciseSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseSteps_ExerciseSteps_PreviousId",
                table: "ExerciseSteps",
                column: "PreviousId",
                principalTable: "ExerciseSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
