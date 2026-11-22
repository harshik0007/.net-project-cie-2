using System.Data;

namespace CIE_2_PROJECT.Interfaces
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Begin(IsolationLevel isolationLevel);
    }
}
