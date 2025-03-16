using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationforDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    DocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Documenttype = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DocumentID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Document");
        }
    }
}
