using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.Kvp.Storage.EFCore.PostgreSql.Migrations
{
    public partial class cskvpinitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kvp_items",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    feature_id = table.Column<string>(maxLength: 36, nullable: true),
                    set_id = table.Column<string>(maxLength: 36, nullable: true),
                    sub_set_id = table.Column<string>(maxLength: 36, nullable: true),
                    key = table.Column<string>(maxLength: 255, nullable: true),
                    value = table.Column<string>(nullable: true),
                    custom1 = table.Column<string>(maxLength: 255, nullable: true),
                    custom2 = table.Column<string>(maxLength: 255, nullable: true),
                    created_utc = table.Column<DateTime>(nullable: false),
                    modified_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kvp_items", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_kvp_items_feature_id",
                table: "kvp_items",
                column: "feature_id");

            migrationBuilder.CreateIndex(
                name: "ix_kvp_items_key",
                table: "kvp_items",
                column: "key");

            migrationBuilder.CreateIndex(
                name: "ix_kvp_items_set_id",
                table: "kvp_items",
                column: "set_id");

            migrationBuilder.CreateIndex(
                name: "ix_kvp_items_sub_set_id",
                table: "kvp_items",
                column: "sub_set_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kvp_items");
        }
    }
}
