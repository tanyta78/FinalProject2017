using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AuctionHub.Data.Migrations
{
    public partial class AuctionDataModelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Close",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "Create",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "Open",
                table: "Auctions",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Auctions",
                newName: "EndDate");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Auctions",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Auctions",
                newName: "Open");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Auctions",
                newName: "Duration");

            migrationBuilder.AddColumn<DateTime>(
                name: "Close",
                table: "Auctions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "Auctions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
