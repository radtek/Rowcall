using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RowcallBackend.Migrations
{
    public partial class classroomowner_added_11_36 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "ClassRoom",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassRoom_TeacherId",
                table: "ClassRoom",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassRoom_AspNetUsers_TeacherId",
                table: "ClassRoom",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassRoom_AspNetUsers_TeacherId",
                table: "ClassRoom");

            migrationBuilder.DropIndex(
                name: "IX_ClassRoom_TeacherId",
                table: "ClassRoom");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "ClassRoom");
        }
    }
}
