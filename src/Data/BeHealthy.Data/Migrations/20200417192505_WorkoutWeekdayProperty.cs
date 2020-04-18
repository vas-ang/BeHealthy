namespace BeHealthy.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class WorkoutWeekdayProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Weekday",
                table: "Workout",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weekday",
                table: "Workout");
        }
    }
}
