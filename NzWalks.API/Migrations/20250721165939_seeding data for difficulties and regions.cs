using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NzWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class seedingdatafordifficultiesandregions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "b10feddb-72d1-4bdb-9a80-806d873b7d63", "Easy" },
                    { "cc3ce127-c947-43a3-8fa9-27663247daa3", "Hard" },
                    { "e6b1b599-03c5-4d6f-8e0a-b93289f4650e", "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { "28c791cd-4e28-4a03-b4a7-78e80387fa91", "NZ-002", "South Island", "https://example.com/south-island.jpg" },
                    { "bc1e31f0-ec44-4980-bb40-d470e4f5ed0e", "NZ-001", "North Island", "https://example.com/north-island.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: "b10feddb-72d1-4bdb-9a80-806d873b7d63");

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: "cc3ce127-c947-43a3-8fa9-27663247daa3");

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: "e6b1b599-03c5-4d6f-8e0a-b93289f4650e");

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: "28c791cd-4e28-4a03-b4a7-78e80387fa91");

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: "bc1e31f0-ec44-4980-bb40-d470e4f5ed0e");
        }
    }
}
