using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiNinjectStudio.Domain.Migrations
{
    public partial class KasperPresentation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Email", "FirstName", "IsDeleted", "LastName", "Password", "PhoneNumber", "RoleID" },
                values: new object[,]
                {
                    { 3, "john@movia.com", "John", false, "Johnsen", "mZJJn++VwBYw/hMVu9kP1mJuvXf2NuDM6TwpzM/E70k=", null, 1 }
                });

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 3
                );
        }
    }
}
