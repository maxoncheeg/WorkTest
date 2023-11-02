﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Saga.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FirstState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentState = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    RequestId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ResponseAddress = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirstState", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "SecondState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentState = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    RequestId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ResponseAddress = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondState", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FirstState");

            migrationBuilder.DropTable(
                name: "SecondState");
        }
    }
}
