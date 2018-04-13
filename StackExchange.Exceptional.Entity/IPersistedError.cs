using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackExchange.Exceptional.Entity
{
    /// <summary>
    /// This interface is a means of providing a simplified form of <see cref="Error"/> for communication between the <see cref="EntityErrorStore"/>
    /// and the implementation of <see cref="IPersistenceProvider"/> in the user's code. The components of this interface that absolutely must be
    /// implemented are the GUID and FullJson properties. ErrorHash would be recommended for matching existing errors. Everything else is only required
    /// if you want to see it when viewing the ErrorList and ErrorInfo views.
    /// </summary>
    public interface IPersistedError
    {
        /// <summary>
        /// REQUIRED: This is the primary identifier of the Error.
        /// </summary>
        Guid GUID { get; }
        /// <summary>
        /// Recommended: This will be a relatively unique value generated from the details of the Error that can be used for easily finding duplicate Errors.
        /// </summary>
        int? ErrorHash { get; }
        /// <summary>
        /// REQUIRED: This is where all the details of the Error are stored. It must be stored and retreived so that the Error object can be recreated.
        /// </summary>
        string FullJson { get; }
        /// <summary>
        /// Optional: This allows you to distinguish between different applications being logged to the same place.
        /// </summary>
        string ApplicationName { get; }
        /// <summary>
        /// Optional: This must be set only if you actually want to be able to see how many duplicate Errors were encountered.
        /// </summary>
        int? DuplicateCount { get; }
        /// <summary>
        /// Optional: This must be set only if you want to be able to see when the Error was most recently encountered.
        /// </summary>
        DateTime? LastDuplicateDate { get; }
        /// <summary>
        /// Optional: This must be set only if you want to be able to see when an Error was flagged for deletion.
        /// </summary>
        DateTime? DeletionDate { get; }
        /// <summary>
        /// Optional: This is used to prevent the DeleteAllErrors method from deleting particular errors.
        /// </summary>
        bool IsProtected { get; }
    }
}
