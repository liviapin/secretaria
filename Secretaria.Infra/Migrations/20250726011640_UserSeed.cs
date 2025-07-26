using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

#nullable disable

namespace Secretaria.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO SecretariaDB.dbo.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp) VALUES(N'236eddc8-353f-4ee8-8fc4-b0b98d99479a', N'Admin', N'ADMIN', NULL);");
            migrationBuilder.Sql("INSERT INTO SecretariaDb.dbo.AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount) VALUES(N'5b5557c9-1c9d-40aa-a615-2dc6536dd548', N'admin@admin.com', N'ADMIN@ADMIN.COM', N'admin@admin.com', N'ADMIN@ADMIN.COM', 0, N'AQAAAAIAAYagAAAAEFA7pKK7XgRvhzWs47ULdPgxEK3rd3mRw6WZWtPRo2swVJveeuOuZnjTewvYCY2Msg==', N'GBOE7FTH54AMZWBLQR5OBOO7CBTKXG4K', N'f31fc58c-a7fa-48af-8ea0-615bfd9595d4', NULL, 0, 0, NULL, 1, 0);");
            migrationBuilder.Sql("INSERT INTO SecretariaDb.dbo.AspNetUserRoles (UserId, RoleId) VALUES(N'5b5557c9-1c9d-40aa-a615-2dc6536dd548', N'236eddc8-353f-4ee8-8fc4-b0b98d99479a');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM SecretariaDb.dbo.AspNetUserRoles WHERE UserId=N'5b5557c9-1c9d-40aa-a615-2dc6536dd548' AND RoleId=N'236eddc8-353f-4ee8-8fc4-b0b98d99479a';");
            migrationBuilder.Sql("DELETE FROM SecretariaDb.dbo.AspNetUsers WHERE Id=N'5b5557c9-1c9d-40aa-a615-2dc6536dd548';");
            migrationBuilder.Sql("DELETE FROM SecretariaDB.dbo.AspNetRoles WHERE Id=N'504e9f54-5940-4721-8252-4f60ae6d64f8';");
        }
    }
}