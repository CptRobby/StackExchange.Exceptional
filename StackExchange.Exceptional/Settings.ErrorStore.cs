﻿using System.ComponentModel;
using System.Configuration;

namespace StackExchange.Exceptional
{
    public partial class Settings
    {
        /// <summary>
        /// The ErrorStore section of the configuration, optional and will default to a MemoryErrorStore if not specified
        /// </summary>
        [ConfigurationProperty("ErrorStore")]
        public ErrorStoreSettings ErrorStore => this["ErrorStore"] as ErrorStoreSettings;
    }

    /// <summary>
    /// A settings object describing an error store
    /// </summary>
    public class ErrorStoreSettings : ConfigurationElement
    {
        /// <summary>
        /// The type of error store to use, File, SQL, Memory, etc.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type => this["type"] as string;

        /// <summary>
        /// The path to use on file based error stores
        /// </summary>
        [ConfigurationProperty("path")]
        public string Path => this["path"] as string;

        /// <summary>
        /// The connection string to use on database based error stores.  If provided, ConnectionStringName is ignored.
        /// </summary>
        [ConfigurationProperty("connectionString")]
        public string ConnectionString => this["connectionString"] as string;

        /// <summary>
        /// The name of the connection string to use from the application's configuration.
        /// </summary>
        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName => this["connectionStringName"] as string;

        /// <summary>
        /// The size of this error log, either how many to keep or how many to display depending on type
        /// </summary>
        [ConfigurationProperty("size"), DefaultValue(typeof(int), "200")]
        public int Size => (int)this["size"];

        /// <summary>
        /// Defines whether the ErrorStore should try to use Error.LastDuplicateDate or not
        /// </summary>
        [ConfigurationProperty("includeLastDuplicateDate"), DefaultValue(false)]
        public bool IncludeLastDuplicateDate { get { return this["includeLastDuplicateDate"] as bool? ?? false; } }

        /// <summary>
        /// If this is true, then the criteria for rolling up errors uses the Error.LastDuplicateDate instead of Error.CreationDate.
        /// This way the RollupSeconds would be the minimum number of seconds that would have to pass without matching errors occurring
        /// before a new record would be created. Has no effect if IncludeLastDuplicateDate is not true.
        /// </summary>
        [ConfigurationProperty("rollupUsingLastDuplicateDate"), DefaultValue(false)]
        public bool RollupUsingLastDuplicateDate { get { return this["rollupUsingLastDuplicateDate"] as bool? ?? false; } }

        /// <summary>
        /// The duration in seconds of error groups to rollup, similar errors within this timespan will be shown as duplicates
        /// </summary>
        [ConfigurationProperty("rollupSeconds"), DefaultValue(typeof(int), "600")]
        public int RollupSeconds => (int)this["rollupSeconds"];

        /// <summary>
        /// The size of the backup queue to use for the log, after rollups, it's how many entries in memory can be stored before culling the oldest
        /// </summary>
        [ConfigurationProperty("backupQueueSize"), DefaultValue(typeof(int), "1000")]
        public int BackupQueueSize => (int)this["backupQueueSize"];
    }
}