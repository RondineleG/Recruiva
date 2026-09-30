using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recruiva.Web.Migrations
{
    /// <inheritdoc />
    public partial class ImproveJobDetailsAndProfileOnboarding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Candidates_AddressId",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Advertisers_AddressId",
                table: "Advertisers");

            migrationBuilder.DropIndex(
                name: "IX_Advertisers_TaxId",
                table: "Advertisers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationInstructions",
                table: "Jobs",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfOpenings",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "Jobs",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Candidates",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Candidates",
                type: "UNIQUEIDENTIFIER",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "UNIQUEIDENTIFIER");

            migrationBuilder.AlterColumn<string>(
                name: "TaxId",
                table: "Advertisers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Advertisers",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Advertisers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_AddressId",
                table: "Candidates",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisers_AddressId",
                table: "Advertisers",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisers_TaxId",
                table: "Advertisers",
                column: "TaxId",
                unique: true,
                filter: "[TaxId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Candidates_AddressId",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Advertisers_AddressId",
                table: "Advertisers");

            migrationBuilder.DropIndex(
                name: "IX_Advertisers_TaxId",
                table: "Advertisers");

            migrationBuilder.DropColumn(
                name: "ApplicationInstructions",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "NumberOfOpenings",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "Jobs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Candidates",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Candidates",
                type: "UNIQUEIDENTIFIER",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "UNIQUEIDENTIFIER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaxId",
                table: "Advertisers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Advertisers",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Advertisers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_AddressId",
                table: "Candidates",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisers_AddressId",
                table: "Advertisers",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisers_TaxId",
                table: "Advertisers",
                column: "TaxId",
                unique: true);
        }
    }
}
