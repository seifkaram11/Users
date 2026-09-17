using Dapper;
using Core.Entities;
using Core.RepositoryContracts;
using Infrastructure.DbContext;

namespace Infrastructure.Repository;

public class UsersRepository : IUsersRepository
{
    DapperDbContext _dbContext;

    public UsersRepository(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Users?> AddUserAsync(Users? user)
    {
        if(user is null)return null;
        var check=await GetUserByEmailAsync(user.Email!);
        if(check is not null)return null;

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
        var res = await conn.ExecuteAsync(sql, user);
        conn.Close();
        return res > 0 ? user : null;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        const string sql_ =
            """
            DELETE FROM "UserRoles"
            WHERE "UserID" = @UserID;
            """;

        var conn_=_dbContext.connection;
        conn_.Open();
        var res_ = await conn_.ExecuteAsync(sql_, new { UserID = userId });
        conn_.Close();
        const string sql =
            """
            DELETE FROM "Users"
            WHERE "UserID" = @UserID;
            """;
        var conn=_dbContext.connection;
        conn.Open();
        var res = await conn.ExecuteAsync(sql, new { UserID = userId });
        conn.Close();
        return res > 0;
    }

    public async Task<IEnumerable<Users>> GetAllUsersAsync()
    {
        const string sql =
            """
            SELECT *
            FROM "Users";
            """;
        var conn=_dbContext.connection;
        conn.Open();
        var res = await conn.QueryAsync<Users>(sql);
        conn.Close();
        return res;
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
        var res=await conn
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
        var res=await conn
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
        var res = await conn.ExecuteAsync(sql, user);
        conn.Close();
        return res == 1 ? user : null;
    }

    public async Task<bool> AddRoleToUserAsync(Guid userId, Guid roleId)
    {
        const string sql=
        """
        INSERT INTO "UserRoles"("RoleId","UserID")
        VALUES(@RoleId,@UserID);
        """;
        var conn=_dbContext.connection;
        conn.Open();
        var res = await conn.ExecuteAsync(sql, new { RoleId = roleId, UserID = userId });
        conn.Close();
        return res == 1;
    }
}
