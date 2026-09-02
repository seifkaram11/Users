using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace eCommerce.Infrastructure.DbContext;

class DapperDbContext
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
