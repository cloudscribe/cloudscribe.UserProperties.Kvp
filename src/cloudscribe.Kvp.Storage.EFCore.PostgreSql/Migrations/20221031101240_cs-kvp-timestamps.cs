using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cloudscribe.Kvp.Storage.EFCore.PostgreSql.Migrations
{
    public partial class cskvptimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"DO $$
            BEGIN
                ALTER TABLE kvp_items
                  ALTER created_utc TYPE timestamptz USING created_utc AT TIME ZONE 'UTC'
                , ALTER created_utc SET DEFAULT now();

                ALTER TABLE kvp_items
                  ALTER modified_utc TYPE timestamptz USING modified_utc AT TIME ZONE 'UTC'
                , ALTER modified_utc SET DEFAULT now();
            END$$;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
