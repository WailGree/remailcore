using Microsoft.EntityFrameworkCore.Migrations;

namespace RemailCore.Migrations
{
    public partial class ChangeEmailPropertyNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sender",
                table: "Emails",
                newName: "From");

            migrationBuilder.RenameColumn(
                name: "Seen",
                table: "Emails",
                newName: "Unread");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Unread",
                table: "Emails",
                newName: "Seen");

            migrationBuilder.RenameColumn(
                name: "From",
                table: "Emails",
                newName: "Sender");
        }
    }
}
