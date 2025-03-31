using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class Addednullableandconsistency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_Departments_Departmentid",
                table: "Instructor");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_Departmentid",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DeptID",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "age",
                table: "Students",
                newName: "Age");

            migrationBuilder.RenameColumn(
                name: "Departmentid",
                table: "Students",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_Departmentid",
                table: "Students",
                newName: "IX_Students_DepartmentId");

            migrationBuilder.RenameColumn(
                name: "age",
                table: "Instructor",
                newName: "Age");

            migrationBuilder.RenameColumn(
                name: "Departmentid",
                table: "Instructor",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Instructor_Departmentid",
                table: "Instructor",
                newName: "IX_Instructor_DepartmentId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Departments",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "Departments",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Departments",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Departments",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_Departments_DepartmentId",
                table: "Instructor",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_DepartmentId",
                table: "Students",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_Departments_DepartmentId",
                table: "Instructor");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_DepartmentId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Students",
                newName: "Departmentid");

            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Students",
                newName: "age");

            migrationBuilder.RenameIndex(
                name: "IX_Students_DepartmentId",
                table: "Students",
                newName: "IX_Students_Departmentid");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Instructor",
                newName: "Departmentid");

            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Instructor",
                newName: "age");

            migrationBuilder.RenameIndex(
                name: "IX_Instructor_DepartmentId",
                table: "Instructor",
                newName: "IX_Instructor_Departmentid");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Departments",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Departments",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Departments",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Departments",
                newName: "id");

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
                name: "FK_Instructor_Departments_Departmentid",
                table: "Instructor",
                column: "Departmentid",
                principalTable: "Departments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_Departmentid",
                table: "Students",
                column: "Departmentid",
                principalTable: "Departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
