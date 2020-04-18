namespace BeHealthy.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class WorkoutSetsRepsAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "WorkoutExercise");

            migrationBuilder.AddColumn<int>(
                name: "Repetitions",
                table: "WorkoutExercise",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sets",
                table: "WorkoutExercise",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Repetitions",
                table: "WorkoutExercise");

            migrationBuilder.DropColumn(
                name: "Sets",
                table: "WorkoutExercise");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "WorkoutExercise",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
