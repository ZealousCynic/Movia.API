using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiNinjectStudio.Domain.Migrations
{
    public partial class AddRecordNumberOfPassenger1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NumberOfPassengers",
                columns: new[] { "ID", "CreateDT", "IsDeleted", "Latitude", "Longitude", "RouteBusID", "Total" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 10, 27, 8, 48, 0, 0, DateTimeKind.Unspecified), false, 11.792655999999999, 55.464244999999998, 2, 8 },
                    { 2, new DateTime(2020, 10, 27, 8, 58, 0, 0, DateTimeKind.Unspecified), false, 11.790853, 55.463222999999999, 2, 12 },
                    { 3, new DateTime(2020, 10, 27, 9, 3, 0, 0, DateTimeKind.Unspecified), false, 11.790682, 55.461714999999998, 2, 19 },
                    { 4, new DateTime(2020, 10, 27, 9, 11, 0, 0, DateTimeKind.Unspecified), false, 11.793170999999999, 55.459572999999999, 2, 27 },
                    { 5, new DateTime(2020, 10, 27, 9, 19, 0, 0, DateTimeKind.Unspecified), false, 11.793428, 55.457773000000003, 2, 37 },
                    { 6, new DateTime(2020, 10, 27, 9, 27, 0, 0, DateTimeKind.Unspecified), false, 11.786647, 55.450325999999997, 2, 44 },
                    { 7, new DateTime(2020, 10, 27, 9, 34, 0, 0, DateTimeKind.Unspecified), false, 11.780639000000001, 55.447989999999997, 2, 56 },
                    { 8, new DateTime(2020, 10, 27, 9, 42, 0, 0, DateTimeKind.Unspecified), false, 11.780639000000001, 55.442439999999998, 2, 61 },
                    { 9, new DateTime(2020, 10, 27, 9, 51, 0, 0, DateTimeKind.Unspecified), false, 11.787162, 55.43835, 2, 67 },
                    { 10, new DateTime(2020, 10, 27, 9, 59, 0, 0, DateTimeKind.Unspecified), false, 11.790596000000001, 55.434454000000002, 2, 58 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NumberOfPassengers",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NumberOfPassengers",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NumberOfPassengers",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NumberOfPassengers",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "NumberOfPassengers",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "NumberOfPassengers",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "NumberOfPassengers",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "NumberOfPassengers",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "NumberOfPassengers",
                keyColumn: "ID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "NumberOfPassengers",
                keyColumn: "ID",
                keyValue: 10);
        }
    }
}
