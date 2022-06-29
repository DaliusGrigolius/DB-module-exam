using Microsoft.EntityFrameworkCore;

namespace Repository.DbConfigs
{
    public interface IDbConfigurations
    {
        string ConnectionString { get; set; }
        DbContextOptions Options { get; set; }
    }
}