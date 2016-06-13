using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackExchange.Exceptional.Entity
{
    public class EntityErrorStore : ErrorStore
    {
        public static IPersistanceProvider PersistanceProvider { get; set; }

        public EntityErrorStore(ErrorStoreSettings settings) : base(settings) { }

        public EntityErrorStore(int rollupSeconds, int backupQueueSize = 1000) : base(rollupSeconds, backupQueueSize) { }

        /// <summary>
        /// Gets whether the ErrorStore will use Error.LastDuplicateDate or not
        /// </summary>
        public override bool IncludeLastDuplicateDate
        {
            get { return Settings.Current.ErrorStore.IncludeLastDuplicateDate; }
        }

        /// <summary>
        /// If this is true, then the criteria for rolling up errors uses the Error.LastDuplicateDate instead of Error.CreationDate.
        /// This way the RollupSeconds would be the minimum number of seconds that would have to pass without matching errors occurring
        /// before a new record would be created. Has no effect if IncludeLastDuplicateDate is not true.
        /// </summary>
        public bool RollupUsingLastDuplicateDate
        {
            get { return IncludeLastDuplicateDate && Settings.Current.ErrorStore.RollupUsingLastDuplicateDate; }
        }

        protected override bool DeleteAllErrors(string applicationName = null)
        {
            if (!IsConfigured()) throw new NotImplementedException();
            return PersistanceProvider.DeleteAllErrors(applicationName);
        }

        protected override bool DeleteError(Guid guid)
        {
            if (!IsConfigured()) throw new NotImplementedException();
            return PersistanceProvider.DeleteError(guid);
        }

        protected override int GetAllErrors(List<Error> list, string applicationName = null)
        {
            if (!IsConfigured()) throw new NotImplementedException();
            var sourceList = new List<IPersistedError>();
            int retval = PersistanceProvider.GetAllErrors(sourceList, applicationName);
            list.AddRange(sourceList.Select(ConvertToError));
            return retval;
        }

        protected override Error GetError(Guid guid)
        {
            if (!IsConfigured()) throw new NotImplementedException();
            return ConvertToError(PersistanceProvider.GetError(guid));
        }

        protected override int GetErrorCount(DateTime? since = default(DateTime?), string applicationName = null)
        {
            if (!IsConfigured()) throw new NotImplementedException();
            return PersistanceProvider.GetErrorCount(since, applicationName);
        }

        protected override void LogError(Error error)
        {
            if (!IsConfigured()) throw new NotImplementedException();
            var retval = PersistanceProvider.LogError(error);
            if (retval != null && retval.GUID != error.GUID) error.IsDuplicate = true;
        }

        protected override bool ProtectError(Guid guid)
        {
            if (!IsConfigured()) throw new NotImplementedException();
            return PersistanceProvider.ProtectError(guid);
        }

        protected Error ConvertToError(IPersistedError source)
        {
            var e = Error.FromJson(source.FullJson);
            e.DuplicateCount = source.DuplicateCount;
            e.LastDuplicateDate = source.LastDuplicateDate;
            e.DeletionDate = source.DeletionDate;
            e.IsProtected = source.IsProtected;
            return e;
        }

        private bool _AttemptedConfig = false;
        protected bool IsConfigured()
        {
            if (PersistanceProvider != null) return true;
            if (_AttemptedConfig) return false;
            _AttemptedConfig = true;
            var assm = System.Reflection.Assembly.GetExecutingAssembly();
            var ti = typeof(IPersistanceProvider);
            var implTypes = assm.GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.GetInterface(ti.Name) == ti);
            var match = implTypes.FirstOrDefault(x => x.GetConstructor(Type.EmptyTypes) != null);
            if (match == null) return false;
            PersistanceProvider = Activator.CreateInstance(match, true) as IPersistanceProvider;
            return PersistanceProvider != null;
        }
    }
}
