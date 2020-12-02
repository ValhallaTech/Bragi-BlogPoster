using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogPosts.Data.Migrations
{
    public partial class Update01 : Migration
    {
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropColumn(
                                        name: "Created",
                                        table: "Post" );

            migrationBuilder.DropColumn(
                                        name: "Updated",
                                        table: "Post" );
        }

        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.AddColumn<DateTime>(
                                                 name: "Created",
                                                 table: "Post",
                                                 type: "datetime2",
                                                 nullable: false,
                                                 defaultValue: new DateTime( 1, 1, 1, 0, 0, 0, 0,
                                                                             DateTimeKind.Unspecified ) );

            migrationBuilder.AddColumn<DateTime>(
                                                 name: "Updated",
                                                 table: "Post",
                                                 type: "datetime2",
                                                 nullable: true );
        }
    }
}