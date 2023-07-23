using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskList.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class stuff_changed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "TaskItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "TaskItem",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
