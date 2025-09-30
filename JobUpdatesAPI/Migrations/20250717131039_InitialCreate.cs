using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobUpdatesAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    MinSalaryExpectation = table.Column<int>(type: "integer", nullable: false),
                    MaxSalaryExpectation = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                });

            migrationBuilder.CreateTable(
                name: "KeywordModel",
                columns: table => new
                {
                    KeywordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Keyword = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordModel", x => x.KeywordId);
                });

            migrationBuilder.CreateTable(
                name: "JobKeywords",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "integer", nullable: false),
                    KeywordId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobKeywords", x => new { x.JobId, x.KeywordId });
                    table.ForeignKey(
                        name: "FK_JobKeywords_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobKeywords_KeywordModel_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "KeywordModel",
                        principalColumn: "KeywordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobStatuses",
                columns: table => new
                {
                    JobStatusId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusName = table.Column<string>(type: "text", nullable: false),
                    JobUpdateId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobStatuses", x => x.JobStatusId);
                });

            migrationBuilder.CreateTable(
                name: "JobUpdates",
                columns: table => new
                {
                    JobUpdateId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobId = table.Column<int>(type: "integer", nullable: false),
                    JobStatusId = table.Column<short>(type: "smallint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobUpdates", x => x.JobUpdateId);
                    table.ForeignKey(
                        name: "FK_JobUpdates_JobStatuses_JobStatusId",
                        column: x => x.JobStatusId,
                        principalTable: "JobStatuses",
                        principalColumn: "JobStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobUpdates_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "JobStatuses",
                columns: new[] { "JobStatusId", "JobUpdateId", "StatusName" },
                values: new object[,]
                {
                    { (short)1, null, "N/A" },
                    { (short)2, null, "Applied" },
                    { (short)3, null, "Rejection" },
                    { (short)4, null, "Holding CV" },
                    { (short)5, null, "Awaiting Response" },
                    { (short)6, null, "Scheduled Phone Call" },
                    { (short)7, null, "Screening/ Pre-Interview" },
                    { (short)8, null, "Interview" },
                    { (short)9, null, "Offer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobKeywords_KeywordId",
                table: "JobKeywords",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_JobStatuses_JobUpdateId",
                table: "JobStatuses",
                column: "JobUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_JobUpdates_JobId",
                table: "JobUpdates",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobUpdates_JobStatusId",
                table: "JobUpdates",
                column: "JobStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobStatuses_JobUpdates_JobUpdateId",
                table: "JobStatuses",
                column: "JobUpdateId",
                principalTable: "JobUpdates",
                principalColumn: "JobUpdateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobUpdates_Jobs_JobId",
                table: "JobUpdates");

            migrationBuilder.DropForeignKey(
                name: "FK_JobStatuses_JobUpdates_JobUpdateId",
                table: "JobStatuses");

            migrationBuilder.DropTable(
                name: "JobKeywords");

            migrationBuilder.DropTable(
                name: "KeywordModel");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "JobUpdates");

            migrationBuilder.DropTable(
                name: "JobStatuses");
        }
    }
}
