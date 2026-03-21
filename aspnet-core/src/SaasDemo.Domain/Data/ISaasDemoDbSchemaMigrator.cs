using System.Threading.Tasks;

namespace SaasDemo.Data;

public interface ISaasDemoDbSchemaMigrator
{
    Task MigrateAsync();
}
