using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.Kvp.Storage.EFCore.SQLite.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KvpItems",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    Custom1 = table.Column<string>(maxLength: 255, nullable: true),
                    Custom2 = table.Column<string>(maxLength: 255, nullable: true),
                    FeatureId = table.Column<string>(maxLength: 36, nullable: true),
                    Key = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedUtc = table.Column<DateTime>(nullable: false),
                    SetId = table.Column<string>(maxLength: 36, nullable: true),
                    SubSetId = table.Column<string>(maxLength: 36, nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KvpItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KvpItems_FeatureId",
                table: "KvpItems",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_KvpItems_Key",
                table: "KvpItems",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_KvpItems_SetId",
                table: "KvpItems",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_KvpItems_SubSetId",
                table: "KvpItems",
                column: "SubSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KvpItems");
        }
    }
}
