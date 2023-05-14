using System.Data.Common;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SqlKata;
using SqlKata.Compilers;

namespace Loghmani.ECommerce.Old.Data;

public class DatabaseContext
{
    private IConfiguration Configuration { get; }

    private string DatabaseName
    {
        get
        {
            return nameof(DatabaseContext).Replace(
                nameof(HttpContext).Replace(
                    nameof(WebRequestMethods.Http),
                    string.Empty
                ),
                string.Empty
            );
        }
    }

    public DatabaseContext(IConfiguration configuration)
    {
        
        Configuration = configuration;
        //
    }

    /*
    public Task<IDbSetting> Setting()
    {
        return
            Task.FromResult(DbSettingMapper
                .Get<MySqlConnection>()); //DbSettingMapper.Get(typeof(MySqlConnection)));
    }
    */

    protected Task<Compiler> Compiler()
    {
        return Task.FromResult<Compiler>(new MySqlCompiler());
    }

    public async Task<SqlResult> Compile(Query query)
    {
        return (await Compiler()).Compile(query);
    }

    public async Task<DbConnection> Connection()
    {
        return await Task.FromResult<DbConnection>(new MySqlConnection(connectionString: (await ConnectionString()).ConnectionString));
    }

    private Task<DbConnectionStringBuilder> ConnectionString()
    {
        return Task.FromResult<DbConnectionStringBuilder>(new MySqlConnectionStringBuilder()
        {
            Server = Configuration[string.Format("{0}:{1}:{2}",
                DatabaseName,
                nameof(MySql),
                nameof(MySqlConnectionStringBuilder.Server)
                )],
            Port = 0,
            Database = Configuration[string.Format("{0}:{1}:{2}",
                DatabaseName,
                nameof(MySql),
                nameof(MySqlConnectionStringBuilder.Database)
            )],
            UserID = Configuration[string.Format("{0}:{1}:{2}",
                DatabaseName,
                nameof(MySql),
                nameof(MySqlConnectionStringBuilder.UserID)
            )],
            Password = Configuration[string.Format("{0}:{1}:{2}",
                DatabaseName,
                nameof(MySql),
                nameof(MySqlConnectionStringBuilder.Password)
            )]
        });
    }
}