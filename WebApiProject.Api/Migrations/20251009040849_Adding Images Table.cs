using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApiProject.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddingImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("ae79f298-0623-4c6f-b2ac-311d82441b6e"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("b345125c-0708-4db9-8406-d94480e4ed24"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("eaee90e7-ab71-4c05-8d9f-fc1f257a3acb"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("03d24924-cf37-44e5-b521-c51c423def2b"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("0d46b810-adc2-499b-8192-f805821c1de9"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("67d9f43c-93bc-44a0-b405-f0b8dd44a49e"));

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    NamePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1f5bd5e1-fc8e-4255-abf1-64d4d379be09"), "Easy" },
                    { new Guid("96f5fc1d-c54b-4806-acd6-2790f010a485"), "Hard" },
                    { new Guid("b461241c-b6c1-4904-b8f9-37bd6f54e9b7"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("73b15f4e-3582-405d-9dcc-cc86e08ef510"), "US", "United States", "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a4/Flag_of_the_United_States.svg/2560px-Flag_of_the_United_States.svg.png" },
                    { new Guid("859de3f5-d36f-480f-a66e-025a37947eb5"), "TW", "Taiwan", "https://upload.wikimedia.org/wikipedia/commons/thumb/7/72/Flag_of_the_Republic_of_China.svg/800px-Flag_of_the_Republic_of_China.svg.png" },
                    { new Guid("bf9205ee-553d-48c3-8017-9dca8a170734"), "MX", "Mexico", "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Flag_of_Mexico.svg/1200px-Flag_of_Mexico.svg.png" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("1f5bd5e1-fc8e-4255-abf1-64d4d379be09"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("96f5fc1d-c54b-4806-acd6-2790f010a485"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("b461241c-b6c1-4904-b8f9-37bd6f54e9b7"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("73b15f4e-3582-405d-9dcc-cc86e08ef510"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("859de3f5-d36f-480f-a66e-025a37947eb5"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("bf9205ee-553d-48c3-8017-9dca8a170734"));

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("ae79f298-0623-4c6f-b2ac-311d82441b6e"), "Hard" },
                    { new Guid("b345125c-0708-4db9-8406-d94480e4ed24"), "Easy" },
                    { new Guid("eaee90e7-ab71-4c05-8d9f-fc1f257a3acb"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("03d24924-cf37-44e5-b521-c51c423def2b"), "MX", "Mexico", "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Flag_of_Mexico.svg/1200px-Flag_of_Mexico.svg.png" },
                    { new Guid("0d46b810-adc2-499b-8192-f805821c1de9"), "TW", "Taiwan", "https://upload.wikimedia.org/wikipedia/commons/thumb/7/72/Flag_of_the_Republic_of_China.svg/800px-Flag_of_the_Republic_of_China.svg.png" },
                    { new Guid("67d9f43c-93bc-44a0-b405-f0b8dd44a49e"), "US", "United States", "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a4/Flag_of_the_United_States.svg/2560px-Flag_of_the_United_States.svg.png" }
                });
        }
    }
}
