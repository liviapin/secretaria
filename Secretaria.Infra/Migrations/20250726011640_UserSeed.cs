using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Secretaria.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO SecretariaDb.dbo.AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount) VALUES(N'5b5557c9-1c9d-40aa-a615-2dc6536dd548', N'admin@admin.com', N'ADMIN@ADMIN.COM', N'admin@admin.com', N'ADMIN@ADMIN.COM', 0, N'AQAAAAIAAYagAAAAEFA7pKK7XgRvhzWs47ULdPgxEK3rd3mRw6WZWtPRo2swVJveeuOuZnjTewvYCY2Msg==', N'GBOE7FTH54AMZWBLQR5OBOO7CBTKXG4K', N'f31fc58c-a7fa-48af-8ea0-615bfd9595d4', NULL, 0, 0, NULL, 1, 0);");
            migrationBuilder.Sql("INSERT INTO SecretariaDb.dbo.AspNetUserRoles (UserId, RoleId) SELECT N'5b5557c9-1c9d-40aa-a615-2dc6536dd548', r.Id FROM ASpNetRoles r WHERE r.Name = 'Admin';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM SecretariaDb.dbo.AspNetUserRoles WHERE UserId=N'5b5557c9-1c9d-40aa-a615-2dc6536dd548' AND RoleId=N'e6561005-a6e6-4055-893f-609986ef2333';");
            migrationBuilder.Sql("DELETE FROM SecretariaDb.dbo.AspNetUsers WHERE Id=N'5b5557c9-1c9d-40aa-a615-2dc6536dd548';");
        }
    }
}
