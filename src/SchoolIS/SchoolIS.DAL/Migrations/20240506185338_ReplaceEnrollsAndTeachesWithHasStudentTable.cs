using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolIS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceEnrollsAndTeachesWithHasStudentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HasSubject_User_StudentId",
                table: "HasSubject");

            migrationBuilder.DropTable(
                name: "Teaches");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "HasSubject",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_HasSubject_StudentId",
                table: "HasSubject",
                newName: "IX_HasSubject_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HasSubject_User_UserId",
                table: "HasSubject",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HasSubject_User_UserId",
                table: "HasSubject");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "HasSubject",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_HasSubject_UserId",
                table: "HasSubject",
                newName: "IX_HasSubject_StudentId");

            migrationBuilder.CreateTable(
                name: "Teaches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubjectId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeacherId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teaches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teaches_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teaches_User_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teaches_SubjectId",
                table: "Teaches",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Teaches_TeacherId",
                table: "Teaches",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_HasSubject_User_StudentId",
                table: "HasSubject",
                column: "StudentId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
