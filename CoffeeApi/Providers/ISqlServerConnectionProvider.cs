using System.Data;

namespace Coffee.API.Providers
{
    public interface ISqlServerConnectionProvider
    {
        IDbConnection GetDbConnection();
    }
}
