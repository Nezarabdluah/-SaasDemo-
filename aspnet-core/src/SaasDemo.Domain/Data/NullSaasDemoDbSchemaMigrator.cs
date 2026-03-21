using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SaasDemo.Data;

/* This is used if database provider does't define
 * ISaasDemoDbSchemaMigrator implementation.
 */
public class NullSaasDemoDbSchemaMigrator : ISaasDemoDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
