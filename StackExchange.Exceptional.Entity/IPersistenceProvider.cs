using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackExchange.Exceptional.Entity
{
    /// <summary>
    /// This interface must be implemented in user code 
    /// </summary>
    public interface IPersistenceProvider
    {
        bool DeleteAllErrors(string applicationName = null);
        bool DeleteError(Guid guid);
        int GetAllErrors(List<IPersistedError> list, string applicationName = null);
        IPersistedError GetError(Guid guid);
        int GetErrorCount(DateTime? since = null, string applicationName = null);
        IPersistedError LogError(Error error);
        bool ProtectError(Guid guid);
    }
}
