using Core.Entities;
using Core.RepositoryContracts;
using Dapper;
using Infrastructure.DbContext;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository;

public class RolesRepository : IRolesRepository
{
    DapperDbContext _dbContext;
    ILogger<RolesRepository> _logger;

    public RolesRepository(DapperDbContext dbContext, ILogger<RolesRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Roles>> GetAllRolesAsync()
    {
        const string sql =
            """
            SELECT *
            FROM "Roles";
            """;
        var conn=_dbContext.connection;
        conn.Open();
        var res=await _dbContext.connection.QueryAsync<Roles>(sql);
        conn.Close();
        return res;
    }

    public async Task<IEnumerable<Roles>> GetRolesByUserIdAsync(Guid userId)
    {
        const string sql=
        """
        SELECT R.*
        FROM "Roles" R
        INNER JOIN "UserRoles" UR ON R."RoleId"=UR."RoleId"
        WHERE UR."UserID"=@userId;
        """;
        var conn=_dbContext.connection;
        conn.Open();
        var res=await conn.QueryAsync<Roles>(sql,new {userId=userId});
        conn.Close();
        return res;
    }

    public async Task<IEnumerable<Guid>> GetRolesIdsByNameAsync(IEnumerable<string> roleNames)
    {
        var roleIds = new List<Guid>();
        foreach(var roleName in roleNames)
        {
            const string getRoleId=
                """
                SELECT "RoleId"
                FROM "Roles"
                WHERE "RoleName"=@roleName;
                """;
            var conn=_dbContext.connection;
            conn.Open();
            var roleId=await _dbContext.connection.QuerySingleOrDefaultAsync<Guid>(getRoleId,new {roleName=roleName});
            conn.Close();
            roleIds.Add(roleId);
        }
        return roleIds;
    }

    public async Task<bool> UpdateRoleAsync(Guid roleId, string newRoleName)
    {
        const string sql=
        """
        UPDATE "Roles"
        SET "RoleName"=@newRoleName
        WHERE "RoleId"=@roleId;
        """;
        var conn=_dbContext.connection;
        conn.Open();
        var rowsAffected=await _dbContext.connection.ExecuteAsync(sql,new {roleId=roleId,newRoleName=newRoleName});
        conn.Close();
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateUsersRolesAsync(Guid userId, IEnumerable<string> roleNames)
    {
        try
        {
            const string sql=
            """
            DELETE FROM "UserRoles"
            WHERE "UserID"=@userId;
            """;
            var conn=_dbContext.connection;
            conn.Open();
            await _dbContext.connection.ExecuteAsync(sql,new {userId=userId});
            conn.Close();
            var roleIds=await GetRolesIdsByNameAsync(roleNames);
            foreach(var roleId in roleIds)
            {
                if(roleId!=Guid.Empty)
                {
                    const string insertSql=
                    """
                    INSERT INTO "UserRoles" ("UserID","RoleId")
                    VALUES (@userId,@roleId);
                    """;
                    conn.Open();
                    await _dbContext.connection.ExecuteAsync(insertSql,new {userId=userId,roleId=roleId});
                    conn.Close();
                }
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Error occurred while updating user roles for userId: {UserId}",userId);
            return false;
        }
        return true;
    }

    public async Task<bool> AddRoleAsync(Roles role)
    {
        const string sql=
        """
        INSERT INTO "Roles" ("RoleId","RoleName")
        VALUES (@RoleId,@RoleName);
        """;
        var conn=_dbContext.connection;
        conn.Open();
        var rowsAffected=await _dbContext.connection.ExecuteAsync(sql,role);
        conn.Close();
        return rowsAffected > 0;
    }
}
