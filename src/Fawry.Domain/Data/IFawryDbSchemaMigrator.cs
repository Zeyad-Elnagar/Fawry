using System.Threading.Tasks;

namespace Fawry.Data;

public interface IFawryDbSchemaMigrator
{
    Task MigrateAsync();
}
