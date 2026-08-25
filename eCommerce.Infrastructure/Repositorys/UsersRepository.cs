using AutoMapper;
using Dapper;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DbContext;

namespace eCommerce.Infrastructure.Repository;

class UsersRepository : IUsersRepository
{
    DapperDbContext _dbContext;

    public UsersRepository(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Users?> AddUserAsync(Users user)
    {
        var check=await GetUserByEmailAsync(user.Email!);
        if(check is not null)return null;

        user.UserID = Guid.NewGuid();

        const string sql =
             """
            INSERT INTO "Users"(
                "UserID",
                "Email",
                "PasswordHash",
                "Name",
                "Gender",
                "RefreshToken",
                "RefreshTokenExpiryTime"
            )
            VALUES(
                @UserID,
                @Email,
                @PasswordHash,
                @Name,
                @Gender,
                @RefreshToken,
                @RefreshTokenExpiryTime
            );
            """;
        var conn=_dbContext.connection;
        conn.Open();
        var res = await _dbContext.connection.ExecuteAsync(sql, user);
        conn.Close();
        return res > 0 ? user : null;
    }

    public async Task<Users?> GetUserByEmailAsync(string email)
    {
        const string sql =
            """
            SELECT *
            FROM "Users"
            WHERE "Email" = @Email;
            """;

        var conn=_dbContext.connection;
        conn.Open();
        var res=await _dbContext.connection
            .QueryFirstOrDefaultAsync<Users>(
                sql,
                new { Email = email });
        conn.Close();
        return res;
    }

    public async Task<Users?> GetUserByIDAsync(Guid userID)
    {
        const string sql =
             """
            SELECT *
            FROM "Users"
            WHERE "UserID" = @UserID;
            """;

        var conn=_dbContext.connection;
        conn.Open();
        var res=await _dbContext.connection
            .QueryFirstOrDefaultAsync<Users>(
                sql,
                new { UserID = userID });
        conn.Close();
        return res;
    }

    public async Task<Users?> UpdateUserAsync(Users user)
    {
        const string sql =
            """
            UPDATE "Users"
            SET
                "Email" = @Email,
                "PasswordHash" = @PasswordHash,
                "Name" = @Name,
                "Gender" = @Gender,
                "RefreshToken" = @RefreshToken,
                "RefreshTokenExpiryTime" = @RefreshTokenExpiryTime
            WHERE "UserID" = @UserID;
            """;

        var conn=_dbContext.connection;
        conn.Open();
        var res = await _dbContext.connection.ExecuteAsync(sql, user);
        conn.Close();
        return res == 1 ? user : null;
    }
}
