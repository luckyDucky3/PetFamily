using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "species",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    specie_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_species", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "volunteer",
                columns: table => new
                {
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    experience_years = table.Column<int>(type: "integer", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    RequisitesForHelpDetails = table.Column<string>(type: "jsonb", nullable: false),
                    SocialNetworksDetails = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteer", x => x.volunteer_id);
                });

            migrationBuilder.CreateTable(
                name: "breed",
                columns: table => new
                {
                    breed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    breed_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    specie_id = table.Column<Guid>(type: "uuid", nullable: false),
                    specie_id1 = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_breed", x => x.breed_id);
                    table.ForeignKey(
                        name: "fk_breed_species_specie_id",
                        column: x => x.specie_id1,
                        principalTable: "species",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_breed_species_specie_id1",
                        column: x => x.specie_id,
                        principalTable: "species",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "pet",
                columns: table => new
                {
                    pet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false),
                    breed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    info_about_health = table.Column<string>(type: "text", nullable: false),
                    weight = table.Column<double>(type: "double precision", nullable: true),
                    height = table.Column<double>(type: "double precision", nullable: true),
                    phone_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    is_castrate = table.Column<bool>(type: "boolean", nullable: false),
                    is_vaccinate = table.Column<bool>(type: "boolean", nullable: false),
                    birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pet", x => x.pet_id);
                    table.ForeignKey(
                        name: "fk_pet_volunteer_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "volunteer",
                        principalColumn: "volunteer_id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_breed_specie_id",
                table: "breed",
                column: "specie_id1");

            migrationBuilder.CreateIndex(
                name: "ix_breed_specie_id1",
                table: "breed",
                column: "specie_id");

            migrationBuilder.CreateIndex(
                name: "ix_pet_volunteer_id",
                table: "pet",
                column: "volunteer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "breed");

            migrationBuilder.DropTable(
                name: "pet");

            migrationBuilder.DropTable(
                name: "species");

            migrationBuilder.DropTable(
                name: "volunteer");
        }
    }
}
