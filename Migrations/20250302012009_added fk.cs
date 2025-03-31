using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class addedfk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_Departmentid",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "Departmentid",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeptID",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_Departmentid",
                table: "Students",
                column: "Departmentid",
                principalTable: "Departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_Departmentid",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DeptID",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "Departmentid",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_Departmentid",
                table: "Students",
                column: "Departmentid",
                principalTable: "Departments",
                principalColumn: "id");
        }
    }
}
