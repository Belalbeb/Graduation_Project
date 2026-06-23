using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Migrations
{
    /// <inheritdoc />
    public partial class updatecoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicablePlans",
                table: "Coupons");

            migrationBuilder.CreateTable(
                name: "CouponSubscriptionPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CouponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponSubscriptionPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponSubscriptionPlan_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouponSubscriptionPlan_subscriptionPlans_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "subscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CouponSubscriptionPlan_CouponId",
                table: "CouponSubscriptionPlan",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponSubscriptionPlan_SubscriptionPlanId",
                table: "CouponSubscriptionPlan",
                column: "SubscriptionPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponSubscriptionPlan");

            migrationBuilder.AddColumn<string>(
                name: "ApplicablePlans",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
