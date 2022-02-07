﻿namespace Pantrymo.Domain.Services.Sync
{
    public record class SyncStatus(DateTime LastSuccessfulSync, DateTime LastFailedSync, Exception? LastFailure, int RecordSyncCount)
    {
        public DateTime LastUpdated => Succeeded ? LastSuccessfulSync : LastFailedSync;

        public bool Succeeded => LastSuccessfulSync > LastFailedSync;

        public SyncStatus UpdateSuccess(int newSynced) => new SyncStatus(DateTime.Now, LastFailedSync, LastFailure, RecordSyncCount + newSynced);

        public SyncStatus UpdateFailure(Exception? e) => new SyncStatus(LastSuccessfulSync, DateTime.Now, e ?? new Exception("unknown erro"), RecordSyncCount);
    }
}
