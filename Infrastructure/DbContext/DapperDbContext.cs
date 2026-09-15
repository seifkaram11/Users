using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.DbContext;

public class DapperDbContext
{

    IConfiguration _configuration;
    IDbConnection _connection;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        string? connString=_configuration.GetConnectionString("POSTGRES");

        _connection=new NpgsqlConnection(connString);
    }

    public IDbConnection connection => _connection;
}
