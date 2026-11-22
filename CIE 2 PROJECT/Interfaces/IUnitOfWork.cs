using System.Data.SqlClient;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IUnitOfWork : System.IDisposable
    {
        SqlConnection Connection { get; }
        SqlTransaction Transaction { get; }
        void Commit();
        void Rollback();
    }
}
